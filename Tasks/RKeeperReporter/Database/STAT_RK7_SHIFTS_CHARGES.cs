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
    
    public partial class STAT_RK7_SHIFTS_CHARGES
    {
        public System.Guid GLOBALSHIFTGUID { get; set; }
        public Nullable<System.Guid> WAITERGUID { get; set; }
        public Nullable<System.Guid> WAITERSGUID { get; set; }
        public Nullable<System.Guid> MANAGERGUID { get; set; }
        public Nullable<System.Guid> DISCOUNTGUID { get; set; }
        public Nullable<System.Guid> RESTAURANTGUID { get; set; }
        public Nullable<System.Guid> MIDSERVERGUID { get; set; }
        public Nullable<System.Guid> STATIONGUID { get; set; }
        public Nullable<System.Guid> REPCATEGORYGUID { get; set; }
        public Nullable<System.Guid> ORDERCATEGORYGUID { get; set; }
        public Nullable<System.Guid> CURRENCYGUID { get; set; }
        public Nullable<System.Guid> VOIDGUID { get; set; }
        public string CHECKIDENT { get; set; }
        public Nullable<System.DateTime> SHIFTDATE { get; set; }
        public string WAITER { get; set; }
        public string WAITERS { get; set; }
        public string MANAGER { get; set; }
        public string DISCOUNT { get; set; }
        public string RESTAURANT { get; set; }
        public string MIDSERVER { get; set; }
        public string STATION { get; set; }
        public string REPCATEGORY { get; set; }
        public string CURRENCY { get; set; }
        public string VOID { get; set; }
        public Nullable<decimal> BASICSUM { get; set; }
        public Nullable<decimal> NATIONALSUM { get; set; }
        public Nullable<decimal> PRLISTSUM { get; set; }
        public Nullable<decimal> PAIDSUM { get; set; }
        public Nullable<int> CHECKCOUNT { get; set; }
        public Nullable<int> GUESTCOUNT { get; set; }
        public Nullable<int> ORDERCOUNT { get; set; }
        public Nullable<int> SHIFTNUM { get; set; }
        public string CARDCODE { get; set; }
        public Nullable<int> CHECKNUM { get; set; }
        public Nullable<int> VISIT { get; set; }
        public Nullable<int> MIDSRV { get; set; }
        public Nullable<int> UNI { get; set; }
        public Nullable<int> DUNI { get; set; }
        public Nullable<int> BINDINGUNI { get; set; }
        public Nullable<int> NONZERODISC { get; set; }
        public Nullable<int> NONZEROBONUS { get; set; }
        public Nullable<System.Guid> DISHGUID { get; set; }
        public string DISH { get; set; }
    }
}
