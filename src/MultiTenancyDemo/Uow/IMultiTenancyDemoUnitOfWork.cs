using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MultiTenancyDemo.Data;

namespace MultiTenancyDemo.Uow
{
    public interface IMultiTenancyDemoUnitOfWork
    {
        void SaveChanges();

        Task SaveChangesAsync();

        TDbContext GetDbContext<TDbContext>(MultiTenantType? multiTenantType) where TDbContext : DbContext;
    }
}