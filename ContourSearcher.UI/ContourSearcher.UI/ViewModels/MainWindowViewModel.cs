using CSharpCppInteroperability.Wrappers;
using MVVMBase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContourSearcher.UI.ViewModels
{
    internal class MainWindowViewModel : TitledViewModel
    {
        #region Fields
        private string m_pathToImg;
        #endregion

        #region Properties
        public string PathToImg 
        {
            get=>m_pathToImg;
            set
            { 
                Set(ref m_pathToImg, value); 
                LoadAndDisplay();
            }
        }
        #endregion

        #region Ctor
        public MainWindowViewModel()
        {
            SetTitle("Contour Searcher V 1.0.0.0"); 

            m_pathToImg = string.Empty;
        }
        #endregion

        #region Methods
        private void LoadAndDisplay()
        {
            OpenCVWrapper.LoadAndDisplayImage(PathToImg, 1);
        }
        #endregion
    }
}
