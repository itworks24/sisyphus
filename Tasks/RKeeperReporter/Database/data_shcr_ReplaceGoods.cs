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
    
    public partial class data_shcr_ReplaceGoods
    {
        public System.Guid GUID { get; set; }
        public Nullable<System.Guid> server_guid { get; set; }
        public string hash { get; set; }
        public Nullable<short> Deleted { get; set; }
        public Nullable<System.DateTime> dateinsert { get; set; }
        public Nullable<int> good_rid { get; set; }
        public Nullable<int> replace_grp_rid { get; set; }
        public Nullable<double> ratio { get; set; }
        public Nullable<int> good_order { get; set; }
        public Nullable<int> goodgroup_rid { get; set; }
        public string name { get; set; }
        public string code_text { get; set; }
        public Nullable<int> code_num { get; set; }
        public Nullable<int> type { get; set; }
        public Nullable<int> measure_rid { get; set; }
        public string measure_name { get; set; }
    }
}
