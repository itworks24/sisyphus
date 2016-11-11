namespace Sisyphus.Settings
{
    public class BackupSettings : SettingsRepresent
    {
        public string ListFile { get; set; }
        public string EnterprisePath { get; set; }
        public string DestinationPath { get; set; }
        public string OutputFormat { get; set; } = "{Name}_{date}";
        public bool UseLog { get; set; } = true;
        public bool Restart { get; set; } = false;
        public string Server { get; set; }
        public string ServiceName { get; set; } = "1C:Enterprise 8.3 Server Agent";

        public BackupSettings(string groupName) : base(groupName)
        {
        }
    }
}
