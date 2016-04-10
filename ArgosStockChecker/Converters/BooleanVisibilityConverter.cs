using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ArgosStockChecker.Converters
{
    public class BooleanVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null)
                return Visibility.Collapsed;

            bool boolVal;

            bool.TryParse(value.ToString(), out boolVal);

            return boolVal ? Visibility.Visible : Visibility.Collapsed;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
