/*
    大段代码摘抄自Abp（全局过滤器） 
    https://github.com/aspnetboilerplate/aspnetboilerplate/blob/dev/src/Abp.EntityFrameworkCore/EntityFrameworkCore/AbpDbContext.cs
 */
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MultiTenancyDemo.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using MultiTenancyDemo.Data;
using MultiTenancyDemo.Uow;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace MultiTenancyDemo.Data
{
    /// <inheritdoc />
    /// <summary>
    /// 多租户Demo
    /// </summary>
    public class MultiTenancyDbContext:DbContext
    {
        private readonly IMultiTenancyDemoUnitOfWork _unitOfWork;
        public DbSet<User> User{get;set;}

        public DbSet<Goods> Goods{get;set;}

        public DbSet<Order> Order{get;set;}
        
        public DbSet<Tenant> Tenant { get; set; }

        private static MethodInfo ConfigureGlobalFiltersMethodInfo = typeof(MultiTenancyDbContext).GetMethod(nameof(ConfigureGlobalFilters), BindingFlags.Instance | BindingFlags.NonPublic);

        private int CurrentTenantId
        {
            get
            {
                return _unitOfWork.GetTenant().Id;
            }
        }

        public MultiTenancyDbContext(DbContextOptions<MultiTenancyDbContext> options,IMultiTenancyDemoUnitOfWork unitOfWork):base(options)
        {
            this._unitOfWork = unitOfWork;
        }

        public override int SaveChanges()
        {
            ApplyMultiTenant();
            
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ApplyMultiTenant();
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected virtual void ApplyMultiTenant()
        {
            foreach(var entry in this.ChangeTracker.Entries())
            {
                ApplyConcepts(entry,null);
            }
        }

        protected virtual void ApplyConcepts(EntityEntry entry, long? userId)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    ApplyConceptsForAddedEntity(entry, userId);
                    break;
                case EntityState.Modified:
                    ApplyConceptsForModifiedEntity(entry, userId);
                    break;
                case EntityState.Deleted:
                    ApplyConceptsForDeletedEntity(entry, userId);
                    break;
            }
        }

        protected virtual void ApplyConceptsForAddedEntity(EntityEntry entry, long? userId)
        {
            CheckAndSetMustHaveTenantIdProperty(entry.Entity);
            CheckAndSetMayHaveTenantIdProperty(entry.Entity);
            CheckAndBaseProperty(entry.Entity);
        }

        protected virtual void ApplyConceptsForModifiedEntity(EntityEntry entry, long? userId)
        {
            CheckAndSetMustHaveTenantIdProperty(entry.Entity);
            CheckAndSetMayHaveTenantIdProperty(entry.Entity);
            CheckAndUpdateBaseProperty(entry.Entity);
        }

        protected virtual void ApplyConceptsForDeletedEntity(EntityEntry entry, long? userId)
        {
            CancelDeletionForSoftDelete(entry);
            SetDeletionAuditProperties(entry,userId);
        }

        protected virtual void SetDeletionAuditProperties(object entityAsObj, long? userId)
        {
            if (entityAsObj is IHasDeletionTime)
            {
                var entity = entityAsObj as IHasDeletionTime;

                if (entity.DeletionTime == null)
                {
                    entity.DeletionTime = DateTime.Now;
                }
            }

            if (entityAsObj is IDeletionAudited)
            {
                var entity = entityAsObj as IDeletionAudited;

                if (entity.DeleterUserId != null)
                {
                    return;
                }

                if (userId == null)
                {
                    entity.DeleterUserId = null;
                    return;
                }

                //Special check for multi-tenant entities
                if (entity is IMayHaveTenant || entity is IMustHaveTenant)
                {
                    //Sets LastModifierUserId only if current user is in same tenant/host with the given entity
                    if ((entity is IMayHaveTenant && (entity as IMayHaveTenant).TenantId == CurrentTenantId ||
                        (entity is IMustHaveTenant && (entity as IMustHaveTenant).TenantId == CurrentTenantId)))
                    {
                        entity.DeleterUserId = userId;
                    }
                    else
                    {
                        entity.DeleterUserId = null;
                    }
                }
                else
                {
                    entity.DeleterUserId = userId;
                }
            }
        }

        protected virtual void CancelDeletionForSoftDelete(EntityEntry entry)
        {
            if (!(entry.Entity is ISoftDelete))
            {
                return;
            }

            entry.Reload();
            entry.State = EntityState.Modified;
            (entry.Entity as ISoftDelete).IsDeleted = true;
        }
        public virtual void CheckAndBaseProperty(object entity)
        {
            if(!(entity is IHasCreateTime))
            {
                return;
            }
            else
            {
                var entityWithCreateTime=entity as IHasCreateTime;
                entityWithCreateTime.CreateTime=DateTime.Now;
            }
            
        }

        public virtual void CheckAndUpdateBaseProperty(object entity)
        {
            if(!(entity is IHasUpdateTime))
            {
                return;
            }
            else
            {
                var entityWithCreateTime=entity as IHasUpdateTime;
                entityWithCreateTime.UpdateTime=DateTime.Now;
            }
            
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

            Tenant _tenant = _unitOfWork.GetTenant();
                 

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
            if (!(entityAsObj is IMayHaveTenant))
            {
                return;
            }

            var entity = entityAsObj as IMayHaveTenant;

            Tenant _tenant = _unitOfWork.GetTenant();
            if (entity.TenantId != null)
            {
                return;
            }

            entity.TenantId = _tenant.Id;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach(var entityType in modelBuilder.Model.GetEntityTypes())
            {
                 ConfigureGlobalFiltersMethodInfo
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType });
            }
            modelBuilder.ConfigureMultiTenancyDbContext();
        }

        protected void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType entityType)
            where TEntity : class
        {
            if (entityType.BaseType == null && ShouldFilterEntity<TEntity>(entityType))
            {
                var filterExpression = CreateFilterExpression<TEntity>();
                if (filterExpression != null)
                {
                    modelBuilder.Entity<TEntity>().HasQueryFilter(filterExpression);
                }
            }
        }

        protected virtual bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType) where TEntity : class
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            if (typeof(IMayHaveTenant).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            if (typeof(IMustHaveTenant).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 代码出自Abp框架：
        /// https://github.com/aspnetboilerplate/aspnetboilerplate/blob/dev/src/Abp.EntityFrameworkCore/EntityFrameworkCore/AbpDbContext.cs
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        protected virtual Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
            where TEntity : class
        {
            Expression<Func<TEntity, bool>> expression = null;

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                /* This condition should normally be defined as below:
                 * !IsSoftDeleteFilterEnabled || !((ISoftDelete) e).IsDeleted
                 * But this causes a problem with EF Core (see https://github.com/aspnet/EntityFrameworkCore/issues/9502)
                 * So, we made a workaround to make it working. It works same as above.
                 */

                Expression<Func<TEntity, bool>> softDeleteFilter = e => !((ISoftDelete)e).IsDeleted || ((ISoftDelete)e).IsDeleted != true;
                expression = expression == null ? softDeleteFilter : CombineExpressions(expression, softDeleteFilter);
            }

            if (typeof(IMayHaveTenant).IsAssignableFrom(typeof(TEntity)))
            {
                /* This condition should normally be defined as below:
                 * !IsMayHaveTenantFilterEnabled || ((IMayHaveTenant)e).TenantId == CurrentTenantId
                 * But this causes a problem with EF Core (see https://github.com/aspnet/EntityFrameworkCore/issues/9502)
                 * So, we made a workaround to make it working. It works same as above.
                 */
                Expression<Func<TEntity, bool>> mayHaveTenantFilter = e => ((IMayHaveTenant)e).TenantId == CurrentTenantId || (((IMayHaveTenant)e).TenantId == CurrentTenantId) == true;
                expression = expression == null ? mayHaveTenantFilter : CombineExpressions(expression, mayHaveTenantFilter);
            }

            if (typeof(IMustHaveTenant).IsAssignableFrom(typeof(TEntity)))
            {
                /* This condition should normally be defined as below:
                 * !IsMustHaveTenantFilterEnabled || ((IMustHaveTenant)e).TenantId == CurrentTenantId
                 * But this causes a problem with EF Core (see https://github.com/aspnet/EntityFrameworkCore/issues/9502)
                 * So, we made a workaround to make it working. It works same as above.
                 */
                Expression<Func<TEntity, bool>> mustHaveTenantFilter = e => ((IMustHaveTenant)e).TenantId == CurrentTenantId || (((IMustHaveTenant)e).TenantId == CurrentTenantId) == true;
                expression = expression == null ? mustHaveTenantFilter : CombineExpressions(expression, mustHaveTenantFilter);
            }

            return expression;
        }

        /// <summary>
        /// 代码出自Abp框架：
        /// https://github.com/aspnetboilerplate/aspnetboilerplate/blob/dev/src/Abp.EntityFrameworkCore/EntityFrameworkCore/AbpDbContext.cs
        /// </summary>
        /// <param name="expression1"></param>
        /// <param name="expression2"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected virtual Expression<Func<T, bool>> CombineExpressions<T>(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expression1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expression1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expression2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expression2.Body);

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
        }

        /// <summary>
        /// 代码出自Abp框架：
        /// https://github.com/aspnetboilerplate/aspnetboilerplate/blob/dev/src/Abp.EntityFrameworkCore/EntityFrameworkCore/AbpDbContext.cs
        /// </summary>
        class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                {
                    return _newValue;
                }

                return base.Visit(node);
            }
        }
    }
}