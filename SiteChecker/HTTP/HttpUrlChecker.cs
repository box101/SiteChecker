namespace SiteChecker {
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    /// <inheritdoc cref="IHttpUrlChecker"/>
    public class HttpUrlChecker : IHttpUrlChecker
    {
        /// <inheritdoc />
        public async Task<IDictionary<UrlCheckTask, HttpUrlCheckResult>> CheckUrlList(IEnumerable<UrlCheckTask> urlList)
        {
            var resultList = new ConcurrentDictionary<UrlCheckTask, HttpUrlCheckResult>();

            var tasks = urlList.Select( async task =>
            {
                var checkResult = await CheckUrlAsync(task.Url, "HEAD", task.Login, task.Password);
                resultList[task] = checkResult;    
            });

            await Task.WhenAll(tasks);
            return resultList;
        }

        /// <inheritdoc />
        public async Task<HttpUrlCheckResult> CheckUrlAsync(string url, string httpMethod = "HEAD", string userName = null, string password = null)
        {            
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method  = httpMethod;
            httpWebRequest.Timeout = this.Timeout <= 0 ? 5000 : this.Timeout;
            httpWebRequest.Headers.Add(HttpRequestHeader.Connection, "close");

            //  Некоторые сервера отвечают только браузерам
            httpWebRequest.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36");

            if (!string.IsNullOrWhiteSpace(userName))
            {
                httpWebRequest.Credentials = new NetworkCredential(userName, password);
            }

            try 
            {
                using (var resp = (HttpWebResponse)await httpWebRequest.GetResponseAsync()) 
                {
                    var respStatusCode = (int)resp.StatusCode;

                    var result = new HttpUrlCheckResult
                                 {
                                     CheckDateTime = DateTime.Now,
                                     Url           = url,
                                     Result        = respStatusCode >= 200 && respStatusCode < 300,
                                     HttpStatus    = respStatusCode,
                                     HttpMethod    = httpMethod,
                                 };
                
                    return result;
                }
            }
            catch(WebException webException)
            {
                var webExceptionInfo = webException.GetWebExceptionInfo();

                // Не все сервера поддерживают HTTP-HEAD запросы, при этом вместо корректного 405 возвращают... всё что угодно
                if (webExceptionInfo.HttpStatus >= 400 && httpMethod == "HEAD")
                {
                    return await CheckUrlAsync(url, "GET", userName, password);
                }

                // Переадресация
                if (webExceptionInfo.HttpStatus >= 300 && webExceptionInfo.HttpStatus < 400)
                {
                    if (webExceptionInfo.ResponseHeaders.AllKeys.Contains("Location"))
                    {
                        var newLocation = webExceptionInfo.ResponseHeaders.Get("Location");
                        return await CheckUrlAsync(newLocation);
                    }
                }

                return new HttpUrlCheckResult
                       {
                           Result        = false, 
                           ErrorMessage  = webExceptionInfo.ErrorMessage, 
                           HttpStatus    = webExceptionInfo.HttpStatus,
                           CheckDateTime = DateTime.Now, 
                           HttpMethod    = httpMethod,
                           Url           = url,
                       };
            }
            catch(Exception exception)
            {
                return new HttpUrlCheckResult
                       {
                           Result        = false, 
                           ErrorMessage  = exception.ToString(), 
                           CheckDateTime = DateTime.Now, 
                           Url           = url,
                           HttpMethod    = httpMethod,
                       };
            }
        }

        /// <inheritdoc />
        public int Timeout { get; set; }
    }
}