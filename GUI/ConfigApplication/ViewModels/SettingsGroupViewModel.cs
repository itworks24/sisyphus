using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using Sisyphus.Logging;
using Sisyphus.Settings;
using Sisyphus.Tasks;
using ConfigApplication.Annotations;

namespace ConfigApplication.ViewModels
{
    public class SettingsGroupViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly TaskBase _taskInstance;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void NotifyAllPropertyChanged()
        {
            OnPropertyChanged(string.Empty);
        }

        private string _groupName;
        private IEnumerable<Type> _settingsTypes;
        private GroupSettings _serviceSettings;

        public string GroupName => _groupName;

        public ObservableCollection<SettingsViewModel> SettingsViewModelObservableCollection
        {
            get
            {
                var result = new ObservableCollection<SettingsViewModel>();
                _settingsTypes.ToList().ForEach(t => result.Add(new SettingsViewModel(t, _groupName)));
                return result;
            }
        }

        public object Settings => _serviceSettings;

        public ICommand RunTaskCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ObservableCollection<LogMessage> Logging => new ObservableCollection<LogMessage>(_taskInstance);

        private volatile bool _executing = false;
        public bool Executing
        {
            get { return _executing; }
            set
            {
                _executing = value;
                NotifyAllPropertyChanged();
            }
        }

        private void RunTaskInThread()
        {
            Executing = true;
            try
            {
                _taskInstance.ExecuteTask(_groupName,
                    createEmailReport: _serviceSettings.Report,
                    sendReportIntervalInSeconds: _serviceSettings.SendReportInterval * 60,
                    reportInformationLevel: _serviceSettings.ReportInformationLevel,
                    maxErrorCount:_serviceSettings.MaxErrorCount);
            }
            catch (Exception e)
            {
                _taskInstance.CreateLogRecord(e);
            }
            finally
            {
                Executing = false;
            }
        }

        private void RunTask()
        {
            var newThread = new Thread(RunTaskInThread);
            newThread.Start();
        }

        public SettingsGroupViewModel(Type componentType, IEnumerable<Type> taskSettings, string groupName)
        {
            _groupName = groupName;
            _settingsTypes = taskSettings;
            _taskInstance = Activator.CreateInstance(componentType) as TaskBase;
            _serviceSettings = GroupSettings.GetGroupSettings(_taskInstance, GroupName);
            RunTaskCommand = new RelayCommand(arg => RunTask());
            SaveCommand = new RelayCommand(arg => _serviceSettings.SaveSettings());
        }
    }
}
