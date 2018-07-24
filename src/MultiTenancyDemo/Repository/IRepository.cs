using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiTenancyDemo.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IRepository<TDbContext,TEntity,TKey>
    {
         TDbContext GetDbContext();

         IQueryable<TEntity> GetAll();

        bool Create(TEntity entity);

        Task<bool> CreateAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        bool Remove(TEntity entity);

        bool RemoveRange(IList<TEntity> entities);
    }
}