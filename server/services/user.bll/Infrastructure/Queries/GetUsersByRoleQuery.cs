using MediatR;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Queries
{
    public class GetUsersByRoleQuery : IRequest<IEnumerable<UserNameViewModel>>
    {
        public IEnumerable<string> Roles { get; set; }

        public GetUsersByRoleQuery(IEnumerable<string> roles)
        {
            Roles = roles;
        }
    }
}
