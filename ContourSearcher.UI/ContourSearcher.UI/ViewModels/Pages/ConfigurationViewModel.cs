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

namespace ContourSearcher.UI.ViewModels.Pages
{
    internal class ConfigurationViewModel : ViewModelBase
    {
        #region Fields
        private ICVSystem m_cvSystem;

        private string m_ChosenImagePath;
        private string m_ImageLoadingWindowName;
        private ObservableCollection<string> m_LoadedImages;
        private ImageToLoadType m_imageToLoadType;

        #endregion

        #region Properties

        public string ChosenImagePath 
        {
            get=>m_ChosenImagePath;
            set=>SetToDefaultIfNull(ref m_ChosenImagePath, value, string.Empty);
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

        public ImageToLoadType ImageToLoadType
        { get => m_imageToLoadType; set => Set(ref m_imageToLoadType, value); }

        #endregion

        #region Commands

        public ICommand OnRefreshButtonPressed { get; }

        public ICommand OnDeleteButtonPressed { get; }

        public ICommand OnAddToImageProcessingPressed { get; }

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

            m_LoadedImages = new ObservableCollection<string>();
            m_imageToLoadType = ImageToLoadType.CV_LOAD_IMAGE_COLOR;

            OnRefreshButtonPressed = new Command(
                OnRefreshButtonPressedExecute,
                CanOnRefreshButtonPressedExecute
                );

            OnDeleteButtonPressed = new Command(
                OnDeleteButtonPressedExecute, 
                CanOnDeleteButtonPressedExecute
                );

            OnAddToImageProcessingPressed = new Command(
                OnAddToImageProcessingExecute,
                CanOnAddToImageProcessingExecute
                );

            ShareData.InsertItem(Constants.ACTIVE_WINDOW_LIST_COLLECTION, new List<string>());
        }

        #endregion

        #region Methods

        private void RefreshLoadedImages()
        {
            var imgs = ShareData.GetItem<List<string>>(Constants.IMAGE_LIST_COLLECTION);

            LoadedImages.Clear();
            foreach (var imgsItem in imgs)
            {
                LoadedImages.Add(imgsItem);
            }
        }

        private void RefreshLoadedImages(List<string> imgs)
        {
            LoadedImages.Clear();
            foreach (var imgsItem in imgs)
            {
                LoadedImages.Add(imgsItem);
            }
        }

        #region On Refresh button Pressed Execute

        private bool CanOnRefreshButtonPressedExecute(object p) => true;

        private void OnRefreshButtonPressedExecute(object p)
        {
            RefreshLoadedImages();
        }

        #endregion

        #region On Delete Button Pressed Execute

        private bool CanOnDeleteButtonPressedExecute(object p) => 
            !string.IsNullOrEmpty(ChosenImagePath);

        private void OnDeleteButtonPressedExecute(object p)
        {
            var imgs = ShareData.GetItem<List<string>>(Constants.IMAGE_LIST_COLLECTION);
            imgs.Remove(ChosenImagePath);

            RefreshLoadedImages(imgs);
        }

        #endregion

        #region On Add To Image Processing
        private bool CanOnAddToImageProcessingExecute(object p) =>
            ValidateField(0);

        private void OnAddToImageProcessingExecute(object p)
        {
            try
            {
                ShareData.GetItem<List<string>>(Constants.ACTIVE_WINDOW_LIST_COLLECTION).Add(ImageLoadingWindowName);
                m_cvSystem.LoadToOpenCV(ChosenImagePath, ImageLoadingWindowName, (Int32)ImageToLoadType);
                m_cvSystem.DisplayImageInWindow(ImageLoadingWindowName, ImageLoadingWindowName);
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
