using System;
using System.Collections.Generic;
using Sisyphus.Tasks;
using System.Linq;
using System.Data.Entity.Core.EntityClient;
using RKeeperReporter.RKeeperExchange;
using RKeeperReporter.Database;
using Sisyphus;
using System.Xml;

namespace RKeeperReporter.Database
{
    public partial class RKeeperEntities
    {
        public RKeeperEntities(string connectionString) : base(connectionString)
        {
            // ReSharper disable once UnusedVariable
            var ensureDllIsCopied =
                System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
    }
}

namespace RKeeperReporter.RKeeperExchange
{
    public partial class Element
    {
        public bool Equals(Element other)
        {
            // If parameter is null return false.
            if (other == null) return false;
            return codeField == other.codeField && nameField == other.nameField;
        }
    }

    public class TableRepresent
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public int Hall { get; set; }
    }

    public class RKReport : Report
    {
        public TableRepresent Table { get; set; }
        public Element Shift { get; set; }
    }
}

namespace Sysiphus.Tasks.SampleTask
{
    public static class DateExteninsion
    {
        /// <summary>
        /// Gets the 12:00:00 instance of a DateTime
        /// </summary>
        public static DateTime AbsoluteStart(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        /// <summary>
        /// Gets the 11:59:59 instance of a DateTime
        /// </summary>
        public static DateTime AbsoluteEnd(this DateTime dateTime)
        {
            return AbsoluteStart(dateTime).AddDays(1).AddTicks(-1);
        }
    }

    public static class RkeeperLoader
    {
        public class RkeeperLoaderSettings
        {
            public string ProviderName { get; set; }
            public string ConnectionString { get; set; }
            public int UploadLastDays { get; set; }
            public DateTime ReportStartDateTime { get; set; }
            public DateTime ReportEndDateTime { get; set; }
            public short ClassificationGroupSIFR { get; set; }
            public int restaurantCode { get; set; }
            public int databasePrefix { get; set; }
            public int menuItemCode { get; set; } = -1;
            public int shiftNum { get; set; } = -1;
        }

        internal static string RemoveInvalidXmlChars(string text)
        {
            var validXmlChars = text.Where(ch => XmlConvert.IsXmlChar(ch)).ToArray();
            return new string(validXmlChars);
        }

        internal static string GetEntityConnection(RkeeperLoaderSettings settings)
        {
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder
            {
                Provider = settings.ProviderName,
                ProviderConnectionString = settings.ConnectionString,
                Metadata =
                    "res://*/Database.RKeeperDB.csdl|res://*/Database.RKeeperDB.ssdl|res://*/Database.RKeeperDB.msl"
            };

            // Set the provider name. 

            //entityBuilder.ConnectionString = settings.ConnectionString;

            // Set the provider-specific connection string. 

            // Set the Metadata location. 

            return entityBuilder.ConnectionString;
        }

        internal static string ObjectRepresent(object obj, int indent = 0)
        {
            var type = obj.GetType();
            if (type.IsPrimitive) return "";
            string result = "";
            string currentIndent = "";
            for (var i = 0; i < indent; i++) currentIndent += " ";
            try
            {
                foreach (var field in type.GetFields())
                {
                    var fieldValue = field.GetValue(obj);
                    result += $"{currentIndent}{field.Name} : {fieldValue} \n";
                }
            }
            catch { }
            return result;
        }

        private static int AddPrefix(int code, int prefix)
        {
            return prefix * code.ToString().Length * 10 + code;
        }

