using game.bll.Infrastructure.Commands.Card.Abstract;
using game.bll.Infrastructure.DataTransferObjects;
using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.dal.Models;
using System.Security.Claims;

namespace game.bll.Infrastructure.Commands.Card
{
    public class TeaCommand : ICardCommand<Tea>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        public ClaimsPrincipal? User { get; set; }

        public TeaCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard)
        {
            _unitOfWork = unitOfWork;
            _simpleAddToBoard = simpleAddToBoard;
        }

        public async Task OnEndRound(BoardCard boardCard)
        {
            if (User == null) throw new EntityNotFoundException(nameof(ClaimsPrincipal));
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.BoardId == boardCard.BoardId,
                    transform: x => x.AsNoTracking()
                );
            if (!cards.Any()) return;
            var nigiriCount = cards.Where(c => c.CardType.SushiType() == SushiType.Nigiri).Count();
            var sushiCount = cards
                .GroupBy(c => c.CardType)
                .Select(c => c.Count())
                .OrderByDescending(c => c)
                .FirstOrDefault();
            var point = new int[] { sushiCount, nigiriCount }.Max();
            var player = _unitOfWork.PlayerRepository.Get(
                        filter: x => x.BoardId == boardCard.BoardId,
                        transform: x => x.AsNoTracking()
                        ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(TeaCommand));
            if (player == null) throw new EntityNotFoundException(nameof(player));
            foreach (var card in cards.Where(c => c.CardType == CardType.Tea && !c.IsCalculated))
            {
                card.IsCalculated = true;
                _unitOfWork.BoardCardRepository.Update(card);
                player.Points += point;
            }
            _unitOfWork.PlayerRepository.Update(player);
            await _unitOfWork.Save();
        }

        public async Task OnEndTurn(Player player, PlayCardDTO playCardDTO)
        {
            await _simpleAddToBoard.AddToBoard(_unitOfWork, playCardDTO.HandCardId, player.BoardId, User);
        }
    }
}
