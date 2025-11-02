using System.Globalization;

namespace WGU_App_RileyJuniewic.Data.Misc.Converters;

public class ErrorsToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var errors = value as IDictionary<string, List<string>>;
        var propertyName = parameter as string;
        
        if (errors == null || string.IsNullOrEmpty(propertyName))
            return false;
            
        if (errors.TryGetValue(propertyName, out var propertyErrors))
            return propertyErrors.Count > 0;
            
        return false;
    }
    
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}