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
    public class SashimiCommand : ICardCommand<Sashimi>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        public ClaimsPrincipal? User { get; set; }

        public SashimiCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard)
        {
            _unitOfWork = unitOfWork;
            _simpleAddToBoard = simpleAddToBoard;
        }

        public async Task CalculateEndRound(BoardCard boardCard)
        {
            if (User == null) throw new EntityNotFoundException(nameof(ClaimsPrincipal));
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.BoardId == boardCard.BoardId && x.CardType == CardType.Tempura && !x.IsCalculated,
                    transform: x => x.AsNoTracking()
                );
            if (!cards.Any()) return;
            var player = _unitOfWork.PlayerRepository.Get(
                        filter: x => x.BoardId == boardCard.BoardId,
                        transform: x => x.AsNoTracking()
                        ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(UramakiCommand));
            if (player == null) throw new EntityNotFoundException(nameof(player));
            var points = cards.Count() / 3 * 10;
            player.Points += points;
            _unitOfWork.PlayerRepository.Update(player);
            foreach (var card in cards)
            {
                card.IsCalculated = true;
                _unitOfWork.BoardCardRepository.Update(card);
            }
            await _unitOfWork.Save();
        }

        public async Task OnPlay(Player player, PlayCardDTO playCardDTO)
        {
            await _simpleAddToBoard.AddToBoard(_unitOfWork, playCardDTO.HandCardId, player.BoardId, User);
        }
    }
}
