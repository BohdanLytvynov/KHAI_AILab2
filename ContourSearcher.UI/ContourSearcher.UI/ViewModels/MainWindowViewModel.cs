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
        //Dictionary<string, string> m_PathToTemp;
        //private bool m_ImagesSync;

        private Task m_CleanImagesTask;

        private bool m_LoadImageEnabled;

        private bool m_ImageProcessingEnabled;

        private bool m_ContourSearchEnabled;

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

        #endregion

        #region Commands

        public ICommand OnLoadImageTabButtonPressed { get; }

        public ICommand OnImageProcessTabButton { get; }

        public ICommand OnSearchContoursTabPressed { get; }
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
            m_Frame = new object();
            m_rightFrame = new object();
            //m_PathToTemp = new Dictionary<string, string>();
            //m_ImagesSync = false;
            //var pathToCurrent = Environment.CurrentDirectory;
            //var pathToTempFolder = pathToCurrent + Path.DirectorySeparatorChar + "Temp";

            //IOHelper.CreateDirectoryIfNotExists(pathToTempFolder);

            //AddToTempFilePath(CLEAN_IMAGE_TEMP, pathToTempFolder + Path.DirectorySeparatorChar + "ImageClean.txt");

            
            //Set up Timer
            m_CleanImagesTask = new Task(() =>
            {
                var timer = new System.Timers.Timer(TimeSpan.FromSeconds(30));
                timer.Enabled = false;
                timer.AutoReset = true;
                timer.Elapsed += Timer_Elapsed;

                timer.Start();
            });

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
            #endregion
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (true)
            {
                //m_ImagesSync = true;

                //OpenCVWrapper.CallImagesCleanUp(m_PathToTemp[CLEAN_IMAGE_TEMP]);

                //var imgs = IOHelper.ReadFromFile(m_PathToTemp[CLEAN_IMAGE_TEMP]);

                //if (!string.IsNullOrEmpty(imgs))
                //{ 
                //    var arr = imgs.Split(',');

                //    foreach (var image in arr)
                //    {
                //        QueueJobToDispatcher(() =>
                //        { 
                //            //WindowNames.Remove(image);
                //        });
                //    }
                //}
                
                //m_ImagesSync = false;
            }
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
            m_pageManager.Switch(nameof(ContourSearcherPage));
        }

        #endregion

        #endregion

        #endregion
    }
}
