using MediatR;
using System.Security.Claims;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Queries
{
    public class GetUserQuery : IRequest<UserViewModel>
    {
        public string? Id { get; set; }
        public ClaimsPrincipal? User { get; set; }

        public GetUserQuery(string? id, ClaimsPrincipal? user = null)
        {
            Id = id;
            User = user;
        }
    }
}
