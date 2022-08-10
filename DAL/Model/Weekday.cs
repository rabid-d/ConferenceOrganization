using System.ComponentModel.DataAnnotations;

namespace ReportViewerMvcWebApplication.Models;

public class WeekdayAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return false;
        }
        DateTime date = (DateTime)value;
        if ((date.DayOfWeek == DayOfWeek.Saturday) || (date.DayOfWeek == DayOfWeek.Sunday))
        {
            return false;
        }
        return true;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Value cannot be null");
        }
        DateTime date = (DateTime)value;
        if ((date.DayOfWeek == DayOfWeek.Saturday) || (date.DayOfWeek == DayOfWeek.Sunday))
        {
            return new ValidationResult("You can't select weekends");
        }
        return ValidationResult.Success;
    }
}
