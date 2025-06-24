using System.Globalization;
using Microsoft.Maui.Graphics;

namespace SKANSAPUNG.MAUI.Converters;

public class AttendanceStatusToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string status) 
            return Colors.Black;

        return status.ToLowerInvariant() switch
        {
            "hadir" => Colors.Green,
            "sakit" => Colors.Orange,
            "izin" => Colors.Blue,
            "alpa" => Colors.Red,
            _ => Colors.Black
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
