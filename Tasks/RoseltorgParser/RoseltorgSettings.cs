using System.ComponentModel;
using Sisyphus.Settings;

namespace Sysiphus.Tasks.Settings
{
    class RoseltorgSettings : SettingsRepresent
    {

        [Category("Sample category")]
        [DisplayName("Sample property name")]
        public string SampleProperty { get; set; } = "SamplePropertyText";

        public RoseltorgSettings(string groupName) : base(groupName)
        {
        }
    }
}
