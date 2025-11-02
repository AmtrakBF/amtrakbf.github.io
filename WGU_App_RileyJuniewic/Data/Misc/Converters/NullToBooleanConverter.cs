using System.Globalization;

namespace WGU_App_RileyJuniewic.Data.Misc.Converters;

public class NullToBooleanConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var parameterString = parameter as string;
        var inverse = bool.TryParse(parameterString, out var parameterBoolValue) ? parameterBoolValue : false;

        if (inverse)
        {
            if (value is null)
                return true;
            return false;
        }

        if (value is null)
            return false;
        return true;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}