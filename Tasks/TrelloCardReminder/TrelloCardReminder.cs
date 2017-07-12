using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Manatee.Trello;
using Manatee.Trello.ManateeJson;
using Manatee.Trello.WebApi;
using Sisyphus.Tasks;

namespace Sisyphus
{

    public struct ListsStruct
    {
        public List TrelloInWorkList;
        public List TrelloFirstList;
        public readonly bool BoardHaveAllList;

        public ListsStruct(TrelloCardReminderSettings settings, Board board)
        {
            TrelloInWorkList = null;
            TrelloFirstList = null;
            BoardHaveAllList = false;
            foreach (var list in board.Lists)
            {
                if (list.Name == settings.TrelloInWorkListName)
                    TrelloInWorkList = list;
                else if (list.Name == settings.TrelloFirstListName)
                    TrelloFirstList = list;
            }
            if (TrelloInWorkList == null || TrelloFirstList == null) return;
            BoardHaveAllList = true;
        }
    }

    internal static class TrelloRequestCounter
    {
        private const int TrelloInterval = 1;
        private const int TrelloMaxRequestCountPerInterval = 50;
        private static int _trelloRequestCount;
        public static int TrelloPostCount
        {
            get { return _trelloRequestCount; }
            set
            {
                _trelloRequestCount += value;
                if (_trelloRequestCount <= TrelloMaxRequestCountPerInterval) return;
                Thread.Sleep(TrelloInterval * 1000);
                _trelloRequestCount = 0;
            }
        }
    }

    public partial class TrelloCardReminder
    {

        public bool ExecuteProcess()
        {
            var settings = GetSettings(typeof(TrelloCardReminderSettings)) as TrelloCardReminderSettings;

            var serializer = new ManateeSerializer();
            TrelloConfiguration.Serializer = serializer;
            TrelloConfiguration.Deserializer = serializer;
            TrelloConfiguration.JsonFactory = new ManateeFactory();
            TrelloConfiguration.RestClientProvider = new WebApiClientProvider();
            TrelloAuthorization.Default.AppKey = settings.TrelloAppKey;
            TrelloAuthorization.Default.UserToken = settings.TrelloUserToken;

            var organization = new Organization(settings.OrganizationID);
            var organizationMembers = organization.Members.Select(t => t.UserName).ToArray();
            TrelloRequestCounter.TrelloPostCount += 2;

            foreach (var board in organization.Boards)
            {
                if (!board.Members.Any(t => t == Member.Me)) board.Memberships.Add(Member.Me, BoardMembershipType.Normal);

                var listStruct = new ListsStruct(settings, board);
                if (!listStruct.BoardHaveAllList) { continue; }

                var boardName = board.Name;

                var powerUpSettings = board.GetPowerUpSettings(settings.PowerUpId);

                foreach (var card in listStruct.TrelloInWorkList.Cards.Where(c => !c.IsArchived.GetValueOrDefault()))
                {

                    var estimateHours = Double.MaxValue;
                    if (powerUpSettings != null)
                    {
                        var estimeteValue = powerUpSettings.Where(t => t.Id == $"{settings.EstimateValuePrefix}{card.Id}{settings.EstimateValuePostfix}").FirstOrDefault();
                        if (estimeteValue != null) estimateHours = Convert.ToDouble(estimeteValue.Value);
                    }

                    var cardHistory = new CardHistory(listStruct, card, organizationMembers);
                    var allWorkTime = cardHistory.InWorkActionRecordList.Sum(t => t.Duration) + cardHistory.Comments.Sum(t => t.Value);
                    var lastAction = cardHistory.InWorkActionRecordList.FirstOrDefault();
                    var lastPeriodWorkTime = lastAction.Duration;

                    if (allWorkTime > estimateHours)
                    {
                        if (settings.MoveCardIfEstimateExceeded) { card.List = listStruct.TrelloFirstList; card.Position = 1; }
                        if (settings.LeaveComment) card.Comments.Add($"@{lastAction.BeginerUserName}\n{settings.MoveCommentContent}");
                    }
                    else if (lastAction.Duration > settings.MaxTimeToStayInWork)
                    {
                        if (settings.MoveCardIfMaxTimeExceeded) { card.List = listStruct.TrelloFirstList; card.Position = 1; }
                        if (settings.LeaveComment) card.Comments.Add($"@{lastAction.BeginerUserName}\n{settings.MoveCommentContent}");
                    }
                    else if (allWorkTime > (estimateHours - settings.WarnningCommentLeaveTime))
                    {
                        if (settings.LeaveComment) card.Comments.Add($"@{lastAction.BeginerUserName}\n{settings.WarningCommentContent}");
                    }
                    else if (lastAction.Duration > (settings.MaxTimeToStayInWork - settings.WarnningCommentLeaveTime))
                    {
                        if (settings.LeaveComment) card.Comments.Add($"@{lastAction.BeginerUserName}\n{settings.WarningCommentContent}");
                    }
                }
            }
            return true;
        }
    }

    public partial class TrelloCardReminder : TaskBase
    {
        public override ExecutionDelegate ExecutuionMethod => ExecuteProcess;
        public override IEnumerable<Type> TaskSettings => new[] { typeof(TrelloCardReminderSettings) };
    }
}
