using System.Globalization;
using System.Windows.Controls;

namespace UserControls.Validators
{
    internal class DoubleValueValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double v;

            string str = value.ToString();

            if (!string.IsNullOrEmpty(str) && str[str.Length - 1].Equals('.'))
                str += "0";

            if (!double.TryParse(str, new CultureInfo("en-US"), out v))
            {
                return new ValidationResult(false, "Not a number!");
            }
            else
            {
                return new ValidationResult(true, "");
            }
        }
    }
}
