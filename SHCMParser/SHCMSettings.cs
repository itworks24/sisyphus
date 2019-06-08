using System.ComponentModel;
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

        [Category("Store house test")]
        [DisplayName("ParentTreeRid")]
        public uint ParentTreeRid { get; set; } = 0;

        [Category("Store house test")]
        [DisplayName("ParentTreeAbbr")]
        public string ParentTreeAbbr { get; set; } = "";

        [Category("DB")]
        [DisplayName("Provider name")]
        public string ProviderName { get; set; } = "System.Data.SqlClient";

        [Category("DB")]
        [DisplayName("Connection string")]
        public string ConnectionString { get; set; } = "data source=localhost;initial catalog=LondonRKeeper;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";

        [Category("Misc")]
        [DisplayName("Select first")]
        public int SelectFirst { get; set; } = 10;

        [Category("Misc")]
        [DisplayName("Reload goods")]
        public bool ReloadGoods { get; set; } = false;

        [Category("Misc")]
        [DisplayName("Reload goods cmplects")]
        public bool ReloadGoodsBaseComplects { get; set; } = false;

        [Category("Misc")]
        [DisplayName("Reload cmplects HDR")]
        public bool ReloadComplectsHDR { get; set; } = true;

        [Category("Misc")]
        [DisplayName("Reload cmplects list")]
        public bool ReloadComplectsList { get; set; } = true;

        [Category("Misc")]
        [DisplayName("Dont reload tree elements")]
        public bool DontReloadTreeElements { get; set; } = true;

        public SHCMSettings(string groupName) : base(groupName)
        {
        }
    }
}
