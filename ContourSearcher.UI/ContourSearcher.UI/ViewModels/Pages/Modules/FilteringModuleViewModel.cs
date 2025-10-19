using ContourSearcher.UI.Constant;
using ContourSearcher.UI.ViewModels.Models.Filters;
using ContourSearcher.UI.ViewModels.Pages.Modules.Base;
using ContourSearcherBusinessLayer;
using CSharpBusinessLayer.Validators;
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

        private string m_FilteringWindowName;
        #endregion

        #region Properties
        public ObservableCollection<FilterViewModel> Filters 
        { get => m_filters; set => Set(ref m_filters, value); }

        public string FilteringWindowName 
        {
            get=> m_FilteringWindowName; 
            set=> Set(ref m_FilteringWindowName, value);
        }
        #endregion

        #region IData Error

        public override string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(FilteringWindowName):
                        SetValidArrayValue(0, ValidationHelper.ValidateEmptyText(FilteringWindowName, out error));
                        break;
                }
                return error;
            }
        }

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
            m_FilteringWindowName = Constants.DEFAULT_INPUT_VALUE;
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
