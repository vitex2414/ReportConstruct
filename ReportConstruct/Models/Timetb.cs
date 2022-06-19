using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReportConstruct.Models
{
    public partial class Timetb
    {
        public string Id { get; set; }
        [Display(Name = "Дата")]
        public DateTime Dates { get; set; }
        [Display(Name = "ФИО")]
        public string Names { get; set; }
        [Display(Name = "Время прихода")]
        public string Timecome { get; set; }
        [Display(Name = "Опоздание")]
        public string Late { get; set; }
        [Display(Name = "Уход на обед")]
        public string Lunchleave { get; set; }
        [Display(Name = "Приход с обеда")]
        public string Lunchcome { get; set; }
        [Display(Name = "Время ухода")]
        public string Timeleave { get; set; }
        [Display(Name = "Подразделение")]
        public string Groupname { get; set; }

        [Display(Name = "Обьяснительная")]
        public bool IsChecked { get; set; }

  

    }
}
