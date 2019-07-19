using LMS.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Core.ViewModels
{
    public class ModuleEndInRange : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            DateTime dateTime;

            var module = (ModuleViewModel)context.ObjectInstance;
            var courseStart = module.CourseStart;
            var courseEnd = module.CourseEnd;


            var isValid = DateTime.TryParse(Convert.ToString(value),
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out dateTime);

            if (!isValid || dateTime > courseEnd || dateTime < courseStart || dateTime < module.StartDateTime)
            {
                return new ValidationResult("Slutdatum måste ligga inom kurstiden och efter modulens start");
            }

            return ValidationResult.Success;
        }
    }
}
   
