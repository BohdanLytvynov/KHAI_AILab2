using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UserControls
{
    /// <summary>
    /// Interaction logic for Slider.xaml
    /// </summary>
    public partial class Slider : UserControl, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Fields
        private string m_IntegerPartValueString;
        private string m_FloatPartValueString;
        #endregion

        #region Properties

        public string IntegerPartValueString
        {
            get => m_IntegerPartValueString;
            set
            {
                Set(ref m_IntegerPartValueString, value);
            }
        }

        public string FloatPartValueString
        {
            get => m_FloatPartValueString;
            set
            {
                Set(ref m_FloatPartValueString, value);
            }
        }

        #endregion

        #region DependencyProperties

        public string Deliminator
        {
            get { return (string)GetValue(DeliminatorProperty); }
            set { SetValue(DeliminatorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Deliminator.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeliminatorProperty;

        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelTextProperty;

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty;

        public Style BorderStyle
        {
            get { return (Style)GetValue(BorderStyleProperty); }
            set { SetValue(BorderStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderStyleProperty;

        public Style StackVerticalStyle
        {
            get { return (Style)GetValue(StackVerticalStyleProperty); }
            set { SetValue(StackVerticalStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StackVerticalStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StackVerticalStyleProperty;

        public Style StackHorizontalStyle
        {
            get { return (Style)GetValue(StackHorizontalStyleProperty); }
            set { SetValue(StackHorizontalStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StackHorizontalStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StackHorizontalStyleProperty;

        public Style LabelStyle
        {
            get { return (Style)GetValue(LabelStyleProperty); }
            set { SetValue(LabelStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelStyleProperty;

        public Style IntegerPartTextBoxStyle
        {
            get { return (Style)GetValue(IntegerPartTextBoxStyleProperty); }
            set { SetValue(IntegerPartTextBoxStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IntegerPartTextBoxStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IntegerPartTextBoxStyleProperty;

        public Style FloatPartTextBoxStyleProperty
        {
            get { return (Style)GetValue(FloatPartTextBoxStylePropertyProperty); }
            set { SetValue(FloatPartTextBoxStylePropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FloatPartTextBoxStyleProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FloatPartTextBoxStylePropertyProperty;

        public Style SliderStyle
        {
            get { return (Style)GetValue(SliderStyleProperty); }
            set { SetValue(SliderStyleProperty, value); }
        }

        #region IDataErrorInfo Implementation
        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case nameof(IntegerPartValueString):
                        int v;
                        if (!int.TryParse(IntegerPartValueString, out v))
                        {
                            error = "Not a number!";
                        }
                        else
                        {
                            var value = this.Value.ToString();
                            var arr = value.Split(',');
                            if (value.Contains(','))
                            {
                                this.Value = double.Parse(IntegerPartValueString + "," + arr[1]);
                            }
                            else
                            {
                                this.Value = double.Parse(IntegerPartValueString);
                            }
                        }
                        break;
                    case nameof(FloatPartValueString):
                        if (!isNumbers(FloatPartValueString, out error)) { }
                        else
                        {
                            var value = this.Value.ToString();
                            var arr = value.Split(',');
                            if (value.Contains(','))
                            {
                                this.Value = double.Parse(arr[0] + "," + FloatPartValueString);
                            }
                        }
                        break;
                }
                return error;
            }
        }

        private bool isNumbers(string value, out string error)
        {
            error = string.Empty;
            foreach (var item in value)
            {
                if (!Char.IsDigit(item))
                {
                    error = "Not a number!";
                    return false;
                }
            }

            return true;
        }

        #endregion

        // Using a DependencyProperty as the backing store for SliderStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliderStyleProperty;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propName = "")
        {
            if (field == null) throw new ArgumentNullException(nameof(field));

            if (value.Equals(field))
            {
                return false;
            }
            else
            {
                field = value;
                OnPropertyChanged(propName);
                return true;
            }
        }
        #endregion

        #endregion

        #region Ctor

        static Slider()
        {
            DeliminatorProperty =
            DependencyProperty.Register("Deliminator", typeof(string),
            typeof(Slider), new PropertyMetadata(",", OnDeliminatorPropertyChanged));

            LabelStyleProperty =
            DependencyProperty.Register("LabelStyle", typeof(Style),
                typeof(Slider), new PropertyMetadata(new Style(), OnLabelStyleChanged));

            IntegerPartTextBoxStyleProperty =
            DependencyProperty.Register("IntegerPartTextBoxStyle", typeof(Style),
                typeof(Slider), new PropertyMetadata(new Style(), OnIntegerPartTextBoxStyleChanged));

            FloatPartTextBoxStylePropertyProperty =
            DependencyProperty.Register("FloatPartTextBoxStyleProperty",
                typeof(Style), typeof(Slider), new PropertyMetadata(new Style(), OnFloatPartTextBoxStyleChanged));

            StackVerticalStyleProperty =
            DependencyProperty.Register("StackVerticalStyle", typeof(Style),
                typeof(Slider), new PropertyMetadata(new Style(), OnStackVerticalStyleChanged));

            StackHorizontalStyleProperty =
            DependencyProperty.Register("StackHorizontalStyle", typeof(Style),
                typeof(Slider), new PropertyMetadata(new Style(), OnStackHorizontalStyleChanged));

            SliderStyleProperty =
            DependencyProperty.Register("SliderStyle", typeof(Style),
            typeof(Slider), new PropertyMetadata(new Style(), OnSliderStyleChanged));

            ValueProperty =
            DependencyProperty.Register("Value", typeof(double),
                typeof(Slider), new PropertyMetadata(0.0, OnValuePropertyChanged));

            LabelTextProperty =
            DependencyProperty.Register("LabelText", typeof(string),
            typeof(Slider), new PropertyMetadata(string.Empty, OnLabelTextPropertyChanged));

            BorderStyleProperty =
            DependencyProperty.Register("BorderStyle", typeof(Style),
            typeof(Slider), new PropertyMetadata(new Style(), OnBorderStylePropertyChanged));
        }

        private static void OnBorderStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            This.BorderStyle = (Style)e.NewValue;
        }

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            var newValue = (double)e.NewValue;
            if (This.Value != newValue)
                This.Value = newValue;

            var currentValue = This.Value.ToString();
            var arr = currentValue.Split(',');
            if (currentValue.Contains(','))
            {
                This.IntegerPartValueString = arr[0];
                This.FloatPartValueString = arr[1];
            }
            else
            {
                This.IntegerPartValueString = arr[0];
            }
        }

        private static void OnDeliminatorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            This.Deliminator = e.NewValue.ToString();
        }

        public Slider()
        {
            m_IntegerPartValueString = "0";
            m_FloatPartValueString = "0";
            InitializeComponent();
        }

        #endregion

        #region Methods

        private static void OnLabelTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            This.LabelText = e.NewValue.ToString();
        }

        private static void OnStackHorizontalStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            This.StackHorizontal.Style = (Style)e.NewValue;
        }

        private static void OnStackVerticalStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            This.StackVertical.Style = (Style)e.NewValue;
        }

        private static void OnLabelStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            This.Label.Style = (Style)e.NewValue;
        }

        private static void OnFloatPartTextBoxStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            This.IntegerPart.Style = (Style)e.NewValue;
        }

        private static void OnIntegerPartTextBoxStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            This.FloatPart.Style = (Style)e.NewValue;
        }

        private static void OnSliderStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (Slider)d;
            This.SliderValue.Style = (Style)e.NewValue;
        }

        #endregion
    }
}
