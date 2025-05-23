using ContourSearcher.UI.Enums;
using CSharpBusinessLayer.Validators;
using MVVMBase.Attributes;
using MVVMBase.Commands;
using MVVMBase.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ContourSearcher.UI.ViewModels.Pages
{
    internal class ImageProcessingViewModel : ViewModelBase
    {
        #region Fields
        private const string ORIGINAL_IMAGE_NAME = "Original";
        private const string SMOOTHED_IMAGE_NAME = "Smoothed Image";
        private const string DEFAULT_INPUT_VALUE = "Enter value";

        private string m_ImageLoadingWindowName;
        private string m_ImageSmoothingWindowName;
        private bool m_useExistingWindowT2;
        private string m_SourceWindow;

        private ImageToLoadType m_imageToLoadType;
        private ObservableCollection<string> m_WindowNames;
        private SmoothingType m_SmoothingType;

        private string m_size1;
        private string m_size2;

        private string m_sigma1;
        private string m_sigma2;
        #endregion

        #region Properties

        public ImageToLoadType ImageToLoadType
        { get => m_imageToLoadType; set => Set(ref m_imageToLoadType, value); }

        public ObservableCollection<string> WindowNames
        { get => m_WindowNames; set => m_WindowNames = value; }

        [ValidateProperty]
        public string ImageLoadingWindowName
        { get => m_ImageLoadingWindowName; set => Set(ref m_ImageLoadingWindowName, value); }

        [ValidateProperty]
        public string ImageSmoothingWindowName
        { get => m_ImageSmoothingWindowName; set => Set(ref m_ImageSmoothingWindowName, value); }

        public bool UseExistsingWindowT2
        { get => m_useExistingWindowT2; set => Set(ref m_useExistingWindowT2, value); }

        public string ImgSource
        { get => m_SourceWindow; set => Set(ref m_SourceWindow, value); }

        public SmoothingType SmoothingType
        { get => m_SmoothingType; set => Set(ref m_SmoothingType, value); }

        [ValidateProperty]
        public string Size1 { get => m_size1; set => Set(ref m_size1, value); }

        [ValidateProperty]
        public string Size2 { get => m_size2; set => Set(ref m_size2, value); }

        [ValidateProperty]
        public string Sigma1 { get => m_sigma1; set => Set(ref m_sigma1, value); }

        [ValidateProperty]
        public string Sigma2 { get => m_sigma2; set => Set(ref m_sigma2, value); }

        #endregion

        #region IData Error Info
        public override string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case nameof(ImageLoadingWindowName):
                        SetValidArrayValue(0, ValidationHelper.ValidateEmptyText(ImageLoadingWindowName, out error));
                        break;
                    case nameof(ImageSmoothingWindowName):
                        SetValidArrayValue(1, ValidationHelper.ValidateEmptyText(ImageSmoothingWindowName, out error));
                        break;
                    case nameof(Size1):
                        SetValidArrayValue(2, ValidationHelper.ValidateNumber(Size1, out error, DEFAULT_INPUT_VALUE));
                        break;
                    case nameof(Size2):
                        SetValidArrayValue(3, ValidationHelper.ValidateNumber(Size2, out error, DEFAULT_INPUT_VALUE));
                        break;
                    case nameof(Sigma1):
                        SetValidArrayValue(4, ValidationHelper.ValidateNumber(Sigma1, out error, DEFAULT_INPUT_VALUE));
                        break;
                    case nameof(Sigma2):
                        SetValidArrayValue(5, ValidationHelper.ValidateNumber(Sigma2, out error, DEFAULT_INPUT_VALUE));
                        break;
                }

                return error;
            }
        }
        #endregion

        #region Commands

        public ICommand OnAddToImageProcessingPressed { get; }

        public ICommand OnSmoothImageButtonPressed { get; }

        #endregion

        #region Ctor
        public ImageProcessingViewModel()
        {
            InitValidArray(this);

            m_imageToLoadType = ImageToLoadType.CV_LOAD_IMAGE_COLOR;
            m_ImageLoadingWindowName = ORIGINAL_IMAGE_NAME;
            m_ImageSmoothingWindowName = SMOOTHED_IMAGE_NAME;

            m_WindowNames = new ObservableCollection<string>();
            m_SourceWindow = DEFAULT_INPUT_VALUE;
            m_useExistingWindowT2 = false;
            m_SmoothingType = SmoothingType.CV_BLUR_NO_SCALE;
            m_size1 = DEFAULT_INPUT_VALUE;
            m_size2 = DEFAULT_INPUT_VALUE;
            m_sigma1 = DEFAULT_INPUT_VALUE;
            m_sigma2 = DEFAULT_INPUT_VALUE;

            OnAddToImageProcessingPressed = new Command(
                OnAddToImageProcessingExecute,
                CanOnAddToImageProcessingExecute
                );

            OnSmoothImageButtonPressed = new Command(
                OnSmoothButtonPressedExecute,
                CanOnSmoothButtonPressedExecute
                );
        }
        #endregion

        #region Methods

        #region Image Processing
        private bool CanOnAddToImageProcessingExecute(object p) =>
            ValidateField(0);

        private void OnAddToImageProcessingExecute(object p)
        {
            WindowNames.Add(ImageLoadingWindowName);
            //m_CleanImagesTask.Start();
            //OpenCVWrapper.LoadImageToOpenCV(PathToImg, ImageLoadingWindowName, (int)m_imageToLoadType);
            //OpenCVWrapper.DisplayImageInWindow(ImageLoadingWindowName, ImageLoadingWindowName);
        }
        #endregion

        #region Smoothing

        private bool CanOnSmoothButtonPressedExecute(object p) =>
            ValidateFields(0, 5);

        private void OnSmoothButtonPressedExecute(object p)
        {
            //WindowNames.Add(ImageSmoothingWindowName);
            //OpenCVWrapper.SmoothImage(ImgSource, ImageSmoothingWindowName, (int)SmoothingType,
            //    int.Parse(Size1),
            //    int.Parse(Size2),
            //    double.Parse(Sigma1)
            //    , double.Parse(Sigma2));

            //OpenCVWrapper.DisplayImageInWindow(ImageSmoothingWindowName, ImageSmoothingWindowName);
        }

        #endregion

        #endregion
    }
}
