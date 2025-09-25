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
    internal class DoubleToStringFloatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string delim = parameter.ToString();
            var str = value.ToString();

            if (!str.Contains(delim.ToCharArray().FirstOrDefault()))
                return 0;

            var arr = str?.Split(delim.ToCharArray());

            if (arr != null && arr.Length == 2)
                return arr[1];
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double v;

            if (double.TryParse(parameter.ToString(), out v))
            { 
                return v;
            }

            return DependencyProperty.UnsetValue;

        }
    }
}
