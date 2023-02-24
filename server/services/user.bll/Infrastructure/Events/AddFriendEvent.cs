using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Events
{
    public class AddFriendEvent : INotification
    {
        public Guid ReceiverId { get; set; }
        public required UserNameViewModel SenderUser { get; set; }
    }
}
