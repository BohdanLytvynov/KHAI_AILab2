using ContourSearcher.UI.Constant;
using ContourSearcher.UI.ViewModels.Pages.Modules.Base;
using ContourSearcherBusinessLayer;
using CSharpBusinessLayer.ML.DiseaseClassifier.Base;
using CSharpBusinessLayer.Validators;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MVVMBase.Attributes;
using MVVMBase.Commands;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace ContourSearcher.UI.ViewModels.Pages.Modules
{
    internal class SkinDiseaseDetectionModuleViewModel : ModuleBaseViewModel
    {
        #region Fields
        private IDiseaseClassifier m_diseaseClassifier;
        private string m_diagnosis;
        private bool m_DebugEnabled;
        private Visibility m_debugImgNameVisibility;
        private Visibility m_chartVisibility;
        private string m_DebugImageName;
        private ObservableCollection<ObservableValue> m_probabilities;

        private ISeries[] m_series;
        private Axis[] m_xAxes;
        private Axis[] m_yAxes;
        #endregion

        #region Properties
        public string Diagnosis { get => m_diagnosis; set => Set(ref m_diagnosis, value); }
        public bool DebugEnabled 
        {
            get => m_DebugEnabled;
            set 
            {
                Set(ref m_DebugEnabled, value);
                if (value)
                {
                    DebugImageNameVisibility = Visibility.Visible;
                }
                else
                { 
                    DebugImageNameVisibility = Visibility.Collapsed;
                }
            }
        }
        public Visibility DebugImageNameVisibility 
        { get => m_debugImgNameVisibility; set => Set(ref m_debugImgNameVisibility, value); }

        public Visibility ChartVisibility 
        { get => m_chartVisibility; set => Set(ref m_chartVisibility, value); }

        public ISeries[] Series { get => m_series; set => Set(ref m_series, value); }
        public Axis[] XAxes { get => m_xAxes; set => Set(ref m_xAxes, value); }
        public Axis[] YAxes { get => m_yAxes; set => Set(ref m_yAxes, value); }

        [ValidateProperty]
        public string DebugImageName { get => m_DebugImageName; set => Set(ref m_DebugImageName, value); }
        #endregion

        #region IDataErrorInfo

        public override string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(DebugImageName):
                        SetValidArrayValue(0, ValidationHelper.ValidateEmptyText(DebugImageName, out error));
                        break;
                }
                return error;
            }
        }

        #endregion

        #region Commands
        public ICommand OnCalculateButtonPressed { get; }
        #endregion

        #region Ctor
        public SkinDiseaseDetectionModuleViewModel(ICVSystem cVSystem, 
            IDiseaseClassifier diseaseClassifier) : this()
        {
            CVSystem = cVSystem ?? throw new ArgumentNullException(nameof(cVSystem));
            m_diseaseClassifier = diseaseClassifier ?? throw new ArgumentNullException(nameof(diseaseClassifier));
            InitChart();
        }

        public SkinDiseaseDetectionModuleViewModel()
        {
            this.InitValidArray(this);
            ModuleName = Constants.SkinDiseaseDetection.Name;
            m_diagnosis = Constants.SkinDiseaseDetection.DiagnosisDefault;
            m_DebugImageName = Constants.DEFAULT_INPUT_VALUE;
            m_DebugEnabled = false;
            m_debugImgNameVisibility = Visibility.Collapsed;
            m_chartVisibility = Visibility.Collapsed;

            OnCalculateButtonPressed = new Command
                (
                    OnCalculateButtonPressedExecute,
                    CanOnCalculateButtonPressedExecute
                );
        }
        #endregion

        #region Methods

        #region On Calculate Button Pressed

        private bool CanOnCalculateButtonPressedExecute(object p)
        {
            if (DebugEnabled)
            {
                return !string.IsNullOrEmpty(ImgNameForProcessing) 
                    && !string.IsNullOrEmpty(DebugImageName);
            }
            else
            { 
                return !string.IsNullOrEmpty(ImgNameForProcessing);
            }
        }

        private void OnCalculateButtonPressedExecute(object p)
        {
            Task<string>.Run(() =>
            {
                byte[] rgbImg = CVSystem.GetImageForSkinDiseaseScanner(
                    ImgNameForProcessing, 224,224, DebugEnabled, DebugImageName);
                if (rgbImg == null || rgbImg.Length == 0)
                    throw new Exception("Fail to get an image from the OpenCV System!");

                return m_diseaseClassifier.Predict(rgbImg);

            }).ContinueWith(
                t => {

                    Dispatcher.Invoke(() =>
                    {
                        float[] predictions = t.Result;
                        int maxIndex = Array.IndexOf(predictions, predictions.Max());
                        Diagnosis = m_diseaseClassifier.Labels[maxIndex];
                        int length = predictions.Length;
                        ChartVisibility = Visibility.Visible;
                        for (int i = 0; i < length; ++i)
                        {
                            m_probabilities[i].Value = predictions[i];
                        }
                    });

                    if (DebugEnabled)
                    {
                        CVSystem.DisplayImageInWindow(DebugImageName, DebugImageName);
                    }

                });
        }
        #endregion

        private void InitChart()
        {
            m_probabilities = new ObservableCollection<ObservableValue>()
            {
                new(0), new(0), new(0), new(0), new(0)
            };

            m_series = new ISeries[]
            {
                new RowSeries<ObservableValue>
                {
                    Values = m_probabilities,
                    Name = "Probability",
                    Stroke = null,
                    DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.End,
                    DataLabelsFormatter = point => $"{point.Model.Value * 100:N1}%",
                    DataLabelsPaint = new SolidColorPaint(SKColors.Black)
                }
            };

            m_xAxes = new Axis[] { new Axis { MinLimit = 0, MaxLimit = 1, Labeler = v => $"{v * 100}%" } };
            m_yAxes = new Axis[]
                {
                    new Axis
                    {
                        Labels = m_diseaseClassifier.Labels,
                        LabelsRotation = 0
                    }
                };
        }

        #endregion
    }
}
