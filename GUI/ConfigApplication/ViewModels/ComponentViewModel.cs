using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Sisyphus;
using Sisyphus.Services;
using Sisyphus.Settings;
using Sisyphus.Tasks;

namespace ConfigApplication.ViewModels
{
    public class ComponentViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        private void NotifyAllPropertyChanged()
        {
            NotifyPropertyChanged(string.Empty);
        }

        public ObservableCollection<TaskRepresent> Tasks { get; private set; }

        public bool AccountMatch => new LogonControlViewModel().AccountMatch;

        public ICommand ReloadTasks { get; private set; }
        public ICommand EditConfigCommand { get; private set; }

        private static void EditConfig()
        {
            SettingsIni.EditIniFileManual();
        }

        private void LoadTasks()
        {
            Tasks = new ObservableCollection<TaskRepresent>();
            var taskEnumerator = TasksEnumerator.GetTaskEnumertor();
            foreach (var task in taskEnumerator)
                Tasks.Add(new TaskRepresent(task));
            NotifyAllPropertyChanged();
        }

        public ComponentViewModel()
        {
            Misc.FPReset();
            LoadTasks();
            ReloadTasks = new RelayCommand(arg => LoadTasks());
            EditConfigCommand = new RelayCommand(arg => EditConfig());
            var timer = new System.Timers.Timer(1000);
            timer.Elapsed += (sender, e) => NotifyPropertyChanged("AccountMatch");
            timer.Start();
        }
    }
}
