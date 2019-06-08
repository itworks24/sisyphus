using SHOLE.Procs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHTree
{
    class Program
    {
        private static void LoadTree(StreamWriter streamWriter, uint ParentTreeRid = 0, int level = 0)
        {
            if (level > 0) return;
            var goodsTree = new GoodsTreeProc();
            if (ParentTreeRid != 0)
            {
                goodsTree = new GoodsTreeProc()
                {
                    input = new GoodsTreeProcInputDS()
                    {
                        goodstree_parent_rid = (int)ParentTreeRid
                    }
                };
            }

            goodsTree.Execute();

            foreach (var treeElement in goodsTree.result)
            {
                if (treeElement.goodstree_rid == ParentTreeRid) continue;
                var tab = String.Format($"{{0, {(level + 1) * 2}}}", "");
                streamWriter.WriteLine($"{tab} {treeElement.goodstree_name}, {treeElement.goodstree_rid}, parent rid {treeElement.goodstree_parent_rid}");
                LoadTree(streamWriter, treeElement.goodstree_rid, level + 1);
            }
        }
        static void Main(string[] args)
        {
            SHOLE.Execute.SHOLEConnector.CurrentConnector.Init("10.20.1.2", 1004, "VLADAS", "123");
            var connected = SHOLE.Execute.SHOLEConnector.CurrentConnector.Connect();

            var goodsTree = new GoodsTreeProc();
            goodsTree.Execute();
            //foreach (var good in goodsTree.result.Take(4))
            //{
            //    Console.WriteLine(good.goodstree_name);
            //}

            var cmList = new CmListProc()
            {
                input = new CmListProcInputDS()
                {
                    cm_tree_rid = 1865,
                    someParam1 = 0,
                    startDate = DateTime.Now
                }
            };

            cmList.Execute();
            Console.WriteLine();
            using (var writetext = new StreamWriter($"Groups {DateTime.Now.ToString("dd.MM.yyyy HH_mm_ss")}.txt"))
            {
                foreach (var list in cmList.output1.Where(item=> item.cm_comp_rid == 32362))
                {
                    var output = $"cm_comp_name:{list.cm_comp_name}, cm_comp_rid:{list.cm_comp_rid}, cm_out:{list.cm_out}, cm_netto:{list.cm_netto}, cm_brutto:{list.cm_brutto}, cm_rid:{list.cm_rid}, cm_date:{list.cm_date}";
                    Console.WriteLine(output);
                    writetext.WriteLine(output);
                }
            }
            //using (var writetext = new StreamWriter($"Groups {DateTime.Now.ToString("dd.MM.yyyy HH_mm_ss")}.txt"))
            //{
            //    //    LoadTree(writetext);
            //}
            Console.ReadKey();
        }
    }
}
