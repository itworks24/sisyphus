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
            using (var writetext = new StreamWriter($"Groups {DateTime.Now.ToString("dd.MM.yyyy HH_mm_ss")}.txt"))
            {
                LoadTree(writetext);
            }      
            Console.ReadKey();
        }
    }
}
