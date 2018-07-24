using Microsoft.AspNetCore.Http;

namespace MultiTenancyDemo.Middleware
{
    public interface ITenantResolverProvider
    {
         MultiTenancyDemo.Data.Tenant GetTenant(HttpContext context);
    }
}