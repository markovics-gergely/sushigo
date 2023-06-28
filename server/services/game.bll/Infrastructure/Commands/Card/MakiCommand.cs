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
    public class MakiCommand : ICardCommand<MakiRoll>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        public ClaimsPrincipal? User { get; set; }

        public MakiCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard)
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
                .OrderByDescending(c => c.Select(cc => int.Parse(cc.AdditionalInfo["maki"])).Sum())
                .Take(2)
                .Select(c => c.Key);
            var point = 6;
            foreach (var board in boards) {
                var player = _unitOfWork.PlayerRepository.Get(
                        filter: x => x.BoardId == board,
                        transform: x => x.AsNoTracking()
                        ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(UramakiCommand));
                if (player == null) throw new EntityNotFoundException(nameof(player));
                player.Points += point;
                _unitOfWork.PlayerRepository.Update(player);
                point -= 3;
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
