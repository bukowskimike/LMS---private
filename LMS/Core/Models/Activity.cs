using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Models
{
    public class Activity
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Namn")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}")]
        [Display(Name = "Start")]
        public DateTime StartDateTime { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}")]
        [Display(Name = "Slut")]
        public DateTime EndDateTime { get; set; }
        
        // Foreign Keys
        public int ModuleId { get; set; }
        public int ActivityTypeId { get; set; }

        //Navigation properties
        [Display(Name = "Modul")]
        public Module Module { get; set; }
        [Display(Name = "Typ")]
        public ActivityType ActivityType { get; set; }
        public ICollection<Document> Documents { get; set; }
    }
}
