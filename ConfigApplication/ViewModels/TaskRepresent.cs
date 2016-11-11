using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using Sisyphus.Logging;
using Sisyphus.Tasks;
using ConfigApplication.Annotations;

namespace ConfigApplication.ViewModels
{
    public class TaskRepresent : INotifyPropertyChanged
    {
        private readonly TaskBase _taskInstance;
        private readonly Type _componentType;

        public string TaskName => _taskInstance.TaskName;
        public string Version => _taskInstance.Version;

        private SettingsGroupsViewModel _settingsGroupsViewModels;

        public SettingsGroupsViewModel TaskSettingsGroups
        {
            get
            {
                if (_settingsGroupsViewModels != null) return _settingsGroupsViewModels;
                _settingsGroupsViewModels = new SettingsGroupsViewModel(_componentType, _taskInstance.TaskSettings);
                return _settingsGroupsViewModels;
            }
        }

        public TaskRepresent(Type componentType)
        {
            _componentType = componentType;
            _taskInstance = Activator.CreateInstance(componentType) as TaskBase;
            AddGroupCommand = new RelayCommand(arg => AddGroup());
        }

        private void AddGroup()
        {
            if (string.IsNullOrEmpty(NewGroupName)) return;
            TaskSettingsGroups.AddGroup(NewGroupName);
            NewGroupName = "";
            NotifyAllPropertyChanged();
        }

        private void NotifyAllPropertyChanged()
        {
            OnPropertyChanged(string.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string NewGroupName { get; set; }

        public ICommand AddGroupCommand { get; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}