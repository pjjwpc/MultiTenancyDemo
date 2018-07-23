using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MultiTenancyDemo.Uow;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;
using MultiTenancyDemo.Data;
using Newtonsoft.Json;

namespace MultiTenancyDemo.Middleware
{
    public class MultiTenantMiddleware
    {
        private static Tenant defaultTenant=new Tenant()
        {
            Id=-1
        };
        private static IDictionary<string,Tenant> TenantIdDic=new Dictionary<string,Tenant>(); 
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        
        public MultiTenantMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<MultiTenantMiddleware>();
        }
        
        public async Task Invoke(HttpContext context,IMultiTenancyDemoUnitOfWork multiTenancyDemoUnitOfWork
                                ,CacheManager.Core.ICacheManager<Tenant> cacheManager)
        {
            string path=context.Request.Path;
            string url = context.Request.GetDisplayUrl();
            url=url.Remove(url.IndexOf(path));
            
            _logger.LogError(url);
            Tenant tenant;
            if(TenantIdDic.TryGetValue(url,out tenant))
            {
                multiTenancyDemoUnitOfWork.SetTenantInfo(tenant);
            }
            else
            {
             tenant=cacheManager.Get(url);
               if(tenant==null)
               {
                   TenantIdDic.TryAdd(url,defaultTenant);
               }
               else
               {
                   multiTenancyDemoUnitOfWork.SetTenantInfo(tenant);
                   TenantIdDic.TryAdd(url,tenant);
               }
            }
            _logger.LogError("开始识别租户");
            await _next.Invoke(context);
            _logger.LogError($"识别出租户信息\n TenantInfo:{JsonConvert.SerializeObject(tenant)}");
        }
    }
}