using MediatR;

namespace game.bll.Infrastructure.Commands
{
    public class UseJobCommand : IRequest
    {
        public long JobId { get; set; }

        public UseJobCommand(long jobId)
        {
            JobId = jobId;
        }
    }
}
