using System.ComponentModel;
using Sisyphus.Settings;

namespace SampleTask
{
    class SampleTaskSettings : SettingsRepresent
    {

        [Category("Sample category")]
        [DisplayName("Sample property name")]
        public string SampleProperty { get; set; } = "SamplePropertyText";

        [Category("Sample category")]
        [DisplayName("Execution dealy in seconds. Max value is 10.")]
        public int WaitTime { get; set; } = 5;

        public SampleTaskSettings(string groupName) : base(groupName)
        {
        }
    }
}
