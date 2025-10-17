using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mat = UserControls.Math.Matrix<double>;

namespace UserControls
{
    /// <summary>
    /// Interaction logic for MatrixPresenter.xaml
    /// </summary>
    public partial class MatrixPresenter : UserControl, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Fields
        private string m_RowsString;
        private string m_ColumnsString;

        private bool m_ValueFlow;
        private bool m_TextBoxFlow;
        #endregion

        #region Properties
        public string RowsString 
        {
            get=> m_RowsString;
            set=> Set(ref m_RowsString, value);
        }

        public string ColumnsString
        { 
            get => m_ColumnsString;
            set => Set(ref m_ColumnsString, value);
        }
        #endregion

        #region Dependency Properties

        public double RowHeight
        {
            get { return (double)GetValue(RowHeightProperty); }
            set { SetValue(RowHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RowHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowHeightProperty;

        public bool Valid
        {
            get { return (bool)GetValue(ValidProperty); }
            set { SetValue(ValidProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Valid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValidProperty;



        public bool CallGetValueFunction
        {
            get { return (bool)GetValue(CallGetValueFunctionProperty); }
            set { SetValue(CallGetValueFunctionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CallGetValueFunction.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CallGetValueFunctionProperty;

        public Action<Mat> GetMatrixFunction
        {
            get { return (Action<Mat>)GetValue(GetMatrixFunctionProperty); }
            set { SetValue(GetMatrixFunctionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GetMatrixFunction.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GetMatrixFunctionProperty;

        public Mat Matrix
        {
            get { return (Mat)GetValue(MatrixProperty); }
            set { SetValue(MatrixProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Matrix.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MatrixProperty;

        #endregion

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

                if (m_ValueFlow)
                    return error;

                uint r, c = 0;
                switch (columnName)
                {
                    case nameof(RowsString):
                    case nameof(ColumnsString):
                        if (uint.TryParse(RowsString, out r) && uint.TryParse(ColumnsString, out c))
                        {
                            m_TextBoxFlow = true;
                            var current = GetMatrix(this.Content);
                            Mat matrix = new Mat(r, c, 0);
                            matrix.FillCurrent(current);
                            this.Matrix = matrix;
                            m_TextBoxFlow = false;
                            Valid  = true;
                        }
                        else
                        {
                            error = "Incorrect Input";
                            Valid = false;
                        }
                        break;
                }
                return error;
            }
        }
        #endregion

        #region Ctor

        static MatrixPresenter()
        {
            #region Init DP
            MatrixProperty =
            DependencyProperty.Register("Matrix", typeof(Mat),
                typeof(MatrixPresenter), new PropertyMetadata(new Mat(), OnMatrixPropertyChanged));

            RowHeightProperty =
            DependencyProperty.Register("RowHeight", typeof(double), 
            typeof(MatrixPresenter), new PropertyMetadata((double)0, OnRowHeightPropertyChanged));

            ValidProperty =
            DependencyProperty.Register("Valid", typeof(bool), 
            typeof(MatrixPresenter), new PropertyMetadata(false, OnValidPropertyChanged));

            CallGetValueFunctionProperty =
            DependencyProperty.Register("CallGetValueFunction", 
            typeof(bool), typeof(MatrixPresenter), new PropertyMetadata(false, OnCallGetValueFunctionPropertyChanged));

            GetMatrixFunctionProperty =
            DependencyProperty.Register("GetMatrixFunction", 
            typeof(Action<Mat>), typeof(MatrixPresenter),
            new PropertyMetadata(default, OnMatrixFunctionPropertyChanged));
            #endregion
        }
        
        public MatrixPresenter()
        {
            InitializeComponent();
            m_ColumnsString = string.Empty;
            m_RowsString = string.Empty;
            m_ValueFlow = false;
            m_TextBoxFlow = false;
        }

        #endregion

        #region Methods
        private void DrawMatrix(Mat src)
        {
            if (src == null)
                return;

            double width = this.Content.ActualWidth - (this.Content.Margin.Right + this.Content.Margin.Left);
            double height = this.RowHeight;

            if(width == 0 || height == 0)
                return;

            uint rows = src.Rows;
            uint cols = src.Cols;

            double cellwidth = width/(double)cols;
            for (uint i = 0; i < rows; i++)
            {
                MatrixRow matrixRow = new MatrixRow(width, height, cellwidth);
                for (uint j = 0; j < cols; j++)
                {
                    matrixRow.AddCell(src[i, j].ToString());
                }
                this.Content.Children.Add(matrixRow);
            }
        }

        private static void OnMatrixFunctionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (MatrixPresenter)d;
            This.GetMatrixFunction = (Action<Mat>)e.NewValue;
        }

        private static void OnRowHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (MatrixPresenter)d;
            This.RowHeight = (double)e.NewValue;
        }
        
        private static void OnMatrixPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (MatrixPresenter)d;
            if (e.NewValue == null)
                return;

            This.Matrix = (Mat)e.NewValue;
            This.DrawMatrix(This.Matrix);
            This.Valid = true;

            if (This.m_TextBoxFlow)
                return;

            This.m_ValueFlow = true;

            This.RowsString = This.Matrix.Rows.ToString();
            This.ColumnsString = This.Matrix.Cols.ToString();

            This.m_ValueFlow = false;
        }

        private static void OnValidPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (MatrixPresenter)d;
            var v = (bool)e.NewValue;
            if(v != This.Valid)
                This.Valid = v;
        }

        private static void OnCallGetValueFunctionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var This = (MatrixPresenter)d;
            var v = (bool)e.NewValue;
            if (This.CallGetValueFunction != v)
            {
                var res = GetMatrix(This.Content);

                This.GetMatrixFunction?.Invoke(res);
                This.CallGetValueFunction = v;
            }
        }

        #endregion

        private void ClearContent()
        {
            foreach (var item in Content.Children)
            {
                (item as MatrixRow).Clear();
            }

            this.Content.Children.Clear();
        }

        private void Control_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ClearContent();

            DrawMatrix(this.Matrix);
        }

        private static Mat GetMatrix(StackPanel content)
        {
            Mat res = new Mat();
            List<List<double>> m = new List<List<double>>();
            foreach (var row in content.Children)
            {
                var r = row as MatrixRow;
                var list = r.GetRow();
                m.Add(list);
            }
            res.Populate(m);

            return res;
        }
    }
}
