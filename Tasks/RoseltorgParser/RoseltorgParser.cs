using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Security.Policy;
using System.Text;
using HtmlAgilityPack;
using Sisyphus.Tasks;
using Sysiphus.Tasks.Roseltorg;
using Newtonsoft.Json;
using SevenZip;
using File = Sysiphus.Tasks.Roseltorg.File;

namespace Sysiphus.Tasks
{

    public static class FileParses
    {
        private static IEnumerable<string> ExtractArchieve(string zipPath, string fileName)
        {
            var sourceName = fileName;

            var args = Encoding.GetEncoding(1251).GetString(Encoding.Default.GetBytes($"e \"{fileName}\" -y -o\"{Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName))}\""));

            var p = new ProcessStartInfo
            {
                FileName = zipPath,
                Arguments = args,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            var x = Process.Start(p);
            x.WaitForExit();
            var output = x.StandardOutput.ReadToEnd();
            var error = x.StandardError.ReadToEnd();
            if (string.IsNullOrEmpty(error))
                return Directory.GetFiles(Path.GetDirectoryName(fileName), "*", SearchOption.AllDirectories);
            return new List<string>();
            throw new Exception("Can not open the file as archive");
        }

        public static void ExtractArchieves(string zipPath, string path)
        {
            var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".rar") || s.EndsWith(".zip") || s.EndsWith(".gzip")); ;
            foreach (var file in files)
            {
                //try
                //{
                var extractedFiles = ExtractArchieve(zipPath, file);
                //}
                //catch (Exception e) { }
            }
        }
    }

    public static class ProcedureDownloader
    {
        public static IEnumerable<string> GetProceduresUrlList()
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

        public static string GetDecodedString(string sourceName, string encodedString)
        {

            var encodingArray = new List<Encoding>
            {
                Encoding.UTF8,
                Encoding.Default,
                Encoding.ASCII,
                Encoding.GetEncoding(1252),
                Encoding.GetEncoding(1251),
                Encoding.GetEncoding(866),
            };

            foreach (var encoding in encodingArray)
            {
                foreach (var decoder in encodingArray)
                {
                    var bytes = Encoding.Convert(encoding, decoder, encoding.GetBytes(encodedString));
                    var result = Encoding.UTF8.GetString(bytes);
                    if (result.Contains(sourceName)) return result;
                }
            }
            return "";
        }

        public static string GetFileNameFromContentDisposition(string sourceName, string contentDispositionString)
        {

            var correctFileName = "";
            try
            {
                var contentDisposition = new ContentDisposition(contentDispositionString);
                correctFileName = contentDisposition.FileName;
                var decodedString = System.Web.HttpUtility.UrlDecode(correctFileName);
                return decodedString;
            }
            catch (Exception e)
            {

            }

            const string lookFor = "filename=";
            var index = contentDispositionString.IndexOf(lookFor, StringComparison.CurrentCultureIgnoreCase);
            if (index >= 0)
                correctFileName = contentDispositionString.Substring(index + lookFor.Length);
            
            
            return GetDecodedString(sourceName, correctFileName);
        }

        public static void DownloadProcedure(string procedureUrl, string destinationPath = "..")
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
                    var fileName = Path.Combine(filesDirectory, file.name);
                    client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");
                    client.DownloadFile(file.link, fileName);
                    var correctFileName = GetFileNameFromContentDisposition(file.name, client.ResponseHeaders["content-disposition"]);
                    
                    try
                    {
                        System.IO.File.Move(fileName, Path.Combine(filesDirectory, correctFileName));
                    }
                    catch (Exception e)
                    {
                        System.IO.File.Delete(fileName);
                    }
                }
            }
        }
    }

    public partial class RoseltorgParser
    {

        public bool ExecuteProcess()
        {
            var settings = GetSettings(typeof(Settings.RoseltorgSettings)) as Settings.RoseltorgSettings;

            foreach (var procedureUrl in ProcedureDownloader.GetProceduresUrlList())
            {
                CreateLogRecord(procedureUrl);
                ProcedureDownloader.DownloadProcedure(procedureUrl, settings.ExportPath);
            }

            FileParses.ExtractArchieves(settings.ZipPath, settings.ExportPath);

            return true;
        }
    }

    public partial class RoseltorgParser : TaskBase
    {
        public override ExecutionDelegate ExecutuionMethod => ExecuteProcess;
        public override IEnumerable<Type> TaskSettings => new[] { typeof(Settings.RoseltorgSettings) };
    }
}
