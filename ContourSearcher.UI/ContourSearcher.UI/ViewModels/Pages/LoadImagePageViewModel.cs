using ContourSearcher.UI.Constant;
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
                var list = ShareData.GetItem<List<string>>(Constants.LOADED_IMAGE_LIST_COLLECTION);
                list.Add(PathToImg);
            } 
        }

        #endregion

        #region Ctor
        public LoadImagePageViewModel()
        {
            m_pathToImg = string.Empty;
            ShareData.InsertItem(Constants.LOADED_IMAGE_LIST_COLLECTION, new List<string>());
        }
        #endregion

        #region Methods

        #endregion
    }
}
