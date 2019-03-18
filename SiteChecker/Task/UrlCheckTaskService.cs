namespace SiteChecker 
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json;

    /// <inheritdoc cref="IUrlCheckTaskService"/>
    public class UrlCheckTaskService : IUrlCheckTaskService
    {
        private static readonly object SyncObj = new object();

        private readonly IHttpUrlChecker httpUrlChecker;
        private readonly IDataStore dataStore;
        private readonly ILogger logger;
        private Timer timer;

        /// <summary> Конструктор </summary>
        public UrlCheckTaskService(
            IHttpUrlChecker httpUrlChecker, 
            IDataStore dataStore, 
            ILoggerFactory loggerFactory)
        {
            this.httpUrlChecker = httpUrlChecker;
            this.dataStore      = dataStore;
            this.logger = loggerFactory.CreateLogger<UrlCheckTaskService>();
        }

        /// <inheritdoc />
        public void ProcessAllTasks(int asyncWaitMillisec)
        {
            lock(SyncObj)
            {
                try
                {
                    var checkTasks   = GetTasksList();
                    var checkUrlList = httpUrlChecker.CheckUrlList(checkTasks);
                    checkUrlList.Wait(asyncWaitMillisec);

                    using (var transaction = dataStore.BeginTransaction())
                    {
                        foreach (var entry in checkUrlList.Result)
                        {
                            var task        = entry.Key;
                            var checkResult = entry.Value;

                            var checkResultJson = JsonConvert.SerializeObject(checkResult);
                            task.HttpStatusCode = checkResult.HttpStatus;
                            task.LastCheckDateTime = checkResult.CheckDateTime;
                            task.LastCheckResultJson = checkResultJson;
                            this.dataStore.Update(task);

                            var taskResult = new UrlCheckTaskResult
                                             {
                                                 UrlCheckTask = task,
                                                 UrlCheckTaskId = task.Id,
                                                 CheckDateTime = checkResult.CheckDateTime,
                                                 LastCheckResultJson = checkResultJson
                                             };

                            this.dataStore.Add(taskResult);


                            logger.LogDebug("Обработана задач проверки доступнусти адреса. Результат\n{0}", checkResult);
                        }

                        this.dataStore.SaveChanges();
                        transaction.Commit();
                    }

                    logger.LogInformation("Обработан список задач проверки доступнусти адресов по протоколу HTTP. Данные сохранены.");
                
                }
                catch (Exception exception)
                {
                    logger.LogError(exception, "Ошибка обработки списка задач проверки доступнусти адресов по протоколу HTTP");
                    throw;
                }
            }
        }

        /// <inheritdoc />
        public IList<UrlCheckTask> GetTasksList()
        {
            return this.dataStore.GetAll<UrlCheckTask>().ToList();
        }

        /// <inheritdoc />
        public void StartProcessTimer(int secInterval)
        {
            logger.LogInformation("ЗАПУЩЕН таймер обработки очереди задач проверки доступнусти урлов");
            timer = new Timer(state => ProcessAllTasks(secInterval * 1000), null, 0, 1000 * secInterval);
        }

        /// <inheritdoc />
        public void StopProcessTimer()
        {
            timer?.Dispose();
            timer = null;

            logger.LogInformation("Таймер обработки очереди задач проверки доступнусти урлов - ОСТАНОВЛЕН");
        }
    }
}