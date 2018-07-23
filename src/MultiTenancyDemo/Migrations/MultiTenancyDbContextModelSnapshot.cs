﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MultiTenancyDemo.Data;

namespace MultiTenancyDemo.Migrations
{
    [DbContext(typeof(MultiTenancyDbContext))]
    partial class MultiTenancyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("MultiTenancyDemo.Data.Goods", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Image");

                    b.Property<string>("Name")
                        .HasMaxLength(200);

                    b.Property<double>("Price");

                    b.Property<int>("Status");

                    b.Property<int>("TenantId");

                    b.Property<DateTime>("UpdateTime");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("TenantId");

                    b.HasIndex("UserId");

                    b.ToTable("Goods");
                });

            modelBuilder.Entity("MultiTenancyDemo.Data.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("OrderDes");

                    b.Property<int>("TenantId");

                    b.Property<DateTime>("UpdateTime");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("TenantId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("MultiTenancyDemo.Data.Tenant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Connection")
                        .HasMaxLength(200);

                    b.Property<DateTime>("CreateTime");

                    b.Property<DateTime>("DeleteTime");

                    b.Property<string>("HostName");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .HasMaxLength(200);

                    b.Property<int>("TenantDbType");

                    b.Property<int>("TenantType");

                    b.Property<DateTime>("UpdateTime");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.ToTable("Tenants");
                });

            modelBuilder.Entity("MultiTenancyDemo.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("Name");

                    b.Property<int>("Status");

                    b.Property<int>("TenantId");

                    b.Property<DateTime>("UpdateTime");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("TenantId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("MultiTenancyDemo.Data.Goods", b =>
                {
                    b.HasOne("MultiTenancyDemo.Data.Tenant", "Tenant")
                        .WithMany()
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MultiTenancyDemo.Data.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MultiTenancyDemo.Data.Order", b =>
                {
                    b.HasOne("MultiTenancyDemo.Data.Tenant", "Tenant")
                        .WithMany()
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MultiTenancyDemo.Data.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MultiTenancyDemo.Data.User", b =>
                {
                    b.HasOne("MultiTenancyDemo.Data.Tenant", "Tenant")
                        .WithMany()
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
