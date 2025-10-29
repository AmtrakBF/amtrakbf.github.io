using System.ComponentModel.DataAnnotations;

namespace WGU_App_RileyJuniewic.Data.Misc.Attributes;

public class DateTimeComparerAttribute(string property, bool shouldBeLarger) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var comparisonProperty = validationContext.ObjectType.GetProperty(property);

        if (comparisonProperty == null)
        {
            return new ValidationResult($"The {comparisonProperty} property is null");
        }

        var comparisonValue = comparisonProperty.GetValue(validationContext.ObjectInstance);

        var validValue1 = DateTime.TryParse(value?.ToString(), out var value1);
        var validValue2 = DateTime.TryParse(comparisonValue?.ToString(), out var value2);

        if (!validValue1 || !validValue2)
        {
            return new ValidationResult("Value is not of the type DateTime");
        }

        if (shouldBeLarger && value1 <= value2)
            return new ValidationResult(ErrorMessage ?? $"Value cannot be lower than {value2}", memberNames: new string[] { $"{validationContext.MemberName}" });

        if (!shouldBeLarger && value1 >= value2)
            return new ValidationResult(ErrorMessage ?? $"Value cannot be higher than {value2}", memberNames: new string[] { $"{validationContext.MemberName}" });

        return ValidationResult.Success;
    }
}