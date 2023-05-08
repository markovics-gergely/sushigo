using lobby.dal.Domain;
using Microsoft.EntityFrameworkCore;

namespace lobby.dal
{
    public class LobbyDbContext : DbContext
    {
        public DbSet<Lobby> Lobbies => Set<Lobby>();
        public DbSet<Player> Players => Set<Player>();
        public DbSet<Message> Messages => Set<Message>();
        public LobbyDbContext(DbContextOptions<LobbyDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Player>()
                .HasOne(p => p.Lobby)
                .WithMany(l => l.Players)
                .HasForeignKey(p => p.LobbyId);

            builder.Entity<Message>()
                .HasOne(m => m.Lobby)
                .WithMany(l => l.Messages)
                .HasForeignKey(m => m.LobbyId);
        }
    }
}
