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
    
    public partial class STAT_RK7_SHIFTS_PARAMETERS_VAL
    {
        public System.Guid GUIDSTRING { get; set; }
        public Nullable<System.Guid> PLAN_PARAMETERS_GUID { get; set; }
        public Nullable<decimal> VALUE { get; set; }
        public Nullable<System.DateTime> CALENDAR_DATE { get; set; }
        public Nullable<System.Guid> RESTAURANTGUID { get; set; }
        public Nullable<System.Guid> CONCEPTGUID { get; set; }
        public Nullable<System.Guid> PLAN_LEVELS_GUID { get; set; }
    }
}