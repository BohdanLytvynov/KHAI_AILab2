using ContourSearcher.UI.PageManagers;
using ContourSearcher.UI.PageManagers.Interfaces;
using ContourSearcher.UI.Views.Pages;
using CSharpBusinessLayer.Helpers;
using MVVMBase.Commands;
using MVVMBase.ViewModels;
using System.IO;
using System.Windows.Input;

namespace ContourSearcher.UI.ViewModels
{
    internal class MainWindowViewModel : TitledViewModel
    {
        #region Fields

        private bool m_LoadImageEnabled;

        private bool m_ImageProcessingEnabled;

        private bool m_ContourSearchEnabled;

        private bool m_DiseaseScannerEnabled;

        private IPageManager m_pageManager;

        private object m_Frame;

        private object m_rightFrame;

        //Temp paths
        private const string CLEAN_IMAGE_TEMP = "CleanImagesTempPath";

        #endregion

        #region Properties

        public object Frame 
        { get=>m_Frame; set=>Set(ref m_Frame, value); }

        public object RightFrame 
        { get=>m_rightFrame; set=>Set(ref m_rightFrame, value); }

        public bool LoadImageEnabled 
        { get=>m_LoadImageEnabled; set=>Set(ref m_LoadImageEnabled, value); }

        public bool ImageProcessingEnabled 
        { get=>m_ImageProcessingEnabled; set=>Set(ref m_ImageProcessingEnabled, value); }

        public bool ContourSearcherEnabled 
        { get=>m_ContourSearchEnabled; set=>Set(ref m_ContourSearchEnabled, value); }

        public bool DiseaseScannerEnabled
        { get => m_DiseaseScannerEnabled; set => Set(ref m_DiseaseScannerEnabled, value); }

        #endregion

        #region Commands

        public ICommand OnLoadImageTabButtonPressed { get; }

        public ICommand OnImageProcessTabButton { get; }

        public ICommand OnSearchContoursTabPressed { get; }

        public ICommand OnSkinDiseaseScannerPressed { get; }
        #endregion

        #region Ctor
        public MainWindowViewModel(IPageManager pageManager) : this()
        {
            m_pageManager = pageManager;

            m_pageManager.OnPageChanged += OnSwitchPage;
        }

        public MainWindowViewModel()
        {
            #region Set init values
            SetTitle("Contour Searcher V 1.0.0.0");            
            #endregion

            #region Init fields
            m_LoadImageEnabled = true;
            m_ImageProcessingEnabled = false;
            m_ContourSearchEnabled = false;
            m_DiseaseScannerEnabled = false;
            m_Frame = new object();
            m_rightFrame = new object();

            #endregion

            #region Init Commands
            OnLoadImageTabButtonPressed = new Command
                (
                    OnLoadImageTabButtonPressedExecute,
                    CanOnLoadImageTabButtonPressedExecute
                );

            OnImageProcessTabButton = new Command
                (
                    OnProcessImageButtonTabPressedExecute,
                    CanOnProcessImageButtonTabPressedExecute
                );

            OnSearchContoursTabPressed = new Command
                (
                    OnSearchContourTabPressedExecute,
                    CanOnSearchContourTabPressedExecute
                );

            OnSkinDiseaseScannerPressed = new Command
                (
                    OnSkinDiseaseScannerButtonPressedExecute,
                    CanOnSkinDiseaseScannerPressedExecute
                );
            #endregion
        }

        #endregion

        #region Methods

        private void OnSwitchPage(object src, PageManagerEventArgs e)
        {
            switch (e.Frame)
            {
                case Frames.Main:
                    Frame = e.Page;
                    break;
                case Frames.Right:
                    RightFrame = e.Page;
                    break;
            }
        }

        private void AddToTempFilePath(string key, string path)
        {
            //m_PathToTemp.Add(key, path);

            IOHelper.CreateFileIfNotExists(path);
            IOHelper.WriteToFile("", path);//Clear the temp file to be able to use it after restart
        }

        #region Commands

        #region On Load Image Button Tab Pressed

        private bool CanOnLoadImageTabButtonPressedExecute(object p) => true;

        private void OnLoadImageTabButtonPressedExecute(object p)
        { 
            LoadImageEnabled = true;
            ImageProcessingEnabled = false;
            ContourSearcherEnabled = false;
            DiseaseScannerEnabled = false;
            m_pageManager.Switch(nameof(LoadImagePage));
        }
        #endregion

        #region On Process Image Button Tab Pressed

        private bool CanOnProcessImageButtonTabPressedExecute(object p) => true;

        private void OnProcessImageButtonTabPressedExecute(object p)
        {
            LoadImageEnabled = false;
            ImageProcessingEnabled = true;
            ContourSearcherEnabled = false;
            DiseaseScannerEnabled = false;
            m_pageManager.Switch(nameof(ImageProcessingPage));
        }

        #endregion

        #region On Search Contour Tab Pressed

        private bool CanOnSearchContourTabPressedExecute(object p) => true;

        private void OnSearchContourTabPressedExecute(object p)
        {
            LoadImageEnabled = false;
            ImageProcessingEnabled = false;
            ContourSearcherEnabled = true;
            DiseaseScannerEnabled = false;
            m_pageManager.Switch(nameof(ContourSearcherPage));
        }

        #endregion

        #region On Skin Disease Scanner Pressed
        private bool CanOnSkinDiseaseScannerPressedExecute(object p) => true;

        private void OnSkinDiseaseScannerButtonPressedExecute(object p)
        { 
            LoadImageEnabled = false;
            ImageProcessingEnabled = false;
            ContourSearcherEnabled = false;
            DiseaseScannerEnabled = true;
            m_pageManager.Switch(nameof(SkinDiseaseDetectionPage));
        }
        #endregion

        #endregion

        #endregion
    }
}
