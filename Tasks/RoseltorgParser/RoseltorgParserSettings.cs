using System.ComponentModel;
using Sisyphus.Settings;

namespace Sysiphus.Tasks.Settings
{
    class RoseltorgSettings : SettingsRepresent
    {

        [Category("Target")]
        [DisplayName("Directory")]
        public string SampleProperty { get; set; } = @"\roseltorg";

        public RoseltorgSettings(string groupName) : base(groupName)
        {
        }
    }
}
