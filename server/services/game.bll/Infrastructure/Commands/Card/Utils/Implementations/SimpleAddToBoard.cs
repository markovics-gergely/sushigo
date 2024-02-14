using game.dal.Domain;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.dal.Models.Types;

namespace game.bll.Infrastructure.Commands.Card.Utils.Implementations
{
    public class SimpleAddToBoard : ISimpleAddToBoard
    {
        private readonly IUnitOfWork _unitOfWork;
        public SimpleAddToBoard(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddNigiriToBoard(Player player, HandCard handCard)
        {
            if (handCard == null) throw new EntityNotFoundException(nameof(handCard));
            if (player == null) throw new EntityNotFoundException(nameof(player));

            // Get free wasabi entity of the player if there is any
            var wasabis = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.BoardId == player.BoardId && x.CardInfo.CardType == CardType.Wasabi,
                    transform: x => x.AsNoTracking(),
                    includeProperties: nameof(BoardCard.CardInfo)
                ).ToList();

            // Create relationship between the board and card info
            var boardCard = new BoardCard
            {
                BoardId = player.BoardId,
                GameId = player.GameId,
                CardInfo = handCard.CardInfo,
            };

            // Remove card from the hand
            _unitOfWork.HandCardRepository.Delete(handCard);

            // Search for any unused wasabi
            var wasabi = wasabis.Where(x => x.CardInfo.CustomTag == null).FirstOrDefault();

            // If there was a wasabi to use
            if (wasabi != null)
            {
                // Set flag for the wasabi and the nigiri card
                wasabi.CardInfo.CustomTag = CardTagType.USED;
                boardCard.CardInfo.CustomTag = CardTagType.WASABI;
                _unitOfWork.CardInfoRepository.Update(wasabi.CardInfo);
                _unitOfWork.CardInfoRepository.Update(boardCard.CardInfo);
            }

            _unitOfWork.BoardCardRepository.Insert(boardCard);
            await _unitOfWork.Save();
        }

        public async Task AddToBoard(Player player, HandCard handCard)
        {
            if (handCard == null) throw new EntityNotFoundException(nameof(handCard));
            if (player == null) throw new EntityNotFoundException(nameof(player));

            // Create relationship between the board and card info
            var boardCard = new BoardCard
            {
                BoardId = player.BoardId,
                GameId = player.GameId,
                CardInfo = handCard.CardInfo,
            };

            // Remove card from the hand
            _unitOfWork.HandCardRepository.Delete(handCard);

            _unitOfWork.BoardCardRepository.Insert(boardCard);
            await _unitOfWork.Save();
        }
    }
}
