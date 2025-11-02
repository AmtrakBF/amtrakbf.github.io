using System.Globalization;

namespace WGU_App_RileyJuniewic.Data.Misc.Converters;

public class ErrorsToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var errors = value as IDictionary<string, List<string>>;
        var propertyName = parameter as string;
        
        if (errors == null || string.IsNullOrEmpty(propertyName))
            return string.Empty;
            
        if (errors.TryGetValue(propertyName, out var propertyErrors))
            return string.Join(", ", propertyErrors);
            
        return string.Empty;
    }
    
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}