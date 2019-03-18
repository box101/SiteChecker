namespace SiteChecker 
{
    using System.Linq;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;

    /// <inheritdoc cref="IDataStore"/>
    public class DbContextDataStore : IDataStore
    {
        private readonly DbContext dbContext;
        public DbContextDataStore(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<T> GetAll<T>() where T: class, new()
        {
            return dbContext.Set<T>().AsQueryable();
        }

        public void Update<T>(T entity) where T: class, new()
        {
            dbContext.Set<T>().Update(entity);
        }

        public void Add<T>(T entity) where T: class, new()
        {
            dbContext.Set<T>().Add(entity);
        }

        public void Remove<T>(T entity) where T: class, new()
        {
            dbContext.Set<T>().Remove(entity);
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return this.dbContext.Database.BeginTransaction();
        }
    }
}