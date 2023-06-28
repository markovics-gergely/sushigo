using Microsoft.EntityFrameworkCore;
using shared.Data.Repository.Interfaces;

namespace shared.Data.Repository.Implementations
{
    public class DbContextProvider<TDbContext> : IDbContextProvider where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;

        public DbContextProvider(TDbContext dbContext) {
            _dbContext = dbContext;
        }

        public DbContext GetDbContext()
        {
            return _dbContext;
        }
    }
}