        public static IEnumerable<RKReport> GetReports(RkeeperLoaderSettings settings)
        {
            using (var db = new RKeeperEntities(GetEntityConnection(settings)))
            {
                db.Database.CommandTimeout = 1800;

                var startDateTime = settings.UploadLastDays > 0 ? DateTime.Now.AddDays(-settings.UploadLastDays) : settings.ReportStartDateTime;
                var endDateTime = settings.UploadLastDays > 0 ? DateTime.Now : settings.ReportEndDateTime;

                startDateTime = startDateTime.AbsoluteStart();
                endDateTime = endDateTime.AbsoluteEnd();

                var menuItemCode = settings.menuItemCode;

                var classificationGroupSIFR = settings.ClassificationGroupSIFR;

                var restaurantCode = settings.restaurantCode;

                var shiftNum = settings.shiftNum;

                var reports = (from PAYBINDING in db.PAYBINDINGS
                               join CURRLINE in db.CURRLINES
                                     on new { visit = PAYBINDING.VISIT, midServer = PAYBINDING.MIDSERVER, currUNI = PAYBINDING.CURRUNI ?? -1 }
                                       equals new { visit = CURRLINE.VISIT, midServer = CURRLINE.MIDSERVER, currUNI = CURRLINE.UNI }
                               join PRINTCHECK in db.PRINTCHECKS
                                     on new { visit = CURRLINE.VISIT, midServer = CURRLINE.MIDSERVER, currUNI = CURRLINE.CHECKUNI ?? -1 }
                                       equals new { visit = PRINTCHECK.VISIT, midServer = PRINTCHECK.MIDSERVER, currUNI = PRINTCHECK.UNI }
                               join CURRENCYTYPE in db.CURRENCYTYPES
                                     on CURRLINE.IHIGHLEVELTYPE equals CURRENCYTYPE.SIFR
                               join CURRENCY in db.CURRENCIES
                                     on CURRLINE.SIFR equals CURRENCY.SIFR
                               join CASHGROUP in db.CASHGROUPS
                                     on PAYBINDING.MIDSERVER equals CASHGROUP.SIFR
                               join RESTAURANT in db.RESTAURANTS
                                     on CASHGROUP.RESTAURANT equals RESTAURANT.SIFR
                               join VISIT in db.VISITS
                                     on new { visit = CURRLINE.VISIT, midServer = PAYBINDING.MIDSERVER }
                                          equals new { visit = VISIT.SIFR, midServer = VISIT.MIDSERVER }
                               join ORDER in db.ORDERS
                                     on new { visit = PAYBINDING.VISIT, identInVisit = PAYBINDING.ORDERIDENT ?? -1, midServer = PAYBINDING.MIDSERVER }
                                          equals new { visit = ORDER.VISIT, identInVisit = ORDER.IDENTINVISIT, midServer = ORDER.MIDSERVER }
                               join SALEOBJECT in db.SALEOBJECTSSet
                                    on new { visit = PAYBINDING.VISIT, midServer = PAYBINDING.MIDSERVER, dishUNI = PAYBINDING.DISHUNI ?? -1, chargeUNI = PAYBINDING.CHARGEUNI ?? -1 }
                                        equals new { visit = SALEOBJECT.VISIT, midServer = SALEOBJECT.MIDSERVER, dishUNI = SALEOBJECT.DISHUNI, chargeUNI = SALEOBJECT.CHARGEUNI ?? -1 }
                               join GLOBALSHIFT in db.GLOBALSHIFTS
                                     on new { shift = ORDER.ICOMMONSHIFT ?? -1, midServer = ORDER.MIDSERVER }
                                          equals new { shift = GLOBALSHIFT.SHIFTNUM, midServer = GLOBALSHIFT.MIDSERVER }
                               join SESSIONDISH in db.SESSIONDISHES
                                     on new { visit = PAYBINDING.VISIT, midServer = PAYBINDING.MIDSERVER, dishUNI = SALEOBJECT.DISHUNI }
                                         equals new { visit = SESSIONDISH.VISIT, midServer = SESSIONDISH.MIDSERVER, dishUNI = SESSIONDISH.UNI }
                               join MENUITEM in db.MENUITEMS
                                     on SESSIONDISH.SIFR equals MENUITEM.SIFR
                               join DISHGROUP in db.DISHGROUPS
                                     on MENUITEM.SIFR equals DISHGROUP.CHILD
                               join CLASSIFICATORGROUP in db.CLASSIFICATORGROUPS
                                     on new { classoficatorId = DISHGROUP.PARENT ?? -1, ClassificationGroupSIFR = classificationGroupSIFR }
                                          equals new { classoficatorId = CLASSIFICATORGROUP.SIFR * 256 + CLASSIFICATORGROUP.NUMINGROUP, ClassificationGroupSIFR = classificationGroupSIFR == 0 ? (short)0 : CLASSIFICATORGROUP.SIFR }
                               join DISCPART in db.DISCPARTS
                                     on new { visit = SALEOBJECT.VISIT, MidServer = SALEOBJECT.MIDSERVER, bindingUNI = PAYBINDING.UNI }
                                         equals new { visit = DISCPART.VISIT, MidServer = DISCPART.MIDSERVER, bindingUNI = DISCPART.BINDINGUNI } into LEFTJOINDISCPARTS
                               join TABLE in db.TABLES
                                     on ORDER.TABLEID equals TABLE.SIFR into TABLES
                               where (shiftNum == -1 || GLOBALSHIFT.SHIFTNUM == shiftNum) 
                                     && (shiftNum != -1 || (GLOBALSHIFT.SHIFTDATE.Value >= startDateTime && GLOBALSHIFT.SHIFTDATE.Value <= endDateTime))
                                     && (restaurantCode == 0 || RESTAURANT.CODE == restaurantCode)
                                     && (PRINTCHECK.STATE == 6)
                                     && (menuItemCode <= 0 || MENUITEM.CODE == menuItemCode)
                                    

                               select new { CLASSIFICATORGROUP, RESTAURANT, CURRENCYTYPE, CURRENCY, PAYBINDING, VISIT, GLOBALSHIFT, LEFTJOINDISCPARTS, MENUITEM, SALEOBJECT, TABLES })
                               .ToList();
                var groups = from report in reports
                             group report by new
                             {
                                 ClassficatorGroup = report.CLASSIFICATORGROUP,
                                 Restaurant = report.RESTAURANT,
                                 CurrencyType = report.CURRENCYTYPE,
                                 Currency = report.CURRENCY,
                                 DiscountType = new { code = 1, name = "Без скидки" },
                                 MenuItem = report.MENUITEM,
                                 Visit = report.VISIT,
                                 GlobalShiftStartTime = report.GLOBALSHIFT.SHIFTDATE.Value.AbsoluteStart(),
                                 SaleObject = report.SALEOBJECT,
                                 Table = report.TABLES.DefaultIfEmpty(null).First(),
                                 Shift = report.GLOBALSHIFT,
                             }
                             into groupedReports
                             select new RKReport
                             {
                                 ClassficatorGroup = new Element { Code = groupedReports.Key.ClassficatorGroup.CODE ?? -1, Name = RemoveInvalidXmlChars(groupedReports.Key.ClassficatorGroup.NAME) },
                                 Restaurant = new Element { Code = AddPrefix(groupedReports.Key.Restaurant.CODE ?? -1, settings.databasePrefix), Name = RemoveInvalidXmlChars(groupedReports.Key.Restaurant.NAME) },
                                 CurrencyType = new Element { Code = groupedReports.Key.CurrencyType.CODE ?? -1, Name = RemoveInvalidXmlChars(groupedReports.Key.CurrencyType.NAME) },
                                 Currency = new Element { Code = groupedReports.Key.Currency.CODE ?? -1, Name = RemoveInvalidXmlChars(groupedReports.Key.Currency.NAME) },
                                 DiscountType = new Element { Code = groupedReports.Key.DiscountType.code, Name = RemoveInvalidXmlChars(groupedReports.Key.DiscountType.name) },

                                 Visit = new Element { Code = groupedReports.Key.Visit.SIFR, Name = groupedReports.Key.Visit.STARTTIME.ToString() },
                                 MenuItem = new Element { Code = groupedReports.Key.MenuItem.CODE ?? 0, Name = RemoveInvalidXmlChars(groupedReports.Key.MenuItem.NAME) },

                                 Sum = groupedReports.Sum(x => x.PAYBINDING.PAYSUM ?? 0),
                                 PaySum = groupedReports.Sum(x => x.PAYBINDING.PRICESUM ?? 0),
                                 DiscountSum = 0,
                                 Quntity = groupedReports.Sum(x => x.SALEOBJECT.QUANTITY ?? 0),

                                 VisitQuitTime = groupedReports.Key.GlobalShiftStartTime,

                                 Table = new TableRepresent { Code = groupedReports.Key.Table?.CODE ?? -1, Name = RemoveInvalidXmlChars(groupedReports.Key.Table?.NAME ?? "Без стола"), Hall = groupedReports.Key.Table?.HALL ?? -1 },

                                 Shift = new Element { Code = groupedReports.Key.Shift.SHIFTNUM, Name = RemoveInvalidXmlChars($"{groupedReports.Key.Shift.STARTTIME}")}
                             };

                var discountSIFR = new List<int>();
                foreach (var report in reports)
                    foreach (var discount in report.LEFTJOINDISCPARTS)
                        discountSIFR.Add(discount.SIFR ?? -1);

                var discountObjects = (from DISCOUNT in db.DISCOUNTS
                                      where discountSIFR.Contains(DISCOUNT.SIFR)
                                      select DISCOUNT).ToList();

                var discounts = new List<RKReport>();
                foreach (var report in reports)
                {
                    foreach (var discpart in report.LEFTJOINDISCPARTS)
                    {
                        var discount = discountObjects.FirstOrDefault(discountLambda => discountLambda.SIFR == discpart.SIFR);
                        var currentTable = report.TABLES.DefaultIfEmpty(null).First();
                        var table = currentTable == null ?
                                                new { code = -1, name = "Без стола", hall = -1 } :
                                                new { code = currentTable?.CODE ?? -1, name = currentTable?.NAME ?? "Без стола", hall = currentTable?.HALL ?? -1 };
                        var currentRKReport = new RKReport
                        {
                            ClassficatorGroup = new Element { Code = report.CLASSIFICATORGROUP.CODE ?? -1, Name = RemoveInvalidXmlChars(report.CLASSIFICATORGROUP.NAME) },
                            Restaurant = new Element { Code = AddPrefix(report.RESTAURANT.CODE ?? -1, settings.databasePrefix), Name = RemoveInvalidXmlChars(report.RESTAURANT.NAME) },
                            CurrencyType = new Element { Code = report.CURRENCYTYPE.CODE ?? -1, Name = RemoveInvalidXmlChars(report.CURRENCYTYPE.NAME) },
                            Currency = new Element { Code = report.CURRENCY.CODE ?? -1, Name = RemoveInvalidXmlChars(report.CURRENCY.NAME) },
                            DiscountType = new Element { Code = discount.SIFR, Name = RemoveInvalidXmlChars(discount.NAME) },

                            Visit = new Element { Code = report.VISIT.SIFR, Name = report.VISIT.STARTTIME.ToString() },
                            MenuItem = new Element { Code = report.MENUITEM.CODE ?? 0, Name = RemoveInvalidXmlChars(report.MENUITEM.NAME) },

                            Sum = 0,
                            PaySum = 0,
                            DiscountSum = -discpart.SUM ?? 0,
                            Quntity = 0,

                            VisitQuitTime = report.GLOBALSHIFT.SHIFTDATE.Value.AbsoluteStart(),

                            Table = new TableRepresent { Code = table.code, Name = RemoveInvalidXmlChars(table.name), Hall = table.hall }
                        };
                        discounts.Add(currentRKReport);
                    }
                }

                var result = groups.Union(discounts);

                return result.OrderBy(x => x.Restaurant.Code).ThenBy(x => x.VisitQuitTime);
            }
        }
    }

