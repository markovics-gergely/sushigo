using game.bll.Infrastructure.Commands.Card.Utils;
using game.bll.Infrastructure.DataTransferObjects;
using game.dal.Domain;
using game.dal.Types;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card
{
    public class ChopSticksCommand : ICardCommand<Chopsticks>
    {
        public ClaimsPrincipal? User { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;

        public ChopSticksCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard)
        {
            _unitOfWork = unitOfWork;
            _simpleAddToBoard = simpleAddToBoard;
        }

        public async Task OnEndRound(BoardCard boardCard)
        {
            // Set calculated flag for the card
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
            // Find card entity to play from the hand
            var replace = _unitOfWork.HandCardRepository.Get(
                    filter: x => x.HandId == player.HandId && x.Id == Guid.Parse(playAfterTurnDTO.AdditionalInfo[Additional.Tagged]),
                    transform: x => x.AsNoTracking()
                    ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(ChopSticksCommand));
            if (replace == null) throw new EntityNotFoundException(nameof(replace));

            // Delete the card from the hand end the chopsticks card from the board
            _unitOfWork.HandCardRepository.Delete(replace.Id);
            _unitOfWork.BoardCardRepository.Delete(playAfterTurnDTO.BoardCardId);

            // Add replacable card to the board
            var boardCard = new BoardCard
            {
                CardType = replace.CardType,
                BoardId = player.BoardId,
                GameId = player.GameId,
                AdditionalInfo = replace.AdditionalInfo,
            };
            _unitOfWork.BoardCardRepository.Insert(boardCard);

            // Get chopsticks card back to the players hand
            var handCard = new HandCard
            {
                CardType = CardType.Chopsticks,
                HandId = player.HandId,
                GameId = player.GameId,
            };
            _unitOfWork.HandCardRepository.Insert(handCard);
            await _unitOfWork.Save();
        }
    }
}
