using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ContourSearcher.UI.CustomUserControls.Converters
{
    [ValueConversion(typeof(string), typeof(double))]
    internal class SliderMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string int_str = (string)values[0];
            string flt_str = (string)values[1];

            if(string.IsNullOrEmpty(int_str) && string.IsNullOrEmpty(flt_str))
                return DependencyProperty.UnsetValue;

            string concat = int_str + "," + flt_str;
            return float.Parse(concat);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var str = value.ToString();

            if (string.IsNullOrEmpty(str))
                return new object[] { DependencyProperty.UnsetValue, DependencyProperty.UnsetValue };

            if(str.Contains('.'))
                return str.Split('.');

            if(str.Contains(','))
                return str.Split(',');

            return new object[] { DependencyProperty.UnsetValue, DependencyProperty.UnsetValue }; ;
        }
    }
}
