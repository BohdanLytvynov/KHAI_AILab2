using ContourSearcher.UI.Constant;
using ContourSearcher.UI.Enums;
using ContourSearcher.UI.ViewModels.Pages.Modules.Base;
using ContourSearcherBusinessLayer;
using CSharpBusinessLayer.Validators;
using MVVMBase.Attributes;
using MVVMBase.Commands;
using System.Windows;
using System.Windows.Input;

namespace ContourSearcher.UI.ViewModels.Pages.Modules
{
    internal class EdgeDetectorViewModel : ModuleBaseViewModel
    {
        #region Fields
        private EdgeDetector m_EdgeDetevtor;
        private string m_EdgeDetectionWindowName;
        private double m_Threshold1;
        private double m_Threshold2;
        private double m_kernelSize;
        private double m_Scale;
        private double m_delta;
        private FilterBorderType m_FilterBorderType;
        private bool m_L2Factor;

        private Visibility m_SobelParams;
        private Visibility m_CannyParams;
        private Visibility m_LaplacianParams;
        #endregion

        #region Properties
        public EdgeDetector EdgeDetectorType 
        { 
            get => m_EdgeDetevtor;
            set
            { 
                Set(ref m_EdgeDetevtor, value);

                DisplayPropriateParams();
            }
        }

        [ValidateProperty]
        public string EdgeDetectionWindowName
        {
            get => m_EdgeDetectionWindowName;
            set => Set(ref m_EdgeDetectionWindowName, value);
        }

        public double Threshold1
        { get => m_Threshold1; set => Set(ref m_Threshold1, value); }

        public double Threshold2
        { get => m_Threshold2; set => Set(ref m_Threshold2, value); }

        public double KernelSize 
        { get => m_kernelSize; set => Set(ref m_kernelSize, value); }

        public double Scale 
        { get => m_Scale; set => Set(ref m_Scale, value); }

        public double Delta 
        { get => m_delta; set => Set(ref m_delta, value); }

        public FilterBorderType FilterBorderType 
        { get => m_FilterBorderType; set => Set(ref m_FilterBorderType, value); }

        public bool L2Factor 
        { get => m_L2Factor; set => Set(ref m_L2Factor, value); }

        public Visibility SobelParamsVisibility 
        { get => m_SobelParams; set => Set(ref m_SobelParams, value); }

        public Visibility CannyParamsVisibility 
        { get => m_CannyParams; set => Set(ref m_CannyParams, value); }

        public Visibility LaplacianParamsVisibility 
        { get => m_LaplacianParams; set => Set(ref m_LaplacianParams, value); }
        #endregion

        #region Commands
        public ICommand OnCalculateButtonPressed { get; }
        #endregion

        #region IDataErrorInfo

        public override string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(EdgeDetectionWindowName):
                        SetValidArrayValue(0, ValidationHelper.ValidateEmptyText(EdgeDetectionWindowName, out error));
                        break;
                }
                return error;
            }
        }

        #endregion

        #region Ctor
        public EdgeDetectorViewModel(ICVSystem cVSystem) : this()
        {
            CVSystem = cVSystem ?? throw new ArgumentNullException(nameof(cVSystem));
        }

        public EdgeDetectorViewModel()
        {
            InitValidArray(this);
            ModuleName = Constants.EdgeDetection.Name;
            m_EdgeDetectionWindowName = Constants.DEFAULT_INPUT_VALUE;
            m_Threshold1 = 100;
            m_kernelSize = 3;
            m_Scale = 1.0;
            m_delta = 0.0;
            m_FilterBorderType = FilterBorderType.BORDER_REFLECT_101;
            m_L2Factor = false;

            m_SobelParams = Visibility.Collapsed;
            m_CannyParams = Visibility.Collapsed;
            m_LaplacianParams = Visibility.Visible;

            OnCalculateButtonPressed = new Command
                (
                    OnCalculateButtonPressedExecute,
                    CanOnCalculateButtonPressedExecute
                );
        }
        #endregion

        #region Methods
        #region On Calculate Button Pressed
        private bool CanOnCalculateButtonPressedExecute(object p) => 
            EdgeDetectorType != EdgeDetector.None && ValidateField(0) && Threshold1 > 0;

        private void OnCalculateButtonPressedExecute(object p)
        {
            try
            {
                switch (EdgeDetectorType)
                {
                    case EdgeDetector.Sobel:
                        CVSystem.SobelEdgeDetect(ImgNameForProcessing, EdgeDetectionWindowName, Threshold1, 
                            (int)KernelSize, Scale, Delta, (int)FilterBorderType);
                        break;
                    case EdgeDetector.Canny:
                        CVSystem.CannyEdgeDetect(ImgNameForProcessing, EdgeDetectionWindowName, Threshold1, Threshold2, (int)KernelSize, L2Factor);
                        break;
                    case EdgeDetector.Laplacian:
                        CVSystem.LaplacianEdgeDetect(ImgNameForProcessing, EdgeDetectionWindowName, (int)KernelSize, Scale, Delta, (int)FilterBorderType);
                        break;
                }

                CVSystem.DisplayImageInWindow(EdgeDetectionWindowName, EdgeDetectionWindowName);
                OnRefreshActiveButtonPressedExecute(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Constants.MODULE_ERROR_MSG, ex.Message),
                    Constants.EdgeDetection.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        private void DisplayPropriateParams()
        {
            switch (EdgeDetectorType)
            {
                case EdgeDetector.None:
                    SobelParamsVisibility = Visibility.Collapsed;
                    CannyParamsVisibility = Visibility.Collapsed;
                    LaplacianParamsVisibility = Visibility.Visible;
                    break;
                case EdgeDetector.Sobel:
                    SobelParamsVisibility= Visibility.Visible;
                    CannyParamsVisibility= Visibility.Collapsed;
                    LaplacianParamsVisibility = Visibility.Visible;
                    break;
                case EdgeDetector.Canny:
                    SobelParamsVisibility = Visibility.Collapsed;
                    CannyParamsVisibility= Visibility.Visible;
                    LaplacianParamsVisibility = Visibility.Visible;
                    break;
                case EdgeDetector.Laplacian:
                    LaplacianParamsVisibility = Visibility.Collapsed;
                    SobelParamsVisibility = Visibility.Visible;
                    CannyParamsVisibility = Visibility.Collapsed;
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
