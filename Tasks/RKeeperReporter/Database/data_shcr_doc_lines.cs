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
    
    public partial class data_shcr_doc_lines
    {
        public System.Guid GUID { get; set; }
        public Nullable<System.Guid> server_guid { get; set; }
        public Nullable<System.DateTime> dateinsert { get; set; }
        public Nullable<int> doc_rid { get; set; }
        public Nullable<System.Guid> doc_guid { get; set; }
        public Nullable<int> RID { get; set; }
        public Nullable<int> good_rid { get; set; }
        public Nullable<int> measure_rid { get; set; }
        public string good_code_text { get; set; }
        public Nullable<int> good_code_num { get; set; }
        public string good_name { get; set; }
        public string measure_name { get; set; }
        public Nullable<double> good_quantity { get; set; }
        public string attr_values { get; set; }
        public Nullable<int> profit_rid { get; set; }
        public Nullable<int> invoice_doc_rid { get; set; }
        public Nullable<System.DateTime> date_invoice_doc { get; set; }
        public string code_text_off { get; set; }
        public Nullable<int> code_num_off { get; set; }
        public Nullable<int> mask_off { get; set; }
        public Nullable<int> doc_type_off { get; set; }
        public string receiver_name { get; set; }
        public Nullable<double> cost_sum { get; set; }
        public Nullable<double> cost_nds { get; set; }
        public Nullable<double> cost_nsp { get; set; }
        public Nullable<int> cm_rid { get; set; }
        public string cm_name { get; set; }
        public Nullable<int> good2_rid { get; set; }
        public Nullable<int> measure2_rid { get; set; }
        public string good2_code_rid { get; set; }
        public Nullable<int> good2_code_num { get; set; }
        public string good2_name { get; set; }
        public string measure2_name { get; set; }
        public Nullable<double> good2_quantity { get; set; }
        public Nullable<double> good_quantity2 { get; set; }
        public Nullable<int> options { get; set; }
        public Nullable<double> cost_over { get; set; }
        public Nullable<double> nds { get; set; }
        public Nullable<double> nsp { get; set; }
        public Nullable<double> quantity_over { get; set; }
        public string measure_over { get; set; }
        public Nullable<double> sum_no_tax { get; set; }
        public Nullable<System.DateTime> alt_date { get; set; }
    }
}
