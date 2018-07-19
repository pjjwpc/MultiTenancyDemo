using Microsoft.EntityFrameworkCore;

namespace MultiTenancyDemo.DbContextProvider
{
    public class DbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
    where TDbContext : DbContext
    {
        public TDbContext DbContext;
        public DbContextProvider(TDbContext dbContext)
        {
            this.DbContext=dbContext;
        }

        public TDbContext GetDbContext()
        {
            return DbContext;
        }
    }
}