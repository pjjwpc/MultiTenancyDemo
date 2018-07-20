using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using MultiTenancyDemo.Data;
using CacheManager.Core;

namespace MultiTenancyDemo.Uow
{
    public class MultiTenancyDemoUnitOfWork:IMultiTenancyDemoUnitOfWork
    {
        private readonly ICacheManager<Tenant> _cacheManager;

        /// <summary>
        /// 当前活动的DbContext
        /// </summary>
        public Dictionary<string,DbContext> ActiveDbContext;
        //private readonly 

        public MultiTenancyDemoUnitOfWork(ICacheManager<Tenant> cacheManager)
        {
            this._cacheManager = cacheManager;

            ActiveDbContext=new Dictionary<string,DbContext>();
        }

        public int? TenantId{get;set;}

        /// <summary>
        /// 设置租户ID
        /// </summary>
        /// <param name="tenantId"></param>
        public void SetTenanId(int? tenantId)
        {
            TenantId=tenantId;
        }

        public int? GetTenanId()
        {
            return TenantId;
        }

        /// <summary>
        /// 保存当次变更-同步
        /// </summary>
        public void SaveChanges()
        {
            foreach(var dbContext in ActiveDbContext.Values)
            {
                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 保存当次变更-异步
        /// </summary>
        /// <returns></returns>
        public async Task SaveChangesAsync()
        {
            foreach(var dbContext in ActiveDbContext.Values)
            {
                await dbContext.SaveChangesAsync();
            }
        }
        /// <summary>
        /// 根据租户信息创建DbContext
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <returns></returns>
        public TDbContext GetDbContext<TDbContext>()
                where TDbContext:DbContext
        {
            if(TenantId.HasValue)
            {
              Tenant tenant=  _cacheManager.Get(TenantId.ToString());
               //new DbContext(new DbContextOptions<TDbContext>().);
            }
            else
            {

            }
            return default(TDbContext);
        }
    }
}