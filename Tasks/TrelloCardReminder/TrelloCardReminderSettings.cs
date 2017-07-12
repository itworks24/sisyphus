using System.ComponentModel;
using Sisyphus.Settings;

namespace Sisyphus
{
    public class TrelloCardReminderSettings : SettingsRepresent
    {

        [Category("Trello auth")]
        [DisplayName("Trello app key")]
        [Description("To get app token go to https://trello.com/app-key")]
        public string TrelloAppKey { get; set; } = "c2e2223a5a17c1bf35330c0e43465098";

        [Category("Trello auth")]
        [DisplayName("Trello user token")]
        [Description("To get app token go to https://trello.com/1/authorize?expiration=never&scope=read,write,account&response_type=token&name=SisyphusTrelloCardReminder&key=c2e2223a5a17c1bf35330c0e43465098")]
        public string TrelloUserToken { get; set; } = "";

        [Category("Trello boards")]
        [DisplayName("Trello 'testing' list name")]
        public string TrelloFirstListName { get; set; } = "Первичный список";

        [Category("Trello boards")]
        [DisplayName("Trello 'in work' list name")]
        public string TrelloInWorkListName { get; set; } = "В работе";

        [Category("Trello boards")]
        [DisplayName("Organiztion id")]
        public string OrganizationID { get; set; } = "";

        [Category("Trello boards")]
        [DisplayName("Board name prefix")]
        public string BoardNamePrefix { get; set; } = "[dip]";

        [Category("PowerUp data")]
        [DisplayName("PowerUp id")]
        public string PowerUpId { get; set; } = "58aa0b4806cee280a969a3c2";

        [Category("PowerUp data")]
        [DisplayName("Plugin data estimate value prefix")]
        public string EstimateValuePrefix { get; set; } = "card-";

        [Category("PowerUp data")]
        [DisplayName("Plugin data estimate value postfix")]
        public string EstimateValuePostfix { get; set; } = "-estemite";

        [Category("Behavior")]
        [DisplayName("Max time card to stay \"in work\" (in hours)")]
        public int MaxTimeToStayInWork { get; set; } = 4;

        [Category("Behavior")]
        [DisplayName("Warning comment leave time (in hours)")]
        public double WarnningCommentLeaveTime { get; set; } = 0.5;

        [Category("Behavior")]
        [DisplayName("Move card if it stays to long \"in work\"")]
        public bool MoveCardIfMaxTimeExceeded { get; set; } = true;

        [Category("Behavior")]
        [DisplayName("Move card if it time limit exceed estimate value")]
        public bool MoveCardIfEstimateExceeded { get; set; } = true;

        [Category("Behavior")]
        [DisplayName("Leave a comment when card moved")]
        public bool LeaveComment { get; set; } = true;

        [Category("Behavior")]
        [DisplayName("Comment content")]
        public string MoveCommentContent { get; set; } = "Card has been moved. Work time has been exceeded. Have a nice day!";

        [Category("Behavior")]
        [DisplayName("Comment content")]
        public string WarningCommentContent { get; set; } = "Card would be mobed soon.";

        public TrelloCardReminderSettings(string groupName) : base(groupName)
        {
        }
    }
}
