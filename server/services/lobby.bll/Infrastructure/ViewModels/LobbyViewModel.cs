using lobby.dal.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lobby.bll.Infrastructure.ViewModels
{
    public class LobbyViewModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime Created { get; set; }
        public IEnumerable<PlayerViewModel> Players { get; set; } = new List<PlayerViewModel>();
        public IEnumerable<MessageViewModel> Messages { get; set; } = new List<MessageViewModel>();
    }
}
