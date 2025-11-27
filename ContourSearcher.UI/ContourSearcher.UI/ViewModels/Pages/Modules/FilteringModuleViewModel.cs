using ContourSearcher.UI.Constant;
using ContourSearcher.UI.Enums;
using ContourSearcher.UI.ViewModels.Models.Filters;
using ContourSearcher.UI.ViewModels.Pages.Modules.Base;
using ContourSearcherBusinessLayer;
using CSharpBusinessLayer.Validators;
using MVVMBase.Commands;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace ContourSearcher.UI.ViewModels.Pages.Modules
{
    internal class FilteringModuleViewModel : ModuleBaseViewModel
    {
        #region Fields
        private ObservableCollection<FilterViewModel> m_filters;

        private string m_FilteringWindowName;
        private FilterTypes m_SelectedFilterName;
        #endregion

        #region Properties
        public ObservableCollection<FilterViewModel> Filters
        { get => m_filters; set => Set(ref m_filters, value); }

        public FilterTypes SelectedFilterName
        { get => m_SelectedFilterName; set => Set(ref m_SelectedFilterName, value); }

        public string FilteringWindowName
        {
            get => m_FilteringWindowName;
            set => Set(ref m_FilteringWindowName, value);
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

        public ICommand OnApplyFiltersButtonPressed { get; }
        #endregion

        #region Ctor
        public FilteringModuleViewModel(ICVSystem cvSystem) : this()
        {
            CVSystem = cvSystem ?? throw new ArgumentNullException(nameof(cvSystem));
        }

        public FilteringModuleViewModel()
        {
            #region Init Fields
            Init();

            #endregion

            #region Init Commands
            OnAddFilterButtonPressed = new Command(
                    OnAddFilterButtonPressedExecute,
                    CanOnAddFilterButtonPressedExecute
                );

            OnApplyFiltersButtonPressed = new Command
                (
                    OnApplyAllFiltersButtonPressedExecute,
                    CanOnApplyAllFiltersButtonPressedExecute
                );
            #endregion
        }
        #endregion

        #region Methods
        #region On Add Filter Button Pressed

        private bool CanOnAddFilterButtonPressedExecute(object p)
        {
            foreach (var item in Filters)
            {
                item.ExternalCheck = ValidateField(0) && !string.IsNullOrEmpty(ImgNameForProcessing);
                item.Check();
            }

            return true;
        }

        private void OnAddFilterButtonPressedExecute(object p)
        {
            FilterViewModel filterViewModel = null;
            switch (SelectedFilterName)
            {
                case FilterTypes.Averaging_Filter:
                    filterViewModel = new AveragingFilterViewModel(Filters.Count + 1);
                    break;
                case FilterTypes.Simple_Blur:
                    filterViewModel = new SimpleBlurViewModel(Filters.Count + 1);
                    break;
                case FilterTypes.Custom_Filter:
                    filterViewModel = new CustomFilterViewModel(Filters.Count + 1);
                    break;
                case FilterTypes.Gaussian_Blur:
                    filterViewModel = new GaussianFilterViewModel(Filters.Count + 1);
                    break;
                case FilterTypes.Bilateral_Filter:
                    filterViewModel = new BilateralFilterViewModel(Filters.Count + 1);
                    break;
            }

            filterViewModel.OnApplyButtonPressedEvent += FilterViewModel_OnApplyButtonPressedEvent;
            filterViewModel.OnDeleteButtonPressedEvent += FilterViewModel_OnDeleteButtonPressedEvent;

            Filters.Add(filterViewModel);
        }

        private void FilterViewModel_OnDeleteButtonPressedEvent(FilterViewModel obj)
        {
            obj.OnDeleteButtonPressedEvent -= FilterViewModel_OnDeleteButtonPressedEvent;
            obj.OnApplyButtonPressedEvent -= FilterViewModel_OnApplyButtonPressedEvent;
            Filters.Remove(obj);

            RedrawFilterCollection();
        }

        private void FilterViewModel_OnApplyButtonPressedEvent(FilterViewModel obj)
        {
            if (!CanOnApplyAllFiltersButtonPressedExecute(null))
            {
                MessageBox.Show("Not all Filters are Valid!", "Contour Searcher", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ApplyFilter(ImgNameForProcessing, FilteringWindowName, obj);

            CVSystem.DisplayImageInWindow(FilteringWindowName, FilteringWindowName);
            OnRefreshActiveButtonPressedExecute(null);

            Filters.Remove(obj);
            RedrawFilterCollection();
        }

        #endregion

        #region On Apply All Filters Button Pressed

        private bool CanOnApplyAllFiltersButtonPressedExecute(object p)
        {
            if(Filters.Count == 0)
                return false;

            bool FiltersValid = true;
            foreach (var item in Filters)
            {
                if (!item.IsValid)
                {
                    FiltersValid = false;
                    break;
                }
            }

            return FiltersValid && ValidateField(0) && !string.IsNullOrEmpty(ImgNameForProcessing);
        }

        private void OnApplyAllFiltersButtonPressedExecute(object p)
        {
            int i = 0;
            foreach (var f in Filters)
            {
                if (i == 0)
                {
                    ApplyFilter(ImgNameForProcessing, FilteringWindowName, f);
                    continue;
                }

                ApplyFilter(FilteringWindowName, FilteringWindowName, f);
                ++i;
            }
            CVSystem.DisplayImageInWindow(FilteringWindowName, FilteringWindowName);
            Filters.Clear();
        }

        #endregion

        private void Init()
        {
            this.InitValidArray(1);
            ModuleName = Constants.Filtering.Name;
            m_filters = new ObservableCollection<FilterViewModel>();
            m_FilteringWindowName = Constants.DEFAULT_INPUT_VALUE;
        }

        private void RedrawFilterCollection()
        {
            List<FilterViewModel> filtersTemp = new List<FilterViewModel>();
            filtersTemp.AddRange(Filters);
            Filters.Clear();

            foreach (FilterViewModel filterViewModel in filtersTemp)
            {
                filterViewModel.ShowNumber = Filters.Count + 1;
                Filters.Add(filterViewModel);
            }

        }

        private void ApplyFilter(string src, string dest, FilterViewModel obj)
        {
            switch (obj.FilterType)
            {
                case FilterTypes.Averaging_Filter:
                    CVSystem.AverageImage(src, dest
                        , (int)obj.Depth, obj.KernelRows, obj.KernelColumns,
                        obj.AnchorX, obj.AnchorY, obj.Normalize, (int)obj.BorderType);
                    break;
                case FilterTypes.Simple_Blur:
                    CVSystem.Blur(src, dest,
                        obj.KernelRows, obj.KernelColumns, obj.AnchorX, obj.AnchorY,
                        (int)obj.BorderType);
                    break;
                case FilterTypes.Custom_Filter:
                    CustomFilterViewModel cfvm = (CustomFilterViewModel)obj;
                    CVSystem.CustomFilter(src, dest,
                        cfvm.GetMatrix().matrix, (int)obj.Depth, obj.AnchorX, obj.AnchorY,
                        cfvm.Delta, (int)obj.BorderType);
                    break;
                case FilterTypes.Gaussian_Blur:
                    GaussianFilterViewModel gfVM = (GaussianFilterViewModel)obj;
                    CVSystem.GaussianBlur(src, dest,
                        obj.KernelRows, obj.KernelColumns, gfVM.Sigma1, gfVM.Sigma2, (int)obj.BorderType);
                    break;
                case FilterTypes.Bilateral_Filter:
                    BilateralFilterViewModel biVM = (BilateralFilterViewModel)obj;
                    CVSystem.BilateralFilter(src, dest,
                        biVM.Diameter, biVM.SigmaColor, biVM.SigmaSpace, (int)obj.BorderType);
                    break;
            }
        }

        #endregion
    }
}
