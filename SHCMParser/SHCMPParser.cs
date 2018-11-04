using SHCMParserDB;
using SHOLE.Procs;
using Sisyphus.Tasks;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Linq;

namespace Sysiphus.Tasks
{
    public partial class SHCMPParser
    {

        internal static string GetEntityConnection(Settings.SHCMSettings settings)
        {
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder
            {
                //Provider = settings.ProviderName,
                ProviderConnectionString = settings.ConnectionString
            };
            return entityBuilder.ConnectionString;
        }

        public bool ExecuteProcess()
        {
            var settings = GetSettings(typeof(Settings.SHCMSettings)) as Settings.SHCMSettings;

            SHOLE.Execute.SHOLEConnector.CurrentConnector.Init(settings.SHServerAddress, settings.SHServerPort, settings.SHUserName, settings.SHPassword);
            var connected = SHOLE.Execute.SHOLEConnector.CurrentConnector.Connect();
            //if (!connected)
            //{
            //    CreateLogRecord($"Can't connect to SH: {SHOLE.Execute.SHOLEConnector.CurrentConnector.LastError()}", System.Diagnostics.EventLogEntryType.Error);
            //    return false;
            //}


            CreateLogRecord($"Export started");
            var startTime = DateTime.Now;
            using (var db = new DBModel(settings.ConnectionString))
            {
                var goodsTree = new GoodsTreeProc();
                var error = goodsTree.Execute();

                foreach (var goodsTreeElement in goodsTree.result)
                {
                    var goods = new GoodsProc()
                    {
                        input = new GoodsProcInputDS() { goodstree_rid = goodsTreeElement.goodstree_rid }
                    };

                    goods.Execute();
                    db.GoodsAttrs.AddRange(goods.goodsAttrsDS.Select(item => new GoodsAttrsDB(item))
                                                             .Where(item => !db.GoodsAttrs.Any(element => item.goods_rid == element.goods_rid)));
                }
                db.SaveChanges();
            }
            CreateLogRecord($"Export finished, it takes {(DateTime.Now - startTime).TotalMinutes} min");
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
