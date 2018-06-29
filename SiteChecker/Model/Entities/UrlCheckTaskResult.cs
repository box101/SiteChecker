namespace SiteChecker 
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary> Результат выполнения Задачи периодической проверки доступнусти URL-адреса по протоколу HTTP </summary>
    public class UrlCheckTaskResult
    {
        /// <summary> Идентификатор </summary>
        public int Id { get; set; }
                
        /// <summary> Идентификатор задачи приверки (внешний ключ) </summary>
        public int UrlCheckTaskId { get; set;}

        /// <summary> Задача проверки </summary>
        [Required]
        public virtual UrlCheckTask UrlCheckTask { get; set; }

        /// <summary> Время проверки </summary>
        public DateTime CheckDateTime { get; set; }

        /// <summary> Результат последней проверки в формате JSON </summary>
        public string LastCheckResultJson { get; set; }
    }
}