namespace SiteChecker 
{
    using System.IO;
    using System.Net;
    using System.Text;

    /// <summary>
    /// Расширения для WebException
    /// </summary>
    public static class WebExceptionExtensions
    {
        /// <summary>
        /// Собираем максимум информации из тела и заголовков http-ответа
        /// </summary>
        public static WebExceptionInfo GetWebExceptionInfo(this WebException webException)
        {
            var responseText    = new StringBuilder();
            var httpStatus      = 0;
            var responseHeaders = new WebHeaderCollection();

            if (webException.Response is HttpWebResponse webResponse)
            {
                httpStatus      = (int)webResponse.StatusCode;
                responseHeaders = webResponse.Headers;
                responseText.AppendLine($"HTTP STATUS: {httpStatus}\n");

                // Заголовки http-ответа
                responseText.AppendLine(webResponse.Headers.ToString());
                responseText.AppendLine();

                // Тело http-ответа
                using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    responseText.AppendLine(streamReader.ReadToEnd());
                }
                responseText.AppendLine("\n");
            }

            var errorMessage = responseText.ToString();
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                errorMessage = webException.Message;
            }
            return new WebExceptionInfo { ErrorMessage = errorMessage, HttpStatus = httpStatus, ResponseHeaders = responseHeaders };
        }
    }
}