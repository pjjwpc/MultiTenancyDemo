using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiTenancyDemo.Uow
{
    public class MultiTenancyDemoUnitOfWork:IMultiTenancyDemoUnitOfWork
    {
        /// <summary>
        /// 当前活动的DbContext
        /// </summary>
        private Dictionary<string,DbContext> ActiveDbContext;
        //private readonly 

        public MultiTenancyDemoUnitOfWork()
        {
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
        {
            if(TenantId.HasValue)
            {

            }else
            {

            }
            return default(TDbContext);
        }
    }
}