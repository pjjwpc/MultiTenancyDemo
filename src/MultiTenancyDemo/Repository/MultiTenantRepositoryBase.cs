using System.Linq;
using MultiTenancyDemo.Data;

namespace MultiTenancyDemo.Repository
{
    public class MultiTenantRepositoryBase<TEntity> 
        : MultiTenantRepositoryBase<TEntity, int>,
          IMultiTenantRepositoryBase<TEntity>
          where TEntity:class
    {
        public MultiTenantRepositoryBase(IDbContextProvider<MultiTenancyDbContext> dbContenxtProvider) 
            : base(dbContenxtProvider)
        {
        }
    }
    
    public class MultiTenantRepositoryBase<TEntity, TKey> 
        : Repository<MultiTenancyDbContext, TEntity, TKey>, 
          IMultiTenantRepositoryBase<TEntity, TKey>
          where TEntity :class
    {
        public MultiTenantRepositoryBase(IDbContextProvider<MultiTenancyDbContext> dbContenxtProvider) 
            : base(dbContenxtProvider)
        {
            
        }

        public IQueryable<TEntity> GetAll()
        {
          return Table.AsQueryable<TEntity>();
        }
    }
}