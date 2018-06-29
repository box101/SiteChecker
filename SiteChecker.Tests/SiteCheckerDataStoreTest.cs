namespace SiteChecker.Tests {
    using System;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class SiteCheckerDataStoreTest
    {
        [Fact]
        public void Test_UrlCheckTask_UrlMustNotBeEmpty()
        {          
            var options   = new DbContextOptionsBuilder<SiteCheckerDbContext>().UseInMemoryDatabase("Test_UrlCheckTask_UrlMustNotBeEmpty").Options;
            var dataStore = new DbContextDataStore(new SiteCheckerDbContext(options));

            var emptyUrlCheckTask = new UrlCheckTask();
            Assert.Throws<InvalidOperationException>(() =>
            {
                dataStore.Add(emptyUrlCheckTask);
                dataStore.SaveChanges();
            }); 

            dataStore.Add(new UrlCheckTask { Url = "http:/123/" });
            dataStore.SaveChanges();
        }

        [Fact]
        public void Test_UrlCheckTask_UrlMustBeUnique()
        {          
            var options   = new DbContextOptionsBuilder<SiteCheckerDbContext>().UseInMemoryDatabase("Test_UrlCheckTask_UrlMustBeUnique").Options;
            var dataStore = new DbContextDataStore(new SiteCheckerDbContext(options));

            Assert.Equal(0, dataStore.GetQueriable<UrlCheckTask>().Count());

            var newTask = new UrlCheckTask { Url = "123" };

            dataStore.Add(newTask);
            dataStore.SaveChanges();

            Assert.Throws<InvalidOperationException>(() =>
            {
                dataStore.Add(new UrlCheckTask { Url = "123" });
                dataStore.SaveChanges();
            }); 

            dataStore.Add(new UrlCheckTask { Url = "234" });
            dataStore.SaveChanges();            
        }
    }
}