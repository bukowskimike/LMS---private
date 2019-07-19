using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Core.Models
{
    public class Module
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDateTime { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDateTime { get; set; }
        
        // Foreign Keys
        public int CourseId { get; set; }
        
        //Navigation properties 
        [Display(Name = "Kurs")]
        public Course Course { get; set; }
        [Display(Name = "Typ")]
        public ICollection<Activity> Activities { get; set; }
        public ICollection<Document> Documents { get; set; }
    }
}
