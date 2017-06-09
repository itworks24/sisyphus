using System;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Sisyphus;
using Sisyphus.Common;
using Sisyphus.Services;
using ConfigApplication.Annotations;

namespace ConfigApplication.ViewModels
{
    internal class RelayCommand : ICommand
    {
        public RelayCommand(Action<object> action)
        {
            ExecuteDelegate = action;
        }

        // ReSharper disable once UnassignedGetOnlyAutoProperty
        private Predicate<object> CanExecuteDelegate { get; }
        private Action<object> ExecuteDelegate { get; }

        public bool CanExecute(object parameter)
        {
            if (CanExecuteDelegate != null)
            {
                return CanExecuteDelegate(parameter);
            }
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            ExecuteDelegate?.Invoke(parameter);
        }
    }

    public sealed class LogonControlViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool AccountMatch => new LogonControll.UserAccount(ServiceControll.GetServiceLogonName(Scheduler.currentServiceName)) == CurrentUserAccount;

        public LogonControll.UserAccount CurrentUserAccount
        {
            get
            {
                string curentDomainName;
                try
                {
                    curentDomainName = Environment.UserDomainName == Domain.GetComputerDomain().Name
                        ? "."
                        : Environment.UserDomainName;
                }
                catch (Exception)
                {
                    curentDomainName = ".";
                }
                var userName = Environment.UserName.ToLower() == "system" ? "localsystem" : Environment.UserName;
                return new LogonControll.UserAccount($"{userName}@{curentDomainName}");
            }
        }

        public string CurrentUser => CurrentUserAccount.AccountName;
        public Color BgColor => AccountMatch ? Colors.White : Colors.Coral;

        public ICommand ChangeCurrentUserCommand { get; set; }

        public static void ChangeLogonAccount(NetworkCredential credential = null)
        {
            ProcessStartInfo startInfo;
            if (ServiceControll.GetServiceLogonName(Scheduler.currentServiceName).ToLower() == "localsystem")
            {
                startInfo = new ProcessStartInfo()
                {
                    FileName = "psexec.exe",
                    Verb ="runas",
                    UseShellExecute = true,
                    // ReSharper disable once AssignNullToNotNullAttribute
                    WorkingDirectory = Path.GetDirectoryName(Application.ResourceAssembly.Location),
                    Arguments = $"-i -d -s {Application.ResourceAssembly.Location}",
                };
                try
                {
                    Process.Start(startInfo);
                    Application.Current.Shutdown();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Operation failed. Error represent: {e.Message}");
                }
                return;
            }
            if (credential == null)
                CommonFunctions.GetCredentialsVistaAndUp(out credential, $"Please enter password for account {ServiceControll.GetServiceLogonName(Scheduler.currentServiceName)}", "Leave account name field empty.");
            if (credential == null) return;
            if (credential.UserName == "") { credential.UserName = ServiceControll.GetServiceLogonName(Scheduler.currentServiceName).GetUserName(); credential.Domain = ServiceControll.GetServiceLogonName(Scheduler.currentServiceName).GetDomain(); }

            startInfo = new ProcessStartInfo()
            {
                Domain = credential.Domain,
                UserName = credential.UserName,
                Password = new SecureString(),
                FileName = Application.ResourceAssembly.Location,
                UseShellExecute = false,
                // ReSharper disable once AssignNullToNotNullAttribute
                WorkingDirectory = Path.GetDirectoryName(Application.ResourceAssembly.Location),
            };
            credential.Password.ToCharArray().ToList().ForEach(p => startInfo.Password.AppendChar(p));

            try
            {
                Process.Start(startInfo);
                Application.Current.Shutdown();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Operation failed. Error represent: {e.Message}");
            }
        }

        public LogonControlViewModel()
        {
            ChangeCurrentUserCommand = new RelayCommand(arg => ChangeLogonAccount());
            var timer = new System.Timers.Timer(10000);
            timer.Elapsed += (sender, e) => OnPropertyChanged("");
            timer.Start();
        }
    }
}
