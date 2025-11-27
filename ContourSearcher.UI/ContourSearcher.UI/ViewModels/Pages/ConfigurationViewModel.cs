using ContourSearcher.UI.Constant;
using ContourSearcher.UI.DataExchange;
using ContourSearcher.UI.Enums;
using CSharpBusinessLayer.Validators;
using MVVMBase.Attributes;
using MVVMBase.Commands;
using MVVMBase.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ContourSearcherBusinessLayer;
using ContourSearcher.UI.Helpers;

namespace ContourSearcher.UI.ViewModels.Pages
{
    internal class ConfigurationViewModel : ViewModelBase
    {
        #region Fields
        private ICVSystem m_cvSystem;

        private string m_ChosenImagePath;
        private string m_ImageLoadingWindowName;
        private string m_ImageForReleaseName;
        private string m_SelectedSourceWindow;
        private string m_CloneImageName;

        private ObservableCollection<string> m_LoadedImages;
        private ObservableCollection<string> m_ActiveWindows;
        private ImageToLoadType m_imageToLoadType;

        #endregion

        #region Properties

        public string ChosenImagePath 
        {
            get=>m_ChosenImagePath;
            set=>SetToDefaultIfNull(ref m_ChosenImagePath, value, string.Empty);
        }

        public string SelectedSourceWindow 
        {
            get=>m_SelectedSourceWindow;
            set=>SetToDefaultIfNull(ref m_SelectedSourceWindow, value, string.Empty);
        }

        public string ImageForReleaseName 
        {
            get=> m_ImageForReleaseName; 
            set=> SetToDefaultIfNull(ref m_ImageForReleaseName, value, string.Empty);
        }

        [ValidateProperty]
        public string CloneImageName 
        { 
            get=>m_CloneImageName;
            set=>Set(ref m_CloneImageName, value);
        }

        [ValidateProperty]
        public string ImageLoadingWindowName
        {
            get => m_ImageLoadingWindowName;
            set => Set(ref m_ImageLoadingWindowName, value); 
        }

        public ObservableCollection<string> LoadedImages
        {
            get=>m_LoadedImages; 
            set=>m_LoadedImages = value;
        }

        public ObservableCollection<string> ActiveWindows 
        { 
            get=>m_ActiveWindows;
            set=>m_ActiveWindows = value;
        }

        public ImageToLoadType ImageToLoadType
        {
            get => m_imageToLoadType; 
            set => Set(ref m_imageToLoadType, value); 
        }

        #endregion

        #region Commands

        public ICommand OnRefreshButtonPressed { get; }

        public ICommand OnRefreshActiveWindowsButtonPressed { get; }

        public ICommand OnDeleteButtonPressed { get; }

        public ICommand OnAddToImageProcessingPressed { get; }

        public ICommand OnReleaseImageButtonPressed { get; }

        public ICommand OnCloneImageButtonPressed { get; }

        #endregion

        #region IData Error Info

        public override string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case nameof(ImageLoadingWindowName):
                        SetValidArrayValue(0, ValidationHelper.ValidateEmptyText(ImageLoadingWindowName, out error));
                        break;
                    case nameof(CloneImageName):
                        SetValidArrayValue(1, ValidationHelper.ValidateEmptyText(CloneImageName, out error));
                        break;
                }

