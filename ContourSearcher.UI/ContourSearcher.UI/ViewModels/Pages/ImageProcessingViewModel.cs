using ContourSearcher.UI.Constant;
using ContourSearcher.UI.Enums;
using ContourSearcher.UI.PageManagers.Interfaces;
using ContourSearcher.UI.Views.Pages.Modules;
using CSharpBusinessLayer.Validators;
using MVVMBase.Attributes;
using MVVMBase.Commands;
using MVVMBase.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace ContourSearcher.UI.ViewModels.Pages
{
    internal class ImageProcessingViewModel : ViewModelBase
    {
        #region Fields
        private IPageManager m_pageManager;
        private Page m_HistogramPage;
        private Page m_EqualizerPage;
        private Page m_FilterPage;
        
        //private bool m_useDynamicMode;
        //private ICVSystem m_CVSystem;

        #region Smoothing

        private SmoothingType m_SmoothingType;

        private int m_size1;
        private int m_size2;

        private double m_sigma1;
        private double m_sigma2;

        #endregion

        #endregion

        #region Properties

        public Page HistogramPage { get => m_HistogramPage; set => Set(ref m_HistogramPage, value); }

        public Page EqualizerPage { get=>m_EqualizerPage; set=>Set(ref m_EqualizerPage, value); }

        public Page FilteringPage { get=> m_FilterPage; set => Set(ref m_FilterPage, value) ; }
        //public bool UseDynamicMode
        //{ get => m_useDynamicMode; set => Set(ref m_useDynamicMode, value); }

        #region Smoothing

        public SmoothingType SmoothingType
        {
            get => m_SmoothingType;
            set => Set(ref m_SmoothingType, value);
        }

        public int Size1
        {
            get => m_size1;
            set => Set(ref m_size1, value);
        }

        public int Size2
        {
            get => m_size2;
            set => Set(ref m_size2, value);
        }

        public double Sigma1
        {
            get => m_sigma1;

            set => Set(ref m_sigma1, value);
        }

        public double Sigma2
        {
            get => m_sigma2;
            set => Set(ref m_sigma2, value);
        }

        #endregion

        #endregion

        #region Commands

        public ICommand OnSmoothImageButtonPressed { get; }

        public ICommand OnRefreshActiveWindowsPressed { get; }

        public ICommand OnCalculateHistogramPressed { get; }

        #endregion
        
        #region Ctor
        public ImageProcessingViewModel(IPageManager pageManager) : this()
        {
            m_pageManager = pageManager ?? throw new ArgumentNullException(nameof(pageManager));
            m_HistogramPage = m_pageManager.GetPage(nameof(HistogramModule));
            m_EqualizerPage = m_pageManager.GetPage(nameof(EqualizerModule));
            m_FilterPage = m_pageManager.GetPage(nameof(FilteringModule));
        }

        public ImageProcessingViewModel()
        {
            InitValidArray(this);
            
            //m_useDynamicMode = false;
            m_SmoothingType = SmoothingType.CV_BLUR_NO_SCALE;

            OnSmoothImageButtonPressed = new Command(
                OnSmoothButtonPressedExecute,
                CanOnSmoothButtonPressedExecute
                );
        }
        #endregion

        #region Methods

        private void SmoothImage()
        {
            if (!CanSmooth())
                return;

            try
            {
                //m_CVSystem.SmoothImage(ImgNameForProcessing, (Int32)SmoothingType, Size1, Size2, Sigma1, Sigma2);
                //m_CVSystem.DisplayImageInExistingWindow(ImgNameForProcessing, ImgNameForProcessing);
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
            CanSmooth();

        private void OnSmoothButtonPressedExecute(object p)
        {
            SmoothImage();
        }

        #endregion
        
        

        private bool CanSmooth()
        {
            return Size1 > 0
                && Size2 > 0
                && Sigma1 > 0
                && Sigma2 > 0;
                //&& !string.IsNullOrEmpty(ImgNameForProcessing);
        }

        #endregion
    }
}
