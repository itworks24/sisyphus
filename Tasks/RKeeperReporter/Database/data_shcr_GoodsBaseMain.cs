//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RKeeperReporter.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class data_shcr_GoodsBaseMain
    {
        public System.Guid GUID { get; set; }
        public Nullable<System.Guid> server_guid { get; set; }
        public Nullable<short> Deleted { get; set; }
        public Nullable<System.DateTime> dateinsert { get; set; }
        public Nullable<int> RID { get; set; }
        public Nullable<int> Parent_RID { get; set; }
        public string good_name { get; set; }
        public string code_text { get; set; }
        public Nullable<int> code_num { get; set; }
        public Nullable<int> good_type { get; set; }
        public Nullable<int> measure_RID { get; set; }
        public Nullable<int> main_cat_RID { get; set; }
        public Nullable<int> acc_cat_RID { get; set; }
        public Nullable<int> nds { get; set; }
        public Nullable<int> nsp { get; set; }
        public Nullable<int> nds_rate { get; set; }
        public Nullable<int> nsp_rate { get; set; }
        public Nullable<decimal> good_sale_price { get; set; }
        public Nullable<int> extinfo { get; set; }
        public Nullable<int> options { get; set; }
        public Nullable<decimal> Purchase_price { get; set; }
        public string parent_name { get; set; }
        public string main_cat_name { get; set; }
        public string acc_cat_name { get; set; }
        public string measure_name { get; set; }
        public Nullable<int> measure_parent_RID { get; set; }
        public string measure_base_name { get; set; }
        public Nullable<decimal> measure_ratio { get; set; }
        public Nullable<int> cm_RID { get; set; }
        public string cm_name { get; set; }
        public Nullable<int> service { get; set; }
        public string service2 { get; set; }
        public Nullable<int> measure2_RID { get; set; }
        public string measure2_name { get; set; }
        public Nullable<System.DateTime> create_date { get; set; }
        public Nullable<int> create_time { get; set; }
        public Nullable<System.DateTime> edit_date { get; set; }
        public Nullable<int> edit_time { get; set; }
        public string Create_user { get; set; }
        public string Edit_user { get; set; }
    }
}
