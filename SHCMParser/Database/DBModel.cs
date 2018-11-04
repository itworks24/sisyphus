namespace SHCMParserDB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;
    using System.Reflection;

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

        public virtual DbSet<GoodsAttrsDB> GoodsAttrs { get; set; }
        //public virtual DbSet<GoodsBaseAttrsDB> GoodsBaseAttrs { get; set; }
        //public virtual DbSet<GoodsBaseComplectsDB> GoodsBaseComplects { get; set; }
        //public virtual DbSet<CmBaseDB> CmBase { get; set; }
        //public virtual DbSet<CmHdrAttrsDB> CmHdrAttrs { get; set; }
        //public virtual DbSet<CmHdrComplectsDB> CmHdrComplects { get; set; }
    }

    public static class DBModelHelper
    {
        public static void UpdateForType(Type sourceType, Type destType, Object source, Object destination)
        {
            var myObjectSourceFields = sourceType.GetFields(
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            var myObjectDestFields = destType.GetFields(
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo sourceField in myObjectSourceFields)
            {
                try
                {
                    var destField = myObjectDestFields.First(item => item.Name == sourceField.Name);
                    switch (Type.GetTypeCode(sourceField.FieldType))
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
                catch (Exception e)
                {
                    continue;
                }
            }
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

    public class GoodsBaseAttrsDB : SHOLE.Procs.GoodsBaseAttrsDS
    {
        [Key]
        public long id { get; set; }

        GoodsBaseAttrsDB() { id = goods_rid; }
    }

    public class GoodsBaseComplectsDB : SHOLE.Procs.GoodsBaseComplectsDS
    {
        [Key, Column(Order = 0)]
        public long goods_id { get; set; }

        [Key, Column(Order = 1)]
        public long id { get; set; }

        GoodsBaseComplectsDB(uint goods_rid) { id = cm_base_rid; goods_id = goods_rid; }
    }

    public class CmBaseDB : SHOLE.Procs.CmBaseDS
    {
        [Key]
        public long id { get; set; }

        CmBaseDB() { id = cm_base_rid; }
    }

    public class CmHdrAttrsDB : SHOLE.Procs.CmHdrAttrsDS
    {
        [Key]
        public long id { get; set; }

        CmHdrAttrsDB() { id = cm_hdr_rid; }
    }

    public class CmHdrComplectsDB : SHOLE.Procs.CmHdrComplectsDS
    {
        [Key, Column(Order = 0)]
        public long cm_item_id { get; set; }

        [Key, Column(Order = 1)]
        public long cm_hdr_id { get; set; }

        CmHdrComplectsDB() { cm_item_id = cm_item_rid; cm_hdr_id = cm_hdr_rid; }
    }
}