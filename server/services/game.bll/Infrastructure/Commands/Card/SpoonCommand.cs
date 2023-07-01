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

        public async Task OnEndTurn(Player player, PlayCardDTO playCardDTO)
        {
            await _simpleAddToBoard.AddToBoard(_unitOfWork, playCardDTO.HandCardId, player.BoardId, User);
        }

        public async Task OnAfterTurn(Player player, PlayAfterTurnDTO playAfterTurnDTO)
        {
            var searchType = Enum.Parse<CardType>(playAfterTurnDTO.AdditionalInfo["spoon"]);
            var hands = _unitOfWork.HandCardRepository.Get(
                    filter: x => x.GameId == player.GameId,
                    transform: x => x.AsNoTracking()
                    );
            var groupped = new LinkedList<IGrouping<Guid, HandCard>>(hands.GroupBy(h => h.HandId));
            var ownNode = groupped.Find(groupped.First(g => g.Key == player.HandId))
                ?? throw new EntityNotFoundException(nameof(HandCard));
            var actualNode = ownNode.Previous ?? groupped.Last
                ?? throw new EntityNotFoundException(nameof(HandCard));
            while (actualNode != ownNode)
            {
                if (actualNode.Value.Any(a => a.CardType == searchType)) break;
                actualNode = actualNode.Previous ?? groupped.Last
                    ?? throw new EntityNotFoundException(nameof(HandCard));
            }
            if (actualNode != ownNode)
            {
                var card = actualNode.Value.First(c => c.CardType == searchType);
                var boardCard = new BoardCard
                {
                    CardType = card.CardType,
                    GameId = card.GameId,
                    BoardId = player.BoardId,
                    AdditionalInfo = card.AdditionalInfo,
                };
                _unitOfWork.BoardCardRepository.Insert(boardCard);
                _unitOfWork.HandCardRepository.Delete(card);
                _unitOfWork.HandCardRepository.Insert(new HandCard
                {
                    CardType = CardType.Spoon,
                    GameId = player.GameId,
                    HandId = card.HandId
                });
            }
            _unitOfWork.BoardRepository.Delete(playAfterTurnDTO.BoardCardId);
            await _unitOfWork.Save();
        }
    }
}
