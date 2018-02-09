using System.ComponentModel;
using Sisyphus.Settings;

namespace Sysiphus.Tasks.RKBot.Settings
{
    class RKRecieverSettings : SettingsRepresent
    {

        [Category("Mongo DB settings")]
        [DisplayName("Mongo DB connection string")]
        public string MongoConnectionString { get; set; } = "";

        [Category("RK7 API")]
        [DisplayName("RK7 XML interface address")]
        public string RK7ApiAddress { get; set; } = "";

        [Category("RK7 API")]
        [DisplayName("RK7 XML interface user")]
        public string RK7ApiUser { get; set; } = "";

        [Category("RK7 API")]
        [DisplayName("RK7 XML interface password")]
        public string RK7ApiPassword { get; set; } = "";

        public RKRecieverSettings(string groupName) : base(groupName)
        {
        }
    }
}
