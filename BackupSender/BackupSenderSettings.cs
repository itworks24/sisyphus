using System.ComponentModel; 
using Sisyphus.Settings;

namespace Sisyphus
{
    enum ProtocolType
    {
        Ftp,
        Sftp
    }

    class BackupSenderSettings : SettingsRepresent
    {
        [Category("Source")]
        [DisplayName("Source directory")]
        public string Directory { get; set; } = "";

        [Category("Source")]
        [DisplayName("Search recursive")]
        public bool Recursive { get; set; } = true;

        [Category("Source")]
        [DisplayName("File mask")]
        public string Mask { get; set; } = "*.*";

        [Category("Source")]
        [DisplayName("Search files older then (h)")]
        public int FileAge { get; set; } = 24 * 7;

        [Category("Source")]
        [DisplayName("Created date pattern (* * * * *) (minutes hours days months days_of_week)")]
        public string DatePattern { get; set; } = "* * * * *";

        [Category("Source")]
        [DisplayName("Move file to directory after sucessfull transfer")]
        public string MoveToDirectory { get; set; } = "";

        [Category("Source")]
        [DisplayName("Delete file after successful transfer")]
        public bool DeleteFile { get; set; } = true;

        [Category("Destination")]
        [DisplayName("Destination path")]
        public string DestinationPath { get; set; } = "";

        [Category("Destination")]
        [DisplayName("Server address")]
        public string ServerAddress { get; set; } = "";

        [Category("Destination")]
        [DisplayName("Protocol")]
        public ProtocolType Protocol { get; set; } = ProtocolType.Sftp;

        [Category("Destination")]
        [DisplayName("User name")]
        public string UserName { get; set; } = "";

        [Category("Destination")]
        [DisplayName("Password")]
        public string Password { get; set; } = "";

        public BackupSenderSettings(string groupName) : base(groupName)
        {
        }
    }
}
