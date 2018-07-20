using Microsoft.EntityFrameworkCore;

namespace MultiTenancyDemo.Repository
{
    public class Repository<TDbContext,TEntity,TKey>:IRepository<TDbContext,TEntity,TKey>
    where TDbContext:DbContext
    {
        private readonly IDbContextProvider<TDbContext> _dbContenxtProvider;
        public Repository(IDbContextProvider<TDbContext> dbContenxtProvider)
        {
            this._dbContenxtProvider=dbContenxtProvider;
        }


    }
}