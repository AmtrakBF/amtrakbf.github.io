using System.Globalization;

namespace WGU_App_RileyJuniewic.Data.Misc.Converters;

public class NullToBooleanConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var parameterString = parameter as string;
        var inverse = bool.TryParse(parameterString, out var parameterBoolValue) ? parameterBoolValue : false;

        if (value is null || (value is string str && str.Trim() == "") ||
            (value is IEnumerable<object> list && !list.Any()) || (value is int num && num == -1))
            return inverse ? true : false;
        return !inverse ? true : false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}