using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Core.Models
{
    public class Document
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Namn")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
        [Display(Name = "Feedback")]
        public string Feedback { get; set; }
        [Display(Name = "Uppladdad datum")]
        public DateTime UploadDateTime { get; set; }
        public DateTime Deadline { get; set; }

        // Foreign Keys
        public string ApplicationUserId { get; set; }
        public int? CourseId { get; set; }
        public int? ModuleId { get; set; }
        public int? ActivityId { get; set; }

        //Navigation properties
        [Display(Name = "Kurs")]
        public Course Course { get; set; }
        [Display(Name = "Modul")]
        public Module Module { get; set; }
        [Display(Name = "Aktivitet")]
        public Activity Activity { get; set; }
    }
}
