using System.ComponentModel.DataAnnotations;

namespace ReportViewerWebApi.ViewModels.ConferenceEditor;

public class WeekdayAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Value cannot be null");
        }
        ConferenceViewModel conference = (ConferenceViewModel)validationContext.ObjectInstance;
        for (DateTime day = conference.DateStart; day <= conference.DateEnd; day = day.AddDays(1))
        {
            if ((day.DayOfWeek == DayOfWeek.Saturday) || (day.DayOfWeek == DayOfWeek.Sunday))
            {
                return new ValidationResult("You can't select weekends");
            }
        }
        return ValidationResult.Success;
    }
}
