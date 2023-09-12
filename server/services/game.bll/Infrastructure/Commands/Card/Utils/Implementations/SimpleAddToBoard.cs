using game.dal.Domain;
using game.dal.Types;
using game.dal.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using shared.bll.Exceptions;
using shared.dal.Models;

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
            var wasabi = _unitOfWork.BoardCardRepository.Get(
                    filter: x => x.BoardId == player.BoardId && x.CardType == CardType.Wasabi,
                    transform: x => x.AsNoTracking()
                ).FirstOrDefault();
            
            // Remove card from the hand
            _unitOfWork.HandCardRepository.Delete(handCard);

            // Create the card to add to the board
            var boardCard = new BoardCard
            {
                CardType = handCard.CardType,
                BoardId = player.BoardId,
                GameId = player.GameId,
                AdditionalInfo = handCard.AdditionalInfo,
            };

            // If there was a wasabi to use
            if (wasabi != null && !wasabi.AdditionalInfo.ContainsKey(Additional.Tagged))
            {
                // Set flag for the wasabi and the nigiri card
                wasabi.AdditionalInfo.TryAdd(Additional.Tagged, "used");
                boardCard.AdditionalInfo.TryAdd(Additional.Tagged, "added");
                _unitOfWork.BoardCardRepository.Update(wasabi);
            }

            _unitOfWork.BoardCardRepository.Insert(boardCard);
            await _unitOfWork.Save();
        }

        public async Task AddToBoard(Player player, HandCard handCard)
        {
            if (handCard == null) throw new EntityNotFoundException(nameof(handCard));
            if (player == null) throw new EntityNotFoundException(nameof(player));

            // Remove card from the hand
            _unitOfWork.HandCardRepository.Delete(handCard);

            // Create the card to add to the board
            var boardCard = new BoardCard
            {
                CardType = handCard.CardType,
                BoardId = player.BoardId,
                GameId = player.GameId,
                AdditionalInfo = handCard.AdditionalInfo,
            };
            _unitOfWork.BoardCardRepository.Insert(boardCard);
            await _unitOfWork.Save();
        }
    }
}
