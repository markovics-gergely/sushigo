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
    public class TemakiCommand : ICardCommand<Temaki>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        public ClaimsPrincipal? User { get; set; }

        public TemakiCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard)
        {
            _unitOfWork = unitOfWork;
            _simpleAddToBoard = simpleAddToBoard;
        }

        public async Task CalculateEndRound(BoardCard boardCard)
        {
            if (User == null) throw new EntityNotFoundException(nameof(ClaimsPrincipal));
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == boardCard.GameId && x.CardType == CardType.MakiRoll && !x.IsCalculated,
                    transform: x => x.AsNoTracking()
                );
            if (!cards.Any()) return;
            var boards = cards
                .GroupBy(c => c.BoardId)
                .Select(c => new { c.Key, Value = c.Select(cc => int.Parse(cc.AdditionalInfo["temaki"])).Sum() });
            var maxCount = boards.MaxBy(b => b.Value)?.Value;
            var minCount = boards.MinBy(b => b.Value)?.Value;
            var maxBoards = boards.Where(b => b.Value == maxCount).Select(b => b.Key).ToList();
            var minBoards = boards.Where(b => b.Value == minCount).Select(b => b.Key).ToList();
            var minPlayers = _unitOfWork.PlayerRepository.Get(
                        filter: x => minBoards.Contains(x.BoardId),
                        transform: x => x.AsNoTracking()
                        ).ToList();
            var maxPlayers = _unitOfWork.PlayerRepository.Get(
                        filter: x => maxBoards.Contains(x.BoardId),
                        transform: x => x.AsNoTracking()
                        ).ToList();
            foreach (var player in maxPlayers)
            {
                player.Points += 4;
                _unitOfWork.PlayerRepository.Update(player);
            }
            foreach (var player in minPlayers)
            {
                player.Points -= 4;
                _unitOfWork.PlayerRepository.Update(player);
            }
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
