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
    
    public partial class CLASSINFOS
    {
        public short REFNO { get; set; }
        public short NUM { get; set; }
        public Nullable<int> GROUP { get; set; }
        public string GUIDSTRING { get; set; }
        public Nullable<int> CONDITIONPARAMETER { get; set; }
        public string CINAMEOFCLASS { get; set; }
        public string CIUSERNAME { get; set; }
        public string CIUSERPLNAME { get; set; }
        public string CIDEFAULTPROP { get; set; }
        public byte[] CIHIDDENPROPS { get; set; }
        public byte[] CIREPONLYPROPS { get; set; }
        public byte[] CIREADONLYPROPS { get; set; }
        public byte[] CISPECEDITPROPS { get; set; }
        public byte[] CIENGPREFPROPS { get; set; }
        public byte[] CICOMPLOGPROPS { get; set; }
        public byte[] CIMASKLOGPROPS { get; set; }
        public byte[] CIHIDELOGVALS { get; set; }
        public Nullable<short> CIOPTIONS { get; set; }
        public string CIREQPROPS { get; set; }
        public string CIFOCUSONPROP { get; set; }
        public byte[] CISQLPROPHELP { get; set; }
        public Nullable<short> CINAMESUNIQUE { get; set; }
        public Nullable<short> CIPARENTPOWCODE { get; set; }
        public byte[] CIDONTCHECKPROPS { get; set; }
        public Nullable<short> CIUPDATERIGHT { get; set; }
        public Nullable<int> HHCONTEXTID { get; set; }
        public string RECSTAMP { get; set; }
        public Nullable<short> DBSTATUS { get; set; }
    }
}