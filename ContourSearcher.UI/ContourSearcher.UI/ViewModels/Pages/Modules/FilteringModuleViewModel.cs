using ContourSearcher.UI.Constant;
using ContourSearcher.UI.ViewModels.Models;
using ContourSearcher.UI.ViewModels.Pages.Modules.Base;
using ContourSearcherBusinessLayer;
using MVVMBase.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ContourSearcher.UI.ViewModels.Pages.Modules
{
    internal class FilteringModuleViewModel : ModuleBaseViewModel
    {
        #region Fields
        private ObservableCollection<FilterViewModel> m_filters;
        #endregion

        #region Properties
        public ObservableCollection<FilterViewModel> Filters 
        { get => m_filters; set => Set(ref m_filters, value); }
        #endregion

        #region Commands
        public ICommand OnAddFilterButtonPressed { get; }
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
            m_filters = new ObservableCollection<FilterViewModel>();
            #endregion

            #region Init Commands
            OnAddFilterButtonPressed = new Command(
                OnAddFilterButtonPressedExecute,
                CanOnAddFilterButtonPressedExecute
                );
            #endregion
        }
        #endregion

        #region Methods
        #region On Add Filter Button Pressed

        private bool CanOnAddFilterButtonPressedExecute(object p)
            => true;

        private void OnAddFilterButtonPressedExecute(object p)
        {
            FilterViewModel filterViewModel = new FilterViewModel(Filters.Count + 1);
            Filters.Add(filterViewModel);
        }

        #endregion
        #endregion
    }
}
