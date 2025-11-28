using ContourSearcher.UI.PageManagers.Interfaces;
using ContourSearcher.UI.Views.Pages.Modules;
using MVVMBase.ViewModels;
using System.Windows.Controls;

namespace ContourSearcher.UI.ViewModels.Pages
{
    internal class ContourSearcherViewModel : ViewModelBase
    {
        #region Fields

        private IPageManager m_pageManager;
        private Page m_edgeDetectionModule;
        private Page m_blobDetectionModule;
        private Page m_blurDetectionModule;
        #endregion

        #region Properties
        public Page EdgeDetectionModule 
        { get => m_edgeDetectionModule; set => Set(ref m_edgeDetectionModule, value); }

        public Page BlobDetectionModule 
        { get => m_blobDetectionModule; set => Set(ref m_blobDetectionModule, value); }

        public Page BlurDetectionModule 
        { get => m_blurDetectionModule; set => Set(ref m_blurDetectionModule, value); }
        #endregion

        #region Ctor
        public ContourSearcherViewModel(IPageManager pageManager) : this()
        {
            m_pageManager = pageManager ?? throw new ArgumentNullException(nameof(pageManager));
            m_edgeDetectionModule = m_pageManager.GetPage(nameof(EdgeDetectionModule));
            m_blobDetectionModule = m_pageManager.GetPage(nameof(BlobDetectionModule));
            m_blurDetectionModule = m_pageManager.GetPage(nameof(BlurDetectionModule));
        }

        public ContourSearcherViewModel()
        {
            
        }
        #endregion

        #region Methods

        #endregion
    }
}
