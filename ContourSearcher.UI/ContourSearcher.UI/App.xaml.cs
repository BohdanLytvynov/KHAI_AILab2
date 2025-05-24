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

                c.AddSingleton<LoadImagePage>();

                c.AddSingleton<ImageProcessingPage>();

                c.AddSingleton<ContourSearcherPage>();

                c.AddSingleton<ConfigurationPage>();
            });

            var provider = ServiceWrapper.ServiceProvider;

            var mainWindow = provider.GetRequiredService<MainWindow>();
            var pm = provider.GetRequiredService<IPageManager>();

            ConfigureVM(typeof(LoadImagePage), typeof(LoadImagePageViewModel), pm, provider);
            ConfigureVM(typeof(ImageProcessingPage), typeof(ImageProcessingViewModel), pm, provider);
            ConfigureVM(typeof(ContourSearcherPage), typeof(ContourSearcherViewModel), pm, provider);
            ConfigureVM(typeof(ConfigurationPage), typeof(ConfigurationViewModel), pm, provider);

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
