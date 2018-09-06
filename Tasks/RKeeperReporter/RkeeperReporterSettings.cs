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

        [Category("DB")]
        [DisplayName("Classification group SIFR")]
        public short ClassificationGroupSIFR { get; set; } = 0;

        [Category("DB")]
        [DisplayName("Restaurant CODE")]
        public int restaurantCode { get; set; } = 0;

        [Category("DB")]
        [DisplayName("Database prefix")]
        public int databasePrefix { get; set; } = 0;

        [Category("DB")]
        [DisplayName("Database code")]
        public int databaseCode { get; set; } = 0;

        [Category("DB")]
        [DisplayName("Database name")]
        public string databaseName { get; set; } = "default DB";

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
        [DisplayName("Upload start date")]
        public DateTime ReportStartDateTime { get; set; } = DateTime.Now;

        [Category("Misc")]
        [DisplayName("Upload end date")]
        public DateTime ReportEndDateTime { get; set; } = DateTime.Now;

        [Category("Misc")]
        [DisplayName("Upload last N days")]
        public int UploadLastDays { get; set; } = 0;

        [Category("Misc")]
        [DisplayName("Send N records per call")]
        public int SendRecordsCount { get; set; } = 200;

        public RkeeperReporterSettings(string groupName) : base(groupName)
        {
        }
    }
}
