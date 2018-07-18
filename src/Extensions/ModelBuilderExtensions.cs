using Microsoft.EntityFrameworkCore;
using MultiTenancyDemo.Data;

namespace MultiTenancyDemo.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ConfigureMultiTenancyDbContext(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(order =>
            {
                order.HasKey(x => x.Id);
                order.HasOne<User>(u => u.User).WithOne("UserId");
            });
        }
    }
}