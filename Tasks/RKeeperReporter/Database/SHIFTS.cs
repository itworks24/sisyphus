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
    
    public partial class SHIFTS
    {
        public int MIDSERVER { get; set; }
        public int ISTATION { get; set; }
        public int SHIFTNUM { get; set; }
        public Nullable<int> ICOMMONSHIFT { get; set; }
        public Nullable<int> STARTSHIFTNUM { get; set; }
        public Nullable<System.DateTime> CREATETIME { get; set; }
        public Nullable<System.DateTime> STARTTIME { get; set; }
        public Nullable<System.DateTime> CLOSETIME { get; set; }
        public Nullable<int> IMANAGER { get; set; }
        public Nullable<decimal> BASICSUM { get; set; }
        public Nullable<decimal> FISCALSUM { get; set; }
        public Nullable<decimal> NATIONALSUM { get; set; }
        public Nullable<short> CLOSED { get; set; }
        public Nullable<short> SENDED { get; set; }
        public Nullable<int> PRINTSHIFTNUM { get; set; }
        public Nullable<int> IPRINTSTATION { get; set; }
        public Nullable<int> IPRINTER { get; set; }
        public string EXTFISCID { get; set; }
        public Nullable<short> PRINTED { get; set; }
        public Nullable<short> ISLASTSHIFT { get; set; }
        public Nullable<int> LASTPRINTNUMBER { get; set; }
        public Nullable<int> ZREPNUM { get; set; }
        public string TRANSACT_GUID { get; set; }
    }
}