using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using MultiTenancyDemo.Data;
using CacheManager.Core;
using Microsoft.Extensions.DependencyInjection;

namespace MultiTenancyDemo.Uow
{
    public class MultiTenancyDemoUnitOfWork:IMultiTenancyDemoUnitOfWork
    {
        private readonly ICacheManager<Tenant> _cacheManager;

        /// <summary>
        /// 当前活动的DbContext
        /// </summary>
        public IDictionary<string,DbContext> ActiveDbContext;

        private readonly IServiceCollection _serviceCollection;
        
        //private readonly 

        public MultiTenancyDemoUnitOfWork(ICacheManager<Tenant> cacheManager,IServiceCollection service)
        {
            this._cacheManager = cacheManager;
            this._serviceCollection=service;
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
        public TDbContext GetDbContext<TDbContext>(MultiTenantType? multiTenantType)
                where TDbContext:DbContext
        {
            DbContext dbContext;
            if(multiTenantType.HasValue&&multiTenantType.Value== MultiTenantType.Tenant
                && TenantId.HasValue&&TenantId>0)
            {
              Tenant tenant=  _cacheManager.Get(TenantId.ToString());
              var dbOptionBuilder=new DbContextOptionsBuilder<TDbContext>();
              var dbOptions= dbOptionBuilder.UseMySql(tenant.Connection);
               dbContext=  new DbContext(dbOptions.Options);
               ActiveDbContext[tenant.Connection]=dbContext;
            }
              
            
            else
            {
               dbContext= _serviceCollection.BuildServiceProvider().GetService<DbContext>();
              // ActiveDbContext[]
            }
            return (TDbContext)dbContext;
        }
    }
}