using ContourSearcher.UI.Constant;
using ContourSearcher.UI.ViewModels.Pages.Modules.Base;
using ContourSearcherBusinessLayer;
using CSharpBusinessLayer.Helpers;
using CSharpBusinessLayer.Validators;
using MVVMBase.Attributes;
using MVVMBase.Commands;
using System.Windows.Input;

namespace ContourSearcher.UI.ViewModels.Pages.Modules
{
    internal class HistogramModuleViewModel : ModuleBaseViewModel
    {
        #region Fields
        private string m_Histname;
        private int m_HistWidth;
        private int m_HistHeight;
        private int m_channels;
        private int m_histDrawingMode;
        #endregion

        #region Properties
        [ValidateProperty]
        public string HistName
        {
            get => m_Histname;
            set => Set(ref m_Histname, value);
        }

        public int HistWidth
        {
            get => m_HistWidth;
            set => Set(ref m_HistWidth, value);
        }

        public int HistHeight
        {
            get => m_HistHeight;
            set => Set(ref m_HistHeight, value);
        }

        public bool BlueChannel
        {
            get => BitSetHelper.GetValue(m_channels, 0);
            set
            {
                bool curr = BitSetHelper.GetValue(m_channels, 0);
                if (curr != value)
                {
                    BitSetHelper.SetValue(ref m_channels, 0, value);
                    OnPropertyChanged(nameof(BlueChannel));
                }
            }
        }

        public bool GreenChannel
        {
            get => BitSetHelper.GetValue(m_channels, 1);
            set
            {
                bool curr = BitSetHelper.GetValue(m_channels, 1);
                if (curr != value)
                {
                    BitSetHelper.SetValue(ref m_channels, 1, value);
                    OnPropertyChanged(nameof(BlueChannel));
                }
            }
        }

        public bool RedChannel
        {
            get => BitSetHelper.GetValue(m_channels, 2);
            set
            {
                bool curr = BitSetHelper.GetValue(m_channels, 2);
                if (curr != value)
                {
                    BitSetHelper.SetValue(ref m_channels, 2, value);
                    OnPropertyChanged(nameof(BlueChannel));
                }
            }
        }

        public bool DrawAllInOnePlot
        {
            get => BitSetHelper.GetValue(m_histDrawingMode, 0);
            set
            {
                bool curr = BitSetHelper.GetValue(m_histDrawingMode, 0);
                if (curr != value)
                {
                    BitSetHelper.SetValue(ref m_histDrawingMode, 0, value);
                    OnPropertyChanged(nameof(DrawAllInOnePlot));
                }
            }
        }
        #endregion

        #region Commands
        public ICommand OnCalculateHistogramPressed { get; }
        #endregion

        #region IData error Info

        public override string this[string columnName]
        {
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case nameof(HistName):
                        SetValidArrayValue(0, ValidationHelper.ValidateEmptyText(HistName, out error));
                        break;
                }
                return error;
            }
        }

        #endregion

        #region Ctor
        public HistogramModuleViewModel(ICVSystem cvSystem) : this()
        {
            CVSystem = cvSystem ?? throw new ArgumentNullException(nameof(cvSystem));
        }

        public HistogramModuleViewModel()
        {
            InitValidArray(this);

            #region Init Fields
            ModuleName = Constants.Histogram.Name;
            m_HistWidth = Constants.Histogram.Width;
            m_HistHeight = Constants.Histogram.Height;
            m_Histname = Constants.DEFAULT_INPUT_VALUE;
            #endregion

            #region Init Commands

            OnCalculateHistogramPressed = new Command(
                OnCalculateHistogramButtonPressedExecute,
                CanOnCalculateHistogramButtonPressedExecute
                );
            #endregion
        }
        #endregion

        #region Methods
        #region On Calculate Histogram Button Pressed

        private bool CanOnCalculateHistogramButtonPressedExecute(object p) =>
            ValidateField(0) && HistHeight > 0 && HistWidth > 0
            && (BlueChannel || GreenChannel || RedChannel)
            && !string.IsNullOrEmpty(ImgNameForProcessing);

        private void OnCalculateHistogramButtonPressedExecute(object p)
        {
            try
            {
                CVSystem.Draw2DHistogram(ImgNameForProcessing, HistName, HistWidth, HistHeight, m_channels, 3, m_histDrawingMode);
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
