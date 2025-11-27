using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContourSearcher.UI.ViewModels.Models.Filters
{
    internal class GaussianFilterViewModel : FilterViewModel
    {
        #region Fields
        private double m_sigma1;
        private double m_sigma2;
        #endregion

        #region Properties
        public double Sigma1 { get => m_sigma1; set => Set(ref m_sigma1, value); }
        public double Sigma2 { get => m_sigma2; set => Set(ref m_sigma2, value); }
        #endregion

        #region Ctor
        public GaussianFilterViewModel(int showNumber) : base(showNumber)
        {
            Init();
        }

        public GaussianFilterViewModel() : base()
        {
            Init();
        }
        #endregion

        #region Methods
        private void Init()
        {
            FilterType = Enums.FilterTypes.Gaussian_Blur;
            FilterName = FilterType.ToString();
            m_sigma1 = 1;
            m_sigma2 = 1;
        }

        public override bool Check()
        {
            IsValid = base.Check() && Sigma1 >= 0 && Sigma2 >= 0;
            return IsValid;
        }
        #endregion
    }
}
