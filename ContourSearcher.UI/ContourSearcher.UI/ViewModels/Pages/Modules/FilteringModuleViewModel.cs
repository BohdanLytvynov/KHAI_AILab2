using ContourSearcher.UI.Constant;
using ContourSearcher.UI.ViewModels.Pages.Modules.Base;
using ContourSearcherBusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContourSearcher.UI.ViewModels.Pages.Modules
{
    internal class FilteringModuleViewModel : ModuleBaseViewModel
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Ctor
        public FilteringModuleViewModel(ICVSystem cvSystem) : this()
        {
            CVSystem = cvSystem ?? throw new ArgumentNullException(nameof(cvSystem));
        }

        public FilteringModuleViewModel()
        {
            #region Init Fields
            ModuleName = Constants.Filtering.Name;
            #endregion
        }
        #endregion

        #region Methods

        #endregion
    }
}
