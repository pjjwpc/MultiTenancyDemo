using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MultiTenancyDemo.Uow;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;
using MultiTenancyDemo.Data;
using Newtonsoft.Json;
using CacheManager.Core;

namespace MultiTenancyDemo.Middleware
{
    public class MultiTenantMiddleware
    {
        private static Tenant defaultTenant=new Tenant()
        {
            Id=-1
        };
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        
        public MultiTenantMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<MultiTenantMiddleware>();
        }
        
        public async Task Invoke(HttpContext context,IMultiTenancyDemoUnitOfWork multiTenancyDemoUnitOfWork
                                ,ICacheManager<Tenant> cacheManager,ITenantResolverProvider tenantResolverProvider)
        {
            Tenant tenant=tenantResolverProvider.GetTenant(context);
            tenant=tenant??defaultTenant;
            multiTenancyDemoUnitOfWork.SetTenantInfo(tenant);
            await _next.Invoke(context);
            _logger.LogError($"识别出租户信息\n TenantInfo:{JsonConvert.SerializeObject(tenant)}");
        }
    }
}