using game.bll.Infrastructure.Commands.Card.Abstract;
using game.bll.Infrastructure.DataTransferObjects;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.bll.Extensions;
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
            boardCard.IsCalculated = true;
            _unitOfWork.BoardCardRepository.Update(boardCard);
            await _unitOfWork.Save();
        }

        public async Task OnEndTurn(Player player, PlayCardDTO playCardDTO)
        {
            await _simpleAddToBoard.AddToBoard(_unitOfWork, playCardDTO.HandCardId, player.BoardId, User);
        }

        public async Task OnAfterTurn(Player player, PlayAfterTurnDTO playAfterTurnDTO)
        {
            var replace = _unitOfWork.HandCardRepository.Get(
                    filter: x => x.HandId == player.HandId && x.Id == Guid.Parse(playAfterTurnDTO.AdditionalInfo["chopsticks"]),
                    transform: x => x.AsNoTracking()
                    ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(ChopSticksCommand));
            if (replace == null) throw new EntityNotFoundException(nameof(replace));
            _unitOfWork.HandCardRepository.Delete(replace.Id);
            _unitOfWork.BoardRepository.Delete(playAfterTurnDTO.BoardCardId);
            var boardCard = new BoardCard
            {
                CardType = replace.CardType,
                BoardId = player.BoardId,
                GameId = player.GameId,
                AdditionalInfo = replace.AdditionalInfo,
            };
            var handCard = new HandCard
            {
                CardType = CardType.Chopsticks,
                HandId = player.HandId,
                GameId = player.GameId,
            };
            _unitOfWork.BoardCardRepository.Insert(boardCard);
            _unitOfWork.HandCardRepository.Insert(handCard);
            await _unitOfWork.Save();
        }
    }
}
