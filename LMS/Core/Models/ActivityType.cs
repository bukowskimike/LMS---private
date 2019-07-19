using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Models
{
    public class ActivityType
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        //Navigation properties
        public ICollection<Activity> Activities { get; set; }
    }
}