using System;
using System.Collections.Generic;
using System.Linq;
using Manatee.Trello;
using Action = Manatee.Trello.Action;

namespace Sisyphus
{
    public struct CardHistory
    {
        public struct ActionRecord
        {
            internal bool Opened;

            private Member Beginer { get; set; }
            private Member Finisher { get; set; }
            private List List { get; set; }


            public DateTime StartDateTime { get; set; }

            public DateTime FinishDateTime { get; set; }

            public string BeginerId => Beginer?.Id != null ? Beginer.Id : string.Empty;

            public string BeginerUserName => Beginer?.UserName != null ? Beginer.UserName : string.Empty;       

            public string FinisherId => Finisher?.Id != null ? Finisher.Id : string.Empty;

            public string FinisherUserName => Finisher?.UserName != null ? Finisher.UserName : string.Empty;

            public string ListId => List?.Id != null ? List.Id : string.Empty;

            public void OpenRecord(Member finisher, DateTime finishDateTime, List list)
            {
                Finisher = finisher;
                FinishDateTime = finishDateTime;
                List = list;
                Opened = true;
            }

            public void CloseRecord(Member beginer, DateTime startDateTime)
            {
                Beginer = beginer;
                StartDateTime = startDateTime;
                Opened = false;
            }
        }

        public struct Comment
        {
            private Member Author { get; set; }
            public DateTime CommentDateTime { get; set; }
            public string AuthorUserName => Author?.UserName != null ? Author.UserName : string.Empty;
            public string Text { get; set; }

            public Comment(Action comment)
            {
                Author = comment.Creator;
                CommentDateTime = comment.Date.GetValueOrDefault();
                Text = comment.Data.Text;
            }
        }

        private Card Card { get; }
        private Member Creator { get; }
        private CardLabelCollection Labels { get; }


        public string BoardId { get { return Card != null ? Card.Board.Id : string.Empty; } }

        public string CardId => Card != null ? Card.Id : string.Empty;

        public string CreatorId => Creator != null ? Creator.Id : string.Empty;

        public DateTime CreationDateTime { get; }

        public string CreatorUserName => Creator != null ? Creator.UserName : string.Empty;

        public List<string> LabelsNames { get { return Labels.Select(a => a.Name).ToList(); } }

        public List<ActionRecord> InWorkActionRecordList { get; }

        public List<Comment> Comments { get; }
        public string Name => Card != null ? Card.Name : string.Empty;
        public string Text => Card != null ? Card.Description : string.Empty;
        public string Url => Card != null ? Card.Url : string.Empty;

        private static List<ActionRecord> FillActionRecordList(IEnumerable<Action> actions, List list, IEnumerable<string> organizationMembers)
        {
            var actionList = new List<ActionRecord>();
            var currentActionRecord = new ActionRecord();
            foreach (var action in actions)
            {
                TrelloRequestCounter.TrelloPostCount += 1;
                if (action.Data.ListBefore != null && action.Data.ListBefore.Id == list.Id)
                {
                    currentActionRecord = new ActionRecord();
                    currentActionRecord.OpenRecord(action.Creator, action.CreationDate, list);
                }
                else if (currentActionRecord.Opened && action.Data.ListAfter != null && action.Data.ListAfter.Id == list.Id)
                {
                    currentActionRecord.CloseRecord(action.Creator, action.CreationDate);
                    if (!organizationMembers.Contains(action.Creator.UserName)) continue;
                    TrelloRequestCounter.TrelloPostCount += 1;
                    actionList.Add(currentActionRecord);
                }
            }
            return actionList;
        }

        public CardHistory(ListsStruct listsStruct, Card card, IEnumerable<string> organizationMembers)
        {
            Card = card;
            Labels = card.Labels;
            var createActionArray =
                listsStruct.TrelloFirstList.Actions.Filter(ActionType.CreateCard).ToArray();
            var hasCreateAction = createActionArray.Any(a => a.Data.Card.Id == card.Id);
            var createAction = hasCreateAction ? createActionArray.First(a => a.Data.Card.Id == card.Id) : null;
            Creator = hasCreateAction ? createAction.Creator : null;
            CreationDateTime = hasCreateAction ? createAction.Date.GetValueOrDefault() : DateTime.Now;

            TrelloRequestCounter.TrelloPostCount += 3;

            InWorkActionRecordList = FillActionRecordList(card.Actions.Filter(ActionType.UpdateCard).ToArray(), listsStruct.TrelloInWorkList, organizationMembers);

            Comments = new List<Comment>();
            foreach (
                var comment in
                    card.Comments.Where(
                        c =>
                            organizationMembers.Contains(c.Creator.UserName) &&
                            (c.Data.Text.StartsWith("+") || c.Data.Text.StartsWith("-"))))
            {
                TrelloRequestCounter.TrelloPostCount += 1;
                Comments.Add(new Comment(comment));
            }
        }
    }
}