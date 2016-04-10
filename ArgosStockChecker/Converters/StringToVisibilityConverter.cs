using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ArgosStockChecker.Converters
{
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strVale = value as string;

            return (string.IsNullOrEmpty(strVale) ? Visibility.Collapsed : Visibility.Visible);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
