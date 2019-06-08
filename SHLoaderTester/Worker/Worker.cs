using SHCMParser.DBModels;
using SHCMParser.StoreHouse;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Linq;

namespace SHLoaderTester.Workers
{
    static class SHWorker
    {
        public struct SettingsStruct
        {
            public string ConnectionString { get; set; }
            public string SHServerAddress { get; set; }
            public uint SHServerPort { get; set; }
            public string SHUserName { get; set; }
            public string SHPassword { get; set; }
        }

        public static SettingsStruct settings;

        internal static string GetEntityConnection(string ConnectionString)
        {
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder
            {
                ProviderConnectionString = ConnectionString
            };
            return entityBuilder.ConnectionString;
        }

        private static DBModel GetDbModel()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DBModel>());
            return new DBModel(settings.ConnectionString);
        }

        private static void InitSHConnection()
        {
            SHOLE.Execute.SHOLEConnector.CurrentConnector.Init(settings.SHServerAddress, settings.SHServerPort, settings.SHUserName, settings.SHPassword);
        }

        public static double LoadComplectsTree(bool Override = false)
        {
            InitSHConnection();

            var startTime = DateTime.Now;

            using (var db = GetDbModel())
            {
                // Сначала загружаем все ветки дерева комплектов
                StoreHouseAPI.LoadCmTreeElements(db);
            }

            return (DateTime.Now - startTime).TotalMinutes;
        }

        public static double LoadBaseComplects(long groupRid, out string errorInfo)
        {
            errorInfo = "";
            InitSHConnection();

            var startTime = DateTime.Now;

            using (var db = GetDbModel())
            {
                // Сначала загружаем все ветки дерева комплектов
                StoreHouseAPI.LoadCmElements(db, groupRid, out errorInfo);
            }

            return (DateTime.Now - startTime).TotalMinutes;
        }

        public static double LoadHDRComplects(bool Override = false)
        {
            InitSHConnection();

            var startTime = DateTime.Now;

            using (var db = GetDbModel())
            {
                // Сначала загружаем все ветки дерева комплектов
                StoreHouseAPI.LoadCmHdrElements(db);
            }

            return (DateTime.Now - startTime).TotalMinutes;
        }


        public static bool ExecuteProcess()
        {

            SHOLE.Execute.SHOLEConnector.CurrentConnector.Init(settings.SHServerAddress, settings.SHServerPort, settings.SHUserName, settings.SHPassword);
            var connected = SHOLE.Execute.SHOLEConnector.CurrentConnector.Connect();

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DBModel>());

            var startTime = DateTime.Now;

            using (var db = new DBModel(settings.ConnectionString))
            {

                // Сначала загружаем все Goods элементы из нужных нам веток дерева
                foreach (var treeElement in db.TreeElementsNeed.ToList())
                {
                    var goodsTree = db.GoodsTree.FirstOrDefault(item => item.goodstree_abbr == treeElement.goodstree_abbr);
                    if (goodsTree != null)
                    {
                        StoreHouseAPI.LoadTree(db, (int)goodsTree.goodstree_rid);
                    }
                }

                // Теперь подгружаем их комплекты
                foreach (var goodsBase in db.GoodsBaseAttrs.Where(goodBase => goodBase.cm_base_rid != 0 && !db.CmHdrAttrs.Any(cmHDR => cmHDR.cm_base_rid == goodBase.cm_base_rid)).ToList())
                {
                    StoreHouseAPI.LoadGoodsComplects(db, goodsBase.cm_base_rid);
                }

                // Напоследок загружаем листы комплектов
                StoreHouseAPI.LoadComplectsList(db);
                SHOLE.Execute.SHOLEConnector.CurrentConnector.Disconnect();
            }
            return true;
        }
    }
}

