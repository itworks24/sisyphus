using SHOLE.Procs;
using Sisyphus.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sysiphus.Tasks
{
    public partial class SHCMPParser
    {
        public bool ExecuteProcess()
        {

            var settings = GetSettings(typeof(Settings.SHCMSettings)) as Settings.SHCMSettings;
            //var connector = new SHOLE.Execute.SHOLEConnector();
            //connector.Init(settings.SHServerAddress, settings.SHServerPort, settings.SHUserName, settings.SHPassword);
            //var connected = connector.Connect();

            SHOLE.Execute.SHOLEConnector.CurrentConnector.Init(settings.SHServerAddress, settings.SHServerPort, settings.SHUserName, settings.SHPassword);
            var connected = SHOLE.Execute.SHOLEConnector.CurrentConnector.Connect();
            var cmTreeProc = new CmTreeProc();
            var error = cmTreeProc.Execute();
            foreach (var goodsTreeElement in cmTreeProc.result.Take(4))
            {
                CreateLogRecord($"Goods tree name: {goodsTreeElement.cm_tree_name}");
            }
            SHOLE.Execute.SHOLEConnector.CurrentConnector.Disconnect();

            return true;
        }
    }

    public partial class SHCMPParser : TaskBase
    {
        public override ExecutionDelegate ExecutuionMethod => ExecuteProcess;
        public override IEnumerable<Type> TaskSettings => new[] { typeof(Settings.SHCMSettings) };
    }
}
