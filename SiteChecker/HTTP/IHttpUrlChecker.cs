namespace SiteChecker 
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Сервис проверки доступности списка URL-адресов по протоколу HTTP
    /// </summary>
    public interface IHttpUrlChecker   
    {
        /// <summary>
        /// Проверка списка задач
        /// </summary>
        /// <param name="urlList"></param>
        /// <returns></returns>
        Task<IDictionary<UrlCheckTask, HttpUrlCheckResult>> CheckUrlList(IEnumerable<UrlCheckTask> urlList);

        /// <summary>
        /// Проверка доступности адреса по протоколу HTTP, согласно указанному методу и учётным данным для аутентификации
        /// </summary>
        /// <param name="url">адрес</param>
        /// <param name="httpMethod">метод HTTP-протокола</param>
        /// <param name="userName">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        Task<HttpUrlCheckResult> CheckUrlAsync(string url, string httpMethod = "HEAD", string userName = null, string password = null);

        /// <summary>
        /// HTTP Request timeout
        /// </summary>
        int Timeout { get; set; }
    }
}