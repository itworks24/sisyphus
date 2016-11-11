using System.ComponentModel;
using Sisyphus.Settings;

namespace Sisyphus.Tasks.Settings
{
    public class TrelloReportSettings : SettingsRepresent
    {

        [Category("Trello auth")]
        [DisplayName("Trello app key")]
        [Description("To get app token go to https://trello.com/app-key")]
        public string TrelloAppKey { get; set; } = "";

        [Category("Trello auth")]
        [DisplayName("Trello user token")]
        [Description("To get app token go to https://trello.com/1/authorize?expiration=never&name=SisyphusTrelloCheckerDipper&key=REPLACEWITHYOURKEY")]
        public string TrelloUserToken { get; set; } = @"https://trello.com/1/authorize?expiration=never&name=SisyphusTrelloReporter&key=REPLACEWITHYOURKEY";

        [Category("Trello boards")]
        [DisplayName("Organiztion id")]
        public string OrganizationID { get; set; } = "";

        [Category("Trello boards")]
        [DisplayName("Count cards in last list as completed")]
        public bool CountLastListCardsAsCompleted { get; set; } = false;

        [Category("Misc")]
        [DisplayName("Address to send report")]
        public string AddressToSendReport { get; set; } = "";

        public TrelloReportSettings(string groupName) : base(groupName)
        {
        }
    }
}
