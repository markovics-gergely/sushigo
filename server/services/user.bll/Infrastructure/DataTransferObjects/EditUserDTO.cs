using Microsoft.AspNetCore.Http;

namespace user.bll.Infrastructure.DataTransferObjects
{
    public class EditUserDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public IFormFile? Avatar { get; set; }
    }
}
