using Microsoft.EntityFrameworkCore;
using shared.dal.Repository.Interfaces;

namespace shared.dal.Repository.Implementations
{
    public class DbContextProvider<TDbContext> : IDbContextProvider where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;

        public DbContextProvider(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbContext GetDbContext()
        {
            return _dbContext;
        }
    }
}
