using game.bll.Infrastructure.Commands.Card.Utils;
using game.bll.Infrastructure.DataTransferObjects;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.dal.Models;

namespace game.bll.Infrastructure.Commands.Card
{
    /// <summary>
    /// Command class to execute chopsticks card functions
    /// </summary>
    public class ChopSticksCommand : ICardCommand<Chopsticks>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        private readonly INoModification _noModification;

        public ChopSticksCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard, INoModification noModification)
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
            // Find card entity to play from the hand
            var replace = _unitOfWork.HandCardRepository.Get(
                    filter: x => x.HandId == player.HandId && x.Id == playAfterTurnDTO.TargetCardId,
                    transform: x => x.AsNoTracking(),
                    includeProperties: nameof(HandCard.CardInfo)
            ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(HandCard));

            // Add replacable card to the board
            var boardCard = new BoardCard
            {
                BoardId = player.BoardId,
                GameId = player.GameId,
                CardInfo = replace.CardInfo,
            };
            _unitOfWork.BoardCardRepository.Insert(boardCard);

            // Delete the card from the hand
            _unitOfWork.HandCardRepository.Delete(replace.Id);

            // Get chopsticks card entity from the hand
            var chopsticks = _unitOfWork.HandCardRepository.Get(
                filter: x => x.Id == playAfterTurnDTO.HandOrBoardCardId,
                transform: x => x.AsNoTracking(),
                includeProperties: nameof(HandCard.CardInfo)
            ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(HandCard));

            // Get chopsticks card back to the players hand
            var handCard = new HandCard
            {
                HandId = player.HandId,
                GameId = player.GameId,
                CardInfo = chopsticks.CardInfo,
            };

            _unitOfWork.BoardCardRepository.Delete(chopsticks);
            _unitOfWork.HandCardRepository.Insert(handCard);

            await _unitOfWork.Save();
        }
    }
}
