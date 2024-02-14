using game.bll.Infrastructure.Commands.Card.Utils;
using game.bll.Infrastructure.DataTransferObjects;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.dal.Models;

namespace game.bll.Infrastructure.Commands.Card
{
    public class SpoonCommand : ICardCommand<Spoon>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        private readonly INoModification _noModification;

        public SpoonCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard, INoModification noModification)
        {
            _unitOfWork = unitOfWork;
            _simpleAddToBoard = simpleAddToBoard;
            _noModification = noModification;
        }

        public Task<List<Guid>> OnEndRound(BoardCard boardCard)
        {
            return Task.FromResult(_noModification.OnEndRound(boardCard));
        }

        public async Task OnEndTurn(Player player, HandCard handCard)
        {
            await _simpleAddToBoard.AddToBoard(player, handCard);
        }

        public async Task OnAfterTurn(Player player, PlayAfterTurnDTO playAfterTurnDTO)
        {
            // Get type of the searched card
            var searchType = playAfterTurnDTO.CardType
                ?? throw new ArgumentNullException(nameof(playAfterTurnDTO));

            // Get hand card entities
            var hands = _unitOfWork.HandCardRepository.Get(
                    filter: x => x.GameId == player.GameId,
                    transform: x => x.AsNoTracking(),
                    includeProperties: nameof(HandCard.CardInfo))
                .GroupBy(h => h.HandId).ToDictionary(h => h.Key, h => h.ToList());

            // Get player ids of the game in order
            var playerIds = _unitOfWork.GameRepository.Get(
                filter: x => x.Id == player.GameId,
                transform: x => x.AsNoTracking()
            ).First().PlayerIds.ToList();

            var playerIndex = playerIds.IndexOf(player.Id);
            var nextPlayerId = playerIds[(playerIndex + 1) % playerIds.Count];

            // Iterate through the players to find a card with the searched type
            while (nextPlayerId != player.Id)
            {
                // Get the next player entity
                var nextPlayer = _unitOfWork.PlayerRepository.Get(
                    filter: x => x.Id == nextPlayerId,
                    transform: x => x.AsNoTracking())
                    .FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Player));

                if (hands.ContainsKey(nextPlayer.HandId) && hands[nextPlayer.HandId].Any(c => c.CardInfo.CardType == searchType))
                {
                    // Get the card from the other player and place it to own board
                    var card = hands[nextPlayer.HandId].First(c => c.CardInfo.CardType == searchType);
                    var boardCard = new BoardCard
                    {
                        GameId = card.GameId,
                        BoardId = player.BoardId,
                        CardInfo = card.CardInfo,
                    };
                    _unitOfWork.BoardCardRepository.Insert(boardCard);
                    _unitOfWork.HandCardRepository.Delete(card);

                    // Get the spoon entity
                    var spoon = _unitOfWork.BoardCardRepository.Get(
                        filter: x => x.Id == playAfterTurnDTO.HandOrBoardCardId,
                        transform: x => x.AsNoTracking(),
                        includeProperties: nameof(BoardCard.CardInfo)
                    ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(Spoon));

                    // Add the spoon to the other player's hand
                    _unitOfWork.HandCardRepository.Insert(new HandCard
                    {
                        GameId = player.GameId,
                        HandId = card.HandId,
                        CardInfo = spoon.CardInfo
                    });

                    // Remove the spoon from the board
                    _unitOfWork.BoardCardRepository.Delete(spoon);

                    await _unitOfWork.Save();
                    break;
                }
                nextPlayerId = playerIds[(playerIndex + 1) % playerIds.Count];
            }
        }
    }
}
