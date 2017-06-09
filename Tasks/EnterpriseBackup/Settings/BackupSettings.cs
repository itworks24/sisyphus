using System.ComponentModel;

namespace Sisyphus.Settings
{
    public class BackupSettings : SettingsRepresent
    {
        [Category("Source")]
        [DisplayName("1C .v8i file")]
        public string ListFile { get; set; }

        [Category("File-type bases")]
        [DisplayName("Path for 1cv8.exe")]
        public string EnterprisePath { get; set; }

        [Category("File-type bases")]
        [DisplayName("Use 7z to archiev 1c base")]
        public bool UseArchiev { get; set; } = true;

        [Category("Clien-server type bases")]
        [DisplayName("Restart 1c server")]
        public bool Restart { get; set; } = false;

        [Category("Clien-server type bases")]
        [DisplayName("Server-name")]
        public string Server { get; set; }

        [Category("Clien-server type bases")]
        [DisplayName("Service name")]
        public string ServiceName { get; set; } = "1C:Enterprise 8.3 Server Agent";

        [Category("Output")]
        [DisplayName("Destination path")]
        public string DestinationPath { get; set; }

        [Category("Output")]
        [DisplayName("Output file format")]
        public string OutputFormat { get; set; } = "{Name}_{date}";
       

        public BackupSettings(string groupName) : base(groupName)
        {
        }
    }
}
