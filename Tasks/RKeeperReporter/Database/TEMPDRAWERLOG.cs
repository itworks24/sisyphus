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
    
    public partial class TEMPDRAWERLOG
    {
        public int MIDSERVER { get; set; }
        public int UNI { get; set; }
        public Nullable<int> ICOMMONSHIFT { get; set; }
        public Nullable<int> DRAWERID { get; set; }
        public Nullable<int> WAITERID { get; set; }
        public Nullable<System.DateTime> OPENTIME { get; set; }
        public Nullable<System.DateTime> CLOSETIME { get; set; }
        public Nullable<int> OPER { get; set; }
        public Nullable<int> KIND { get; set; }
        public Nullable<int> VISIT { get; set; }
        public Nullable<int> ORDERIDENT { get; set; }
        public string TRANSACT_GUID { get; set; }
        public Nullable<int> TEMPDATAKIND { get; set; }
        public Nullable<int> TEMPDATASIGN { get; set; }
        public Nullable<int> TEMPSERVERID { get; set; }
    }
}
