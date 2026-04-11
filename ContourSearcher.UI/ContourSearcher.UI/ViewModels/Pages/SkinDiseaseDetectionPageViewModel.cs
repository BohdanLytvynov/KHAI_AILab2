using MVVMBase.ViewModels;

namespace ContourSearcher.UI.ViewModels.Pages
{
    internal class SkinDiseaseDetectionPageViewModel : ViewModelBase
    {
        #region Fields
        string m_test = nameof(SkinDiseaseDetectionPageViewModel);
        #endregion

        #region Properties
        public string Test 
        { get => m_test; set => Set(ref m_test, value); }
        #endregion

        #region Ctor

        #endregion

        #region Methods

        #endregion
    }
}
