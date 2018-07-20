using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MultiTenancyDemo.Uow
{
    public interface IMultiTenancyDemoUnitOfWork
    {
        void SaveChanges();

        Task SaveChangesAsync();

        TDbContext GetDbContext<TDbContext>() where TDbContext : DbContext;
    }
}