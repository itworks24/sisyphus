using System.ComponentModel;
using Sisyphus.Settings;

namespace Sysiphus.Tasks.Settings
{
    class RoseltorgSettings : SettingsRepresent
    {

        [Category("Misc")]
        [DisplayName("Export path")]
        public string ExportPath { get; set; } = @"c:\temp\roseltorg";

        [Category("Misc")]
        [DisplayName("7-zip path")]
        public string ZipPath { get; set; } = @"C:\Program Files\7-Zip\7z.exe";
 
        public RoseltorgSettings(string groupName) : base(groupName)
        {
        }
    }
}
