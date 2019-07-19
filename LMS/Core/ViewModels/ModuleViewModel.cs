using LMS.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Core.ViewModels
{
    public class ModuleViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ange namn")]
        [Display(Name = "Namn")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Ange namn")]
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Ange startdatum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}")]
        [ModuleStartInRange]
        [Display(Name = "Modulstart")]
        public DateTime StartDateTime { get; set; }
        [Required(ErrorMessage = "Ange slutdatum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}")]
        [ModuleEndInRange]
        [Display(Name = "Modulslut")]
        public DateTime EndDateTime { get; set; }
        [Display(Name = "Kurs")]
        public int CourseId { get; set; }
        public Course Course { get; set; }
        
        public DateTime CourseStart { get; set; }
        public DateTime CourseEnd { get; set; }
    }
}