                return error;
            }
        }

        #endregion

        #region Ctor

        public ConfigurationViewModel(ICVSystem cVSystem) : this()
        {
            m_cvSystem = cVSystem;
        }

        public ConfigurationViewModel()
        {
            InitValidArray(this);

            m_ImageLoadingWindowName = Constants.ORIGINAL_IMAGE_NAME;
            m_ChosenImagePath = string.Empty;
            m_SelectedSourceWindow = string.Empty;
            m_ImageForReleaseName = string.Empty;
            m_CloneImageName = Constants.DEFAULT_INPUT_VALUE;

            m_LoadedImages = new ObservableCollection<string>();
            m_ActiveWindows = new ObservableCollection<string>();

            m_imageToLoadType = ImageToLoadType.IMREAD_COLOR;

            OnRefreshButtonPressed = new Command(
                OnRefreshButtonPressedExecute,
                CanOnRefreshButtonPressedExecute
                );

            OnDeleteButtonPressed = new Command(
                OnDeleteImagePathButtonPressedExecute, 
                CanOnDeleteImagePathButtonPressedExecute
                );

            OnAddToImageProcessingPressed = new Command(
                OnAddToImageProcessingExecute,
                CanOnAddToImageProcessingExecute
                );

            OnRefreshActiveWindowsButtonPressed = new Command(
                OnRefreshActiveWindowsButtonPressedExecute, 
                CanOnRefreshActiveWindowsButtonPressedExecute
                );

            OnCloneImageButtonPressed = new Command
                (
                    OnCloneImageButtonPressedExecute, 
                    CanOnCloneImageButtonPressedExecute
                );

            OnReleaseImageButtonPressed = new Command
                (
                    OnReleaseButtonPressedExecute,
                    CanOnReleaseButtonPressedExecute
                );
        }

        #endregion

        #region Methods

        private void RefreshActiveWindows()
        {
            var imgs = m_cvSystem.GetActiveWindows();
            UIHelper.RefreshObservableCollection(ActiveWindows, imgs);
        }

        private void RefreshLoadedImages(List<string> imgs)
        {
            UIHelper.RefreshObservableCollection(LoadedImages, imgs);
        }

        #region On Refresh button Pressed Execute

        private bool CanOnRefreshButtonPressedExecute(object p) => true;

        private void OnRefreshButtonPressedExecute(object p)
        {
            var imgs = ShareData.GetItem<List<string>>(Constants.LOADED_IMAGE_LIST_COLLECTION);
            UIHelper.RefreshObservableCollection(LoadedImages, imgs);
        }

        #endregion

        #region On Refresh Active Windows Button Pressed

        private bool CanOnRefreshActiveWindowsButtonPressedExecute(object p) => true;

        private void OnRefreshActiveWindowsButtonPressedExecute(object p)
        { 
            RefreshActiveWindows();
        }

        #endregion

        #region On Delete Button Pressed Execute

        private bool CanOnDeleteImagePathButtonPressedExecute(object p) => 
            !string.IsNullOrEmpty(ChosenImagePath);

        private void OnDeleteImagePathButtonPressedExecute(object p)
        {
            var imgs = ShareData.GetItem<List<string>>(Constants.LOADED_IMAGE_LIST_COLLECTION);
            imgs.Remove(ChosenImagePath);
            RefreshLoadedImages(imgs);
        }

        #endregion

        #region On Add To Image Processing
        private bool CanOnAddToImageProcessingExecute(object p) =>
            ValidateField(0) && !string.IsNullOrEmpty(ChosenImagePath);

        private void OnAddToImageProcessingExecute(object p)
        {
            try
            {
                m_cvSystem.LoadToOpenCV(ChosenImagePath, ImageLoadingWindowName, (Int32)ImageToLoadType);
                m_cvSystem.DisplayImageInWindow(ImageLoadingWindowName, ImageLoadingWindowName);
                OnRefreshActiveWindowsButtonPressedExecute(null);
            }
            catch (Exception ex)
            {
#if DEBUG
                throw;
#endif
            }
        }
        #endregion

        #region On Release Image Button Pressed

        private bool CanOnReleaseButtonPressedExecute(object p) => 
            !string.IsNullOrEmpty(ImageForReleaseName);

        private void OnReleaseButtonPressedExecute(object p)
        {
            try
            {
                m_cvSystem.FreeImage(ImageForReleaseName);
                m_cvSystem.DestroyWindow(ImageForReleaseName);
                OnRefreshActiveWindowsButtonPressedExecute(null);
            }
            catch (Exception)
            {
#if DEBUG
                throw;
#endif
            }
        }
        #endregion

        #region On Clone Image Button Pressed

        private bool CanOnCloneImageButtonPressedExecute(object p) => 
            !string.IsNullOrEmpty(SelectedSourceWindow) && ValidateField(1);

        private void OnCloneImageButtonPressedExecute(object p)
        {
            try
            {
                m_cvSystem.CloneImage(SelectedSourceWindow, CloneImageName);
                m_cvSystem.DisplayImageInWindow(CloneImageName, CloneImageName);
                OnRefreshActiveWindowsButtonPressedExecute(null);
            }
            catch (Exception ex)
            {
#if DEBUG
                throw;
#endif
            }
        }
        #endregion

        #endregion
    }
}
