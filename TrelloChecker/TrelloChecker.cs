using System;
using System.Collections.Generic;
using System.Linq;
using Manatee.Trello;
using Manatee.Trello.ManateeJson;
using Manatee.Trello.WebApi;
using Sisyphus.itws;
using Sisyphus.Tasks;
using Label = Manatee.Trello.Label;

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

    public partial class TrelloChecker
    {
        private string LabelName { get; } = "Dipper checks this";
        private LabelColor LabelColor { get; } = LabelColor.Yellow;
        private int MaxLabelCount { get; } = 30;
        private EnterpriseWsWrapper EnterpriseWsWrapper { get; set; }

        private Label CheckLabel(Board board, string labelName, LabelColor labelColor)
        {
            var label = board.Labels.FirstOrDefault(l => l.Name == labelName);
            return label ?? board.Labels.Add(labelName, labelColor);
        }

        private void SyncLabels(Board board, IEnumerable<string> newLabelNameCollection, LabelColor labelColor)
        {
            var boardLabels = board.Labels.Select(l => new { l.Name, l.Color, l.Id }).Where(l => l.Color == labelColor).ToArray();
            var unwantedLabels = boardLabels.Where(l => newLabelNameCollection.All(c => c != l.Name)).ToArray();
            var newLabels = newLabelNameCollection.Where(c => boardLabels.All(l => l.Name != c)).ToArray();
            foreach (var missedLabel in unwantedLabels)
            {
                board.Labels.Delete(missedLabel.Id);
            }
            foreach (var label in newLabels)
            {
                CheckLabel(board, label, labelColor);
            }
        }

        private bool BoardExists(string id)
        {
            try
            {
                var board = new Board(id);
                return !board.IsClosed.GetValueOrDefault();
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void CloneBoard(Board source, Board dest, bool newBoard = false)
        {
            if (newBoard)
            {
                var oldLists = dest.Lists.ToList();
                foreach (var list in oldLists) list.IsArchived = true;
                source.Preferences.Background = source.Preferences.Background;
            }
            foreach (var list in source.Lists.Reverse())
                if (dest.Lists.All(l => l.Name != list.Name))
                    dest.Lists.Add(list.Name);
        }

        private void SyncContractors(Contractor[] contractorsArray, Organization currentOrganiztion, TrelloCheckerSettings settings)
        {
            var emptyBoardContractors =
                contractorsArray.Where(c => string.IsNullOrEmpty(c.BoardId) || !BoardExists(c.BoardId));
            var sourceBoard = new Board(settings.SourceTrelloBoardShortlink);
            foreach (var contractor in contractorsArray.Where(c => !string.IsNullOrEmpty(c.BoardId) && BoardExists(c.BoardId)))
            {
                var boardId = new Board(contractor.BoardId).Id;
                if (boardId == contractor.BoardId) continue;
                contractor.BoardId = boardId;
                EnterpriseWsWrapper.SetContractor(contractor);
            }
            foreach (var contractor in emptyBoardContractors)
            {
                var newContractorsBoard =
                    currentOrganiztion.Boards.Add($"{settings.BoardNamePrefix} {contractor.Represent}");
                CloneBoard(sourceBoard, newContractorsBoard, true);
                contractor.BoardId = newContractorsBoard.Id;
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

            SyncContractors(contractorsArray, organization, settings);

            foreach (var contractor in contractorsArray)
            {
                var board = new Board(contractor.BoardId);
                var label = CheckLabel(board, LabelName, LabelColor);
                SyncLabels(board, nomenclatureArray.Select(t => t.Represent).ToArray(), LabelColor.Purple);

                var listStruct = new ListsStruct(settings, board);
                if (!listStruct.BoardHaveAllList)
                {
                    CreateLogRecord($"There are not enoght lists in board \"{board.Name}\" ({board.Url})", System.Diagnostics.EventLogEntryType.Error);
                    continue;
                }

                foreach (var card in listStruct.TrelloDoneList.Cards.Where(c => !c.IsArchived.GetValueOrDefault()))
                {
                    if (card.Labels.Count(l => l.Name == LabelName) > 0) continue;
                    var cardHistory = new CardHistory(listStruct, card, organizationMembers);
                    var errorRepresent = string.Empty;
                    if (EnterpriseWsWrapper.AddCardHistory(cardHistory, ref errorRepresent))
                        card.Labels.Add(label);
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
