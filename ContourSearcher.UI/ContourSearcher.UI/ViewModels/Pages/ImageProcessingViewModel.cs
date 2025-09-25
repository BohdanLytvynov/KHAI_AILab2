using ContourSearcher.UI.Constant;
using ContourSearcher.UI.DataExchange;
using ContourSearcher.UI.Enums;
using ContourSearcher.UI.Helpers;
using ContourSearcherBusinessLayer;
using CSharpBusinessLayer.Validators;
using MVVMBase.Attributes;
using MVVMBase.Commands;
using MVVMBase.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ContourSearcher.UI.ViewModels.Pages
{
    internal class ImageProcessingViewModel : ViewModelBase
    {
        #region Fields

        private bool m_useDynamicMode;
        private string m_SourceWindow;

        private ObservableCollection<string> m_ActiveWindowNames;
        private SmoothingType m_SmoothingType;

        //private string m_size1;
        private int m_size1;
        private string m_size2;

        private string m_sigma1;
        private string m_sigma2;

        private ICVSystem m_CVSystem;
        #endregion

        #region Properties

        public ObservableCollection<string> ActiveWindows
        { get => m_ActiveWindowNames; set => m_ActiveWindowNames = value; }

        public bool UseDynamicMode 
        { get=>m_useDynamicMode; set=>Set(ref m_useDynamicMode, value); }

        public string ImgSourceForSmooth
        { get => m_SourceWindow; set => Set(ref m_SourceWindow, value); }

        public SmoothingType SmoothingType
        { 
            get => m_SmoothingType;
            set
            {
                Set(ref m_SmoothingType, value);

                if (UseDynamicMode)
                    SmoothImage();
            }
        }

        public int Size1
        {
            get=> m_size1;
            set 
            {
                Set(ref m_size1, value);

                if (UseDynamicMode)
                    SmoothImage();
            }
        }

        //[ValidateProperty]
        //public string Size1 
        //{ 
        //    get => m_size1;
        //    set 
        //    {
        //        Set(ref m_size1, value);

        //        if (UseDynamicMode)
        //            SmoothImage();
        //    }
        //}

        [ValidateProperty]
        public string Size2 
        {
            get => m_size2;
            set 
            {
                Set(ref m_size2, value); 
                
                if(UseDynamicMode)
                    SmoothImage();
            }
        }

        [ValidateProperty]
        public string Sigma1 
        {
            get => m_sigma1;

            set 
            { 
                Set(ref m_sigma1, value);

                if (UseDynamicMode)
                    SmoothImage();
            }
        }

        [ValidateProperty]
        public string Sigma2
        {
            get => m_sigma2;
            set
            {
                Set(ref m_sigma2, value);

                if(UseDynamicMode)
                    SmoothImage();
            }
        }

        #endregion

        #region IData Error Info
        public override string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    //case nameof(Size1):
                    //    SetValidArrayValue(0, ValidationHelper.ValidateNumber(Size1, out error, Constants.DEFAULT_INPUT_VALUE));
                    //    break;
                    case nameof(Size2):
                        SetValidArrayValue(1, ValidationHelper.ValidateNumber(Size2, out error, Constants.DEFAULT_INPUT_VALUE));
                        break;
                    case nameof(Sigma1):
                        SetValidArrayValue(2, ValidationHelper.ValidateNumber(Sigma1, out error, Constants.DEFAULT_INPUT_VALUE));
                        break;
                    case nameof(Sigma2):
                        SetValidArrayValue(3, ValidationHelper.ValidateNumber(Sigma2, out error, Constants.DEFAULT_INPUT_VALUE));
                        break;
                }

                return error;
            }
        }
        #endregion

        #region Commands

        public ICommand OnSmoothImageButtonPressed { get; }

        public ICommand OnRefreshActiveWindowsPressed { get; }

        #endregion

        #region Ctor
        public ImageProcessingViewModel(ICVSystem cVSystem) : this()
        {
            m_CVSystem = cVSystem;
        }

        public ImageProcessingViewModel()
        {
            InitValidArray(this);

            m_ActiveWindowNames = new ObservableCollection<string>();
            m_SourceWindow = string.Empty;
            m_useDynamicMode = false;
            m_SmoothingType = SmoothingType.CV_BLUR_NO_SCALE;
            m_size2 = Constants.DEFAULT_INPUT_VALUE;
            m_sigma1 = Constants.DEFAULT_INPUT_VALUE;
            m_sigma2 = Constants.DEFAULT_INPUT_VALUE;

            OnSmoothImageButtonPressed = new Command(
                OnSmoothButtonPressedExecute,
                CanOnSmoothButtonPressedExecute
                );

            OnRefreshActiveWindowsPressed = new Command(
                OnRefreshActiveButtonPressedExecute,
                CanOnRefreshActiveWindowsButtonPressedExecute
                );
        }
        #endregion

        #region Methods

        private void SmoothImage()
        {
            try
            {
                int size2 = int.Parse(Size2);

                double sigma1 = double.Parse(Sigma1);
                double sigma2 = double.Parse(Sigma2);

                m_CVSystem.SmoothImage(ImgSourceForSmooth, (int)SmoothingType, Size1, size2, sigma1, sigma2);
                m_CVSystem.DisplayImageInExistingWindow(ImgSourceForSmooth, ImgSourceForSmooth);
            }
            catch (Exception ex)
            {
#if DEBUG
                throw;
#endif
            }
        }

        #region On Smooth Button Pressed Execute

        private bool CanOnSmoothButtonPressedExecute(object p) =>
            ValidateFields(0, 3) && !UseDynamicMode;

        private void OnSmoothButtonPressedExecute(object p)
        {
            SmoothImage();
        }

        #endregion

        #region On Refresh Active Windows Pressed

        private bool CanOnRefreshActiveWindowsButtonPressedExecute(object p) => true;

        private void OnRefreshActiveButtonPressedExecute(object p)
        {
            var imgs = ShareData.GetItem<List<string>>(Constants.ACTIVE_WINDOW_LIST_COLLECTION);

            UIHelper.RefreshObservableCollection(ActiveWindows, imgs);
        }

        #endregion

        #endregion
    }
}
