using System.ComponentModel;
using Sisyphus.Settings;

namespace Sysiphus.Tasks.Settings
{
    class SampleTaskSettings : SettingsRepresent
    {

        [Category("Sample category")]
        [DisplayName("Sample property name")]
        public string SampleProperty { get; set; } = "SamplePropertyText";

        [Category("Sample category")]
        [DisplayName("Execution dealy in seconds. Max value is 10.")]
        public int WaitTime { get; set; } = 5;

        [Category("Sample category")]
        [DisplayName("Add error-level log record")]
        public bool AddErrorLogRecord { get; set; } = true;

        [Category("Sample category")]
        [DisplayName("Add information-level log record")]
        public bool AddErrorInformationLogRecord { get; set; } = true;

        public SampleTaskSettings(string groupName) : base(groupName)
        {
        }
    }
}
