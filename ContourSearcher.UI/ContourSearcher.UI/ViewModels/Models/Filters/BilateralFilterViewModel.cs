using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContourSearcher.UI.ViewModels.Models.Filters
{
    internal class BilateralFilterViewModel : FilterViewModel
    {
        #region Fields
        private int m_d;
        private double m_sigmaColor;
        private double m_sigmaSpace;
        #endregion

        #region Properties
        public int Diameter 
        { get => m_d; set => Set(ref m_d, value); }
        public double SigmaColor 
        { get => m_sigmaColor; set => Set(ref m_sigmaColor, value); }
        public double SigmaSpace 
        { get => m_sigmaSpace; set => Set(ref m_sigmaSpace, value); }
        #endregion

        #region Ctor
        public BilateralFilterViewModel(int showNumber) : base(showNumber)
        {
            Init();
        }

        public BilateralFilterViewModel() : base()
        {
            Init();
        }
        #endregion

        #region Methods
        private void Init()
        {
            FilterType = Enums.FilterTypes.Bilateral_Filter;
            FilterName = FilterType.ToString();
        }

        public override bool Check()
        {
            IsValid = SigmaColor >= 0 && SigmaSpace >= 0 && ExternalCheck;
            return IsValid;
        }
        #endregion
    }
}
