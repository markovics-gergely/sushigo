using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace user.bll.Infrastructure.ViewModels
{
    public class FriendListViewModel
    {
        public IEnumerable<UserNameViewModel> Friends { get; set; } = new List<UserNameViewModel>();
        public IEnumerable<UserNameViewModel> Sent { get; set; } = new List<UserNameViewModel>();
        public IEnumerable<UserNameViewModel> Received { get; set; } = new List<UserNameViewModel>();
    }
}
