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
    
    public partial class STAT_RK7_SHIFTS_GPX
    {
        public System.Guid GUIDSTRING { get; set; }
        public System.Guid BASEGUID { get; set; }
        public Nullable<System.Guid> EXPEDITORGUID { get; set; }
        public Nullable<System.Guid> AUTHOR { get; set; }
        public Nullable<System.Guid> CREATOR { get; set; }
        public Nullable<System.DateTime> POINTDATETIME { get; set; }
        public Nullable<System.DateTime> REALDATETIME { get; set; }
        public Nullable<decimal> LAT { get; set; }
        public Nullable<decimal> LON { get; set; }
    }
}