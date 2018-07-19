using Microsoft.EntityFrameworkCore;

namespace MultiTenancyDemo
{
    public interface  IDbContextProvider<out TDbContext>
    where TDbContext:DbContext
    {
        TDbContext GetDbContext();
    }
}