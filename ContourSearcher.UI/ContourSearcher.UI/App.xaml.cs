using System.Configuration;
using System.Data;
using System.Windows;
using ContourSearcher.UI.ViewModels;
using CSharpCppInteroperability.Wrappers;
using Microsoft.Extensions.DependencyInjection;
using ServiceWrapperLib;

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
                c.AddSingleton<MainWindowViewModel>();

                c.AddTransient(p =>
                { 
                    var view = new MainWindow();

                    var vm = p.GetRequiredService<MainWindowViewModel>();

                    view.DataContext = vm;
                    vm.Dispatcher = view.Dispatcher;

                    view.Closed += (object s, EventArgs e) =>
                    { 
                        OpenCVWrapper.FreeResources();
                    };
                    

                    return view;
                });
            });

            var provider = ServiceWrapper.ServiceProvider;

            var mainWindow = provider.GetRequiredService<MainWindow>();

            mainWindow.Show();
        }
    }

}
