using Microsoft.AspNetCore.Identity;
using user.dal.Types;

namespace user.dal.Domain
{
    /// <summary>
    /// Users of the application
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public long Experience { get; set; } = 0;

        public ICollection<GameTypes> GameClaims { get; set; } = new List<GameTypes>();

        public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; } = new List<IdentityUserClaim<Guid>>();
        public virtual ICollection<IdentityUserLogin<Guid>> Logins { get; set; } = new List<IdentityUserLogin<Guid>>();
        public virtual ICollection<IdentityUserToken<Guid>> Tokens { get; set; } = new List<IdentityUserToken<Guid>>();
        public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; } = new List<IdentityUserRole<Guid>>();

        public virtual ICollection<Friend> FriendRequested { get; set; } = new List<Friend>();
        public virtual ICollection<Friend> FriendReceived { get; set; } = new List<Friend>();

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
