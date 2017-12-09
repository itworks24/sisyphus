using System;
using System.Collections.Generic;
using Sisyphus.Tasks;
using System.Linq;
using System.Data.Entity.Core.EntityClient;
using RKeeperReporter.RKeeperExchange;
using RKeeperReporter.Database;
using Sisyphus;

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

    public partial class RkeeperReporter
    {

        internal static string GetEntityConnection(Settings.RkeeperReporterSettings settings)
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

        IEnumerable<Report> GetReports(Settings.RkeeperReporterSettings settings)
        {
            using (var db = new RKeeperEntities(GetEntityConnection(settings)))
            {
                db.Database.CommandTimeout = 1800;

                var startDateTime = settings.UploadLastDays > 0 ? DateTime.Now.AddDays(-settings.UploadLastDays) : settings.ReportStartDateTime;
                var endDateTime = settings.UploadLastDays > 0 ? DateTime.Now : settings.ReportEndDateTime;

                startDateTime = startDateTime.AbsoluteStart();
                endDateTime = endDateTime.AbsoluteEnd();

                var classificationGroupSIFR = settings.ClassificationGroupSIFR;

                var restaurantCode = settings.restaurantCode;

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
                               join GLOBALSHIFT in db.GLOBALSHIFTS
                                     on new { shift = ORDER.ICOMMONSHIFT ?? -1, midServer = ORDER.MIDSERVER }
                                          equals new { shift = GLOBALSHIFT.SHIFTNUM, midServer = GLOBALSHIFT.MIDSERVER }
                               join SALEOBJECT in db.SaleObjects
                                     on new { visit = PAYBINDING.VISIT, midServer = PAYBINDING.MIDSERVER, dishUNI = PAYBINDING.DISHUNI ?? -1, chargeUNI = PAYBINDING.CHARGEUNI ?? -1 }
                                         equals new { visit = SALEOBJECT.Visit, midServer = SALEOBJECT.MidServer, dishUNI = SALEOBJECT.DishUNI, chargeUNI = SALEOBJECT.ChargeUNI }
                               join SESSIONDISH in db.SESSIONDISHES
                                     on new { visit = PAYBINDING.VISIT, midServer = PAYBINDING.MIDSERVER, dishUNI = SALEOBJECT.DishUNI }
                                         equals new { visit = SESSIONDISH.VISIT, midServer = SESSIONDISH.MIDSERVER, dishUNI = SESSIONDISH.UNI }
                               join MENUITEM in db.MENUITEMS
                                     on SESSIONDISH.SIFR equals MENUITEM.SIFR
                               join DISHGROUP in db.DISHGROUPS
                                     on MENUITEM.SIFR equals DISHGROUP.CHILD
                               join CLASSIFICATORGROUP in db.CLASSIFICATORGROUPS
                                     on new { classoficatorId = DISHGROUP.PARENT ?? -1, ClassificationGroupSIFR = classificationGroupSIFR }
                                          equals new { classoficatorId = CLASSIFICATORGROUP.SIFR * 256 + CLASSIFICATORGROUP.NUMINGROUP, ClassificationGroupSIFR = classificationGroupSIFR == 0 ? (short)0 : CLASSIFICATORGROUP.SIFR }
                               join DISCPART in db.DISCPARTS
                                     on new { visit = SALEOBJECT.Visit, MidServer = SALEOBJECT.MidServer, bindingUNI = PAYBINDING.UNI }
                                         equals new { visit = DISCPART.VISIT, MidServer = DISCPART.MIDSERVER, bindingUNI = DISCPART.BINDINGUNI } into LEFTJOINDISCPARTS
                               from LEFTJOINDISCPART in LEFTJOINDISCPARTS.DefaultIfEmpty().Take(1)
                               join DISCOUNT in db.DISCOUNTS
                                     on LEFTJOINDISCPART.SIFR equals DISCOUNT.SIFR into LEFTJOINDISCOUNTS
                               from LEFTJOINDISCOUNT in LEFTJOINDISCOUNTS.DefaultIfEmpty()
                               where GLOBALSHIFT.STARTTIME.Value >= startDateTime && GLOBALSHIFT.STARTTIME.Value < endDateTime
                                     && (restaurantCode == 0 || RESTAURANT.CODE == restaurantCode)
                                     && (PRINTCHECK.STATE == 6)
                               select new { CLASSIFICATORGROUP, RESTAURANT, CURRENCYTYPE, CURRENCY, PAYBINDING, VISIT, GLOBALSHIFT, LEFTJOINDISCOUNT, MENUITEM, SALEOBJECT })
                               .ToList();
                var groups = from report in reports
                             group report by new
                             {
                                 ClassficatorGroup = report.CLASSIFICATORGROUP,
                                 Restaurant = report.RESTAURANT,
                                 CurrencyType = report.CURRENCYTYPE,
                                 Currency = report.CURRENCY,
                                 DiscountType = report.LEFTJOINDISCOUNT == null ?
                                                    new { code = 1, name = "Без скидки" } :
                                                    new { code = report.LEFTJOINDISCOUNT.SIFR, name = report.LEFTJOINDISCOUNT.NAME },
                                 MenuItem = report.MENUITEM,
                                 Visit = report.VISIT,
                                 GlobalShiftStartTime = report.GLOBALSHIFT.STARTTIME.Value.AbsoluteStart(),
                                 SaleObject = report.SALEOBJECT
                             }
                             into groupedReports
                             select new Report
                             {
                                 ClassficatorGroup = new Element { Code = groupedReports.Key.ClassficatorGroup.CODE ?? -1, Name = groupedReports.Key.ClassficatorGroup.NAME },
                                 Restaurant = new Element { Code = groupedReports.Key.Restaurant.CODE ?? -1, Name = groupedReports.Key.Restaurant.NAME },
                                 CurrencyType = new Element { Code = groupedReports.Key.CurrencyType.CODE ?? -1, Name = groupedReports.Key.CurrencyType.NAME },
                                 Currency = new Element { Code = groupedReports.Key.Currency.CODE ?? -1, Name = groupedReports.Key.Currency.NAME },
                                 DiscountType = new Element { Code = groupedReports.Key.DiscountType.code, Name = groupedReports.Key.DiscountType.name },
                                 //Visit = new Element { Code = 0, Name = "" },
                                 Visit = new Element { Code = groupedReports.Key.Visit.SIFR, Name = groupedReports.Key.Visit.STARTTIME.ToString() },
                                 //MenuItem = new Element { Code = 0, Name = "" },
                                 MenuItem = new Element { Code = groupedReports.Key.MenuItem.CODE ?? 0, Name = groupedReports.Key.MenuItem.NAME },

                                 Sum = groupedReports.Sum(x => x.PAYBINDING.PAYSUM ?? 0),
                                 PaySum = groupedReports.Sum(x => x.PAYBINDING.PRICESUM ?? 0),
                                 DiscountSum = -groupedReports.Sum(x => x.PAYBINDING.DISTRDISCOUNTS ?? 0),
                                 Quntity = groupedReports.Sum(x => x.SALEOBJECT.Quantity ?? 0),

                                 VisitQuitTime = groupedReports.Key.GlobalShiftStartTime
                             };

                return groups.OrderBy(x => x.Restaurant.Code).ThenBy(x => x.VisitQuitTime);
            }
        }

        public bool ExecuteProcess()
        {
            var settings = GetSettings(typeof(Settings.RkeeperReporterSettings)) as Settings.RkeeperReporterSettings;
            var groups = GetReports(settings).ToArray();

            var wsWrapper = new EnterpriseWsWrapper(settings);

            var errorInfo = "";
            var reportId = Guid.NewGuid().ToString();
            var result = true;

            for (int i = 0; i <= groups.Count(); i += settings.SendRecordsCount)
            {
                var currentResult = wsWrapper.SendData(groups.Skip(i).Take(settings.SendRecordsCount).ToArray(),
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
