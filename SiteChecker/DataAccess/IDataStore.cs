namespace SiteChecker 
{
    using System.Linq;

    using Microsoft.EntityFrameworkCore.Storage;

    /// <summary>
    /// Репозиторий
    /// </summary>
    public interface IDataStore 
    {
        /// <summary> Запрос к списку объектов </summary>
        IQueryable<T> GetQueriable<T>() where T: class, new();

        /// <summary> Изменение объекта </summary>
        void Update<T>(T entity) where T: class, new();

        /// <summary> Добавление объекта </summary>
        void Add<T>(T entity) where T: class, new();

        /// <summary> Удаление объекта </summary>
        void Remove<T>(T entity) where T: class, new();

        /// <summary> Сохранение локальных изменений </summary>
        void SaveChanges();

        /// <summary> Открытие транзакции </summary>
        IDbContextTransaction BeginTransaction();
    }
}