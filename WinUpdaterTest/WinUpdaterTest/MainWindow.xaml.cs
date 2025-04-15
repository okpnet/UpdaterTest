using AppUpdater;
using Microsoft.Extensions.DependencyInjection;
using System.Reactive.Disposables;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WinUpdaterTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,IDisposable
    {
        readonly CompositeDisposable disposables = new CompositeDisposable();
        readonly AppUpdateService _appUpdateService;
        public MainWindow()
        {
            InitializeComponent();
            Resources["App"] = Application.Current;

            _appUpdateService = App.Services.GetRequiredService<AppUpdateService>();
            disposables.Add(
                _appUpdateService.UpdateStandbyEvent.
                    Subscribe(t => t.IsCancel = true)
                );
            
            _appUpdateService.UpdateDitectAsync();
        }

        public void Dispose()
        {
            disposables.Clear();
        }
    }
}