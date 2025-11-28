using ContourSearcher.UI.Constant;
using ContourSearcher.UI.ViewModels.Pages.Modules.Base;
using ContourSearcherBusinessLayer;

namespace ContourSearcher.UI.ViewModels.Pages.Modules
{
    internal class BlurDetectionModuleViewModel : ModuleBaseViewModel
    {
        #region Properties

        #endregion

        #region IDataErrorInfo

        #endregion

        #region Ctor
        public BlurDetectionModuleViewModel(ICVSystem cVSystem) : this()
        {
            CVSystem = cVSystem ?? throw new ArgumentNullException(nameof(cVSystem));
        }

        public BlurDetectionModuleViewModel()
        {
            ModuleName = Constants.BlurDetection.Name;
        }
        #endregion

        #region Methods

        #endregion
    }
}
