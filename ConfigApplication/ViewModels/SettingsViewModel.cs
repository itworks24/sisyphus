using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Sisyphus.Settings;
using ConfigApplication.Annotations;
using Microsoft.Expression.Interactivity.Core;

namespace ConfigApplication.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
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

        public SettingsRepresent SettingsRepresentInstance { get; private set; }
        public object Settings {
            get { return SettingsRepresentInstance; }
        }
        public  ICommand SaveCommand { get; private set; } 

        public SettingsViewModel(Type settingsType, string groupName)
        {
            SettingsRepresentInstance = (SettingsRepresent)Activator.CreateInstance(settingsType, groupName);
            SaveCommand = new ActionCommand(t=>SettingsRepresentInstance.SetSettings());
        }
    }
}
