namespace SiteChecker.Tests {
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class SiteCheckerDataStoreInitializerTest
    {
        [Fact]
        public void TestDataStoreInitializer()
        {          
            var options = new DbContextOptionsBuilder<SiteCheckerDbContext>()
                .UseInMemoryDatabase("TestDataStoreInitializer")
                .Options;

            var dataStore            = new DbContextDataStore(new SiteCheckerDbContext(options));
            var dataStoreInitializer = new SiteCheckerDataStoreInitializer();
            dataStoreInitializer.Initialize(dataStore);

            // Количество задач в репозитории
            var taskCountInDataStore = dataStore.GetQueriable<UrlCheckTask>().Count();

            // Количество урлов для проверки в инициализаторе репозитория
            var dataStoreInitializerUrlCount = SiteCheckerDataStoreInitializer.UrlList.Distinct().Count();

            Assert.True(taskCountInDataStore > 0);
            Assert.Equal(dataStoreInitializerUrlCount, taskCountInDataStore);
            Assert.Equal(0, dataStore.GetQueriable<UrlCheckTaskResult>().Count());
        }
    }
}