    public partial class RkeeperReporter
    {
        public bool ExecuteProcess()
        {
            var settings = GetSettings(typeof(Settings.RkeeperReporterSettings)) as Settings.RkeeperReporterSettings;
            var currentSettings = new RkeeperLoader.RkeeperLoaderSettings()
            {
                ConnectionString = settings.ConnectionString,
                ProviderName = settings.ProviderName,
                ClassificationGroupSIFR = settings.ClassificationGroupSIFR,
                ReportEndDateTime = settings.ReportEndDateTime,
                ReportStartDateTime = settings.ReportStartDateTime,
                restaurantCode = settings.restaurantCode,
                UploadLastDays = settings.UploadLastDays
            };
            var groups = RkeeperLoader.GetReports(currentSettings).ToArray();

            var wsWrapper = new EnterpriseWsWrapper(settings);

            var errorInfo = "";
            var reportId = Guid.NewGuid().ToString();
            var result = true;

            for (int i = 0; i <= groups.Count(); i += settings.SendRecordsCount)
            {

                var reports = new Reports()
                {
                    Report = groups.Skip(i).Take(settings.SendRecordsCount).ToArray(),
                    DB = new Element() { Code = settings.databaseCode, Name = settings.databaseName }
                };

                var currentResult = wsWrapper.SendData(reports,
                                                        reportId,
                                                        ref errorInfo);
                if (!currentResult)
                    CreateLogRecord(errorInfo, System.Diagnostics.EventLogEntryType.Error);

                result = result & currentResult;
            }

            if (result)
                CreateLogRecord("Upload successfully finished");
            else
                CreateLogRecord("Upload finished with errors");

            return result;
        }
    }

    public partial class RkeeperReporter : TaskBase
    {
        public override ExecutionDelegate ExecutuionMethod => ExecuteProcess;
        public override IEnumerable<Type> TaskSettings => new[] { typeof(Settings.RkeeperReporterSettings) };
    }
}
