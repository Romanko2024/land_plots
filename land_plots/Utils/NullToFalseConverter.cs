using System;
using System.Windows.Data;
using System.Globalization;

namespace LandManagementApp.Utils
{
    public class NullToFalseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value != null;  // повертає true якщо об'єкт не null

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
