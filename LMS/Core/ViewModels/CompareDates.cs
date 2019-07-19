using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Core.ViewModels
{
    public class CompareDates : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var currentView = (LmsViewModel)context.ObjectInstance;
            // var dateTime = (DateTime)value;
            var endDateTime = currentView.EndDateTime;
            var startDateTime = currentView.StartDateTime;
            if(startDateTime > endDateTime)
            {
                return new ValidationResult("Slutdatum måste vara efter Startdatum");
            }
            return ValidationResult.Success;
        }
    }
}
