using ContourSearcher.UI.PageManagers.Interfaces;
using ContourSearcher.UI.Views.Pages.Modules;
using MVVMBase.ViewModels;
using System.Windows.Controls;

namespace ContourSearcher.UI.ViewModels.Pages
{
    internal class SkinDiseaseDetectionPageViewModel : ViewModelBase
    {
        #region Fields
        private Page m_SkinDiseasePage;
        private IPageManager m_pageManager;
        #endregion

        #region Properties
        public Page SkinDiseasePage 
        { get => m_SkinDiseasePage; set=> Set(ref m_SkinDiseasePage, value); }
        #endregion

        #region Ctor
        public SkinDiseaseDetectionPageViewModel(IPageManager pageManager) : this()
        {
            m_pageManager = pageManager ?? throw new ArgumentNullException(nameof(pageManager));
            m_SkinDiseasePage = m_pageManager.GetPage(nameof(SkinDiseaseDetectionModule));
        }

        public SkinDiseaseDetectionPageViewModel()
        {

        }
        #endregion

        #region Methods

        #endregion
    }
}
