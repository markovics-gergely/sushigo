using System.Security.Claims;

namespace game.bll.Infrastructure.DataTransferObjects
{
    public class CreateJobDTO
    {
        public required string Name { get; set; }
        public required DateTime StartTime { get; set; }
        public required ClaimsPrincipal User { get; set; }
    }
}
