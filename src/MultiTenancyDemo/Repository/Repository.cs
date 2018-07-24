using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;


namespace MultiTenancyDemo.Repository
{
    public class Repository<TDbContext,TEntity,TKey>:
        IRepository<TDbContext,TEntity,TKey>
        where TDbContext:DbContext 
        where TEntity : class
    {
        private readonly IDbContextProvider<TDbContext> _dbContenxtProvider;
        public Repository(IDbContextProvider<TDbContext> dbContenxtProvider)
        {
            this._dbContenxtProvider=dbContenxtProvider;
        }

        public virtual TDbContext DbContext=>_dbContenxtProvider.GetDbContext();
        public virtual DbSet<TEntity> Table=> DbContext.Set<TEntity>();

        public virtual TDbContext GetDbContext()
        {
            return _dbContenxtProvider.GetDbContext();
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return Table.AsQueryable<TEntity>();
        }

        public virtual bool Create(TEntity entity)
        {
            Table.Add(entity);
            return true;
        }

        public Task<bool> CreateAsync(TEntity entity)
        {
            return Task.FromResult(Create(entity));
        }

        public Task UpdateAsync(TEntity entity)
        {
            Table.Attach(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}