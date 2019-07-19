using LMS.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Core.ViewModels
{
    public class ActivityEndInRange : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            DateTime dateTime;

            var activity = (LmsViewModel)context.ObjectInstance;
            var moduleStart = activity.ModuleStart;
            var moduleEnd = activity.ModuleEnd;

            var isValid = DateTime.TryParse(Convert.ToString(value),
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out dateTime);

            if (!isValid || dateTime > moduleEnd || dateTime < moduleStart || dateTime < activity.StartDateTime)
            {
                return new ValidationResult("Slutdatum måste ligga inom modultiden och efter aktivitetens start");
            }

            return ValidationResult.Success;
        }
    }
}
