using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using HtmlAgilityPack;
using Sisyphus.Tasks;

namespace Sysiphus.Tasks
{
    public partial class RoseltorgParser
    {
        public bool ExecuteProcess()
        {
            var settings = GetSettings(typeof(Settings.RoseltorgSettings)) as Settings.RoseltorgSettings;

            var url = "https://www.roseltorg.ru/procedures/search/";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var nodes = doc.DocumentNode.SelectNodes("//a").Where(x => x.Attributes["class"].Value == "g-link");
            foreach (var node in nodes)
            {
                CreateLogRecord(node.Attributes["href"].Value); 
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
