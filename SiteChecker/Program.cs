namespace SiteChecker
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    class Program
    {
        /// <summary>
        /// Для отладки через Консоль
        /// </summary>
        static void Main()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var loggerFactory = new LoggerFactory()
                .AddConsole((message,logLevel) => { Console.WriteLine(message); return true; })
                .AddDebug((message,logLevel) => { if (logLevel == LogLevel.Debug) Debug.WriteLine(message); return true; });

            var config = new ConfigurationBuilder().AddJsonFile("SiteChecker.Config.json").Build();
            var connectionString = config.GetConnectionString("MsSqlConnectionString");
            
            var options = new DbContextOptionsBuilder<SiteCheckerDbContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                //.UseSqlServer(connectionString)
                .Options;

            var httpUrlChecker = new HttpUrlChecker();
            var dataStore      = new DbContextDataStore(new SiteCheckerDbContext(options));

            new SiteCheckerDataStoreInitializer().Initialize(dataStore);

            dataStore.GetQueriable<UrlCheckTask>().ToList().ForEach(x => Console.WriteLine(x.Url));

            var urlCheckTaskService = new UrlCheckTaskService(
                httpUrlChecker,
                dataStore,
                loggerFactory
            );

            urlCheckTaskService.StartProcessTimer(15);

            Console.WriteLine("Press ENTER to STOP timer");
            Console.ReadLine();

            urlCheckTaskService.StopProcessTimer();


            Console.WriteLine("Press ANY KEY to EXIT program");
            Console.ReadKey();
        }
    }
}
