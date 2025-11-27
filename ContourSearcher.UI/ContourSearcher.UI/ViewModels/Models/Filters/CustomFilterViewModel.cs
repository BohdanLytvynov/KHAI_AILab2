using Mat = UserControls.Math.Matrix<double>;

namespace ContourSearcher.UI.ViewModels.Models.Filters
{
    internal class CustomFilterViewModel : FilterViewModel
    {
        #region Fields
        private Mat m_kernel;
        private bool m_callGetMatrix;
        private Action<Mat> m_getMatrix;
        private double m_delta;
        #endregion

        #region Properties
        public bool CallGetMatrix
        {
            get => m_callGetMatrix;
            set => Set(ref m_callGetMatrix, value);
        }

        public Action<Mat> GetMatrixFunction
        {
            get => m_getMatrix;
            set => Set(ref m_getMatrix, value);
        }

        public Mat Kernel
        {
            get => m_kernel;
            set => Set(ref m_kernel, value);
        }

        public double Delta 
        { get => m_delta; set => Set(ref m_delta, value); }
        #endregion

        #region Ctor
        public CustomFilterViewModel(int showNumber) : base(showNumber) 
        {
            #region Init Fields
            Init();
            #endregion
        }

        public CustomFilterViewModel() : base()
        {
            Init();
        }
        #endregion

        #region Methods
        private void GetMatrix(Mat matrix)
        {
            Kernel = matrix;
        }

        public Mat GetMatrix()
        {
            CallGetMatrix = !CallGetMatrix;
            return Kernel;
        }

        private void Init()
        {
            FilterType = Enums.FilterTypes.Custom_Filter;
            FilterName = FilterType.ToString();
            m_kernel = new Mat(3, 3, 0);
            m_getMatrix = new Action<Mat>(GetMatrix);
        }
        #endregion
    }
}
