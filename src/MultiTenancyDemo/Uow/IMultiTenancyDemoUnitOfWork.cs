using System.Threading.Tasks;

namespace MultiTenancyDemo.Uow
{
    public interface IMultiTenancyDemoUnitOfWork
    {
        void SaveChanges();

        Task SaveChangesAsync();

        TDbContext GetDbContext<TDbContext>();
    }
}