using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UserControls.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool v;
            if (bool.TryParse(value.ToString(), out v))
            { 
                if(v)
                    return Visibility.Visible;
                else 
                    return Visibility.Collapsed;
            }
            
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                if (visibility == Visibility.Collapsed)
                    return false;
                else if(visibility == Visibility.Visible)
                    return true;
            }

            return false;
        }
    }
}
