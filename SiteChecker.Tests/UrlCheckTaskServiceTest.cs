namespace SiteChecker.Tests
{
    using System.Linq;
    using System.Net;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using Xunit;

    public class UrlCheckTaskServiceTest
    {
        [Fact]
        public void TestService()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var loggerFactory = new LoggerFactory();
           
            var options = new DbContextOptionsBuilder<SiteCheckerDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase_UrlCheckTaskServiceTest_TestService")
                .Options;

            var httpUrlChecker = new HttpUrlChecker();
            var dataStore      = new DbContextDataStore(new SiteCheckerDbContext(options));

            var dataStoreInitializer = new SiteCheckerDataStoreInitializer();
            dataStoreInitializer.Initialize(dataStore);

            // Количество задач в репозитории
            var taskCountInDataStore = dataStore.GetQueriable<UrlCheckTask>().Count();

            // Количество урлов для проверки в инициализаторе репозитория
            var dataStoreInitializerUrlCount = SiteCheckerDataStoreInitializer.UrlList.Distinct().Count();

            Assert.True(taskCountInDataStore > 0);
            Assert.Equal(dataStoreInitializerUrlCount, taskCountInDataStore);
            Assert.Equal(0, dataStore.GetQueriable<UrlCheckTaskResult>().Count());

            var urlCheckTaskService = new UrlCheckTaskService(httpUrlChecker, dataStore, loggerFactory);

            var waitMillisec = 10000;

            urlCheckTaskService.ProcessAllTasks(waitMillisec);
            urlCheckTaskService.ProcessAllTasks(waitMillisec);
            urlCheckTaskService.ProcessAllTasks(waitMillisec);
            urlCheckTaskService.ProcessAllTasks(waitMillisec);

            // Обработка задач была вызвана 4 раза. Результаты проверок сохраняются в БД. Количество выполненых проверок должно быть равно х4 от количества задач.
            var checkResultCount = dataStore.GetQueriable<UrlCheckTaskResult>().Count();
            Assert.Equal(taskCountInDataStore * 4, checkResultCount);
        }
    }
}
