using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using HtmlAgilityPack;
using Sisyphus.Tasks;
using Sysiphus.Tasks.Roseltorg;
using Newtonsoft.Json;

namespace Sysiphus.Tasks
{
    public partial class RoseltorgParser
    {

        private static IEnumerable<string> GetProceduresUrlList()
        {
            const string url = "https://www.roseltorg.ru/procedures/search/";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var nodes = doc.DocumentNode.SelectNodes("//a").Where(x => x.HasAttributes && x.GetAttributeValue("class", "no_class") == "g-link").Select(x => x.Attributes["href"].Value.Split('/')[2]);
            return nodes;
        }

        private static string GetHeadereValue(string header, HtmlNode documentNode)
        {
            var captionNode = documentNode.SelectNodes("//td/h4").FirstOrDefault(x => x.InnerText.Contains(header));
            if (captionNode == null) return "";
            var valueNode = captionNode.ParentNode.ParentNode.SelectSingleNode("td/p");
            return valueNode.InnerText;
        }

        private static Procedure GetProcedure(string procedureCode)
        {
            string url = $"https://www.roseltorg.ru/procedure/{procedureCode}";
            return GetProcedure(new Url(url));
        }

        private static Procedure GetProcedure(Url url)
        {
            var procedureUrl = url.Value;
            var web = new HtmlWeb();
            var doc = web.Load(procedureUrl);

            var procedure = new Procedure
            {
                name = doc.DocumentNode.SelectSingleNode("//h1").InnerText,
                code = GetHeadereValue("Реестровый номер процедуры:", doc.DocumentNode),
                type = GetHeadereValue("Способ проведения:", doc.DocumentNode),
                contractor = GetHeadereValue("компании-заказчика:", doc.DocumentNode),
                contactName = GetHeadereValue("Контактное лицо:", doc.DocumentNode),
                contactInfo = GetHeadereValue("Информация о контактном лице:", doc.DocumentNode),
                organizer = GetHeadereValue("Организатор процедуры:", doc.DocumentNode),
            };

            foreach (var fileRef in doc.DocumentNode.SelectNodes("//a").Where(x => x.GetAttributeValue("href", "not found").Contains("https://files.roseltorg.ru/file.php")))
            {
                procedure.files.Add(new Roseltorg.File
                {
                    name = fileRef.InnerText,
                    link = fileRef.GetAttributeValue("href", "")
                });
            }

            return procedure;
        }

        private static void DownloadProcedure(string procedureUrl, string destinationPath = "..")
        {
            var procedure = GetProcedure(procedureUrl);

            var procedureDirectory = Path.Combine(destinationPath, procedure.code);
            if (!Directory.Exists(procedureDirectory))
                Directory.CreateDirectory(procedureDirectory);

            var serializer = new JsonSerializer();
            using (var sw = new StreamWriter(Path.Combine(procedureDirectory, @"info.json")))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, procedure);
            }

            var filesDirectory = Path.Combine(procedureDirectory, "files");
            if (!Directory.Exists(filesDirectory))
                Directory.CreateDirectory(filesDirectory);

            foreach (var file in procedure.files)
            {
                using (var client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");
                    client.DownloadFile(file.link, Path.Combine(filesDirectory, file.name));
                }
            }
        }


        public bool ExecuteProcess()
        {
            var settings = GetSettings(typeof(Settings.RoseltorgSettings)) as Settings.RoseltorgSettings;

            foreach (var procedureUrl in GetProceduresUrlList())
            {
                CreateLogRecord(procedureUrl);
                DownloadProcedure(procedureUrl, @"c:\temp\roseltorg");
            }

            return true;
        }
    }

    public partial class RoseltorgParser : TaskBase
    {
        public override ExecutionDelegate ExecutuionMethod => ExecuteProcess;
        public override IEnumerable<Type> TaskSettings => new[] { typeof(Settings.RoseltorgSettings) };
    }
}
