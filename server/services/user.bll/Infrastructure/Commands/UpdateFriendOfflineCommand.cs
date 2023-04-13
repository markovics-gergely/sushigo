using MediatR;

namespace user.bll.Infrastructure.Commands
{
    public class UpdateFriendOfflineCommand : IRequest
    {
        public string UserId { get; set; }
        public UpdateFriendOfflineCommand(string userId) {
            UserId = userId;
        }
    }
}
