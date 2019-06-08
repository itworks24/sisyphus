using SHCMParser.DBModels;
using SHOLE.Procs;
using System;
using System.Linq;

namespace SHCMParser.StoreHouse
{
    class StoreHouseAPI
    {

        public static void LoadTreeElements(DBModel db)
        {
            var goodsTree = new GoodsTreeProc();
            goodsTree.Execute();
            db.GoodsTree.AddRange(goodsTree.result.Select(item => new GoodsTreeDB(item))
                                                            .Where(item => !db.GoodsTree.Any(element => item.goodstree_rid == element.goodstree_rid)));
            db.SaveChanges();
        }

        public static void LoadGoodBase(DBModel db, uint goods_rid)
        {
            var goodsBase = new GoodsBaseProc()
            {
                input = new GoodsBaseInputDS()
                {
                    goods_rid = (int)goods_rid
                }
            };

            goodsBase.Execute();
            db.GoodsBaseAttrs.AddRange(goodsBase.goods_attr.Select(item => new GoodsBaseAttrsDB(item))
                                                           .Where(item => !db.GoodsBaseAttrs.Any(element => item.goods_rid == element.goods_rid)));
            db.SaveChanges();
        }

        public static void LoadTree(DBModel db, int ParentTreeRid)
        {
            var goodsTree = new GoodsTreeProc()
            {
                input = new GoodsTreeProcInputDS()
                {
                    goodstree_parent_rid = ParentTreeRid
                }
            };

            var goods = new GoodsProc()
            {
                input = new GoodsProcInputDS() { goodstree_rid = (uint)ParentTreeRid }
            };

            goods.Execute();
            db.GoodsAttrs.AddRange(goods.goodsAttrsDS.Select(item => new GoodsAttrsDB(item))
                                                          .Where(item => !db.GoodsAttrs.Any(element => item.goods_rid == element.goods_rid)));
            db.SaveChanges();

            foreach (var good in goods.goodsAttrsDS.ToList())
            {
                LoadGoodBase(db, good.goods_rid);
            }            

            goodsTree.Execute();

            foreach (var treeElement in goodsTree.result.ToList())
            {
                if (treeElement.goodstree_rid == ParentTreeRid) continue;
                LoadTree(db, (int)treeElement.goodstree_rid);
            }
        }

        public static void LoadGoodsComplects(DBModel db, long cm_base_rid)
        {
            var cmHdr = new CmHdrProc()
            {
                input = new CmHdrProcInputDS()
                {
                    cm_base_rid = (int)cm_base_rid,
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

            // Рекурсивно подгружаем Goods и их комплекты, если нужно
            foreach(var cmHdrComplect in db.CmHdrComplects.Where(item => item.cm_base_rid == cmHdr.input.cm_base_rid))
            {
                LoadGoodBase(db, (uint)cmHdrComplect.cm_item_rid);
                if (cmHdrComplect.cm_item_options == 0)
                    LoadGoodsComplects(db, cmHdrComplect.cm_base_rid);
            }
        }

        public static void LoadComplectsList(DBModel db)
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
}
