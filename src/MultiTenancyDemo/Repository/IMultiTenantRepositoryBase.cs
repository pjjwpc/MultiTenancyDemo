using MultiTenancyDemo.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MultiTenancyDemo.Repository
{
    public interface IMultiTenantRepositoryBase<TEntity>:IMultiTenantRepositoryBase<TEntity,int>
    {
        
    }

    public interface IMultiTenantRepositoryBase<TEntity,TKey>:IRepository<MultiTenancyDbContext,TEntity,TKey>
    {
    }
}