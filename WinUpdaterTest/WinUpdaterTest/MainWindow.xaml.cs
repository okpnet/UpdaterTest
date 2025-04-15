using AppUpdater;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
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
    public partial class MainWindow : Window, IDisposable,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string PropertyName)
        {
            var e = new PropertyChangedEventArgs(PropertyName);
            PropertyChanged?.Invoke(this, e);
        }

        readonly CompositeDisposable disposables = new CompositeDisposable();
        readonly AppUpdateService _appUpdateService;

        Action<bool> triggerAction=default!;

        int _prg = 0;
        public int Progress
        {
            get => _prg;
            set
            {
                _prg = value;
                NotifyPropertyChanged(nameof(Progress));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Resources["App"] = Application.Current;

            _appUpdateService = App.Services.GetRequiredService<AppUpdateService>();

            disposables.Add(
                _appUpdateService.UpdaterDownloadProgressEvent.Subscribe(t =>
                {
                    Progress = t.ProgressPercent;
                }));

            disposables.Add(
                _appUpdateService.UpdateStandbyEvent.Subscribe((t) =>
                {
                    t.ShouldWait = true;
                    triggerAction = (isUpdate) => t.TriggerUpdate(isUpdate);
                }));
            
        }

        public void Dispose()
        {
            disposables.Clear();
        }

        private void DitecedBtn_Click(object sender, RoutedEventArgs e)
        {
            _appUpdateService.UpdateDitectAsync();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CnacelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (triggerAction is not null)
            {
                triggerAction.Invoke(false);
            }
        }

        private void NoeUpdaterBtn_Click(object sender, RoutedEventArgs e)
        {
            if(triggerAction is not null)
            {
                triggerAction.Invoke(true);
            }
        }
    }
}