using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Sisyphus;
using Sisyphus.Services;
using ConfigApplication.Annotations;
using Sisyphus.Common;

namespace ConfigApplication.ViewModels
{
    public class ServiceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string info = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        private void NotifyAllPropertyChanged()
        {
            OnPropertyChanged(string.Empty);
        }

        public string ServiceStatus => ServiceControll.GetServiceState(Scheduler.currentServiceName);
        public bool ServiceInstalled => ServiceControll.ServiceExist(Scheduler.currentServiceName);
        public bool ServiceNotInstalled => !ServiceInstalled;
        public bool ServiceCanStop => ServiceControll.ServiceCanStop(Scheduler.currentServiceName);
        public bool ServiceCanStart => ServiceControll.ServiceCanStart(Scheduler.currentServiceName);
        public bool ServiceCanRestart => ServiceControll.ServiceCanRestart(Scheduler.currentServiceName);
        public string ServiceLogonAccount => new LogonControll.UserAccount(ServiceControll.GetServiceLogonName(Scheduler.currentServiceName)).AccountName;
        public StartupMode ServiceStartupMode
        {
            get { return ServiceControll.GetServiceStartMode(Scheduler.currentServiceName); }
            set
            {
                string errorRepresent;
                if (!ServiceControll.SetServiceStartupMode(Scheduler.currentServiceName, value, out errorRepresent))
                {
                    MessageBox.Show($"Operation failed. Error represent: {errorRepresent}");
                }
                NotifyAllPropertyChanged();
            }
        }

        public ICommand ChangeLogonAccountCommand { get; set; }
        public ICommand InstallServiceCommand { get; set; }
        public ICommand UninstallServiceCommand { get; set; }
        public ICommand StartServiceCommand { get; set; }
        public ICommand StopServiceCommand { get; set; }
        public ICommand RestartServiceCommand { get; set; }

        private void ChangeLogonAccount()
        {
            uint errorCode;
            if(!ServiceControll.SetServiceLogon(Scheduler.currentServiceName, out errorCode))
            {
                MessageBox.Show($"Operation failed. Error code {errorCode}");
            }            
            var logonControl = new LogonControlViewModel();
            if (!logonControl.AccountMatch &&
                    MessageBox.Show("Run this app under service account now?", "Sisyphus",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                LogonControlViewModel.ChangeLogonAccount(); 
            NotifyAllPropertyChanged();
        }

        private void InstallService() { ServiceControll.InstallService(Scheduler.currentServiceName); NotifyAllPropertyChanged(); }
        private void UninstallService() { ServiceControll.UninstallService(Scheduler.currentServiceName); NotifyAllPropertyChanged(); }

        private void StartService()
        {
            if (!(ServiceInstalled && !ServiceCanStop)) return;
            string errorRepresent;
            if (!ServiceControll.StartService(Scheduler.currentServiceName, out errorRepresent))
                MessageBox.Show($"Operation failed. {errorRepresent}"); 
            NotifyAllPropertyChanged();
        }

        private void StopService()
        {
            if (!(ServiceInstalled && ServiceCanStop)) return;
            string errorRepresent;
            if (!ServiceControll.StopService(Scheduler.currentServiceName, out errorRepresent))
                MessageBox.Show($"Operation failed. {errorRepresent}");
            NotifyAllPropertyChanged();
        }

        private void RestartService()
        {
            if (!(ServiceInstalled && ServiceCanStop)) return;
            string errorRepresent;
            if (!ServiceControll.StopService(Scheduler.currentServiceName, out errorRepresent) ||
                 !ServiceControll.StartService(Scheduler.currentServiceName, out errorRepresent))
                MessageBox.Show($"Operation failed. {errorRepresent}");
            NotifyAllPropertyChanged();
        }

        public ServiceViewModel()
        {
            ChangeLogonAccountCommand = new RelayCommand(arg => ChangeLogonAccount());
            InstallServiceCommand = new RelayCommand(arg => InstallService());
            UninstallServiceCommand = new RelayCommand(arg => UninstallService());
            StartServiceCommand = new RelayCommand(arg => StartService());
            StopServiceCommand = new RelayCommand(arg => StopService());
            RestartServiceCommand = new RelayCommand(arg => RestartService());
            var timer = new System.Timers.Timer(10000);
            timer.Elapsed += (sender, e) => NotifyAllPropertyChanged();
            timer.Start();
        }
    }
}
