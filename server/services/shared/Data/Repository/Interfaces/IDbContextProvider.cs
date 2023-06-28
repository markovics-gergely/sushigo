using Microsoft.EntityFrameworkCore;

namespace shared.Data.Repository.Interfaces
{
    public interface IDbContextProvider
    {
        public DbContext GetDbContext();
    }
}
