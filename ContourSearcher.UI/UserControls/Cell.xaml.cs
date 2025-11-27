using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using UserControls.Helpers;

namespace UserControls
{
    /// <summary>
    /// Interaction logic for Cell.xaml
    /// </summary>
    public partial class Cell : UserControl, INotifyPropertyChanged, IDataErrorInfo
    {
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

        #region IDataErrorInfo
        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                double v;
                switch (columnName)
                {
                    case nameof(ValueString):
                        if (double.TryParse(ValueString, out v))
                        {
                            Value = v;
                        }
                        else
                        {
                            error = "Not a number!";
                        }
                        break;
                }
                return error;
            }
        }
        #endregion

        #region Fields
        private string m_ValueString;
        #endregion

        #region Properties
        public double Value { get; set; }
       
        public string ValueString 
        {
            get=> m_ValueString;
            set=> Set(ref m_ValueString, value);
        }
        #endregion

        #region Ctor

        public Cell(string value)
        {
            InitializeComponent();
            m_ValueString = string.Empty;
            ValueString = value;
        }

        #endregion
    }
}
