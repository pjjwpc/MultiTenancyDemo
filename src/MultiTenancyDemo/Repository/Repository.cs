using Microsoft.EntityFrameworkCore;

namespace MultiTenancyDemo.Repository
{
    public class Repository<TEntity>:IRepository<TEntity>
    {
        private readonly DbContext _dbContenxt;
        public Repository(DbContext dbContext)
        {
            this._dbContenxt=dbContext;
        }


    }
}