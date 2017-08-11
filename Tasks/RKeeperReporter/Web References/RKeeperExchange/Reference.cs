﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace RKeeperReporter.RKeeperExchange {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="RKeeperExchangeSoapBinding", Namespace="http://rkeeper.lnd.ru")]
    public partial class RKeeperExchange : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ReceiveDataOperationCompleted;
        
        private System.Threading.SendOrPostCallback ReceiveClassificatorGroupsOperationCompleted;
        
        private System.Threading.SendOrPostCallback ReceiveDishGroupsOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public RKeeperExchange() {
            this.Url = global::RKeeperReporter.Properties.Settings.Default.RKeeperReporter_RKeeperExchange_RKeeperExchange;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event ReceiveDataCompletedEventHandler ReceiveDataCompleted;
        
        /// <remarks/>
        public event ReceiveClassificatorGroupsCompletedEventHandler ReceiveClassificatorGroupsCompleted;
        
        /// <remarks/>
        public event ReceiveDishGroupsCompletedEventHandler ReceiveDishGroupsCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://rkeeper.lnd.ru#RKeeperExchange:ReceiveData", RequestNamespace="http://rkeeper.lnd.ru", ResponseNamespace="http://rkeeper.lnd.ru", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return")]
        public bool ReceiveData([System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)] Report[] Reports, string ReportId, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] ref string ErrorInfo) {
            object[] results = this.Invoke("ReceiveData", new object[] {
                        Reports,
                        ReportId,
                        ErrorInfo});
            ErrorInfo = ((string)(results[1]));
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void ReceiveDataAsync(Report[] Reports, string ReportId, string ErrorInfo) {
            this.ReceiveDataAsync(Reports, ReportId, ErrorInfo, null);
        }
        
        /// <remarks/>
        public void ReceiveDataAsync(Report[] Reports, string ReportId, string ErrorInfo, object userState) {
            if ((this.ReceiveDataOperationCompleted == null)) {
                this.ReceiveDataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnReceiveDataOperationCompleted);
            }
            this.InvokeAsync("ReceiveData", new object[] {
                        Reports,
                        ReportId,
                        ErrorInfo}, this.ReceiveDataOperationCompleted, userState);
        }
        
        private void OnReceiveDataOperationCompleted(object arg) {
            if ((this.ReceiveDataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ReceiveDataCompleted(this, new ReceiveDataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://rkeeper.lnd.ru#RKeeperExchange:ReceiveClassificatorGroups", RequestNamespace="http://rkeeper.lnd.ru", ResponseNamespace="http://rkeeper.lnd.ru", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return")]
        public bool ReceiveClassificatorGroups([System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)] ClassificatorGroup[] ClassificatorGroups, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] ref string ErrorInfo) {
            object[] results = this.Invoke("ReceiveClassificatorGroups", new object[] {
                        ClassificatorGroups,
                        ErrorInfo});
            ErrorInfo = ((string)(results[1]));
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void ReceiveClassificatorGroupsAsync(ClassificatorGroup[] ClassificatorGroups, string ErrorInfo) {
            this.ReceiveClassificatorGroupsAsync(ClassificatorGroups, ErrorInfo, null);
        }
        
        /// <remarks/>
        public void ReceiveClassificatorGroupsAsync(ClassificatorGroup[] ClassificatorGroups, string ErrorInfo, object userState) {
            if ((this.ReceiveClassificatorGroupsOperationCompleted == null)) {
                this.ReceiveClassificatorGroupsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnReceiveClassificatorGroupsOperationCompleted);
            }
            this.InvokeAsync("ReceiveClassificatorGroups", new object[] {
                        ClassificatorGroups,
                        ErrorInfo}, this.ReceiveClassificatorGroupsOperationCompleted, userState);
        }
        
        private void OnReceiveClassificatorGroupsOperationCompleted(object arg) {
            if ((this.ReceiveClassificatorGroupsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ReceiveClassificatorGroupsCompleted(this, new ReceiveClassificatorGroupsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://rkeeper.lnd.ru#RKeeperExchange:ReceiveDishGroups", RequestNamespace="http://rkeeper.lnd.ru", ResponseNamespace="http://rkeeper.lnd.ru", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return")]
        public bool ReceiveDishGroups([System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)] DishGroup[] ClassificatorGroups, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] ref string ErrorInfo) {
            object[] results = this.Invoke("ReceiveDishGroups", new object[] {
                        ClassificatorGroups,
                        ErrorInfo});
            ErrorInfo = ((string)(results[1]));
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void ReceiveDishGroupsAsync(DishGroup[] ClassificatorGroups, string ErrorInfo) {
            this.ReceiveDishGroupsAsync(ClassificatorGroups, ErrorInfo, null);
        }
        
        /// <remarks/>
        public void ReceiveDishGroupsAsync(DishGroup[] ClassificatorGroups, string ErrorInfo, object userState) {
            if ((this.ReceiveDishGroupsOperationCompleted == null)) {
                this.ReceiveDishGroupsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnReceiveDishGroupsOperationCompleted);
            }
            this.InvokeAsync("ReceiveDishGroups", new object[] {
                        ClassificatorGroups,
                        ErrorInfo}, this.ReceiveDishGroupsOperationCompleted, userState);
        }
        
        private void OnReceiveDishGroupsOperationCompleted(object arg) {
            if ((this.ReceiveDishGroupsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ReceiveDishGroupsCompleted(this, new ReceiveDishGroupsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2046.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://rkeeper.lnd.ru")]
    public partial class Report {
        
        private Element classficatorGroupField;
        
        private Element restaurantField;
        
        private Element currencyTypeField;
        
        private Element discountTypeField;
        
        private Element menuItemField;
        
        private Element visitField;
        
        private System.DateTime visitQuitTimeField;
        
        private Element currencyField;
        
        private decimal sumField;
        
        private decimal paySumField;
        
        private decimal discountSumField;
        
        /// <remarks/>
        public Element ClassficatorGroup {
            get {
                return this.classficatorGroupField;
            }
            set {
                this.classficatorGroupField = value;
            }
        }
        
        /// <remarks/>
        public Element Restaurant {
            get {
                return this.restaurantField;
            }
            set {
                this.restaurantField = value;
            }
        }
        
        /// <remarks/>
        public Element CurrencyType {
            get {
                return this.currencyTypeField;
            }
            set {
                this.currencyTypeField = value;
            }
        }
        
        /// <remarks/>
        public Element DiscountType {
            get {
                return this.discountTypeField;
            }
            set {
                this.discountTypeField = value;
            }
        }
        
        /// <remarks/>
        public Element MenuItem {
            get {
                return this.menuItemField;
            }
            set {
                this.menuItemField = value;
            }
        }
        
        /// <remarks/>
        public Element Visit {
            get {
                return this.visitField;
            }
            set {
                this.visitField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime VisitQuitTime {
            get {
                return this.visitQuitTimeField;
            }
            set {
                this.visitQuitTimeField = value;
            }
        }
        
        /// <remarks/>
        public Element Currency {
            get {
                return this.currencyField;
            }
            set {
                this.currencyField = value;
            }
        }
        
        /// <remarks/>
        public decimal Sum {
            get {
                return this.sumField;
            }
            set {
                this.sumField = value;
            }
        }
        
        /// <remarks/>
        public decimal PaySum {
            get {
                return this.paySumField;
            }
            set {
                this.paySumField = value;
            }
        }
        
        /// <remarks/>
        public decimal DiscountSum {
            get {
                return this.discountSumField;
            }
            set {
                this.discountSumField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2046.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://rkeeper.lnd.ru")]
    public partial class Element {
        
        private string nameField;
        
        private int codeField;
        
        private System.Nullable<int> parentField;
        
        /// <remarks/>
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        public int Code {
            get {
                return this.codeField;
            }
            set {
                this.codeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> Parent {
            get {
                return this.parentField;
            }
            set {
                this.parentField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2046.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://rkeeper.lnd.ru")]
    public partial class DishGroup {
        
        private int menuItemCodeField;
        
        private int classificatorGroupCodeField;
        
        private bool primaryField;
        
        /// <remarks/>
        public int MenuItemCode {
            get {
                return this.menuItemCodeField;
            }
            set {
                this.menuItemCodeField = value;
            }
        }
        
        /// <remarks/>
        public int ClassificatorGroupCode {
            get {
                return this.classificatorGroupCodeField;
            }
            set {
                this.classificatorGroupCodeField = value;
            }
        }
        
        /// <remarks/>
        public bool Primary {
            get {
                return this.primaryField;
            }
            set {
                this.primaryField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2046.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://rkeeper.lnd.ru")]
    public partial class ClassificatorGroup {
        
        private Element baseField;
        
        private int groupField;
        
        private int numInGroupField;
        
        /// <remarks/>
        public Element Base {
            get {
                return this.baseField;
            }
            set {
                this.baseField = value;
            }
        }
        
        /// <remarks/>
        public int Group {
            get {
                return this.groupField;
            }
            set {
                this.groupField = value;
            }
        }
        
        /// <remarks/>
        public int NumInGroup {
            get {
                return this.numInGroupField;
            }
            set {
                this.numInGroupField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void ReceiveDataCompletedEventHandler(object sender, ReceiveDataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ReceiveDataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ReceiveDataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public string ErrorInfo {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[1]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void ReceiveClassificatorGroupsCompletedEventHandler(object sender, ReceiveClassificatorGroupsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ReceiveClassificatorGroupsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ReceiveClassificatorGroupsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public string ErrorInfo {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[1]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    public delegate void ReceiveDishGroupsCompletedEventHandler(object sender, ReceiveDishGroupsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2046.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ReceiveDishGroupsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ReceiveDishGroupsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public string ErrorInfo {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[1]));
            }
        }
    }
}

#pragma warning restore 1591