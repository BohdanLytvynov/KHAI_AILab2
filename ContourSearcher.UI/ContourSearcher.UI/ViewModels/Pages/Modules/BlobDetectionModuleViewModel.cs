using ContourSearcher.UI.Constant;
using ContourSearcher.UI.ViewModels.Pages.Modules.Base;
using ContourSearcherBusinessLayer;
using CSharpBusinessLayer.Validators;
using MVVMBase.Attributes;
using MVVMBase.Commands;
using System.Windows;
using System.Windows.Input;

namespace ContourSearcher.UI.ViewModels.Pages.Modules
{
    internal class BlobDetectionModuleViewModel : ModuleBaseViewModel
    {
        #region Fields
        private string m_BlobDetectionWindowName;

        private bool m_filterByArea;
        private double m_MinArea;
        private double m_MaxArea;

        private bool m_filterByCircularity;
        private double m_MinCircularity;

        private bool m_FilterByColor;
        private double m_Color;
        #endregion

        #region Properties

        [ValidateProperty]
        public string BlobDetectionWindowName
        {
            get => m_BlobDetectionWindowName;
            set => Set(ref m_BlobDetectionWindowName, value);
        }

        public bool FilterByArea 
        { get => m_filterByArea; set => Set(ref m_filterByArea, value); }

        public double MinArea 
        { get => m_MinArea; set => Set(ref m_MinArea, value); }

        public double MaxArea 
        { get => m_MaxArea; set => Set(ref m_MaxArea, value); }

        public bool FilterByCircularity 
        { get => m_filterByCircularity; set => Set(ref m_filterByCircularity, value); }

        public double MinCircularity 
        { get => m_MinCircularity; set => Set(ref m_MinCircularity, value); }

        public bool FilterByColor 
        { get => m_FilterByColor; set => Set(ref m_FilterByColor, value); }

        public double Color 
        { get => m_Color; set => Set(ref m_Color, value); }

        #endregion

        #region IDataErrorInfo

        public override string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(BlobDetectionWindowName):
                        SetValidArrayValue(0, ValidationHelper.ValidateEmptyText(BlobDetectionWindowName, out error));
                        break;
                }
                return error;
            }
        }

        #endregion

        #region Commands
        public ICommand OnCalculateButtonPressed { get; }
        #endregion

        #region Ctor
        public BlobDetectionModuleViewModel(ICVSystem cVSystem) : this()
        {
            CVSystem = cVSystem ?? throw new ArgumentNullException(nameof(cVSystem));
        }

        public BlobDetectionModuleViewModel()
        {
            this.InitValidArray(this);
            m_BlobDetectionWindowName = Constants.DEFAULT_INPUT_VALUE;
            ModuleName = Constants.BlobDetection.Name;

            OnCalculateButtonPressed = new Command(
                OnCalculateButtonPressedExecute,
                CanOnCalculateButtonPressedExecute
                );
        }
        #endregion

        #region Methods
        #region On Calculate Button Pressed

        private bool CanOnCalculateButtonPressedExecute(object p)
        {
            if (FilterByArea)
            { 
                return MaxArea > 0 && MinArea > 0 && MaxArea > MinArea;
            }

            if (FilterByCircularity)
            {
                return MinCircularity > 0;
            }

            if (FilterByColor)
            { 
                return Color >= 0 && Color <= 255;
            }

            return false;
        }

        private void OnCalculateButtonPressedExecute(object p)
        {
            try
            {
                CVSystem.BlobDetect(ImgNameForProcessing, BlobDetectionWindowName, FilterByArea,
                    (float)MinArea, (float)MaxArea, FilterByCircularity, (float)MinCircularity, FilterByColor, (byte)Color );

                CVSystem.DisplayImageInWindow(BlobDetectionWindowName, BlobDetectionWindowName);
                OnRefreshActiveButtonPressedExecute(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Constants.MODULE_ERROR_MSG, ex.Message),
                Constants.EdgeDetection.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
        #endregion
    }
}
