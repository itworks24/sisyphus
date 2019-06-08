using SHCMParser.DBModels;
using SHCMParser.StoreHouse;
using Sisyphus.Tasks;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Linq;

namespace Sysiphus.Tasks
{
    public partial class SHCMPLodaer
    {
        internal static string GetEntityConnection(Settings.SHCMSettings settings)
        {
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder
            {
                ProviderConnectionString = settings.ConnectionString
            };
            return entityBuilder.ConnectionString;
        }

        public bool ExecuteProcess()
        {
            var settings = GetSettings(typeof(Settings.SHCMSettings)) as Settings.SHCMSettings;

            SHOLE.Execute.SHOLEConnector.CurrentConnector.Init(settings.SHServerAddress, settings.SHServerPort, settings.SHUserName, settings.SHPassword);
            var connected = SHOLE.Execute.SHOLEConnector.CurrentConnector.Connect();
            
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DBModel>());

            CreateLogRecord($"Export started");
            var startTime = DateTime.Now;

            using (var db = new DBModel(settings.ConnectionString))
            {

                // Сначала загружаем все Goods элементы из нужных нам веток дерева
                CreateLogRecord($"Loading goods elements");
                foreach (var treeElement in db.TreeElementsNeed.ToList())
                {
                    var goodsTree = db.GoodsTree.FirstOrDefault(item => item.goodstree_abbr == treeElement.goodstree_abbr);
                    if (goodsTree != null)
                    {
                        CreateLogRecord($"Loading goods elements of tree rid {goodsTree.goodstree_rid}");
                        StoreHouseAPI.LoadTree(db, (int)goodsTree.goodstree_rid);
                    }
                }

                // Теперь подгружаем их комплекты
                CreateLogRecord($"Loading complects");
                foreach (var goodsBase in db.GoodsBaseAttrs.Where(goodBase => goodBase.cm_base_rid != 0 && !db.CmHdrAttrs.Any(cmHDR => cmHDR.cm_base_rid == goodBase.cm_base_rid)).ToList())
                {
                    StoreHouseAPI.LoadGoodsComplects(db, goodsBase.cm_base_rid);
                }

                // Напоследок загружаем листы комплектов
                CreateLogRecord($"Loading complects lists");
                StoreHouseAPI.LoadComplectsList(db);

                CreateLogRecord($"Export finished, it takes {(DateTime.Now - startTime).TotalMinutes} min");
                SHOLE.Execute.SHOLEConnector.CurrentConnector.Disconnect();
            }
            return true;
        }
    }

    public partial class SHCMPLodaer : TaskBase
    {
        public override ExecutionDelegate ExecutuionMethod => ExecuteProcess;
        public override IEnumerable<Type> TaskSettings => new[] { typeof(Settings.SHCMSettings) };
    }
}
