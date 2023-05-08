using MediatR;
using user.bll.Infrastructure.ViewModels;

namespace user.bll.Infrastructure.Queries
{
    public class GetUserByIdQuery : IRequest<UserViewModel>
    {
        public string Id { get; set; }

        public GetUserByIdQuery(string id)
        {
            Id = id;
        }
    }
}
