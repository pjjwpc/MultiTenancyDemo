using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CacheManager.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MultiTenancyDemo.Data;

namespace MultiTenancyDemo.Uow 
{
    public class MultiTenancyDemoUnitOfWork : IMultiTenancyDemoUnitOfWork 
    {
        private readonly ICacheManager<Tenant> _cacheManager;

        private readonly MultiTenantType _multiTenantType;

        /// <summary>
        /// 当前活动的DbContext
        /// </summary>
        public IDictionary<string, DbContext> ActiveDbContext;

        private readonly IServiceProvider _serviceProvoider;

        //private readonly 

        public MultiTenancyDemoUnitOfWork(ICacheManager<Tenant> cacheManager,IServiceProvider serviceProvider,MultiTenantType multiTenantType) 
        {
            this._cacheManager = cacheManager;
            this._serviceProvoider = serviceProvider;
            this._multiTenantType=multiTenantType;
            ActiveDbContext = new Dictionary<string, DbContext> ();
        }

        public Tenant GetTenant()
        {
            return Tenant;
        }
        
        public MultiTenancyDemoUnitOfWork(Tenant tenant) 
        {
            this.Tenant = tenant;
               
        }
        public Tenant Tenant { get; set; }

        /// <summary>
        /// 设置租户ID
        /// </summary>
        /// <param name="tenantId"></param>
        public void SetTenantInfo (Tenant tenant) 
        {
            Tenant = tenant;
        }

        public Tenant GetTenantInfo () 
        {
            return Tenant;
        }

        /// <summary>
        /// 保存当次变更-同步
        /// </summary>
        public void SaveChanges ()
        {
            foreach (var dbContext in ActiveDbContext.Values) 
            {
               
                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 保存当次变更-异步
        /// </summary>
        /// <returns></returns>
        public async Task SaveChangesAsync () 
        {
            foreach (var dbContext in ActiveDbContext.Values) 
            {
                await dbContext.SaveChangesAsync ();
            }
        }
        /// <summary>
        /// 根据租户信息创建DbContext
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <returns></returns>
        public TDbContext GetDbContext<TDbContext> ()
            where TDbContext : DbContext
        {
            DbContext dbContext;
            if (Tenant!=null 
                && Tenant.TenantType== TenantType.有钱租户
                && Tenant.Id > 0) 
            {
                //暂时没有好的方法来通过容器改变参数的值，
                //就先用拿出DbContext赋值的方式变更数据库
                dbContext = _serviceProvoider.GetService<TDbContext>();
                dbContext.Database.GetDbConnection().ConnectionString=Tenant.Connection;
                
                ActiveDbContext[Tenant.Connection] = dbContext;
            } 
            else 
            {
                //当前应用程序类型不是多租户，或者当前用户是普通用户
                //从容器中拿数据库上下文，不需要创建
                dbContext = _serviceProvoider.GetService<TDbContext>();
                string connetcionString = dbContext.Database.GetDbConnection().ConnectionString;
                ActiveDbContext[connetcionString] = dbContext;
            }
            
            return (TDbContext) dbContext;
        }
    }
}