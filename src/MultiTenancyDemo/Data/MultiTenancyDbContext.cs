using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MultiTenancyDemo.Extensions;
using System;
using MultiTenancyDemo.Data;

namespace MultiTenancyDemo.Data
{
    /// <inheritdoc />
    /// <summary>
    /// 多租户Demo
    /// </summary>
    public class MultiTenancyDbContext:DbContext
    {
        private Tenant _tenant;
        internal void SetTenant(Tenant tenant)
        {
            _tenant=tenant;
        }
        public DbSet<User> User{get;set;}

        public DbSet<Goods> Goods{get;set;}

        public DbSet<Order> Order{get;set;}
        
        public DbSet<Tenant> Tenant { get; set; }
        
        public MultiTenancyDbContext(DbContextOptions<MultiTenancyDbContext> options):base(options)
        {

        }

        public override int SaveChanges()
        {
            AppMultiTenant();
            return base.SaveChanges();
        }


        protected virtual void AppMultiTenant()
        {
            foreach(var entry in this.ChangeTracker.Entries())
            {
                ApplyAbpConcepts(entry,null);
            }
        }

        protected virtual void ApplyAbpConcepts(EntityEntry entry, long? userId)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    ApplyConceptsForAddedEntity(entry, userId);
                    break;
                case EntityState.Modified:
                    //ApplyAbpConceptsForModifiedEntity(entry, userId);
                    break;
                case EntityState.Deleted:
                    //ApplyAbpConceptsForDeletedEntity(entry, userId);
                    break;
            }

            //AddDomainEvents(changeReport.DomainEvents, entry.Entity);
        }

        protected virtual void ApplyConceptsForAddedEntity(EntityEntry entry, long? userId)
        {
            CheckAndSetMustHaveTenantIdProperty(entry.Entity);
            CheckAndSetMayHaveTenantIdProperty(entry.Entity);
        }

        protected virtual void CheckAndSetMustHaveTenantIdProperty(object entityAsObj)
        {

            //Only set IMustHaveTenant entities
            if (!(entityAsObj is IMustHaveTenant))
            {
                return;
            }

            var entity = entityAsObj as IMustHaveTenant;

            //Don't set if it's already set
            if (entity.TenantId != 0)
            {
                return;
            }


            if (_tenant != null)
            {
                entity.TenantId = _tenant.Id;
            }
            else
            {
                throw new Exception("Can not set TenantId to 0 for IMustHaveTenant entities!");
            }
        }
        
        protected virtual void CheckAndSetMayHaveTenantIdProperty(object entityAsObj)
        {
           

            //Only set IMayHaveTenant entities
            if (!(entityAsObj is IMayHaveTenant))
            {
                return;
            }

            var entity = entityAsObj as IMayHaveTenant;

            //Don't set if it's already set
            if (entity.TenantId != null)
            {
                return;
            }

            entity.TenantId = _tenant.Id;
        }

        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureMultiTenancyDbContext();
        }
    }
}