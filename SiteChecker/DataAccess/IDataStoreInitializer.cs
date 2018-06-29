namespace SiteChecker 
{
    /// <summary>
    /// Инициализатор Хранилища данных
    /// </summary>
    public interface IDataStoreInitializer 
    {
        /// <summary>
        /// Инициализация хранилища
        /// </summary>
        /// <param name="dataStore"></param>
        void Initialize(IDataStore dataStore);
    }
}