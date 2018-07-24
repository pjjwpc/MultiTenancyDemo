using CacheManager.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MultiTenancyDemo.Data;
using MultiTenancyDemo.Middleware;
using MultiTenancyDemo.Repository;
using MultiTenancyDemo.Uow;
using System.Collections.Generic;
using System.Linq;

namespace MultiTenancyDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddCacheManagerConfiguration(Configuration);
            services.AddScoped<IMultiTenancyDemoUnitOfWork,MultiTenancyDemoUnitOfWork>();
            services.AddDbContext<MultiTenancyDbContext>(options=>
            {
                options.UseLoggerFactory(Mlogger);
                options.UseMySql("server=localhost;uid=root;pwd=qwe123,.,.;database=MultiTenancy;SslMode=none");
            });
            services.AddSingleton<ITenantResolverProvider,UrlTenantResolverProvider>();
            services.AddSingleton(typeof(MultiTenantType),MultiTenantType.Tenant);
            services.AddScoped(typeof(IDbContextProvider<>),typeof(DbContextProvider<>));
            services.AddScoped(typeof(IRepository<,,>),typeof(Repository<,,>));
            services.AddScoped(typeof(IMultiTenantRepositoryBase<,>),typeof(MultiTenantRepositoryBase<,>));
            services.AddScoped(typeof(IMultiTenantRepositoryBase<>),typeof(MultiTenantRepositoryBase<>));
            services.AddCacheManager();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
        private static ILoggerFactory Mlogger =>new LoggerFactory()
                 .AddDebug((categoryName, logLevel) => (logLevel == LogLevel.Information) && (categoryName == DbLoggerCategory.Database.Command.Name))
                .AddConsole((categoryName, logLevel) => (logLevel == LogLevel.Information) && (categoryName == DbLoggerCategory.Database.Command.Name));
        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env
                    ,IMultiTenantRepositoryBase<Tenant> tenantRepositoy
                    ,ICacheManager<Tenant> cacheManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseMiddleware<MultiTenantMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
           foreach(var temp in tenantRepositoy.GetAll().ToList())
           {
               cacheManager.Add(temp.HostName,temp);
           }
        }
    }
}
