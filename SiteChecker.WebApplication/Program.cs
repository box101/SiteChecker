using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SiteChecker.WebApplication
{
    using System.Net;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        public static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var webHost =  WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .UseStartup<Startup>()
                .Build();

            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager  = services.GetRequiredService<UserManager<IdentityUser>>();
                    
                    RoleInitializer.InitializeAsync(userManager).Wait();

                    webHost.Run();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }            

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }

    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<IdentityUser> userManager)
        {
            const string adminEmail = "admin_sitechecker@yandex.ru";
            const string password   = "Qwer@123";

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var admin  = new IdentityUser { Email = adminEmail, UserName = adminEmail };
                var result = await userManager.CreateAsync(admin, password);
            }
        }
    }


}
