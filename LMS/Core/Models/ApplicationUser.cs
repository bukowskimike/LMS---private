using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        //Foreign key
        public int? CourseId { get; set; }
        public int? DocumentId { get; set; }
        //Navigation property
        public Course Course { get; set; }
        public Document Document { get; set; }
        

    }
}
