﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class RKeeperEntities : DbContext
    {
        public RKeeperEntities()
            : base("name=RKeeperEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ACTIVITYLIST> ACTIVITYLIST { get; set; }
        public virtual DbSet<ALIASES> ALIASES { get; set; }
        public virtual DbSet<ALIASLANGUAGES> ALIASLANGUAGES { get; set; }
        public virtual DbSet<AVAILABILITYSCHEDULES> AVAILABILITYSCHEDULES { get; set; }
        public virtual DbSet<AWARDSPENALTIESDATA> AWARDSPENALTIESDATA { get; set; }
        public virtual DbSet<AWARDSPENALTIESGROUPS> AWARDSPENALTIESGROUPS { get; set; }
        public virtual DbSet<AWARDSPENALTIESTYPES> AWARDSPENALTIESTYPES { get; set; }
        public virtual DbSet<BANDEDPROPSRIGHTS> BANDEDPROPSRIGHTS { get; set; }
        public virtual DbSet<BANDRIGHTS> BANDRIGHTS { get; set; }
        public virtual DbSet<BASEDIALECT> BASEDIALECT { get; set; }
        public virtual DbSet<BONUSTYPES> BONUSTYPES { get; set; }
        public virtual DbSet<BRIGADES> BRIGADES { get; set; }
        public virtual DbSet<BUSINESSPERIODS> BUSINESSPERIODS { get; set; }
        public virtual DbSet<CASHES> CASHES { get; set; }
        public virtual DbSet<CASHGROUPS> CASHGROUPS { get; set; }
        public virtual DbSet<CASHINOUT> CASHINOUT { get; set; }
        public virtual DbSet<CASHREPORTDETAILS> CASHREPORTDETAILS { get; set; }
        public virtual DbSet<CASHSERVDATASTATUSES> CASHSERVDATASTATUSES { get; set; }
        public virtual DbSet<CATEGLIST> CATEGLIST { get; set; }
        public virtual DbSet<CHANGEABLEORDERTYPES> CHANGEABLEORDERTYPES { get; set; }
        public virtual DbSet<CHECKTABLEFIELDS> CHECKTABLEFIELDS { get; set; }
        public virtual DbSet<CHECKTABLES> CHECKTABLES { get; set; }
        public virtual DbSet<CLASSIFICATORGROUPS> CLASSIFICATORGROUPS { get; set; }
        public virtual DbSet<CLASSINFOGROUPS> CLASSINFOGROUPS { get; set; }
        public virtual DbSet<CLASSINFOHIDEITEMS> CLASSINFOHIDEITEMS { get; set; }
        public virtual DbSet<CLASSINFOLOGPROPS> CLASSINFOLOGPROPS { get; set; }
        public virtual DbSet<CLASSINFOPROPRESTRICTS> CLASSINFOPROPRESTRICTS { get; set; }
        public virtual DbSet<CLASSINFOS> CLASSINFOS { get; set; }
        public virtual DbSet<CLASSINFOSQLPROPS> CLASSINFOSQLPROPS { get; set; }
        public virtual DbSet<CLOCKRECS> CLOCKRECS { get; set; }
        public virtual DbSet<COLORMAPPINGS> COLORMAPPINGS { get; set; }
        public virtual DbSet<COLORSCHEMEDETAILS> COLORSCHEMEDETAILS { get; set; }
        public virtual DbSet<COLORSCHEMES> COLORSCHEMES { get; set; }
        public virtual DbSet<CONSUMATORS> CONSUMATORS { get; set; }
        public virtual DbSet<CONSUMTYPES> CONSUMTYPES { get; set; }
        public virtual DbSet<COTUSAGES> COTUSAGES { get; set; }
        public virtual DbSet<CUBEDIALECTS> CUBEDIALECTS { get; set; }
        public virtual DbSet<CURRENCIES> CURRENCIES { get; set; }
        public virtual DbSet<CURRENCIESBANS> CURRENCIESBANS { get; set; }
        public virtual DbSet<CURRENCYFACEVALUES> CURRENCYFACEVALUES { get; set; }
        public virtual DbSet<CURRENCYTYPES> CURRENCYTYPES { get; set; }
        public virtual DbSet<CURRLINES> CURRLINES { get; set; }
        public virtual DbSet<data_shcr_CatFoodCost> data_shcr_CatFoodCost { get; set; }
        public virtual DbSet<data_shcr_cm> data_shcr_cm { get; set; }
        public virtual DbSet<data_shcr_cm_atributes> data_shcr_cm_atributes { get; set; }
        public virtual DbSet<data_shcr_cm_detail> data_shcr_cm_detail { get; set; }
        public virtual DbSet<data_shcr_cm_tech> data_shcr_cm_tech { get; set; }
        public virtual DbSet<data_shcr_cm_version> data_shcr_cm_version { get; set; }
        public virtual DbSet<data_shcr_cmprotocol> data_shcr_cmprotocol { get; set; }
        public virtual DbSet<data_shcr_CorrFullList> data_shcr_CorrFullList { get; set; }
        public virtual DbSet<data_shcr_CorrTree> data_shcr_CorrTree { get; set; }
        public virtual DbSet<data_shcr_CurrencyMapping> data_shcr_CurrencyMapping { get; set; }
        public virtual DbSet<data_shcr_doc> data_shcr_doc { get; set; }
        public virtual DbSet<data_shcr_doc_attrs> data_shcr_doc_attrs { get; set; }
        public virtual DbSet<data_shcr_doc_lines> data_shcr_doc_lines { get; set; }
        public virtual DbSet<data_shcr_docprotocol> data_shcr_docprotocol { get; set; }
        public virtual DbSet<data_shcr_expdoc> data_shcr_expdoc { get; set; }
        public virtual DbSet<data_shcr_expdoc_lines> data_shcr_expdoc_lines { get; set; }
        public virtual DbSet<data_shcr_expdoc_lines_details> data_shcr_expdoc_lines_details { get; set; }
        public virtual DbSet<data_shcr_expdocprotocol> data_shcr_expdocprotocol { get; set; }
        public virtual DbSet<data_shcr_Goods> data_shcr_Goods { get; set; }
        public virtual DbSet<data_shcr_GoodsBaseAttrs> data_shcr_GoodsBaseAttrs { get; set; }
        public virtual DbSet<data_shcr_GoodsBaseCompl> data_shcr_GoodsBaseCompl { get; set; }
        public virtual DbSet<data_shcr_GoodsBaseMain> data_shcr_GoodsBaseMain { get; set; }
        public virtual DbSet<data_shcr_GoodsBaseMeasures> data_shcr_GoodsBaseMeasures { get; set; }
        public virtual DbSet<data_shcr_GoodsBaseStore> data_shcr_GoodsBaseStore { get; set; }
        public virtual DbSet<data_shcr_GoodsBaseWrite_Off> data_shcr_GoodsBaseWrite_Off { get; set; }
        public virtual DbSet<data_shcr_GoodsCtg> data_shcr_GoodsCtg { get; set; }
        public virtual DbSet<data_shcr_GoodsCtg2> data_shcr_GoodsCtg2 { get; set; }
        public virtual DbSet<data_shcr_GoodsTree> data_shcr_GoodsTree { get; set; }
        public virtual DbSet<data_shcr_idoc> data_shcr_idoc { get; set; }
        public virtual DbSet<data_shcr_idoc_lines> data_shcr_idoc_lines { get; set; }
        public virtual DbSet<data_shcr_idocprotocol> data_shcr_idocprotocol { get; set; }
        public virtual DbSet<data_shcr_Messages> data_shcr_Messages { get; set; }
        public virtual DbSet<data_shcr_MGroups> data_shcr_MGroups { get; set; }
        public virtual DbSet<data_shcr_MUnits> data_shcr_MUnits { get; set; }
        public virtual DbSet<data_shcr_OwnAttrs> data_shcr_OwnAttrs { get; set; }
        public virtual DbSet<data_shcr_PLDoc> data_shcr_PLDoc { get; set; }
        public virtual DbSet<data_shcr_PLDoc_details> data_shcr_PLDoc_details { get; set; }
        public virtual DbSet<data_shcr_ReplaceGoods> data_shcr_ReplaceGoods { get; set; }
        public virtual DbSet<data_shcr_ReplaceGrp> data_shcr_ReplaceGrp { get; set; }
        public virtual DbSet<data_shcr_settings> data_shcr_settings { get; set; }
        public virtual DbSet<data_shcr_StoreMapping> data_shcr_StoreMapping { get; set; }
        public virtual DbSet<data_shcr_TDoc> data_shcr_TDoc { get; set; }
        public virtual DbSet<data_shcr_TDoc_details> data_shcr_TDoc_details { get; set; }
        public virtual DbSet<DEFAULTERTYPES> DEFAULTERTYPES { get; set; }
        public virtual DbSet<DELIVERYDATA> DELIVERYDATA { get; set; }
        public virtual DbSet<DELIVERYZONES> DELIVERYZONES { get; set; }
        public virtual DbSet<DEPARTS> DEPARTS { get; set; }
        public virtual DbSet<DEPOSITCOLLECTREASONS> DEPOSITCOLLECTREASONS { get; set; }
        public virtual DbSet<DEVICEDATALOOKUPITEMS> DEVICEDATALOOKUPITEMS { get; set; }
        public virtual DbSet<DEVICEDETAILS> DEVICEDETAILS { get; set; }
        public virtual DbSet<DEVICELICENSES> DEVICELICENSES { get; set; }
        public virtual DbSet<DEVICES> DEVICES { get; set; }
        public virtual DbSet<DISCOUNTCOMPOSITIONS> DISCOUNTCOMPOSITIONS { get; set; }
        public virtual DbSet<DISCOUNTDETAILS> DISCOUNTDETAILS { get; set; }
        public virtual DbSet<DISCOUNTS> DISCOUNTS { get; set; }
        public virtual DbSet<DISCOUNTTYPES> DISCOUNTTYPES { get; set; }
        public virtual DbSet<DISCPARTS> DISCPARTS { get; set; }
        public virtual DbSet<DISHCONSUMATORS> DISHCONSUMATORS { get; set; }
        public virtual DbSet<DISHDISCOUNTS> DISHDISCOUNTS { get; set; }
        public virtual DbSet<DISHGROUPS> DISHGROUPS { get; set; }
        public virtual DbSet<DISHMODIFIERS> DISHMODIFIERS { get; set; }
        public virtual DbSet<DISHRESTS> DISHRESTS { get; set; }
        public virtual DbSet<DISHVOIDS> DISHVOIDS { get; set; }
        public virtual DbSet<DISPLAYRESOLUTIONS> DISPLAYRESOLUTIONS { get; set; }
        public virtual DbSet<DOCUMENTMAKETSCHEMELINKS> DOCUMENTMAKETSCHEMELINKS { get; set; }
        public virtual DbSet<DOCUMENTS> DOCUMENTS { get; set; }
        public virtual DbSet<DOSINGDEVICEASSIGNS> DOSINGDEVICEASSIGNS { get; set; }
        public virtual DbSet<DOSINGDEVICES> DOSINGDEVICES { get; set; }
        public virtual DbSet<DRAWERLOG> DRAWERLOG { get; set; }
        public virtual DbSet<EMPLOYEEGROUPDETAILS> EMPLOYEEGROUPDETAILS { get; set; }
        public virtual DbSet<EMPLOYEEGROUPS> EMPLOYEEGROUPS { get; set; }
        public virtual DbSet<EMPLOYEEROLES> EMPLOYEEROLES { get; set; }
        public virtual DbSet<EMPLOYEES> EMPLOYEES { get; set; }
        public virtual DbSet<ENTRANCECARDTYPES> ENTRANCECARDTYPES { get; set; }
        public virtual DbSet<ENUMSTYPESDATAS> ENUMSTYPESDATAS { get; set; }
        public virtual DbSet<ENUMSTYPESINFOS> ENUMSTYPESINFOS { get; set; }
        public virtual DbSet<EXTBASECONFIGS> EXTBASECONFIGS { get; set; }
        public virtual DbSet<EXTBASEUPGRADES> EXTBASEUPGRADES { get; set; }
        public virtual DbSet<EXTERNALIDS> EXTERNALIDS { get; set; }
        public virtual DbSet<EXTRATABLES> EXTRATABLES { get; set; }
        public virtual DbSet<FILTERS> FILTERS { get; set; }
        public virtual DbSet<FISCDEVPARAMS> FISCDEVPARAMS { get; set; }
        public virtual DbSet<FISCDEVZRKEYS> FISCDEVZRKEYS { get; set; }
        public virtual DbSet<FISCTYPEPROPS> FISCTYPEPROPS { get; set; }
        public virtual DbSet<FORMDETAILS> FORMDETAILS { get; set; }
        public virtual DbSet<FORMS> FORMS { get; set; }
        public virtual DbSet<FORMSCHEMECHILDS> FORMSCHEMECHILDS { get; set; }
        public virtual DbSet<FORMSCHEMEDETAILS> FORMSCHEMEDETAILS { get; set; }
        public virtual DbSet<FORMSCHEMES> FORMSCHEMES { get; set; }
        public virtual DbSet<FUNCTIONKEYGROUPS> FUNCTIONKEYGROUPS { get; set; }
        public virtual DbSet<FUNCTIONKEYS> FUNCTIONKEYS { get; set; }
        public virtual DbSet<FUNDSACCOUNTINGPLACES> FUNDSACCOUNTINGPLACES { get; set; }
        public virtual DbSet<GENERATEDPROPDATAS> GENERATEDPROPDATAS { get; set; }
        public virtual DbSet<GENERATEDPROPGROUPS> GENERATEDPROPGROUPS { get; set; }
        public virtual DbSet<GENERATEDPROPINFOS> GENERATEDPROPINFOS { get; set; }
        public virtual DbSet<GENERATEDPROPTYPES> GENERATEDPROPTYPES { get; set; }
        public virtual DbSet<GENERATORS> GENERATORS { get; set; }
        public virtual DbSet<GLOBALSHIFTS> GLOBALSHIFTS { get; set; }
        public virtual DbSet<GLOBALSHIFTSTATS> GLOBALSHIFTSTATS { get; set; }
        public virtual DbSet<GroupFilters> GroupFilters { get; set; }
        public virtual DbSet<GUESTREPLIES> GUESTREPLIES { get; set; }
        public virtual DbSet<GUESTTYPES> GUESTTYPES { get; set; }
        public virtual DbSet<HALLPLANITEMS> HALLPLANITEMS { get; set; }
        public virtual DbSet<HALLPLANS> HALLPLANS { get; set; }
        public virtual DbSet<HARDWAREINFO> HARDWAREINFO { get; set; }
        public virtual DbSet<HOURLIST> HOURLIST { get; set; }
        public virtual DbSet<IMAGELIST> IMAGELIST { get; set; }
        public virtual DbSet<IMAGENODES> IMAGENODES { get; set; }
        public virtual DbSet<INPDEVTYPES> INPDEVTYPES { get; set; }
        public virtual DbSet<INTERFACEDLL> INTERFACEDLL { get; set; }
        public virtual DbSet<INTFTRANSACTIONS> INTFTRANSACTIONS { get; set; }
        public virtual DbSet<INVOICES> INVOICES { get; set; }
        public virtual DbSet<IR_LASTPARAMETERSVALSBYUSERS> IR_LASTPARAMETERSVALSBYUSERS { get; set; }
        public virtual DbSet<IR_REPORTGROUPS> IR_REPORTGROUPS { get; set; }
        public virtual DbSet<IR_SCHEDULE_CALENDAR> IR_SCHEDULE_CALENDAR { get; set; }
        public virtual DbSet<IR_SCHEDULE_CALENDAR_DETAIL> IR_SCHEDULE_CALENDAR_DETAIL { get; set; }
        public virtual DbSet<IR_SCHEDULE_RECIPIENT> IR_SCHEDULE_RECIPIENT { get; set; }
        public virtual DbSet<IR_SCHEDULE_REPORTS> IR_SCHEDULE_REPORTS { get; set; }
        public virtual DbSet<IR_SCHEDULEREPORTS_EVENTS> IR_SCHEDULEREPORTS_EVENTS { get; set; }
        public virtual DbSet<IR_SENDING_TYPE> IR_SENDING_TYPE { get; set; }
        public virtual DbSet<IR_SETTINGS_MAILING> IR_SETTINGS_MAILING { get; set; }
        public virtual DbSet<ITEMSSALED> ITEMSSALED { get; set; }
        public virtual DbSet<KBDLAYOUTKEYS> KBDLAYOUTKEYS { get; set; }
        public virtual DbSet<KBDLAYOUTS> KBDLAYOUTS { get; set; }
        public virtual DbSet<KBDTYPES> KBDTYPES { get; set; }
        public virtual DbSet<KDSDATA> KDSDATA { get; set; }
        public virtual DbSet<KURSES> KURSES { get; set; }
        public virtual DbSet<LINKEDSYSTEMSCONFS> LINKEDSYSTEMSCONFS { get; set; }
        public virtual DbSet<LINKEDSYSTEMSTYPES> LINKEDSYSTEMSTYPES { get; set; }
        public virtual DbSet<MAKETS> MAKETS { get; set; }
        public virtual DbSet<MAKETSCHEMEDETAILS> MAKETSCHEMEDETAILS { get; set; }
        public virtual DbSet<MAKETSCHEMES> MAKETSCHEMES { get; set; }
        public virtual DbSet<MCRALGORITHMS> MCRALGORITHMS { get; set; }
        public virtual DbSet<MENUITEMS> MENUITEMS { get; set; }
        public virtual DbSet<MESSAGES> MESSAGES { get; set; }
        public virtual DbSet<MODIFIERS> MODIFIERS { get; set; }
        public virtual DbSet<MODIGROUPS> MODIGROUPS { get; set; }
        public virtual DbSet<MODISCHEMEDETAILS> MODISCHEMEDETAILS { get; set; }
        public virtual DbSet<MODISCHEMES> MODISCHEMES { get; set; }
        public virtual DbSet<OLAPCUBEEXTENDEDFIELDS> OLAPCUBEEXTENDEDFIELDS { get; set; }
        public virtual DbSet<OLAPCUBEFIELDS> OLAPCUBEFIELDS { get; set; }
        public virtual DbSet<OLAPCUBEFIELDSDETAILS> OLAPCUBEFIELDSDETAILS { get; set; }
        public virtual DbSet<OLAPCUBES> OLAPCUBES { get; set; }
        public virtual DbSet<OLAPCUBESCHEMES> OLAPCUBESCHEMES { get; set; }
        public virtual DbSet<OLAPREPORTGROUPS> OLAPREPORTGROUPS { get; set; }
        public virtual DbSet<OLAPREPORTRIGHTS> OLAPREPORTRIGHTS { get; set; }
        public virtual DbSet<OLAPREPORTS> OLAPREPORTS { get; set; }
        public virtual DbSet<OPERATIONCLASSES> OPERATIONCLASSES { get; set; }
        public virtual DbSet<OPERATIONDETAILS> OPERATIONDETAILS { get; set; }
        public virtual DbSet<OPERATIONLOG> OPERATIONLOG { get; set; }
        public virtual DbSet<OPERATIONS> OPERATIONS { get; set; }
        public virtual DbSet<OPERATIONSUSERDATA> OPERATIONSUSERDATA { get; set; }
        public virtual DbSet<ORDER_MAP> ORDER_MAP { get; set; }
        public virtual DbSet<ORDERS> ORDERS { get; set; }
        public virtual DbSet<ORDERSESSIONS> ORDERSESSIONS { get; set; }
        public virtual DbSet<ORDERVOIDS> ORDERVOIDS { get; set; }
        public virtual DbSet<ORDERWAITERS> ORDERWAITERS { get; set; }
        public virtual DbSet<PARAMETEREXCEPTIONS> PARAMETEREXCEPTIONS { get; set; }
        public virtual DbSet<PARAMETERHIERARHIES> PARAMETERHIERARHIES { get; set; }
        public virtual DbSet<PARAMETERS> PARAMETERS { get; set; }
        public virtual DbSet<PAYBINDINGS> PAYBINDINGS { get; set; }
        public virtual DbSet<PAYMENTS> PAYMENTS { get; set; }
        public virtual DbSet<PAYMENTSEXTRA> PAYMENTSEXTRA { get; set; }
        public virtual DbSet<PDSCARDS> PDSCARDS { get; set; }
        public virtual DbSet<PERIODDETAILS> PERIODDETAILS { get; set; }
        public virtual DbSet<PERIODGROUPS> PERIODGROUPS { get; set; }
        public virtual DbSet<PERIODS> PERIODS { get; set; }
        public virtual DbSet<PLG_ACC_ACCOUNTS> PLG_ACC_ACCOUNTS { get; set; }
        public virtual DbSet<PLG_ACC_MAPS> PLG_ACC_MAPS { get; set; }
        public virtual DbSet<PLG_ACC_MAPS_CONTENT> PLG_ACC_MAPS_CONTENT { get; set; }
        public virtual DbSet<PLG_DATABASE> PLG_DATABASE { get; set; }
        public virtual DbSet<PLG_DATABASE_GROUP> PLG_DATABASE_GROUP { get; set; }
        public virtual DbSet<PLG_IR_DATASETFIELDS> PLG_IR_DATASETFIELDS { get; set; }
        public virtual DbSet<PLG_IR_DATASETPARAMETERS> PLG_IR_DATASETPARAMETERS { get; set; }
        public virtual DbSet<PLG_IR_DATASETS> PLG_IR_DATASETS { get; set; }
        public virtual DbSet<PLG_IR_DATASETSGROUPS> PLG_IR_DATASETSGROUPS { get; set; }
        public virtual DbSet<PLG_IR_EXT_STORAGE> PLG_IR_EXT_STORAGE { get; set; }
        public virtual DbSet<PLG_IR_EXT_STORAGE_GROUPS> PLG_IR_EXT_STORAGE_GROUPS { get; set; }
        public virtual DbSet<PLG_IR_EXT_STORAGE_SQL_SRV> PLG_IR_EXT_STORAGE_SQL_SRV { get; set; }
        public virtual DbSet<PLG_IR_GENERATE_REPORTS_LOG> PLG_IR_GENERATE_REPORTS_LOG { get; set; }
        public virtual DbSet<PLG_IR_LASTPARAMETERSVALSBYUSERS> PLG_IR_LASTPARAMETERSVALSBYUSERS { get; set; }
        public virtual DbSet<PLG_IR_LINKCONFIG> PLG_IR_LINKCONFIG { get; set; }
        public virtual DbSet<PLG_IR_LINKCONFIGDETAIL> PLG_IR_LINKCONFIGDETAIL { get; set; }
        public virtual DbSet<PLG_IR_REPORTDATASETS> PLG_IR_REPORTDATASETS { get; set; }
        public virtual DbSet<PLG_IR_REPORTGROUPS> PLG_IR_REPORTGROUPS { get; set; }
        public virtual DbSet<PLG_IR_REPORTS> PLG_IR_REPORTS { get; set; }
        public virtual DbSet<PLG_IR_REPORTS_SETTING> PLG_IR_REPORTS_SETTING { get; set; }
        public virtual DbSet<PLG_IR_SCHEDULE_EXPORT> PLG_IR_SCHEDULE_EXPORT { get; set; }
        public virtual DbSet<PLG_IR_SCHEDULE_EXPORTGROUPS> PLG_IR_SCHEDULE_EXPORTGROUPS { get; set; }
        public virtual DbSet<PLG_IR_USEPARAMETERSBYDATASET> PLG_IR_USEPARAMETERSBYDATASET { get; set; }
        public virtual DbSet<PLG_IR_WEB_PLATFORMS> PLG_IR_WEB_PLATFORMS { get; set; }
        public virtual DbSet<PLG_IR_WEB_REPORTS_SETTING> PLG_IR_WEB_REPORTS_SETTING { get; set; }
        public virtual DbSet<PLG_IR_WEBREPORTS> PLG_IR_WEBREPORTS { get; set; }
        public virtual DbSet<PLG_IR_WEBREPORTSGROUPS> PLG_IR_WEBREPORTSGROUPS { get; set; }
        public virtual DbSet<PLG_PLAN_PARAMETERS> PLG_PLAN_PARAMETERS { get; set; }
        public virtual DbSet<PLG_PLAN_PARAMETERS_VAL> PLG_PLAN_PARAMETERS_VAL { get; set; }
        public virtual DbSet<PLG_PLAN_PARAMETERSGROUPS> PLG_PLAN_PARAMETERSGROUPS { get; set; }
        public virtual DbSet<PLG_PLAN_PERIOD> PLG_PLAN_PERIOD { get; set; }
        public virtual DbSet<PLG_REPORTS_HISTORY> PLG_REPORTS_HISTORY { get; set; }
        public virtual DbSet<PLG_TABLE_LINK> PLG_TABLE_LINK { get; set; }
        public virtual DbSet<PLG_TELEGRAM_USERS> PLG_TELEGRAM_USERS { get; set; }
        public virtual DbSet<PLG_TYPE_DATABASE> PLG_TYPE_DATABASE { get; set; }
        public virtual DbSet<PLG_USE_OBJECT_BY_DB> PLG_USE_OBJECT_BY_DB { get; set; }
        public virtual DbSet<PLG_USEPARAMETERSBYLEVEL> PLG_USEPARAMETERSBYLEVEL { get; set; }
        public virtual DbSet<PRICECONSTANTS> PRICECONSTANTS { get; set; }
        public virtual DbSet<PRICECONSTANTTYPES> PRICECONSTANTTYPES { get; set; }
        public virtual DbSet<PRICEFORMULAS> PRICEFORMULAS { get; set; }
        public virtual DbSet<PRICES> PRICES { get; set; }
        public virtual DbSet<PRICETYPES> PRICETYPES { get; set; }
        public virtual DbSet<PRICEVALUES> PRICEVALUES { get; set; }
        public virtual DbSet<PRINTCHECKS> PRINTCHECKS { get; set; }
        public virtual DbSet<PRINTEDDOCUMENTS> PRINTEDDOCUMENTS { get; set; }
        public virtual DbSet<PRINTERASSIGNS> PRINTERASSIGNS { get; set; }
        public virtual DbSet<PRINTERPURPOSES> PRINTERPURPOSES { get; set; }
        public virtual DbSet<PRIVILEGEDETAILS> PRIVILEGEDETAILS { get; set; }
        public virtual DbSet<PRIVILEGEGROUPS> PRIVILEGEGROUPS { get; set; }
        public virtual DbSet<PRIVILEGES> PRIVILEGES { get; set; }
        public virtual DbSet<PROPRIGHTS> PROPRIGHTS { get; set; }
        public virtual DbSet<RATECLASSES> RATECLASSES { get; set; }
        public virtual DbSet<RECOMMENDEDMENUITEMS> RECOMMENDEDMENUITEMS { get; set; }
        public virtual DbSet<REFLINKS> REFLINKS { get; set; }
        public virtual DbSet<REFRESOURCES> REFRESOURCES { get; set; }
        public virtual DbSet<REFTABLES> REFTABLES { get; set; }
        public virtual DbSet<REGISTRATIONS> REGISTRATIONS { get; set; }
        public virtual DbSet<REPORTINGSERVERS> REPORTINGSERVERS { get; set; }
        public virtual DbSet<REPORTSERVERSYNCITEMS> REPORTSERVERSYNCITEMS { get; set; }
        public virtual DbSet<RESTAURANTCONCEPTS> RESTAURANTCONCEPTS { get; set; }
        public virtual DbSet<RESTAURANTFRANCHISES> RESTAURANTFRANCHISES { get; set; }
        public virtual DbSet<RESTAURANTREGIONS> RESTAURANTREGIONS { get; set; }
        public virtual DbSet<RESTAURANTS> RESTAURANTS { get; set; }
        public virtual DbSet<RIGHTGROUPS> RIGHTGROUPS { get; set; }
        public virtual DbSet<RIGHTS> RIGHTS { get; set; }
        public virtual DbSet<ROLEREPSSERVS> ROLEREPSSERVS { get; set; }
        public virtual DbSet<ROLES> ROLES { get; set; }
        public virtual DbSet<SALEDATAS> SALEDATAS { get; set; }
        public virtual DbSet<SCHEDULEDREPORTRECIPIENT> SCHEDULEDREPORTRECIPIENT { get; set; }
        public virtual DbSet<SCHEDULEDREPORTS> SCHEDULEDREPORTS { get; set; }
        public virtual DbSet<scheduler> scheduler { get; set; }
        public virtual DbSet<SCRIPTS> SCRIPTS { get; set; }
        public virtual DbSet<SCRIPTTYPES> SCRIPTTYPES { get; set; }
        public virtual DbSet<SELECTORDETAILS> SELECTORDETAILS { get; set; }
        public virtual DbSet<SELECTORGROUPS> SELECTORGROUPS { get; set; }
        public virtual DbSet<SELECTORHIERARHIES> SELECTORHIERARHIES { get; set; }
        public virtual DbSet<SELECTORS> SELECTORS { get; set; }
        public virtual DbSet<SELECTORTYPES> SELECTORTYPES { get; set; }
        public virtual DbSet<SELECTORTYPEVSTRADEOBJ> SELECTORTYPEVSTRADEOBJ { get; set; }
        public virtual DbSet<SERVICECHECKS> SERVICECHECKS { get; set; }
        public virtual DbSet<SERVICESCHEMES> SERVICESCHEMES { get; set; }
        public virtual DbSet<SERVINGPOSITIONS> SERVINGPOSITIONS { get; set; }
        public virtual DbSet<SESSIONDISHES> SESSIONDISHES { get; set; }
        public virtual DbSet<SHIFTS> SHIFTS { get; set; }
        public virtual DbSet<SQLCONSTS> SQLCONSTS { get; set; }
        public virtual DbSet<STAT_RK7_SHIFTS_PARAMETERS_VAL> STAT_RK7_SHIFTS_PARAMETERS_VAL { get; set; }
        public virtual DbSet<STAT_RK7_SHIFTS_PDSCARDS> STAT_RK7_SHIFTS_PDSCARDS { get; set; }
        public virtual DbSet<STAT_RK7_SHIFTS_REFERENCES> STAT_RK7_SHIFTS_REFERENCES { get; set; }
        public virtual DbSet<statistics> statistics { get; set; }
        public virtual DbSet<STOREHOUSECONFIGS> STOREHOUSECONFIGS { get; set; }
        public virtual DbSet<SYNCDATAS> SYNCDATAS { get; set; }
        public virtual DbSet<TABLEATTRIBUTES> TABLEATTRIBUTES { get; set; }
        public virtual DbSet<TABLEATTRIBUTESASSIGNS> TABLEATTRIBUTESASSIGNS { get; set; }
        public virtual DbSet<TABLEGROUPS> TABLEGROUPS { get; set; }
        public virtual DbSet<TABLES> TABLES { get; set; }
        public virtual DbSet<TARIFFDATA> TARIFFDATA { get; set; }
        public virtual DbSet<TARIFFDETAILS> TARIFFDETAILS { get; set; }
        public virtual DbSet<TARIFFICATIONTYPES> TARIFFICATIONTYPES { get; set; }
        public virtual DbSet<TAXDISHRULES> TAXDISHRULES { get; set; }
        public virtual DbSet<TAXDISHTYPES> TAXDISHTYPES { get; set; }
        public virtual DbSet<TAXES> TAXES { get; set; }
        public virtual DbSet<TAXPARTS> TAXPARTS { get; set; }
        public virtual DbSet<TAXPAYRULES> TAXPAYRULES { get; set; }
        public virtual DbSet<TAXPAYTYPES> TAXPAYTYPES { get; set; }
        public virtual DbSet<TAXRATEFIELDS> TAXRATEFIELDS { get; set; }
        public virtual DbSet<TAXRATES> TAXRATES { get; set; }
        public virtual DbSet<TEMP_TASK> TEMP_TASK { get; set; }
        public virtual DbSet<TEMPACTIVITYLIST> TEMPACTIVITYLIST { get; set; }
        public virtual DbSet<TEMPAWARDSPENALTIESDATA> TEMPAWARDSPENALTIESDATA { get; set; }
        public virtual DbSet<TEMPCASHINOUT> TEMPCASHINOUT { get; set; }
        public virtual DbSet<TEMPCLOCKRECS> TEMPCLOCKRECS { get; set; }
        public virtual DbSet<TEMPCURRLINES> TEMPCURRLINES { get; set; }
        public virtual DbSet<TEMPDELIVERYDATA> TEMPDELIVERYDATA { get; set; }
        public virtual DbSet<TEMPDISCPARTS> TEMPDISCPARTS { get; set; }
        public virtual DbSet<TEMPDISHCONSUMATORS> TEMPDISHCONSUMATORS { get; set; }
        public virtual DbSet<TEMPDISHDISCOUNTS> TEMPDISHDISCOUNTS { get; set; }
        public virtual DbSet<TEMPDISHMODIFIERS> TEMPDISHMODIFIERS { get; set; }
        public virtual DbSet<TEMPDISHRESTS> TEMPDISHRESTS { get; set; }
        public virtual DbSet<TEMPDISHVOIDS> TEMPDISHVOIDS { get; set; }
        public virtual DbSet<TEMPDRAWERLOG> TEMPDRAWERLOG { get; set; }
        public virtual DbSet<TEMPEXTERNALIDS> TEMPEXTERNALIDS { get; set; }
        public virtual DbSet<TEMPEXTRATABLES> TEMPEXTRATABLES { get; set; }
        public virtual DbSet<TEMPGLOBALSHIFTS> TEMPGLOBALSHIFTS { get; set; }
        public virtual DbSet<TEMPGUESTREPLIES> TEMPGUESTREPLIES { get; set; }
        public virtual DbSet<TEMPHARDWAREINFO> TEMPHARDWAREINFO { get; set; }
        public virtual DbSet<TEMPINTFTRANSACTIONS> TEMPINTFTRANSACTIONS { get; set; }
        public virtual DbSet<TEMPINVOICES> TEMPINVOICES { get; set; }
        public virtual DbSet<TEMPITEMSSALED> TEMPITEMSSALED { get; set; }
        public virtual DbSet<TEMPKDSDATA> TEMPKDSDATA { get; set; }
        public virtual DbSet<TEMPMESSAGES> TEMPMESSAGES { get; set; }
        public virtual DbSet<TEMPOPERATIONLOG> TEMPOPERATIONLOG { get; set; }
        public virtual DbSet<TEMPORDERS> TEMPORDERS { get; set; }
        public virtual DbSet<TEMPORDERSESSIONS> TEMPORDERSESSIONS { get; set; }
        public virtual DbSet<TEMPORDERWAITERS> TEMPORDERWAITERS { get; set; }
        public virtual DbSet<TEMPPAYBINDINGS> TEMPPAYBINDINGS { get; set; }
        public virtual DbSet<TEMPPAYMENTS> TEMPPAYMENTS { get; set; }
        public virtual DbSet<TEMPPAYMENTSEXTRA> TEMPPAYMENTSEXTRA { get; set; }
        public virtual DbSet<TEMPPDSCARDS> TEMPPDSCARDS { get; set; }
        public virtual DbSet<TEMPPRINTCHECKS> TEMPPRINTCHECKS { get; set; }
        public virtual DbSet<TEMPPRINTEDDOCUMENTS> TEMPPRINTEDDOCUMENTS { get; set; }
        public virtual DbSet<TEMPREGISTRATIONS> TEMPREGISTRATIONS { get; set; }
        public virtual DbSet<TEMPSESSIONDISHES> TEMPSESSIONDISHES { get; set; }
        public virtual DbSet<TEMPSHIFTS> TEMPSHIFTS { get; set; }
        public virtual DbSet<TEMPTARIFFDATA> TEMPTARIFFDATA { get; set; }
        public virtual DbSet<TEMPTAXPARTS> TEMPTAXPARTS { get; set; }
        public virtual DbSet<TEMPVISITGUESTS> TEMPVISITGUESTS { get; set; }
        public virtual DbSet<TEMPVISITS> TEMPVISITS { get; set; }
        public virtual DbSet<TEMPZREPORTDATA> TEMPZREPORTDATA { get; set; }
        public virtual DbSet<TEMPZREPORTVALUES> TEMPZREPORTVALUES { get; set; }
        public virtual DbSet<TMP_USERS_SESSION> TMP_USERS_SESSION { get; set; }
        public virtual DbSet<TRADEGROUPDETAILS> TRADEGROUPDETAILS { get; set; }
        public virtual DbSet<TRADEGROUPS> TRADEGROUPS { get; set; }
        public virtual DbSet<TRANSACTIONS> TRANSACTIONS { get; set; }
        public virtual DbSet<UNCHANGEABLEORDERTYPES> UNCHANGEABLEORDERTYPES { get; set; }
        public virtual DbSet<VISITGUESTS> VISITGUESTS { get; set; }
        public virtual DbSet<VISITS> VISITS { get; set; }
        public virtual DbSet<WARNGROUPS> WARNGROUPS { get; set; }
        public virtual DbSet<ZREPORTDATA> ZREPORTDATA { get; set; }
        public virtual DbSet<ZREPORTVALUES> ZREPORTVALUES { get; set; }
        public virtual DbSet<PLG_SH4_LINK_STORE> PLG_SH4_LINK_STORE { get; set; }
        public virtual DbSet<REF_STAT_SP> REF_STAT_SP { get; set; }
        public virtual DbSet<RPL8_TRANSACT> RPL8_TRANSACT { get; set; }
        public virtual DbSet<STAT_PR_EMPLOYEEROLES> STAT_PR_EMPLOYEEROLES { get; set; }
        public virtual DbSet<STAT_PR_EMPLOYEES> STAT_PR_EMPLOYEES { get; set; }
        public virtual DbSet<STAT_PR_ROLES> STAT_PR_ROLES { get; set; }
        public virtual DbSet<STAT_PR_THEATRES> STAT_PR_THEATRES { get; set; }
        public virtual DbSet<STAT_RK7_EMPLOYEEROLES> STAT_RK7_EMPLOYEEROLES { get; set; }
        public virtual DbSet<STAT_RK7_SHIFTS_CECKSUM> STAT_RK7_SHIFTS_CECKSUM { get; set; }
        public virtual DbSet<STAT_RK7_SHIFTS_PDSFOLDERS> STAT_RK7_SHIFTS_PDSFOLDERS { get; set; }
        public virtual DbSet<STAT_SH4_CURRENCIES> STAT_SH4_CURRENCIES { get; set; }
        public virtual DbSet<STAT_SH4_GOODGROUPS> STAT_SH4_GOODGROUPS { get; set; }
        public virtual DbSet<STAT_SH4_SALELOCATIONS> STAT_SH4_SALELOCATIONS { get; set; }
        public virtual DbSet<SaleObjects> SaleObjects { get; set; }
        public virtual DbSet<SHEXPENDSDATA> SHEXPENDSDATA { get; set; }
        public virtual DbSet<SHMENUITEMS> SHMENUITEMS { get; set; }
        public virtual DbSet<SHMONEYSALESDATABINDINGS2> SHMONEYSALESDATABINDINGS2 { get; set; }
        public virtual DbSet<SHMONEYSALESDATAMODIFIERS> SHMONEYSALESDATAMODIFIERS { get; set; }
        public virtual DbSet<SHMONEYSALESDATAVOIDS> SHMONEYSALESDATAVOIDS { get; set; }
        public virtual DbSet<STAT_RK7_SHIFTS_CHARGES> STAT_RK7_SHIFTS_CHARGES { get; set; }
        public virtual DbSet<STAT_RK7_SHIFTS_COMPARISON> STAT_RK7_SHIFTS_COMPARISON { get; set; }
        public virtual DbSet<STAT_RK7_SHIFTS_DELIVERY> STAT_RK7_SHIFTS_DELIVERY { get; set; }
        public virtual DbSet<STAT_RK7_SHIFTS_GPX> STAT_RK7_SHIFTS_GPX { get; set; }
        public virtual DbSet<STAT_RK7_SHIFTS_GPX_ORDERS> STAT_RK7_SHIFTS_GPX_ORDERS { get; set; }
        public virtual DbSet<STAT_RK7_SHIFTS_MODIFIERS> STAT_RK7_SHIFTS_MODIFIERS { get; set; }
        public virtual DbSet<STAT_RK7_SHIFTS_PARAMS> STAT_RK7_SHIFTS_PARAMS { get; set; }
        public virtual DbSet<STAT_RK7_SHIFTS_REVENUE> STAT_RK7_SHIFTS_REVENUE { get; set; }
        public virtual DbSet<STAT_RK7_SHIFTS_TAXES> STAT_RK7_SHIFTS_TAXES { get; set; }
        public virtual DbSet<STAT_RK7_SHIFTS_VOIDS> STAT_RK7_SHIFTS_VOIDS { get; set; }
        public virtual DbSet<STAT_SH4_SHIFTS_FOODCOST> STAT_SH4_SHIFTS_FOODCOST { get; set; }
        public virtual DbSet<STAT_SH4_SHIFTS_FOODCOST_LOG> STAT_SH4_SHIFTS_FOODCOST_LOG { get; set; }
        public virtual DbSet<STAT_SH4_SHIFTS_GOODGROUPS> STAT_SH4_SHIFTS_GOODGROUPS { get; set; }
        public virtual DbSet<STAT_SH4_SHIFTS_GOODS> STAT_SH4_SHIFTS_GOODS { get; set; }
        public virtual DbSet<VRK7CUBETEMP101> VRK7CUBETEMP101 { get; set; }
        public virtual DbSet<VRK7CUBETEMP102> VRK7CUBETEMP102 { get; set; }
        public virtual DbSet<VRK7CUBETEMP103> VRK7CUBETEMP103 { get; set; }
        public virtual DbSet<VRK7CUBETEMP104> VRK7CUBETEMP104 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW1000> VRK7CUBEVIEW1000 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW1001> VRK7CUBEVIEW1001 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW1001837> VRK7CUBEVIEW1001837 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW1002> VRK7CUBEVIEW1002 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW1003> VRK7CUBEVIEW1003 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW1004> VRK7CUBEVIEW1004 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW1005> VRK7CUBEVIEW1005 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW1006> VRK7CUBEVIEW1006 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW1007> VRK7CUBEVIEW1007 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW1008> VRK7CUBEVIEW1008 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW101> VRK7CUBEVIEW101 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW102> VRK7CUBEVIEW102 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW103> VRK7CUBEVIEW103 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW104> VRK7CUBEVIEW104 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW10924> VRK7CUBEVIEW10924 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW10925> VRK7CUBEVIEW10925 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW10932> VRK7CUBEVIEW10932 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW10937> VRK7CUBEVIEW10937 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW10938> VRK7CUBEVIEW10938 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW10940> VRK7CUBEVIEW10940 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW10943> VRK7CUBEVIEW10943 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW10947> VRK7CUBEVIEW10947 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW10948> VRK7CUBEVIEW10948 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW2005> VRK7CUBEVIEW2005 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW2006> VRK7CUBEVIEW2006 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW2007> VRK7CUBEVIEW2007 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW2009> VRK7CUBEVIEW2009 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW2011> VRK7CUBEVIEW2011 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW3001> VRK7CUBEVIEW3001 { get; set; }
        public virtual DbSet<VRK7CUBEVIEW3002> VRK7CUBEVIEW3002 { get; set; }
        public virtual DbSet<vrk7RefItemsNames> vrk7RefItemsNames { get; set; }
    }
}
