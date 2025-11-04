using System.ComponentModel.DataAnnotations;

namespace WGU_App_RileyJuniewic.Data.Misc.Attributes;

public class RequireNonDefaultAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;
        var type = value.GetType();

        if (Equals(value, Activator.CreateInstance(Nullable.GetUnderlyingType(type) ?? type)))
        {
            return new ValidationResult($"The {validationContext.DisplayName} field is required.", memberNames: new string[] { $"{validationContext.MemberName}" });
        }
        
        return ValidationResult.Success;
    }
}