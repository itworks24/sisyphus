namespace SHCMParser.DBModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.SqlTypes;
    using System.Linq;

    public class DBModel : DbContext
    {
        // Your context has been configured to use a 'DBModel' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'SHCMParser.DBModel' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'DBModel' 
        // connection string in the application configuration file.
        public DBModel(string connectionString)
            : base(connectionString)
        {
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    Database.SetInitializer<DBModel>(null);
        //    base.OnModelCreating(modelBuilder);
        //}

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<GoodsTreeDB> GoodsTree { get; set; }
        public virtual DbSet<GoodsAttrsDB> GoodsAttrs { get; set; }
        public virtual DbSet<GoodsBaseAttrsDB> GoodsBaseAttrs { get; set; }
        public virtual DbSet<GoodsBaseComplectsDB> GoodsBaseComplects { get; set; }
        public virtual DbSet<CmTreeDB> CmTree { get; set; }
        public virtual DbSet<CmBaseDB> CmBase { get; set; }
        public virtual DbSet<CmHdrAttrsDB> CmHdrAttrs { get; set; }
        public virtual DbSet<CmHdrComplectsDB> CmHdrComplects { get; set; }
        public virtual DbSet<CmListDB> CmList { get; set; }
        public virtual DbSet<TreeElementsNeedDB> TreeElementsNeed { get; set; }
    }

    public static class DBModelHelper
    {
        public static void UpdateForType(Type sourceType, Type destType, Object source, Object destination)
        {
            var myObjectSourceFields = sourceType.GetProperties();
            var myObjectDestFields = destType.GetProperties();

            foreach (var sourceField in myObjectSourceFields)
            {
                System.Reflection.PropertyInfo destField;
                try
                {
                    destField = myObjectDestFields.First(item => item.Name == sourceField.Name);
                }
                catch (Exception e)
                {
                    continue;
                }
                switch (Type.GetTypeCode(sourceField.PropertyType))
                {
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                        destField.SetValue(destination, (long)(uint)sourceField.GetValue(source));
                        break;
                    default:
                        destField.SetValue(destination, sourceField.GetValue(source));
                        break;
                }
            }
        }

    }

    public class GoodsTreeDB
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long goodstree_rid { get; set; }

        public long goodstree_parent_rid { get; set; }

        public string goodstree_name { get; set; }

        public string goodstree_abbr { get; set; }

        public GoodsTreeDB()
        {
        }

        public GoodsTreeDB(SHOLE.Procs.GoodsTreeProcOutputDS source)
        {
            DBModelHelper.UpdateForType(typeof(SHOLE.Procs.GoodsTreeProcOutputDS), typeof(GoodsTreeDB), source, this);
        }
    }

    public class GoodsAttrsDB
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long goods_rid { get; set; }
        public long goodstree_rid { get; set; }
        public string goods_name { get; set; }
        public string goods_abbrtext { get; set; }
        public long goods_abbrnumber { get; set; }
        public long goods_type { get; set; }
        public long goods_munit_rid { get; set; }
        public string goods_munit_name { get; set; }
        public long goods_ctg_rid { get; set; }
        public long goods_ctg2_rid { get; set; }
        public long goods_tax_rate { get; set; }
        public long goods_tax2_rate { get; set; }
        public long goods_tax11_rate { get; set; }
        public long goods_tax22_rate { get; set; }
        public double goods_outprice { get; set; }
        public long goods_extinfo { get; set; }
        public long goods_options { get; set; }
        public double goods_inprice { get; set; }
        public long cmbase_rid { get; set; }
        public string cmbase_name { get; set; }
        public string cmbase_abbrtext { get; set; }
        public long cmbase_abbrnumber { get; set; }
        public long goods_muint_rid { get; set; }
        public string goods_muint_name { get; set; }

        public GoodsAttrsDB(SHOLE.Procs.GoodsAttrsDS source)
        {
            DBModelHelper.UpdateForType(typeof(SHOLE.Procs.GoodsAttrsDS), typeof(GoodsAttrsDB), source, this);
        }
    }

    public class GoodsBaseAttrsDB
    {

        public string goods_ctg2_name { get; set; }

        public string goods_munit2_name { get; set; }

        public string goods_mgroup_name { get; set; }

        public string goods_munit_name { get; set; }

        public double goods_munit_cf { get; set; }

        public string cm_base_name { get; set; }

        public long cm_base_rid { get; set; }

        public long goods_rid1 { get; set; }

        public string goods_name1 { get; set; }

        public long goods_munit1_rid { get; set; }

        public string goods_munit1_name { get; set; }

        private DateTime _goods_createdate;
        public DateTime goods_createdate
        {
            get { return _goods_createdate; }
            set { _goods_createdate = value == DateTime.MinValue ? SqlDateTime.MinValue.Value : value; }
        }

        public long goods_createdate_sec { get; set; }

        private DateTime _goods_lastchange;
        public DateTime goods_lastchange
        {
            get { return _goods_lastchange; }
            set { _goods_lastchange = value == DateTime.MinValue ? SqlDateTime.MinValue.Value : value; }
        }

        public long goods_lastchange_sec { get; set; }

        public string goods_ctg_name { get; set; }

        public string goods_minactivedocdate { get; set; }

        public string goods_tree_name { get; set; }

        public long goods_options { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long goods_rid { get; set; }

        public long goods_tree_rid { get; set; }

        public string goods_name { get; set; }

        public string goods_abbr_text { get; set; }

        public long goods_abbr_number { get; set; }

        public int goods_type { get; set; }

        public long goods_munit_rid { get; set; }
        public long goods_ctg_rid { get; set; }

        public long goods_ctg2_rid { get; set; }

        public long goods_tax_rate { get; set; }

        public long goods_tax2_rate { get; set; }

        public long goods_tax11_rate { get; set; }

        public long goods_tax22_rate { get; set; }

        public double goods_out_price { get; set; }

        public long goods_extinfo { get; set; }

        public double goods_inprice { get; set; }

        public string goods_user { get; set; }

        public GoodsBaseAttrsDB()
        {
        }

        public GoodsBaseAttrsDB(SHOLE.Procs.GoodsBaseAttrsDS source)
        {
            DBModelHelper.UpdateForType(typeof(SHOLE.Procs.GoodsBaseAttrsDS), typeof(GoodsBaseAttrsDB), source, this);
        }
    }

    public class GoodsBaseComplectsDB : SHOLE.Procs.GoodsBaseComplectsDS
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long goods_rid { get; set; }

        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long cm_base_rid { get; set; }

        public string cm_base_abbrttext { get; set; }

        public long cm_base_abbrtnumber { get; set; }

        public string cm_base_name { get; set; }

        public int cm_base_options { get; set; }

        public GoodsBaseComplectsDB(SHOLE.Procs.GoodsBaseComplectsDS source, long new_goods_rid)
        {
            cm_base_rid = source.cm_base_rid;
            cm_base_abbrttext = source.cm_base_abbrttext;
            cm_base_abbrtnumber = source.cm_base_abbrtnumber;
            cm_base_name = source.cm_base_name;
            cm_base_options = source.cm_base_options;
            goods_rid = new_goods_rid;
        }

        public GoodsBaseComplectsDB() { }
    }

    public class CmTreeDB
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long cm_tree_rid { get; set; }

        public long cm_tree_parent_rid { get; set; }

        public string cm_tree_name { get; set; }

        public CmTreeDB()
        {
        }

        public CmTreeDB(SHOLE.Procs.CmTreeDS source)
        {
            DBModelHelper.UpdateForType(typeof(SHOLE.Procs.CmTreeDS), typeof(CmTreeDB), source, this);
        }
    }

    public class CmBaseDB
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long cm_base_rid { get; set; }

        public long cm_tree_rid { get; set; }

        public string cm_base_name { get; set; }

        public string cm_base_abbr { get; set; }

        public long cm_base_options { get; set; }

        public long cm_base_munit_rid { get; set; }

        public long cm_base_abbrnumber { get; set; }

        public string cm_base_munit_name { get; set; }

        public int cm_base_createdate { get; set; }

        public CmBaseDB()
        {
        }

        public CmBaseDB(SHOLE.Procs.CmBaseDS source)
        {
            DBModelHelper.UpdateForType(typeof(SHOLE.Procs.CmBaseDS), typeof(CmBaseDB), source, this);
        }
    }

    public class CmHdrAttrsDB
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long cm_base_rid { get; set; }

        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long cm_hdr_rid { get; set; }

        public long cm_tree_parent_rid { get; set; }

        public string cm_hdr_name { get; set; }

        public string cm_hdr_abbr_text { get; set; }

        public int cm_hdr_options { get; set; }

        public long cm_hdr_munit_rid { get; set; }

        public long cm_hdr_abbr_number { get; set; }

        public string cm_hdr_munit_name { get; set; }

        public string cm_tree_parent_name { get; set; }

        public int cm_hdr_corrbase_rec { get; set; }

        public string cm_hdr_corrbase_name { get; set; }

        public DateTime cm_hdr_createdate { get; set; }

        public int cm_hdr_createdate_sec { get; set; }

        public DateTime cm_hdr_modifydate { get; set; }

        public int cm_hdr_modifydate_sec { get; set; }

        public string cm_hdr_mindateactivedoc { get; set; }

        public string cm_hdr_user { get; set; }

        public CmHdrAttrsDB(SHOLE.Procs.CmHdrAttrsDS source, long new_cm_base_rid)
        {
            cm_base_rid = new_cm_base_rid;
            DBModelHelper.UpdateForType(typeof(SHOLE.Procs.CmHdrAttrsDS), typeof(CmHdrAttrsDB), source, this);
        }
    }

    public class CmHdrComplectsDB
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long cm_base_rid { get; set; }

        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long cm_hdr_id { get; set; }

        [Key, Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long cm_item_rid { get; set; }

        public int cm_item_order { get; set; }

        public int cm_item_options { get; set; }

        public long cm_comp_rid { get; set; }

        public string cm_comp_name { get; set; }

        public long cm_item_muint_rid { get; set; }

        public string cm_item_muint_name { get; set; }

        public double cm_item_gmunit_cf { get; set; }

        public long cm_item_muint_rid2 { get; set; }

        public string cm_item_muint_name2 { get; set; }

        public int cm_item_tgtax_rate { get; set; }

        public int cm_item_tgtax_rate2 { get; set; }

        public double cm_item_brutto { get; set; }

        public double cm_item_proc1 { get; set; }

        public double cm_item_netto { get; set; }

        public double cm_item_proc2 { get; set; }

        public double cm_item_out { get; set; }

        public double cm_item_share { get; set; }

        public int cm_item_replace_rid { get; set; }

        public string cm_item_replace_name { get; set; }

        public double cm_item_pcost { get; set; }

        public double cm_item_tax1 { get; set; }

        public double cm_item_tax2 { get; set; }

        public CmHdrComplectsDB(SHOLE.Procs.CmHdrComplectsDS source, long new_cm_base_rid)
        {
            cm_base_rid = new_cm_base_rid;
            DBModelHelper.UpdateForType(typeof(SHOLE.Procs.CmHdrComplectsDS), typeof(CmHdrComplectsDB), source, this);
        }

        public CmHdrComplectsDB()
        {

        }
    }

    public class CmListDB
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long cm_rid { get; set; }

        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long cm_comp_rid { get; set; }

        public long cm_date { get; set; }

        public int options { get; set; }

        public string cm_comp_name { get; set; }

        public string cm_some_param_str { get; set; }

        public int cm_some_param_int { get; set; }

        public double cm_brutto { get; set; }

        public long cm_munit_rid { get; set; }

        public string cm_munit_name { get; set; }

        public long cm_proc1 { get; set; }

        public long cm_proc2 { get; set; }

        public double cm_netto { get; set; }

        public double cm_out { get; set; }

        public CmListDB(SHOLE.Procs.CmListDS source)
        {
            DBModelHelper.UpdateForType(typeof(SHOLE.Procs.CmListDS), typeof(CmListDB), source, this);
        }

        public CmListDB()
        {

        }
    }

    public class TreeElementsNeedDB
    {
        [Key]
        public long record_rid { get; set; }

        public string goodstree_abbr { get; set; }
    }
}