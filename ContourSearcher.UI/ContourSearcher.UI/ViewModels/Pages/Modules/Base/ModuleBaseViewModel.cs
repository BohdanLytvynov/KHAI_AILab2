using ContourSearcher.UI.Helpers;
using ContourSearcherBusinessLayer;
using MVVMBase.Commands;
using MVVMBase.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ContourSearcher.UI.ViewModels.Pages.Modules.Base
{
    internal abstract class ModuleBaseViewModel : ViewModelBase
    {
        #region Fields
        private string m_ModuleName;
        private string m_ImageNameForProcessing;
        private ObservableCollection<string> m_ActiveWindowNames;
        #endregion

        #region Properties
        public string ModuleName 
        { 
            get=> m_ModuleName;
            set=> Set(ref m_ModuleName, value);
        }

        protected ICVSystem? CVSystem 
        {
            get; set;
        }

        public ObservableCollection<string> ActiveWindows
        {
            get => m_ActiveWindowNames;
            set => m_ActiveWindowNames = value;
        }

        public string ImgNameForProcessing
        {
            get => m_ImageNameForProcessing;
            set => SetToDefaultIfNull(ref m_ImageNameForProcessing, value, string.Empty);
        }

        #endregion

        #region Commands

        public ICommand OnRefreshActiveWindowsPressed { get; }

        #endregion

        #region Ctor
        protected ModuleBaseViewModel()
        {
            #region Init Fields
            m_ImageNameForProcessing = string.Empty;
            m_ActiveWindowNames = new ObservableCollection<string>();
            m_ModuleName = string.Empty;
            #endregion

            #region Init Commands
            OnRefreshActiveWindowsPressed = new Command(
                OnRefreshActiveButtonPressedExecute,
                CanOnRefreshActiveWindowsButtonPressedExecute
                );
            #endregion
        }
        #endregion

        #region Methods
        #region On Refresh Active Windows Pressed

        private bool CanOnRefreshActiveWindowsButtonPressedExecute(object p) => true;

        protected void OnRefreshActiveButtonPressedExecute(object p)
        {
            var imgs = CVSystem?.GetActiveWindows()
                ?? throw new ArgumentNullException($"CV System was null in {this.GetType().Name}");
            UIHelper.RefreshObservableCollection(ActiveWindows, imgs);
        }

        #endregion
        #endregion
    }
}
