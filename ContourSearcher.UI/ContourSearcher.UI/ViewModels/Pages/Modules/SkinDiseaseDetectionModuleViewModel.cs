using ContourSearcher.UI.Constant;
using ContourSearcher.UI.ViewModels.Pages.Modules.Base;
using ContourSearcherBusinessLayer;

namespace ContourSearcher.UI.ViewModels.Pages.Modules
{
    internal class SkinDiseaseDetectionModuleViewModel : ModuleBaseViewModel
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Ctor
        public SkinDiseaseDetectionModuleViewModel(ICVSystem cVSystem) : this()
        {
            CVSystem = cVSystem ?? throw new ArgumentNullException(nameof(cVSystem));
        }

        public SkinDiseaseDetectionModuleViewModel()
        {
            this.InitValidArray(this);
            ModuleName = Constants.SkinDiseaseDetection.Name;
        }
        #endregion

        #region Methods

        #endregion
    }
}
