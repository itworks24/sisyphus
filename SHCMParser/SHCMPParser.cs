using SHCMParser.DBModels;
using SHCMParser;
using SHOLE.Procs;
using Sisyphus.Tasks;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using SHCMParser.StoreHouse;

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

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DBModel>());

            CreateLogRecord($"Export started");
            var startTime = DateTime.Now;
            using (var db = new DBModel(settings.ConnectionString))
            {
                if (settings.ReloadGoods)
                {
                    var goodsTree = new GoodsTreeProc();
                    var error = goodsTree.Execute();

                    foreach (var goodsTreeElement in settings.SelectFirst == 0 ? goodsTree.result : goodsTree.result.Take(settings.SelectFirst))
                    {
                        var goods = new GoodsProc()
                        {
                            input = new GoodsProcInputDS() { goodstree_rid = goodsTreeElement.goodstree_rid }
                        };

                        goods.Execute();
                        db.GoodsAttrs.AddRange(goods.goodsAttrsDS.Select(item => new GoodsAttrsDB(item))
                                                                 .Where(item => !db.GoodsAttrs.Any(element => item.goods_rid == element.goods_rid)));

                        foreach (var good in goods.goodsAttrsDS)
                        {

                            var goodsBase = new GoodsBaseProc()
                            {
                                input = new GoodsBaseInputDS()
                                {
                                    goods_rid = (int)good.goods_rid
                                }
                            };

                            goodsBase.Execute();
                            db.GoodsBaseAttrs.AddRange(goodsBase.goods_attr.Select(item => new GoodsBaseAttrsDB(item))
                                                                           .Where(item => !db.GoodsBaseAttrs.Any(element => item.goods_rid == element.goods_rid)));

                            db.GoodsBaseComplects.RemoveRange(db.GoodsBaseComplects.Where(item => item.goods_rid == (int)good.goods_rid));
                            db.GoodsBaseComplects.AddRange(goodsBase.complects.Select(item => new GoodsBaseComplectsDB(item, (int)good.goods_rid))
                                                                           .Where(item => !db.GoodsBaseComplects.Any(element => item.cm_base_rid == element.cm_base_rid)));
                        }
                        db.SaveChanges();
                    }
                }

                CreateLogRecord($"GoodsBaseComplects count {db.GoodsBaseAttrs.Count()}");

                if (settings.ReloadGoodsBaseComplects)
                {
                    foreach (var goodsBase in db.GoodsBaseAttrs.Where(goodBase => goodBase.cm_base_rid != 0 && !db.CmHdrAttrs.Any(cmHDR => cmHDR.cm_base_rid == goodBase.cm_base_rid)).ToList())
                    {
                        var cmHdr = new CmHdrProc()
                        {
                            input = new CmHdrProcInputDS()
                            {
                                cm_base_rid = (int)goodsBase.cm_base_rid,
                                date = DateTime.Now
                            }
                        };
                        cmHdr.Execute();
                        db.CmHdrAttrs.AddRange(cmHdr.cm_hdr_attr.Select(item => new CmHdrAttrsDB(item, cmHdr.input.cm_base_rid))
                                                                          .Where(item => !db.CmHdrAttrs.Any(element => item.cm_hdr_rid == element.cm_hdr_rid && item.cm_base_rid == element.cm_base_rid)));

                        db.CmHdrComplects.RemoveRange(db.CmHdrComplects.Where(item => item.cm_base_rid == cmHdr.input.cm_base_rid));
                        db.CmHdrComplects.AddRange(cmHdr.complects.Select(item => new CmHdrComplectsDB(item, cmHdr.input.cm_base_rid))
                                                                          .Where(item => !db.CmHdrComplects.Any(element => item.cm_hdr_id == element.cm_hdr_id && item.cm_base_rid == element.cm_base_rid)));

                        db.SaveChanges();
                    }
                }
                CreateLogRecord($"CmHdrAttrs count {db.CmHdrAttrs.Count()}");

                if (settings.ReloadComplectsHDR)
                {
                    foreach (var CmHdrComplect in db.CmHdrComplects.ToList())
                    {
                        if (CmHdrComplect.cm_item_options == 0 && !db.CmHdrAttrs.Any(element => element.cm_base_rid == CmHdrComplect.cm_comp_rid))
                        {
                            var cmHdr = new CmHdrProc()
                            {
                                input = new CmHdrProcInputDS()
                                {
                                    cm_base_rid = (int)CmHdrComplect.cm_comp_rid,
                                    date = DateTime.Now
                                }
                            };
                            cmHdr.Execute();
                            db.CmHdrAttrs.AddRange(cmHdr.cm_hdr_attr.Select(item => new CmHdrAttrsDB(item, cmHdr.input.cm_base_rid))
                                                                              .Where(item => !db.CmHdrAttrs.Any(element => item.cm_hdr_rid == element.cm_hdr_rid && item.cm_base_rid == element.cm_base_rid)));

                            db.CmHdrComplects.RemoveRange(db.CmHdrComplects.Where(item => item.cm_base_rid == cmHdr.input.cm_base_rid));
                            db.CmHdrComplects.AddRange(cmHdr.complects.Select(item => new CmHdrComplectsDB(item, cmHdr.input.cm_base_rid))
                                                                              .Where(item => !db.CmHdrComplects.Any(element => item.cm_hdr_id == element.cm_hdr_id && item.cm_base_rid == element.cm_base_rid)));

                        }
                    }
                    db.SaveChanges();
                }

                CreateLogRecord($"CmHdrAttrs whith out cm list count {db.CmHdrAttrs.Where(cmHdr => !db.CmList.Any(cmList => cmList.cm_comp_rid == cmHdr.cm_base_rid)).Count()}");

                if (settings.ReloadComplectsList)
                {
                    foreach (var treeRid in db.CmHdrAttrs.Where(cmHdr => !db.CmList.Any(cmList => cmList.cm_comp_rid == cmHdr.cm_base_rid)).Select(item => item.cm_tree_parent_rid).Distinct().ToList())
                    {
                        var cmList = new CmListProc()
                        {
                            input = new CmListProcInputDS()
                            {
                                cm_tree_rid = (int)treeRid,
                                someParam1 = 0,
                                startDate = DateTime.Now
                            }
                        };
                        cmList.Execute();
                        db.CmList.AddRange(cmList.output1.Select(item => new CmListDB(item))
                                                                .Where(item => !db.CmList.Any(element => item.cm_rid == element.cm_rid) && item.cm_date == 0));
                        db.SaveChanges();

                    }
                }
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

    public partial class SHCMPTestParser
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

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DBModel>());

            CreateLogRecord($"Export started");
            var startTime = DateTime.Now;
            using (var db = new DBModel(settings.ConnectionString))
            {
                if (!settings.DontReloadTreeElements)
                {
                    CreateLogRecord($"Loading tree elements");
                    StoreHouseAPI.LoadTreeElements(db);
                }
                var goodsTree = db.GoodsTree.FirstOrDefault(item => item.goodstree_abbr == settings.ParentTreeAbbr);
                if (goodsTree != null)
                {
                    CreateLogRecord($"Tree rid {goodsTree.goodstree_rid}");
                    StoreHouseAPI.LoadTree(db, (int)goodsTree.goodstree_rid);
                }
            }

            CreateLogRecord($"Export finished, it takes {(DateTime.Now - startTime).TotalMinutes} min");
            SHOLE.Execute.SHOLEConnector.CurrentConnector.Disconnect();
            return true;
        }
    }

    public partial class SHCMPTestParser : TaskBase
    {
        public override ExecutionDelegate ExecutuionMethod => ExecuteProcess;
        public override IEnumerable<Type> TaskSettings => new[] { typeof(Settings.SHCMSettings) };
    }
}
