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
    
    public partial class TEMPCLOCKRECS
    {
        public int MIDSERVER { get; set; }
        public int IDENT { get; set; }
        public Nullable<int> EMPID { get; set; }
        public Nullable<int> ROLEID { get; set; }
        public string CARDCODE { get; set; }
        public Nullable<System.DateTime> STARTTIME { get; set; }
        public Nullable<System.DateTime> ENDTIME { get; set; }
        public Nullable<System.DateTime> SHIFTSTARTTIME { get; set; }
        public Nullable<System.DateTime> SHIFTDURATION { get; set; }
        public Nullable<System.DateTime> MAXSHIFTDURATION { get; set; }
        public Nullable<int> ISTARTCOMMONSHIFT { get; set; }
        public Nullable<int> IENDCOMMONSHIFT { get; set; }
        public Nullable<int> ISTARTSTATION { get; set; }
        public Nullable<int> IENDSTATION { get; set; }
        public Nullable<int> ISTARTMANAGER { get; set; }
        public Nullable<int> IENDMANAGER { get; set; }
        public Nullable<decimal> DURATION { get; set; }
        public Nullable<decimal> RIGHTDURATION { get; set; }
        public Nullable<short> NOENDINGLABEL { get; set; }
        public Nullable<short> AUTOENTRANCE { get; set; }
        public Nullable<decimal> DELAY { get; set; }
        public Nullable<short> LATENESS { get; set; }
        public Nullable<short> MANUALLYCHANGED { get; set; }
        public Nullable<int> STATUS { get; set; }
        public string TRANSACT_GUID { get; set; }
        public Nullable<int> TEMPDATAKIND { get; set; }
        public Nullable<int> TEMPDATASIGN { get; set; }
        public Nullable<int> TEMPSERVERID { get; set; }
    }
}