using Microsoft.EntityFrameworkCore;
using MultiTenancyDemo.Extensions;

namespace MultiTenancyDemo.Data
{
    /// <inheritdoc />
    /// <summary>
    /// 多租户Demo
    /// </summary>
    public class MultiTenancyDbContext:DbContext
    {
        public DbSet<User> User{get;set;}

        public DbSet<Goods> Goods{get;set;}

        public DbSet<Order> Order{get;set;}
        
        public DbSet<Tenant> Tenant { get; set; }
        
        public MultiTenancyDbContext(DbContextOptions<MultiTenancyDbContext> options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureMultiTenancyDbContext();
        }
    }
}