using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace UserControls.Validators
{
    public class DoubleValueRangeValidator : ValidationRule, INotifyPropertyChanged
    {
        private double m_max;
        private double m_min;

        public double Max { get => m_max; set => Set(ref m_max, value); }
        public double Min { get => m_min; set => Set(ref m_min, value); }

        public event PropertyChangedEventHandler? PropertyChanged;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double v = 0;
            string str = value.ToString();

            if (!string.IsNullOrEmpty(str) && str[str.Length - 1].Equals('.'))
                str += "0";

            if (!double.TryParse(str, new CultureInfo("en-US"), out v))
            {
                return new ValidationResult(false, "Not a number!");
            }
            else if (v < Min)
            {
                return new ValidationResult(false, "Not in Bounds of Min!");
            }
            else if (v > Max)
            {
                return new ValidationResult(false, "Not in Bounds of Max!");
            }
            else
            {
                return new ValidationResult(true, "");
            }
        }

        private bool Set(ref double field, double value, [CallerMemberName] string propName = "")
        {
            if (field == value)
                return false;

            field = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            return true;
        }
    }
}
