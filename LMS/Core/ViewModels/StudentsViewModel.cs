using LMS.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Core.ViewModels
{
    public class StudentsViewModel
    {
        public string Id { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "{0}et måste vara minst {2} och max {1} tecken långt.", MinimumLength = 1)]
        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "{0}et måste vara minst {2} och max {1} tecken långt.", MinimumLength = 2)]
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "Kurs")]
        public string Course { get; set; }
        [Display(Name = "Kurs")]
        public int? CourseId { get; set; }
        [Display(Name = "Roll")]
        public string RoleName { get; set; }
    }
}
