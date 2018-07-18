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
                order.Property<int>("Id").ValueGeneratedOnAdd();
                order.Property<int>("TenancyId");
                order.Property<string>("OrderDes");
                order.HasKey(x => x.Id);
                order.Property<int>("UserId").IsRequired();
                order.HasIndex("UserId","TenancyId");
                order.ToTable("Order");
                order.HasOne(r=>r.User).WithMany(u=>u.Orders).HasForeignKey(s=>s.UserId);
            });
            modelBuilder.Entity<User>(user=>{
                user.ToTable("User");
                user.Property<int>("Id").ValueGeneratedOnAdd();
                user.Property<int>("TenancyId");
                user.Property<string>("Name");
                user.Property<UserStatus>("Status");
                user.HasKey("Id");
                user.HasIndex("Name");
                user.HasOne(r=>r.TenantInfo).WithOne(r=>r.User).HasForeignKey<User>(u=>u.TenancyId);
            });
            modelBuilder.Entity<Goods>(Goods=>{
                Goods.ToTable("Goods");
                Goods.Property<int>("Id");
                Goods.HasKey("Id");
                Goods.Property<string>("Name");
                Goods.Property<string>("Image");
                Goods.Property<int>("TenancyId");
                Goods.Property<double>("Price");
                Goods.Property<GoodsStatus>("Status");
                Goods.HasIndex("Name");
            });
            modelBuilder.Entity<TenantInfo>(tenant=>{
                tenant.ToTable("TenantInfo");
                tenant.HasKey("Id");
                tenant.Property<string>("Name");
                tenant.Property<string>("Connection");
                tenant.Property<TenantType>("TenantType");
                tenant.Property<TenantDbType>("TenantDbType");
                tenant.HasIndex("Name");
            });
        }
    }
}