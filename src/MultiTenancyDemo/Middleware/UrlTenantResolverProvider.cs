using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using MultiTenancyDemo.Data;

namespace MultiTenancyDemo.Middleware
{
    public class UrlTenantResolverProvider : ITenantResolverProvider
    {
        private static Tenant defaultTenant=new Tenant()
        {
            Id=-1
        };
        private CacheManager.Core.ICacheManager<Tenant> _cacheManager;
        public UrlTenantResolverProvider(CacheManager.Core.ICacheManager<Tenant> cacheManager)
        {
            _cacheManager=cacheManager;
        }

        private static IDictionary<string,Tenant> TenantIdDic=new Dictionary<string,Tenant>(); 

        public Tenant GetTenant(HttpContext context)
        {
            string url = context.Request.Host.Host;
            Tenant tenant;
            if(TenantIdDic.TryGetValue(url,out tenant))
            {
                return tenant;
            }
            else
            {
               tenant = _cacheManager.Get(url);
               if(tenant==null)
               {
                   tenant=defaultTenant;
               }
               
               TenantIdDic.TryAdd(url,tenant);
               
               return tenant;
            }
        }
    }
}