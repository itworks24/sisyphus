using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Manatee.Trello;
using Manatee.Trello.ManateeJson;
using Manatee.Trello.WebApi;
using Sisyphus.itws;
using Sisyphus.Tasks;

namespace Sisyphus
{

    public struct ListsStruct
    {
        public List TrelloInWorkList;
        public List TrelloDoneList;
        public List TrelloFirstList;
        public readonly bool BoardHaveAllList;

        public ListsStruct(TrelloCheckerSettings settings, Board board)
        {
            TrelloInWorkList = null;
            TrelloFirstList = null;
            TrelloDoneList = null;
            BoardHaveAllList = false;
            foreach (var list in board.Lists)
            {
                if (list.Name == settings.TrelloInWorkListName)
                    TrelloInWorkList = list;
                else if (list.Name == settings.TrelloFirstListName)
                    TrelloFirstList = list;
                else if (list.Name == settings.TrelloDoneListName)
                    TrelloDoneList = list;
            }
            if (TrelloInWorkList == null || TrelloFirstList == null || TrelloDoneList == null) return;
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

    public partial class TrelloChecker
    {
        private string LabelName { get; } = "Dipper checks this";
        private LabelColor LabelColor { get; } = LabelColor.Yellow;
        private int MaxLabelCount { get; } = 30;
        private EnterpriseWsWrapper EnterpriseWsWrapper { get; set; }  

        private void SyncContractors(Contractor[] contractorsArray, Organization currentOrganiztion, TrelloCheckerSettings settings)
        {
            var emptyBoardContractors = new List<Contractor>();
            var sourceBoard = new Board(settings.SourceTrelloBoardShortlink);
            foreach (var contractor in contractorsArray)
            {
                if (!contractor.BoardId.BoardExists())
                {
                    emptyBoardContractors.Add(contractor);
                    continue;
                }
                TrelloRequestCounter.TrelloPostCount += 3;
                var boardShortId = new Board(contractor.BoardId).GetBoardId();
                if (boardShortId == contractor.BoardId) continue;
                contractor.BoardId = boardShortId;
                EnterpriseWsWrapper.SetContractor(contractor);
            }
            foreach (var contractor in emptyBoardContractors)
            {
                TrelloRequestCounter.TrelloPostCount += 3;
                var newContractorsBoard =
                    currentOrganiztion.Boards.Add($"{settings.BoardNamePrefix} {contractor.Represent}");
                newContractorsBoard.Preferences.PermissionLevel = BoardPermissionLevel.Org;
                sourceBoard.CloneBoard(newContractorsBoard, true);
                contractor.BoardId = newContractorsBoard.GetBoardId();
                if (!EnterpriseWsWrapper.SetContractor(contractor))
                    newContractorsBoard.IsClosed = true;
            }
        }

        public bool ExecuteProcess()
        {
            var settings = GetSettings(typeof(TrelloCheckerSettings)) as TrelloCheckerSettings;

            EnterpriseWsWrapper = new EnterpriseWsWrapper(settings);
            var contractorsArray = EnterpriseWsWrapper.GetConractorsArray();
            var nomenclatureArray = EnterpriseWsWrapper.GetNomenclatureArray().OrderBy(t => t.Name).Take(MaxLabelCount);

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

            SyncContractors(contractorsArray, organization, settings);

            foreach (var contractor in contractorsArray)
            {
                CreateLogRecord(contractor.BoardId);
                var board = new Board(contractor.BoardId);
                var label = board.CheckLabel(LabelName, LabelColor);
                board.SyncLabels(nomenclatureArray.Select(t => t.Represent).ToArray(), LabelColor.Purple);

                TrelloRequestCounter.TrelloPostCount += 3;
                var listStruct = new ListsStruct(settings, board);
                if (!listStruct.BoardHaveAllList)
                {
                    CreateLogRecord($"There are not enoght lists in board \"{board.Name}\" ({board.Url})", System.Diagnostics.EventLogEntryType.Error);
                    continue;
                }

                if (!board.Members.Any(t => t == Member.Me)) board.Memberships.Add(Member.Me, BoardMembershipType.Normal);

                foreach (var card in listStruct.TrelloDoneList.Cards.Where(c => !c.IsArchived.GetValueOrDefault()))
                {
                    TrelloRequestCounter.TrelloPostCount += 1;
                    if (card.Labels.Count(l => l.Name == LabelName) > 0) continue;
                    CardHistory cardHistory;
                    try
                    {
                        cardHistory = new CardHistory(listStruct, card, organizationMembers);
                    }
                    catch (Exception e)
                    {
                        CreateLogRecord($"Card history retrieve error. \"{board.Name}\" ({card.Url})", System.Diagnostics.EventLogEntryType.Error);
                        continue;
                    }
                    var errorRepresent = string.Empty;
                    if (EnterpriseWsWrapper.AddCardHistory(cardHistory, ref errorRepresent))
                    {
                        TrelloRequestCounter.TrelloPostCount += 1;
                        card.Labels.Add(label);
                    }
                    else
                        CreateLogRecord(errorRepresent, System.Diagnostics.EventLogEntryType.Error);
                }
            }
            return true;
        }
    }

    public partial class TrelloChecker : TaskBase
    {
        public override ExecutionDelegate ExecutuionMethod => ExecuteProcess;
        public override IEnumerable<Type> TaskSettings => new[] { typeof(TrelloCheckerSettings) };
    }
}
