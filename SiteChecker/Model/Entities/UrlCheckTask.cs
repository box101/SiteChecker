namespace SiteChecker {
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary> Задача проверки доступнусти URL-адреса по протоколу HTTP </summary>
    public class UrlCheckTask
    {
        /// <summary> Идентификатор </summary>
        public int Id { get; set; }

        /// <summary> URL </summary>
        [Display(Description = "Сайт", Name = "Сайт"), Required]
        public string Url { get; set; }

        /// <summary> Логин для аутентификации (не обязательный) </summary>
        [Display(Description = "Логин", Name = "Логин")]
        public string Login { get; set; }

        /// <summary> Пароль для аутентификации (не обязательный) </summary>
        [Display(Description = "Пароль", Name = "Пароль")]
        public string Password { get; set; }

        /// <summary> Результат последней проверки в формате JSON </summary>
        [Display(Description = "Резальтат последней проверки (JSON)", Name = "Резальтат последней проверки (JSON)")]
        public string LastCheckResultJson { get; set; }

        /// <summary> Дата-время последней проверки </summary>
        [Display(Description = "Время последней проверки", Name = "Время последней проверки")]
        public DateTime? LastCheckDateTime {get; set;}

        /// <summary> HTTP-Статус ответа удалённого сервера </summary>
        [Display(Description = "HTTP STATUS", Name = "HTTP STATUS")]
        public int HttpStatusCode { get; set;}
    }
}