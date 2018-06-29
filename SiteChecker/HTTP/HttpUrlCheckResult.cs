namespace SiteChecker 
{
    using System;

    /// <summary>
    /// Результат проверки доступности URL по протоколу HTTP
    /// </summary>
    public class HttpUrlCheckResult
    {
        /// <summary> Доступрость </summary>
        public bool Result { get; set; }

        /// <summary> Текст ошибки доступа </summary>
        public string ErrorMessage { get; set; }

        /// <summary> Дата-время проверки </summary>
        public DateTime CheckDateTime { get; set; }

        /// <summary> Проверяемый URL </summary>
        public string Url { get; set; }

        /// <summary>
        /// Метод HTTP-протокола (HEAD, GET)
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// Код ответа HTTP 
        /// </summary>
        public int HttpStatus { get; set; }

        /// <summary>
        /// Для отладки на консоли
        /// </summary>
        public override string ToString()
        {
            return $"{Result}; {Url}; {CheckDateTime:G}; HTTP STATUS: {HttpStatus}; {ErrorMessage}";
        }
    }
}