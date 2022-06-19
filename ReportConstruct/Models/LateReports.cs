using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportConstruct.Models
{
    public partial class LateReports
    {
        public string Id { get; set; }
        public string Id_late_day { get; set; }


        [Display(Name = "Наименование файла")]
        public string Names { get; set; }
        public string Path { get; set; }
        [Display(Name = "Комментарий")]
        public string Comments { get; set; }
    }
}
