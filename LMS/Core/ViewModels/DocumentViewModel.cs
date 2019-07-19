using LMS.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Core.ViewModels
{
    public class DocumentViewModel
    {
        public int Id { get; set; }
        [NotMappedAttribute]
        public IFormFile File { get; set; }
        [Display(Name = "Namn")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
        [Display(Name = "Feedback")]
        public string Feedback { get; set; }
        [Display(Name = "Uppladdad datum")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}")]
        public DateTime UploadDateTime { get; set; }
        public DateTime Deadline { get; set; }
        public string ApplicationUserId { get; set; }
        public int? CourseId { get; set; }
        public int? ModuleId { get; set; }
        public int? ActivityId { get; set; }
    }
}
