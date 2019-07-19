using LMS.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Core.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        public DateTime EndDateTime { get; set; }
        
        //Navigation properties
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public ICollection<Module> Modules { get; set; }
        public ICollection<Document> Documents { get; set; }
    }
}
