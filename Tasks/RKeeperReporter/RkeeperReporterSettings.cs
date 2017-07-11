using System.ComponentModel;
using Sisyphus.Settings;
using System;

namespace Sysiphus.Tasks.Settings
{
    class RkeeperReporterSettings : SettingsRepresent
    {

        [Category("DB")]
        [DisplayName("Connection string")]
        public string ConnectionString { get; set; } = "data source=localhost;initial catalog=LondonRKeeper;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";

        [Category("DB")]
        [DisplayName("Provider name")]
        public string ProviderName { get; set; } = "System.Data.SqlClient";

        [Category("Enterprise auth")]
        [DisplayName("Web Service defenition path")]
        public string EnterpriseWsPath { get; set; } = "";

        [Category("Enterprise auth")]
        [DisplayName("Enterprise User name")]
        public string EnterpriseUserName { get; set; } = "";

        [Category("Enterprise auth")]
        [DisplayName("Enterprise password")]
        public string EnterprisePassword { get; set; } = "";

        [Category("Misc")]
        [DisplayName("Upload date")]
        public DateTime ReportDateTime { get; set; } = DateTime.Now;

        [Category("Misc")]
        [DisplayName("Upload last N days")]
        public int UploadLastDays { get; set; } = 0;

        public RkeeperReporterSettings(string groupName) : base(groupName)
        {
        }
    }
}
