using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace WinUpdaterTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Services { get; protected set; } = ConfigureServices();


        private static IServiceProvider ConfigureServices()
        {
            IServiceCollection? services = new ServiceCollection();
            services.AddSingleton(provider =>
            {
                return AppUpdater.AppUpdateService.CreateAppUpdateService(
                    "YwKJzdkVJ9OclIASn6hxm0QCvNB1/6ZTL6k1/K8Gmzk=",
                    new Uri("https://github.com/okpnet/UpdaterTest/releases/download/0.0.1/appcast.xml"),
                    () =>
                    {
                        // アプリケーションを終了する処理
                        Application.Current.Shutdown();
                    }
                );
            });
            return services.BuildServiceProvider();
        }
    }

}
