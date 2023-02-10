using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using user.dal.Comparers;
using user.dal.Converters;
using user.dal.Domain;
using user.dal.Types;

namespace user.dal
{
    public class UserDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
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
                .Property(u => u.GameClaims)
                .HasConversion<EnumCollectionJsonValueConverter<GameTypes>>()
                .Metadata.SetValueComparer(new CollectionValueComparer<GameTypes>());

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.UserRoles)
                .WithOne()
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Entity<Friend>()
                .HasOne(f => f.Sender)
                .WithMany()
                .HasForeignKey(f => f.SenderId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Entity<Friend>()
                .HasOne(f => f.Receiver)
                .WithMany()
                .HasForeignKey(f => f.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
}
