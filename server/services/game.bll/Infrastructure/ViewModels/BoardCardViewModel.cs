using shared.dal.Models;

namespace game.bll.Infrastructure.ViewModels
{
    public class BoardCardViewModel
    {
        public Guid Id { get; set; }
        public CardType CardType { get; set; }
        public Dictionary<string, string> AdditionalInfo { get; set; } = new Dictionary<string, string>();
    }
}
