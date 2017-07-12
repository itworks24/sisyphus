using System.ComponentModel;
using Sisyphus.Settings;

namespace Sisyphus.Tasks.Settings
{
    class PowershellExecutorSettings : SettingsRepresent
    {

        [Category("Command")]
        [DisplayName("Single command")]
        public string SingleCommand { get; set; } = "";

        [Category("Command")]
        [DisplayName("Script file path")]
        public string ScriptPath { get; set; } = "";

        [Category("Command")]
        [DisplayName("Arguments")]
        public string Arguments { get; set; } = "";

        [Category("Command")]
        [DisplayName("Work directory")]
        public string WorkDirectory { get; set; } = "";

        public PowershellExecutorSettings(string groupName) : base(groupName)
        {
        }
    }
}
