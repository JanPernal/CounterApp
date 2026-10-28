using System.Globalization;

namespace CounterApp.Converters;

public class HexToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string hex && !string.IsNullOrWhiteSpace(hex))
        {
            var baseColor = Color.FromArgb(hex);
            var hue = baseColor.GetHue();
            return parameter switch
            {
                "Light" => Color.FromHsla(hue, 0.70, 0.85),
                "Dark" => Color.FromHsla(hue, 1, 0.24),
                _ => baseColor
            };
        }

        return Colors.Gray;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}