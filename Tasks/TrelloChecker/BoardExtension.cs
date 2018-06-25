using Manatee.Trello;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sisyphus
{
    internal static class BoardExtension
    {
        internal static string GetBoardId(this IBoard board)
        {
            TrelloRequestCounter.TrelloPostCount += 1;
            var url = new Uri(board.Url);
            var boardShortId = url.Segments.Reverse().Skip(1).Take(1).First();
            return boardShortId;
        }

        internal static bool BoardExists(this string id)
        {
            Board board;
            var boardExists = false;
            for (var i = 0; i < 3; i++)
            {
                try
                {
                    TrelloRequestCounter.TrelloPostCount++;
                    board = new Board(id);
                    boardExists = !board.IsClosed.GetValueOrDefault();
                    break;
                }
                catch (Exception e)
                {
                }
            }
            if (!boardExists)
            {
                return false;
            }
            return boardExists;
        }

        internal static ILabel CheckLabel(this Board board, string labelName, LabelColor labelColor)
        {
            TrelloRequestCounter.TrelloPostCount += 3;
            var label = board.Labels.FirstOrDefault(l => l.Name == labelName);
            var task = board.Labels.Add(labelName, labelColor);
            task.RunSynchronously();
            return task.Result;
        }

        internal static void SyncLabels(this Board board, IEnumerable<string> newLabelNameCollection, LabelColor labelColor)
        {
            var boardLabels = board.Labels.Select(l => new { l.Name, l.Color, l.Id }).Where(l => l.Color == labelColor).ToArray();
            var unwantedLabels = boardLabels.Where(l => newLabelNameCollection.All(c => c != l.Name)).ToArray();
            var newLabels = newLabelNameCollection.Where(c => boardLabels.All(l => l.Name != c)).ToArray();
            TrelloRequestCounter.TrelloPostCount += boardLabels.Count() + unwantedLabels.Count() + newLabels.Count();
            foreach (var missedLabel in unwantedLabels)
            {
                TrelloRequestCounter.TrelloPostCount++;
                //board.Labels.Delete(missedLabel.Id);
            }
            foreach (var label in newLabels)
            {
                TrelloRequestCounter.TrelloPostCount += 3;
                CheckLabel(board, label, labelColor);
            }
        }

        internal static void CloneBoard(this Board source, Board dest, bool newBoard = false)
        {
            if (newBoard)
            {
                TrelloRequestCounter.TrelloPostCount += 2;
                var oldLists = dest.Lists.ToList();
                foreach (var list in oldLists) list.IsArchived = true;
                source.Preferences.Background = source.Preferences.Background;
            }
            foreach (var list in source.Lists.Reverse())
            {
                TrelloRequestCounter.TrelloPostCount++;
                if (!dest.Lists.All(l => l.Name != list.Name)) continue;
                TrelloRequestCounter.TrelloPostCount += 1;
                dest.Lists.Add(list.Name);
            }
        }
    }
}
