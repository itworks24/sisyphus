﻿using System.ComponentModel;
using Sisyphus.Settings;

namespace Sysiphus.Tasks.Settings
{
    class SHCMSettings : SettingsRepresent
    {

        [Category("Store house")]
        [DisplayName("Server address")]
        public string SHServerAddress { get; set; }

        [Category("Store house")]
        [DisplayName("Server port")]
        public uint SHServerPort { get; set; } = 9700;

        [Category("Store house")]
        [DisplayName("User name")]
        public string SHUserName { get; set; }

        [Category("Store house")]
        [DisplayName("Password")]
        public string SHPassword { get; set; }

        [Category("DB")]
        [DisplayName("Provider name")]
        public string ProviderName { get; set; } = "System.Data.SqlClient";

        [Category("DB")]
        [DisplayName("Connection string")]
        public string ConnectionString { get; set; } = "data source=localhost;initial catalog=LondonRKeeper;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";

        public SHCMSettings(string groupName) : base(groupName)
        {
        }
    }
}