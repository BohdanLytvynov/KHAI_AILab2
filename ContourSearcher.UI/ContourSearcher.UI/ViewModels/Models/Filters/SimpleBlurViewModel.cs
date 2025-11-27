namespace ContourSearcher.UI.ViewModels.Models.Filters
{
    internal class SimpleBlurViewModel : FilterViewModel
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Ctor
        public SimpleBlurViewModel(int showNumber) : base(showNumber)
        {
            Init();
        }

        public SimpleBlurViewModel() : base()
        {
            Init();
        }
        #endregion

        #region Methods
        private void Init()
        {
            FilterType = Enums.FilterTypes.Simple_Blur;
            FilterName = FilterType.ToString();
        }
        #endregion
    }
}
