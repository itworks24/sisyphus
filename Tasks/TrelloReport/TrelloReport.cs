using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Xsl;
using Manatee.Trello;
using Manatee.Trello.ManateeJson;
using Manatee.Trello.WebApi;
using Sisyphus.Mail;
using Sisyphus.Tasks.Settings;
using Sisyphus.Tasks.Sisyphus.Tasks.TrelloReportStructs;

namespace Sisyphus.Tasks
{
    namespace Sisyphus.Tasks.TrelloReportStructs
    {
        public struct MemberRepresent
        {
            public string FullName { get; set; }
            public string Id { get; set; }

            [XmlArray("MemembersOverdueCards"), XmlArrayItem("CardRepresent")]
            public List<CardRepresent> MemembersOverdueCards { get; set; }

            public MemberRepresent(Member member)
            {
                MemembersOverdueCards = new List<CardRepresent>();
                FullName = member.FullName;
                Id = member.Id;
            }

            public MemberRepresent(MemberRepresent member)
            {
                MemembersOverdueCards = new List<CardRepresent>();
                FullName = member.FullName;
                Id = member.Id;
            }
        }

        public struct CardRepresent
        {
            public string Text { get; set; }
            public DateTime DueDate { get; set; }
            public string DueDateRepresent { get; set; }
            public string CardUrl { get; set; }

            [XmlArray("Members"), XmlArrayItem("MemberRepresent")]
            public List<MemberRepresent> Members { get; set; }

            public CardRepresent(Card card)
            {
                Text = card.Name;
                DueDate = card.DueDate.GetValueOrDefault().ToLocalTime();
                DueDateRepresent = card.DueDate.GetValueOrDefault().ToLocalTime().ToLongDateString();
                CardUrl = card.Url;
                Members = new List<MemberRepresent>();
                foreach (var member in card.Members)
                    Members.Add(new MemberRepresent(member));
            }
        }

        public struct Report
        {
            public string CardsCount
            {
                get { return Cards.Count.ToString(); }
                set { }
            }

            public string CompletedCardsCount
            {
                get { return CompletedCards.Count.ToString(); }
                set { }
            }
            public string OverdueCardsCount
            {
                get { return OverdueCards.Count.ToString(); }
                set { }
            }

            public string OrganizationUrl { get; set; }
            public string OrganizationName { get; set; }

            [XmlArray("Cards"), XmlArrayItem("CardRepresent")]
            public List<CardRepresent> Cards { get; set; }

            [XmlArray("CompletedCards"), XmlArrayItem("CardRepresent")]
            public List<CardRepresent> CompletedCards { get; set; }

            [XmlArray("OverdueCards"), XmlArrayItem("CardRepresent")]
            public List<CardRepresent> OverdueCards { get; set; }

            [XmlArray("OverdueCardsMembers"), XmlArrayItem("Member")]
            public List<MemberRepresent> OverdueCardsMembers { get; set; }

            public Report(Organization organization, bool countLastListCardsAsCompleted)
            {
                Cards = new List<CardRepresent>();
                CompletedCards = new List<CardRepresent>();
                OverdueCards = new List<CardRepresent>();

                OrganizationUrl = organization.Url;
                OrganizationName = organization.DisplayName;

                foreach (var board in organization.Boards)
                {
                    List lastList;
                    try
                    {
                        lastList = board.Lists.Last();
                    }
                    catch
                    {
                        lastList = null;
                    }
                    foreach (var card in board.Cards)
                    {
                        var cardRepresent = new CardRepresent(card);
                        Cards.Add(cardRepresent);
                        if (card.IsArchived.GetValueOrDefault() || (countLastListCardsAsCompleted && lastList != null && card.List.Id == lastList.Id))
                            CompletedCards.Add(cardRepresent);
                        else if (card.DueDate < DateTime.Now)
                            OverdueCards.Add(cardRepresent);
                    }
                }
                OverdueCards = OverdueCards.OrderBy(t => t.DueDate).ToList();
                OverdueCardsMembers = new List<MemberRepresent>();
                foreach (var cardRepresent in OverdueCards)
                    foreach (var memberRepresent in cardRepresent.Members)
                    {
                        if (OverdueCardsMembers.All(t => t.Id != memberRepresent.Id))
                            OverdueCardsMembers.Add(new MemberRepresent(memberRepresent));
                        OverdueCardsMembers.First(t => t.Id == memberRepresent.Id).MemembersOverdueCards.Add(cardRepresent);
                    }
                OverdueCardsMembers = OverdueCardsMembers.OrderBy(t => t.FullName).ToList();
            }

            public StringBuilder GetReport()
            {
                var stringBuilder = new StringBuilder();
                var xmlSourcePath = Path.GetTempFileName();
                var outputPath = Path.GetTempFileName();

                var formatter = new XmlSerializer(typeof(Report));
                using (var fs = new FileStream(xmlSourcePath, FileMode.OpenOrCreate))
                    formatter.Serialize(fs, this);

                var xslt = new XslCompiledTransform();
                xslt.Load("report.xslt");
                xslt.Transform(xmlSourcePath, outputPath);

                File.ReadAllLines(outputPath).ToList().ForEach(t => stringBuilder.AppendLine(t));
                return stringBuilder;
            }
        }
    }

    public partial class TrelloReport
    {
        public bool ExecuteProcess()
        {

            var settings = GetSettings(typeof(TrelloReportSettings)) as TrelloReportSettings;

            var serializer = new ManateeSerializer();
            TrelloConfiguration.Serializer = serializer;
            TrelloConfiguration.Deserializer = serializer;
            TrelloConfiguration.JsonFactory = new ManateeFactory();
            TrelloConfiguration.RestClientProvider = new WebApiClientProvider();
            TrelloAuthorization.Default.AppKey = settings.TrelloAppKey;
            TrelloAuthorization.Default.UserToken = settings.TrelloUserToken;

            var organization = new Organization(settings.OrganizationID);
            var report = new Report(organization, settings.CountLastListCardsAsCompleted);

            var subject = $"Отчет Trello от {DateTime.Now.ToLocalTime().ToLongDateString()} { DateTime.Now.ToLocalTime().ToLongTimeString()}";
            if(string.IsNullOrEmpty(settings.AddressToSendReport))
                ReportMailSender.SendReportMail(subject, report.GetReport().ToString());
            else
                ReportMailSender.SendMail(settings.AddressToSendReport, subject, report.GetReport().ToString());
            return true;
        }
    }

    public partial class TrelloReport : TaskBase
    {
        public override ExecutionDelegate ExecutuionMethod => ExecuteProcess;
        public override IEnumerable<Type> TaskSettings => new[] { typeof(TrelloReportSettings) };
    }
}
