using ContourSearcher.UI.Constant;
using ContourSearcher.UI.ViewModels.Pages.Modules.Base;
using ContourSearcherBusinessLayer;
using MVVMBase.Commands;
using System.Windows;
using System.Windows.Input;

namespace ContourSearcher.UI.ViewModels.Pages.Modules
{
    internal class BlurDetectionModuleViewModel : ModuleBaseViewModel
    {
        #region Fields
        private double m_blurDetectionResult;
        #endregion

        #region Properties
        public double BlurDetectionResult 
        { get => m_blurDetectionResult; set => Set(ref m_blurDetectionResult, value); }
        #endregion

        #region Commands
        public ICommand OnCalculateButtonPressed { get; }
        #endregion

        #region Ctor
        public BlurDetectionModuleViewModel(ICVSystem cVSystem) : this()
        {
            CVSystem = cVSystem ?? throw new ArgumentNullException(nameof(cVSystem));
        }

        public BlurDetectionModuleViewModel()
        {
            ModuleName = Constants.BlurDetection.Name;

            OnCalculateButtonPressed = new Command
                (
                    OnCalculateButtonPressedExecute,
                    CanOnCalculateButtonPressedExecute
                );
        }
        #endregion

        #region Methods

        #region On Calculate Button Pressed
        private bool CanOnCalculateButtonPressedExecute(object p) => !string.IsNullOrEmpty(ImgNameForProcessing);

        private void OnCalculateButtonPressedExecute(object p)
        {
            try
            {
                var res = CVSystem.GetVarianceOfLaplacian(ImgNameForProcessing);

                if (res == -1)
                {
                    MessageBox.Show($"Unable to process an image <{ImgNameForProcessing}>",
                                Constants.EdgeDetection.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                { 
                    BlurDetectionResult = res;
                }
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
