using System.ComponentModel;
using Sisyphus.Settings;

namespace Sysiphus.Tasks.Settings
{
    class RkeeperReporterSettings : SettingsRepresent
    {

        [Category("DB")]
        [DisplayName("ConnectionString")]
        public string ConnectionString { get; set; } = "";

        [Category("Enterprise auth")]
        [DisplayName("Web Service defenition path")]
        public string EnterpriseWsPath { get; set; } = "";

        [Category("Enterprise auth")]
        [DisplayName("Enterprise User name")]
        public string EnterpriseUserName { get; set; } = "";

        [Category("Enterprise auth")]
        [DisplayName("Enterprise password")]
        public string EnterprisePassword { get; set; } = "";

        public RkeeperReporterSettings(string groupName) : base(groupName)
        {
        }
    }
}
