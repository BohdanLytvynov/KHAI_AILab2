namespace ContourSearcher.UI.ViewModels.Models.Filters
{
    internal class AveragingFilterViewModel : FilterViewModel
    {
        #region Fields
        
        #endregion

        #region Properties

        #endregion

        #region Ctor
        public AveragingFilterViewModel(int showNumber) : base(showNumber)
        {
            Init();
        }

        public AveragingFilterViewModel() : base()
        {
            Init();
        }
        #endregion

        #region Methods
        private void Init()
        { 
            FilterType = Enums.FilterTypes.Averaging_Filter;
            FilterName = FilterType.ToString();
        }
        #endregion
    }
}
