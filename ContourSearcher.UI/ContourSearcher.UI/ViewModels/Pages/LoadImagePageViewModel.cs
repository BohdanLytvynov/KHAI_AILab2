using ContourSearcher.UI.DataExchange;
using MVVMBase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContourSearcher.UI.ViewModels.Pages
{
    internal class LoadImagePageViewModel : ViewModelBase
    {
        #region Fields

        private string m_pathToImg;

        #endregion

        #region Properties
        public string PathToImg
        { 
            get => m_pathToImg; 
            set 
            {
                Set(ref m_pathToImg, value);
                ShareData.InsertItem(nameof(PathToImg), PathToImg);
            } 
        }
        #endregion

        #region Ctor
        public LoadImagePageViewModel()
        {
            m_pathToImg = string.Empty;
        }
        #endregion

        #region Methods



        #endregion
    }
}
