using LMS.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Core.ViewModels
{
    public class LmsViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ange namn")]
        [Display(Name = "Namn")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Ange beskrivning")]
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Ange datum")]
        [DataType(DataType.Date)]
        [Display(Name = "Aktivitetsstart")]
        [ActivityStartInRange]
        public DateTime StartDateTime { get; set; }

        [Required(ErrorMessage = "Ange datum")]
        [DataType(DataType.Date)]
        [Display(Name = "Aktivitetsslut")]
        [ActivityEndInRange]
        public DateTime EndDateTime { get; set; }
        [Display(Name = "Typ")]
        public int ActivityTypeId { get; set; }
        [Display(Name = "Modulnamn")]
        public int ModuleId { get; set; }
        public DateTime ModuleStart { get; set; }
        public DateTime ModuleEnd { get; set; }


    }
}
