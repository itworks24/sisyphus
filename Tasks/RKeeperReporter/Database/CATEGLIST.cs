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
    
    public partial class CATEGLIST
    {
        public int SIFR { get; set; }
        public string GUIDSTRING { get; set; }
        public Nullable<int> CODE { get; set; }
        public string NAME { get; set; }
        public string ALTNAME { get; set; }
        public string COMMENT { get; set; }
        public Nullable<int> PARENT { get; set; }
        public Nullable<int> VISUALTYPE_IMAGE { get; set; }
        public Nullable<int> VISUALTYPE_BCOLOR { get; set; }
        public Nullable<int> VISUALTYPE_TEXTCOLOR { get; set; }
        public Nullable<short> VISUALTYPE_FLAGS { get; set; }
        public Nullable<int> TOPARENTCATEG { get; set; }
        public Nullable<short> STATUS { get; set; }
        public Nullable<int> EXTCODE { get; set; }
        public Nullable<int> EXTRESTID { get; set; }
        public Nullable<short> EDITRIGHT { get; set; }
        public Nullable<short> RIGHTLVL { get; set; }
        public Nullable<short> ADDTOORDER { get; set; }
        public Nullable<int> CATEGKIND { get; set; }
        public string RECSTAMP { get; set; }
        public Nullable<short> DBSTATUS { get; set; }
    }
}
