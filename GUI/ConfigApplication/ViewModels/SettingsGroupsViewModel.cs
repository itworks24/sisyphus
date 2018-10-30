using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Sisyphus.Settings;
using ConfigApplication.Annotations;
using System.Linq;

namespace ConfigApplication.ViewModels
{
    public class SettingsGroupsViewModel : INotifyPropertyChanged
    {
        private readonly SettingsGroupsCollection _settingsGroupsCollection;
        private readonly IEnumerable<Type> _taskSettings;
        private readonly Type _componentType;

        public ObservableCollection<SettingsGroupViewModel> SettingsGroupViewModelObservableCollection
        {
            get
            {
                var result = new ObservableCollection<SettingsGroupViewModel>();
                _settingsGroupsCollection.ToList().ForEach(t => result.Add(new SettingsGroupViewModel(_componentType, _taskSettings, t)));
                return result;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void NotifyAllPropertyChanged()
        {
            OnPropertyChanged(string.Empty);
        }

        public void AddGroup(string groupName)
        {
            _settingsGroupsCollection.Add(groupName);
            NotifyAllPropertyChanged();
        }

        public SettingsGroupsViewModel(Type componentType, IEnumerable<Type> taskSettings)
        {
            Misc.FPReset();
            _taskSettings = taskSettings;
            _componentType = componentType;
            _settingsGroupsCollection = new SettingsGroupsCollection(_taskSettings);
        }
    }
}