using ContourSearcher.UI.Constant;
using ContourSearcher.UI.Enums;
using ContourSearcher.UI.ViewModels.Pages.Modules.Base;
using ContourSearcherBusinessLayer;
using CSharpBusinessLayer.Validators;
using MVVMBase.Attributes;
using MVVMBase.Commands;
using System.Windows;
using System.Windows.Input;

namespace ContourSearcher.UI.ViewModels.Pages.Modules
{
    internal class EqualizerModuleViewModel : ModuleBaseViewModel
    {
        #region Fields
        private EqualizationType m_EqualizationType;
        private string m_EqualizationWindowName;
        private double m_ClipLimit;
        private int m_GridTileWidth;
        private int m_GridTileHeight;
        private Visibility m_AddPropertiesVisibility;
        #endregion

        #region Properties
        public Visibility AddPropertiesVisibility 
        {
            get=> m_AddPropertiesVisibility;
            set=>Set(ref m_AddPropertiesVisibility, value);
        }

        public EqualizationType EqualizationType
        {
            get => m_EqualizationType;
            set
            {
                Set(ref m_EqualizationType, value); 
                
                if(value == EqualizationType.CLAHE)
                    AddPropertiesVisibility = Visibility.Visible;
                else
                    AddPropertiesVisibility = Visibility.Collapsed;
            }
        }

        [ValidateProperty]
        public string EqualizationWindowName
        {
            get => m_EqualizationWindowName;
            set => Set(ref m_EqualizationWindowName, value);
        }

        public double ClipLimit 
        {
            get=> m_ClipLimit;
            set=>Set(ref m_ClipLimit, value);
        }

        public int GridTileWidth 
        {
            get=> m_GridTileWidth;
            set=>Set(ref m_GridTileWidth, value);
        }

        public int GridTileHeight 
        {
            get=> m_GridTileHeight;
            set=> Set(ref m_GridTileHeight, value);
        }
        #endregion

        #region Command

        public ICommand OnEqualizeButtonPressed { get; }

        #endregion

        #region IData Error

        public override string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(EqualizationWindowName):
                        SetValidArrayValue(0, ValidationHelper.ValidateEmptyText(EqualizationWindowName, out error));
                        break;
                }
                return error;
            }
        }

        #endregion

        #region Ctor

        public EqualizerModuleViewModel(ICVSystem cvSystem) : this()
        {
            CVSystem = cvSystem ?? throw new ArgumentNullException(nameof(cvSystem));
        }

        public EqualizerModuleViewModel()
        {
            this.InitValidArray(this);
            #region Init Fields
            ModuleName = Constants.Equalizer.Name;
            m_EqualizationType = EqualizationType.NONE;
            m_AddPropertiesVisibility = Visibility.Collapsed;
            m_EqualizationWindowName = Constants.DEFAULT_INPUT_VALUE;
            m_ClipLimit = Constants.Equalizer.ClipLimit;
            m_GridTileHeight = Constants.Equalizer.GridTileHeight;
            m_GridTileWidth = Constants.Equalizer.GridTileWidth;
            #endregion

            #region Init Commands
            OnEqualizeButtonPressed = new Command(
                OnEqualizeButtonPressedExecute,
                CanOnEqualizeButtonPressedExecute
                );
            #endregion
        }
        #endregion

        #region Methods

        #region On Equalize Button Pressed

        private bool CanOnEqualizeButtonPressedExecute(object p) =>
            EqualizationType != EqualizationType.NONE &&
            ValidateField(0) &&
            !string.IsNullOrEmpty(EqualizationWindowName)
            && !string.IsNullOrEmpty(ImgNameForProcessing);

        private void OnEqualizeButtonPressedExecute(object p)
        {
            try
            {
                if(CVSystem == null)
                    throw new ArgumentNullException(nameof(CVSystem));

                switch (EqualizationType)
                {
                    case EqualizationType.SIMPLE_EQUALIZE:
                        CVSystem!.Simple_Equalize(ImgNameForProcessing, EqualizationWindowName);
                        break;
                    case EqualizationType.CLAHE:
                        CVSystem!.CLAHE_Equalize(ImgNameForProcessing, 
                            EqualizationWindowName,
                            ClipLimit,
                            GridTileWidth,
                            GridTileHeight);
                        break;
                    case EqualizationType.SEPARATE_CHANNELS:
                        CVSystem!.Separate_Equalize(ImgNameForProcessing, EqualizationWindowName);
                        break;
                    case EqualizationType.YCrCb:
                        CVSystem.YCrCb_Equalize(ImgNameForProcessing, EqualizationWindowName);
                        break;
                }
                CVSystem.DisplayImageInWindow(EqualizationWindowName, EqualizationWindowName);
                OnRefreshActiveButtonPressedExecute(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Constants.MODULE_ERROR_MSG, ex.Message),
                                    Constants.EdgeDetection.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #endregion
    }
}
