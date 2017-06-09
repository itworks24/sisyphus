using System.ComponentModel;
using Sisyphus.Settings;

namespace Sisyphus
{
    public class TrelloCheckerSettings : SettingsRepresent
    {

        [Category("Trello auth")]
        [DisplayName("Trello app key")]
        [Description("To get app token go to https://trello.com/app-key")]
        public string TrelloAppKey { get; set; } = "";

        [Category("Trello auth")]
        [DisplayName("Trello user token")]
        [Description("To get app token go to https://trello.com/1/authorize?expiration=never&name=SisyphusTrelloCheckerDipper&key=REPLACEWITHYOURKEY")]
        public string TrelloUserToken { get; set; } = "";

        [Category("Trello boards")]
        [DisplayName("Trello 'testing' list name")]
        public string TrelloFirstListName { get; set; } = "Первичный список";

        [Category("Trello boards")]
        [DisplayName("Trello 'in work' list name")]
        public string TrelloInWorkListName { get; set; } = "В работе";

        [Category("Trello boards")]
        [DisplayName("Trello 'done' list name")]
        public string TrelloDoneListName { get; set; } = "Готово!";
        
        [Category("Trello boards")]
        [DisplayName("Source Trello board shortlink")]
        public string SourceTrelloBoardShortlink { get; set; } = "";

        [Category("Trello boards")]
        [DisplayName("Organiztion id")]
        public string OrganizationID { get; set; } = "";

        [Category("Trello boards")]
        [DisplayName("Board name prefix")]
        public string BoardNamePrefix { get; set; } = "[dip]";

        [Category("Enterprise auth")]
        [DisplayName("Web Service defenition path")]
        public string EnterpriseWsPath { get; set; } = "";

        [Category("Enterprise auth")]
        [DisplayName("Enterprise User name")]
        public string EnterpriseUserName { get; set; } = "";

        [Category("Enterprise auth")]
        [DisplayName("Enterprise password")]
        public string EnterprisePassword { get; set; } = "";


        public TrelloCheckerSettings(string groupName) : base(groupName)
        {
        }
    }
}
