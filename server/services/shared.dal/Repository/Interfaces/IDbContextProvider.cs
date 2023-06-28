using Microsoft.EntityFrameworkCore;

namespace shared.dal.Repository.Interfaces
{
    public interface IDbContextProvider
    {
        public DbContext GetDbContext();
    }
}
