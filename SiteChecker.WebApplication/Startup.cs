using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiteChecker.WebApplication.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SiteChecker.WebApplication
{
    using Microsoft.Extensions.Logging;

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

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<SiteCheckerDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SiteCheckerDbConnectionString")));

            services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<UserManager<IdentityUser>>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            services.AddSingleton<IHttpUrlChecker, HttpUrlChecker>();
            services.AddTransient<IDataStore>(x =>
            {
                var options = new DbContextOptionsBuilder<SiteCheckerDbContext>()
                    .UseSqlServer(Configuration.GetConnectionString("SiteCheckerDbConnectionString"))
                    .Options;

                return new DbContextDataStore(new SiteCheckerDbContext(options));
            }) ;
            services.AddTransient<IUrlCheckTaskService, UrlCheckTaskService>() ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IUrlCheckTaskService urlCheckTaskService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            int.TryParse(this.Configuration["UrlCheckIntervalSeconds"], out var checkIntervalSec);
            urlCheckTaskService.StartProcessTimer(checkIntervalSec);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });            
        }
    }
}
