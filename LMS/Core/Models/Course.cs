using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Kursnamn")]
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
        
        //Navigation properties
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public ICollection<Module> Modules { get; set; }
        public ICollection<Document> Documents { get; set; }


    }
}
