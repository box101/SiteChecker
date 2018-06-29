namespace SiteChecker 
{
    using System.Collections.Generic;

    /// <summary>
    /// Сервис обработки списка задач проверки доступнусти адресов по протоколу HTTP
    /// </summary>
    public interface IUrlCheckTaskService 
    {
        /// <summary>
        /// Обработка всего списка задач
        /// </summary>
        void ProcessAllTasks(int asyncWaitMillisec);

        /// <summary>
        /// Получение списка задач
        /// </summary>
        /// <returns></returns>
        IList<UrlCheckTask> GetTasksList();

        /// <summary>
        /// Старт таймера обработки списка задач
        /// </summary>
        void StartProcessTimer(int secInterval);

        /// <summary>
        /// Остановка таймера обработки списка задач
        /// </summary>
        void StopProcessTimer();
    }
}