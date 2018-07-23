using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MultiTenancyDemo.Data;

namespace MultiTenancyDemo.Uow
{
    public interface IMultiTenancyDemoUnitOfWork
    {
        Tenant GetTenant();

        void SetTenantInfo(Tenant tenant);

        void SaveChanges();

        Task SaveChangesAsync();

        TDbContext GetDbContext<TDbContext>(MultiTenantType? multiTenantType) where TDbContext : DbContext;
    }
}