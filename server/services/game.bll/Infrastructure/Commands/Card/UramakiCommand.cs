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
    public class UramakiCommand : ICardCommand<Uramaki>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISimpleAddToBoard _simpleAddToBoard;
        public ClaimsPrincipal? User { get; set; }

        public UramakiCommand(IUnitOfWork unitOfWork, ISimpleAddToBoard simpleAddToBoard)
        {
            _unitOfWork = unitOfWork;
            _simpleAddToBoard = simpleAddToBoard;
        }

        public async Task CalculateEndRound(BoardCard boardCard)
        {
            if (User == null) throw new EntityNotFoundException(nameof(ClaimsPrincipal));
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.GameId == boardCard.GameId && x.CardType == CardType.Uramaki && !x.IsCalculated,
                    transform: x => x.AsNoTracking()
                );
            if (!cards.Any()) return;
            var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == User.GetGameIdFromJwt()
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(UramakiCommand));
            if (game == null) throw new EntityNotFoundException(nameof(game));
            game.AdditionalInfo.TryGetValue("uramaki", out string? uramakiCount);
            var uramaki = int.Parse(uramakiCount ?? "0");
            if (uramaki < 3)
            {
                var boards = cards.GroupBy(c => c.BoardId)
                    .OrderByDescending(c => c.Select(cc => int.Parse(cc.AdditionalInfo["uramaki"])).Sum())
                    .Take(3 - uramaki)
                    .Select(c => c.Key);
                foreach (var board in boards)
                {
                    var player = _unitOfWork.PlayerRepository.Get(
                        filter: x => x.BoardId == board,
                        transform: x => x.AsNoTracking()
                        ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(UramakiCommand));
                    if (player == null) throw new EntityNotFoundException(nameof(player));
                    var point = uramaki switch
                    {
                        0 => 8,
                        1 => 5,
                        2 => 2,
                        _ => 0,
                    };
                    uramaki++;
                    player.Points += point;
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

        public async Task OnPlay(Player player, PlayCardDTO playCardDTO)
        {
            if (User == null) throw new EntityNotFoundException(nameof(ClaimsPrincipal));
            var handCard = _unitOfWork.HandCardRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == playCardDTO.HandCardId
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(UramakiCommand));
            if (handCard == null) throw new EntityNotFoundException(nameof(handCard));
            var cards = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.BoardId == player.BoardId && x.CardType == CardType.Uramaki,
                    transform: x => x.AsNoTracking()
                );
            var points = cards.Select(c => int.Parse(c.AdditionalInfo["point"])).Sum() + int.Parse(handCard.AdditionalInfo["point"]);
            if (points >= 10)
            {
                var game = _unitOfWork.GameRepository.Get(
                    transform: x => x.AsNoTracking(),
                    filter: x => x.Id == User.GetGameIdFromJwt()
                ).FirstOrDefault() ?? throw new EntityNotFoundException(nameof(UramakiCommand));
                if (game == null) throw new EntityNotFoundException(nameof(game));
                game.AdditionalInfo.TryGetValue("uramaki", out string? uramakiCount);
                var uramaki = int.Parse(uramakiCount ?? "0");
                var point = int.Parse(uramakiCount ?? "0") switch
                {
                    0 => 8,
                    1 => 5,
                    2 => 2,
                    _ => 0,
                };
                player.Points += point;
                _unitOfWork.PlayerRepository.Update(player);
                foreach (var card in cards)
                {
                    _unitOfWork.BoardCardRepository.Delete(card);
                }
                _unitOfWork.HandCardRepository.Delete(handCard);
                game.AdditionalInfo["uramaki"] = (uramaki + 1).ToString();
                _unitOfWork.GameRepository.Update(game);
            } else
            {
                _unitOfWork.HandCardRepository.Delete(playCardDTO.HandCardId);
                var boardCard = new BoardCard
                {
                    CardType = handCard.CardType,
                    BoardId = player.BoardId,
                    GameId = User.GetGameIdFromJwt()
                };
                _unitOfWork.BoardCardRepository.Insert(boardCard);
            }
            await _unitOfWork.Save();
        }
    }
}
