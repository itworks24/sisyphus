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
    
    public partial class TAXRATES
    {
        public int SIFR { get; set; }
        public string GUIDSTRING { get; set; }
        public Nullable<int> TAX { get; set; }
        public Nullable<double> TAXRATE { get; set; }
        public Nullable<short> STATUS { get; set; }
        public Nullable<short> FISCREGTAXTYPE1 { get; set; }
        public Nullable<short> FISCREGTAXTYPE2 { get; set; }
        public Nullable<short> FISCREGTAXTYPE3 { get; set; }
        public string RECSTAMP { get; set; }
        public Nullable<short> DBSTATUS { get; set; }
    }
}
