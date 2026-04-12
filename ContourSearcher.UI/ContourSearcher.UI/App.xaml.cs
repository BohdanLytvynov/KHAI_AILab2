using System.Windows;
using System.Windows.Controls;
using ContourSearcher.UI.PageManagers;
using ContourSearcher.UI.PageManagers.Interfaces;
using ContourSearcher.UI.ViewModels;
using ContourSearcher.UI.ViewModels.Pages;
using ContourSearcher.UI.Views.Pages;
using Microsoft.Extensions.DependencyInjection;
using MVVMBase.ViewModels;
using ServiceWrapperLib;
using ContourSearcherBusinessLayer;
using ContourSearcher.UI.ViewModels.Pages.Modules;
using ContourSearcher.UI.Views.Pages.Modules;

namespace ContourSearcher.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ServiceWrapper.Init();

            ServiceWrapper.ConfigureServices(c =>
            {
                c.AddSingleton<ICVSystem, OpenCV>();

                c.AddSingleton<IPageManager, PageManager>();

                //Add View Models
                c.AddSingleton<MainWindowViewModel>();
                c.AddSingleton<LoadImagePageViewModel>();
                c.AddSingleton<ImageProcessingViewModel>();
                c.AddSingleton<ContourSearcherViewModel>();
                c.AddSingleton<ConfigurationViewModel>();
                c.AddSingleton<HistogramModuleViewModel>();
                c.AddSingleton<EqualizerModuleViewModel>();
                c.AddSingleton<FilteringModuleViewModel>();
                c.AddSingleton<EdgeDetectorViewModel>();
                c.AddSingleton<BlobDetectionModuleViewModel>();
                c.AddSingleton<BlurDetectionModuleViewModel>();
                c.AddSingleton<SkinDiseaseDetectionPageViewModel>();
                c.AddSingleton<SkinDiseaseDetectionModuleViewModel>();

                //Add Views
                c.AddTransient(p =>
                { 
                    var view = new MainWindow();

                    var vm = p.GetRequiredService<MainWindowViewModel>();

                    view.DataContext = vm;
                    vm.Dispatcher = view.Dispatcher;

                    view.Closed += (object s, EventArgs e) =>
                    {
                        var cv = p.GetRequiredService<ICVSystem>();

                        (cv as IDisposable)!.Dispose();
                    };
                    
                    return view;
                });

                //Add Pages
                c.AddSingleton<LoadImagePage>();
                c.AddSingleton<ImageProcessingPage>();
                c.AddSingleton<ContourSearcherPage>();
                c.AddSingleton<ConfigurationPage>();
                c.AddSingleton<SkinDiseaseDetectionPage>();
                //Add Pages for Modules
                c.AddSingleton<HistogramModule>();
                c.AddSingleton<EqualizerModule>();
                c.AddSingleton<FilteringModule>();
                c.AddSingleton<EdgeDetectionModule>();
                c.AddSingleton<BlobDetectionModule>();
                c.AddSingleton<BlurDetectionModule>();
                c.AddSingleton<SkinDiseaseDetectionModule>();
            });

            var provider = ServiceWrapper.ServiceProvider;

            var mainWindow = provider.GetRequiredService<MainWindow>();
            var pm = provider.GetRequiredService<IPageManager>();

            //1 Modules Configuration we need it first to be sure, that it will be present in PM during the initialization
            ConfigureVM(typeof(HistogramModule), typeof(HistogramModuleViewModel), pm, provider);
            ConfigureVM(typeof(EqualizerModule), typeof(EqualizerModuleViewModel), pm, provider);
            ConfigureVM(typeof(FilteringModule), typeof(FilteringModuleViewModel), pm, provider);
            ConfigureVM(typeof(EdgeDetectionModule), typeof(EdgeDetectorViewModel), pm, provider);
            ConfigureVM(typeof(BlobDetectionModule), typeof(BlobDetectionModuleViewModel), pm, provider);
            ConfigureVM(typeof(BlurDetectionModule), typeof(BlurDetectionModuleViewModel), pm, provider);
            ConfigureVM(typeof(SkinDiseaseDetectionModule), typeof(SkinDiseaseDetectionModuleViewModel), pm, provider);

            //2 Pages Configuration
            ConfigureVM(typeof(LoadImagePage), typeof(LoadImagePageViewModel), pm, provider);
            ConfigureVM(typeof(ImageProcessingPage), typeof(ImageProcessingViewModel), pm, provider);
            ConfigureVM(typeof(ContourSearcherPage), typeof(ContourSearcherViewModel), pm, provider);
            ConfigureVM(typeof(ConfigurationPage), typeof(ConfigurationViewModel), pm, provider);
            ConfigureVM(typeof(SkinDiseaseDetectionPage), typeof(SkinDiseaseDetectionPageViewModel), pm, provider);

            mainWindow.Show();
            pm.Switch(nameof(LoadImagePage));
            pm.Switch(nameof(ConfigurationPage), Frames.Right);
        }

        private void ConfigureVM(Type view, Type viewModel, 
            IPageManager pageManager, IServiceProvider serviceProvider)
        { 
            var v = (Page)serviceProvider.GetRequiredService(view);
            var vm = (ViewModelBase)serviceProvider.GetRequiredService(viewModel);
            pageManager.RegisterPage(v.GetType().Name, v);
            v.DataContext = vm;
            vm.Dispatcher = v.Dispatcher;
        }
    }

}
