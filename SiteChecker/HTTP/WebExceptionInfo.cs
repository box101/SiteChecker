namespace SiteChecker 
{
    using System.Net;

    /// <summary>
    /// Информация из WebException
    /// </summary>
    public class WebExceptionInfo
    {
        /// <summary>
        /// Текст ошибки
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Код статуса HTTP ответа
        /// </summary>
        public int HttpStatus { get; set; }

        /// <summary>
        /// Заголовки ответа
        /// </summary>
        public WebHeaderCollection ResponseHeaders { get; set;}
    }
}