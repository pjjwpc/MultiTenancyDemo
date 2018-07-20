using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using MultiTenancyDemo.Uow;

namespace MultiTenancyDemo.DbContextProvider
{
    public class DbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
    where TDbContext : DbContext
    {
        private readonly IMultiTenancyDemoUnitOfWork _multiTenancyUnitOfWork;
        
        public DbContextProvider(IMultiTenancyDemoUnitOfWork multiTenancyUnitOfWork)
        {
            this._multiTenancyUnitOfWork=multiTenancyUnitOfWork;
        }

        /// <summary>
        /// 获取DbContext
        /// </summary>
        /// <returns></returns>
        public TDbContext GetDbContext()
        {
            return GetOrCreateDbContext();
        }

        /// <summary>
        /// 从当前的UnitWork中获取DbContext
        /// </summary>
        /// <returns></returns>
        private TDbContext GetOrCreateDbContext()
        {
            return _multiTenancyUnitOfWork.GetDbContext<TDbContext>(null);
        }
    }
}