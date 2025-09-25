using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ContourSearcher.UI.CustomUserControls.Converters
{
    [ValueConversion(typeof(double), typeof(string))]
    internal class DoubleToStringIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string delim = parameter.ToString();
            var arr = value.ToString()?.Split(delim.ToCharArray());

            if (arr != null && arr.Length == 2)
                return arr[0];
            else if (arr != null && arr.Length == 1)
                return arr[0];

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double _out = 0;
            if (double.TryParse(value.ToString(), out _out))
            { 
                return _out;
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
