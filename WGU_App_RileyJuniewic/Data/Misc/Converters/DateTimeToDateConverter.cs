using System.Globalization;

namespace WGU_App_RileyJuniewic.Data.Misc.Converters;

public class DateTimeToDateConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var date = value as DateTime?;
        if (date is null) return "";

        return date.Value.ToString("MMMM dd, yyyy");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}