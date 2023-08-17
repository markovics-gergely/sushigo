using shared.dal.Models;

namespace game.bll.Infrastructure.ViewModels
{
    public class HandCardViewModel
    {
        public Guid Id { get; set; }
        public CardType CardType { get; set; }
        public bool IsSelected { get; set; } = false;
        public Dictionary<string, string> AdditionalInfo { get; set; } = new Dictionary<string, string>();
    }
}
