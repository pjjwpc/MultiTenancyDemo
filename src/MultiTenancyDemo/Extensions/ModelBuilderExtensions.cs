using Microsoft.EntityFrameworkCore;
using MultiTenancyDemo.Data;

namespace MultiTenancyDemo.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ConfigureMultiTenancyDbContext(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tenant>(tenant=>{
                tenant.ToTable("Tenants");
                tenant.HasKey("Id");
                tenant.Property<string>("Name").HasMaxLength(200);
                tenant.Property<string>("Connection").HasMaxLength(200);
                tenant.Property<TenantType>("TenantType");
                tenant.Property<TenantDbType>("TenantDbType");
                tenant.HasIndex("Name");
            });
            modelBuilder.Entity<Order>(order =>
            {
                order.Property<int>("Id").ValueGeneratedOnAdd();
                order.Property<int>("TenantId");
                order.Property<string>("OrderDes");
                order.HasKey(x => x.Id);
                order.Property<int>("UserId").IsRequired();
                //order.HasIndex("UserId","TenantId");
                order.ToTable("Orders");
                
            });
            modelBuilder.Entity<User>(user=>{
                user.ToTable("User");
                user.Property<int>("Id").ValueGeneratedOnAdd();
                user.Property<int>("TenantId");
                user.Property<string>("Name");
                user.Property<UserStatus>("Status");
                user.HasKey("Id");
                user.HasIndex("Name");
            });
            modelBuilder.Entity<Goods>(Goods=>{
                Goods.ToTable("Goods");
                Goods.Property<int>("Id");
                Goods.HasKey("Id");
                Goods.Property<string>("Name").HasMaxLength(200);
                Goods.Property<string>("Image");
                Goods.Property<int>("TenantId");
                Goods.Property<double>("Price");
                Goods.Property<GoodsStatus>("Status");
                Goods.HasIndex("Name");
                Goods.HasIndex("TenantId");
            });
            
        }
    }
}