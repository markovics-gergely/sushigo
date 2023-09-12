using game.bll.Infrastructure.Commands.Card.Utils;
using game.bll.Infrastructure.DataTransferObjects;
using game.dal.Domain;
using game.dal.Types;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card
{
    public class SpoonCommand : ICardCommand<Spoon>
    {
        public ClaimsPrincipal? User { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;

        public SpoonCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard)
        {
            _unitOfWork = unitOfWork;
            _simpleAddToBoard = simpleAddToBoard;
        }

        public async Task OnEndRound(BoardCard boardCard)
        {
            boardCard.IsCalculated = true;
            _unitOfWork.BoardCardRepository.Update(boardCard);
            await _unitOfWork.Save();
        }

        public async Task OnEndTurn(Player player, HandCard handCard)
        {
            await _simpleAddToBoard.AddToBoard(player, handCard);
        }

        public async Task OnAfterTurn(Player player, PlayAfterTurnDTO playAfterTurnDTO)
        {
            // Get type of the searched card
            var searchType = Enum.Parse<CardType>(playAfterTurnDTO.AdditionalInfo[Additional.Tagged]);

            // Get hand card entities
            var hands = _unitOfWork.HandCardRepository.Get(
                    filter: x => x.GameId == player.GameId,
                    transform: x => x.AsNoTracking()
                    );

            // Group cards by the hands and place it to a linked list for iteration
            var groupped = new LinkedList<IGrouping<Guid, HandCard>>(hands.GroupBy(h => h.HandId));

            // Find the node of the player
            var ownNode = groupped.Find(groupped.First(g => g.Key == player.HandId))!;

            // Get the starting node
            var actualNode = ownNode.Previous ?? groupped.Last!;

            // Iterate till it comes full circle
            while (actualNode != ownNode)
            {
                // If the card type is found in this hand, stop searching
                if (actualNode.Value.Any(a => a.CardType == searchType)) break;

                // Continue iteration
                actualNode = actualNode.Previous ?? groupped.Last!;
            }
            // If there was a card found
            if (actualNode != ownNode)
            {
                // Get a card with that type from their hand
                var card = actualNode.Value.First(c => c.CardType == searchType);

                // Place that card to the board of the player with the spoon
                var boardCard = new BoardCard
                {
                    CardType = card.CardType,
                    GameId = card.GameId,
                    BoardId = player.BoardId,
                    AdditionalInfo = card.AdditionalInfo,
                };
                _unitOfWork.BoardCardRepository.Insert(boardCard);
                // Also remove from their hand
                _unitOfWork.HandCardRepository.Delete(card);

                // Add the spoon to their hand
                _unitOfWork.HandCardRepository.Insert(new HandCard
                {
                    CardType = CardType.Spoon,
                    GameId = player.GameId,
                    HandId = card.HandId
                });
            }
            // Remove the spoon played
            _unitOfWork.BoardCardRepository.Delete(playAfterTurnDTO.BoardCardId);
            await _unitOfWork.Save();
        }
    }
}
