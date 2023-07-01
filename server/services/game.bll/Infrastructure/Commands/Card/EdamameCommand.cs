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
    public class EdamameCommand : ICardCommand<Edamame>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        public ClaimsPrincipal? User { get; set; }

        public EdamameCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard)
        {
            _unitOfWork = unitOfWork;
            _simpleAddToBoard = simpleAddToBoard;
        }

        public async Task OnEndRound(BoardCard boardCard)
        {
            if (User == null) throw new EntityNotFoundException(nameof(ClaimsPrincipal));
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == boardCard.GameId,
                    transform: x => x.AsNoTracking()
                );
            if (!cards.Any()) return;
            var boards = cards
                .GroupBy(c => c.BoardId)
                .Select(c => new { c.Key, Value = c.Count() });
            if (boards.Count() > 1)
            {
                foreach ( var board in boards )
                {
                    var player = _unitOfWork.PlayerRepository.Get(
                        filter: x => x.BoardId == board.Key,
                        transform: x => x.AsNoTracking()
                        ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(EdamameCommand));
                    if (player == null) throw new EntityNotFoundException(nameof(player));
                    player.Points += board.Value * boards.Count() - 1;
                    _unitOfWork.PlayerRepository.Update(player);
                }
            }
            foreach (var card in cards)
            {
                card.IsCalculated = true;
                _unitOfWork.BoardCardRepository.Update(card);
            }
            await _unitOfWork.Save();
        }

        public async Task OnEndTurn(Player player, PlayCardDTO playCardDTO)
        {
            await _simpleAddToBoard.AddToBoard(_unitOfWork, playCardDTO.HandCardId, player.BoardId, User);
        }
    }
}
