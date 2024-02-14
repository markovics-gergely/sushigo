using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using shared.dal.Comparers;
using shared.dal.Converters;
using shared.dal.Models.Types;
using user.dal.Domain;

namespace user.dal
{
    public class UserDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
        public DbSet<ApplicationRole> ApplicationRoles => Set<ApplicationRole>();
        public DbSet<Friend> Friends => Set<Friend>();
        public DbSet<History> Histories => Set<History>();
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Tokens)
                .WithOne()
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Entity<ApplicationUser>()
                .Property(u => u.DeckClaims)
                .HasConversion<EnumCollectionJsonValueConverter<DeckType>>()
                .Metadata.SetValueComparer(new CollectionValueComparer<DeckType>());

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.UserRoles)
                .WithOne()
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Entity<Friend>()
                .HasOne(f => f.Sender)
                .WithMany(f => f.FriendRequested)
                .HasForeignKey(f => f.SenderId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Entity<Friend>()
                .HasOne(f => f.Receiver)
                .WithMany(f => f.FriendReceived)
                .HasForeignKey(f => f.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Avatar)
                .WithOne()
                .HasForeignKey<ApplicationUser>(a => a.AvatarId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false);

            builder.Entity<History>()
                .HasOne(h => h.User)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Entity<ApplicationRole>()
                .HasData(
                    new ApplicationRole
                    {
                        Id = new Guid("f4dac384-7da2-4201-a766-4bfb888a54db"),
                        Name = RoleTypes.Classic,
                        NormalizedName = RoleTypes.Classic.ToUpper(),
                        ConcurrencyStamp = "1",
                        Description = "Claim for the classic version of the game"
                    },
                    new ApplicationRole
                    {
                        Id = new Guid("f08ff6b1-cc3f-47f6-b800-95f6b9e29478"),
                        Name = RoleTypes.Party,
                        NormalizedName = RoleTypes.Party.ToUpper(),
                        ConcurrencyStamp = "2",
                        Description = "Claim for the party version of the game"
                    }
                );
        }
    }
}
