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
    
    public partial class data_shcr_cm
    {
        public System.Guid GUID { get; set; }
        public Nullable<System.Guid> server_guid { get; set; }
        public Nullable<System.DateTime> dateinsert { get; set; }
        public Nullable<int> doc_rid { get; set; }
        public Nullable<int> version_rid { get; set; }
        public Nullable<int> parent_rid { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public Nullable<int> options { get; set; }
        public Nullable<int> measure_rid { get; set; }
        public Nullable<int> code_num { get; set; }
        public string measure_name { get; set; }
        public string parent_name { get; set; }
        public Nullable<int> store_rid { get; set; }
        public string store_name { get; set; }
        public Nullable<System.DateTime> create_date { get; set; }
        public Nullable<int> create_time { get; set; }
        public Nullable<System.DateTime> edit_date { get; set; }
        public Nullable<int> edit_time { get; set; }
        public string create_user { get; set; }
        public string edit_user { get; set; }
    }
}
