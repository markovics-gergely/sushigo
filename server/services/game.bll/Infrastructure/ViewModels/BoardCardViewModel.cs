using game.dal.Types;
using shared.dal.Models;

namespace game.bll.Infrastructure.ViewModels
{
    public class BoardCardViewModel
    {
        public Guid Id { get; set; }
        public CardType CardType { get; set; }
        public Dictionary<Additional, string> AdditionalInfo { get; set; } = new Dictionary<Additional, string>();
    }
}
