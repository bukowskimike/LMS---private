using LMS.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Core.ViewModels
{
    public class ActivityStartInRange : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            DateTime dateTime;

            var activity = (LmsViewModel)context.ObjectInstance;
            //var moduleStart = activity.ModuleStart;

            //var moduleEnd = activity.ModuleEnd;

            var isValid = DateTime.TryParse(Convert.ToString(value),
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out dateTime);

            if (!isValid || dateTime < activity.ModuleStart || dateTime > activity.ModuleEnd)
            {
                return new ValidationResult("Startdatum måste ligga inom modultiden");
            }

            return ValidationResult.Success;
        }
    }
}
