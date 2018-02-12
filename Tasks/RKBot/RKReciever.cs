using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;
using LiteDB;
using MongoDB.Bson;
using Newtonsoft.Json;
using Sisyphus.Tasks;
using Sysiphus.Tasks.RKBot.MongoClasses;
using Sysiphus.Tasks.RKBot.Settings;

namespace Sysiphus.Tasks.RKBot
{
    public partial class RKReciever
    {

        public bool ExecuteProcess()
        {
            var settings = GetSettings(typeof(RKRecieverSettings)) as RKRecieverSettings;
            var rkInterface = new RKInterface();
            rkInterface.SetCLientSettings(settings.RK7ApiAddress, settings.RK7ApiUser, settings.RK7ApiPassword);

            var getSystemInfo2 = rkInterface.GetSystemInfo2();
            var getWaiterListResult = rkInterface.GetWaiterList(true);
            var getOrderListResult = rkInterface.GetOrderList();

            var restaurant = new Restaurant
            {
                Id = getSystemInfo2.SystemInfo.Restaurant.id,
                Name = getSystemInfo2.SystemInfo.Restaurant.AnyAttr[0].Value
            };

            var waiters = getWaiterListResult.Waiters.Select(waiter =>
            {
                var firstOrDefault = getOrderListResult.Visit.Select(
                        visit =>
                            visit.Orders.Select(order => new { name = order.WaiterName, code = order.WaiterCode })
                                .FirstOrDefault())
                    .FirstOrDefault(t => t != null && t.code == waiter.Code);
                return new Waiter
                {
                    Code = waiter.Code,
                    Id = int.Parse(waiter.ID),
                    Name = (firstOrDefault ?? new { name = "", code = "" }).name

                };
            }).ToList();

            var parseValue = 0;

            var visits = getOrderListResult.Visit.Select(visit => new Visit
            {
                Id = int.Parse(visit.VisitID),
                GuestCount = int.Parse(visit.GuestsCount),
                Orders = visit.Orders.Select(order => new Order()
                {
                    Id = int.TryParse(order.OrderName.Replace("/", ""), out parseValue) ? parseValue : 0,
                    BillTime = order.BillTime,
                    CreateTime = order.CreateTime,
                    Dessert = order.Dessert,
                    Finished = order.Finished,
                    FinishTime = order.FinishTime,
                    OrderName = order.OrderName,
                    OrderSum = int.Parse(order.OrderSum),
                    Table = new Table()
                    {
                        Code = order.TableCode,
                        Id = int.TryParse(order.TableID, out parseValue) ? parseValue : 0,
                        Name = order.TableName
                    },
                    Version = int.TryParse(order.Version, out parseValue) ? parseValue : 0
                }).ToList()
            }).ToList();

            var shift = new Shift
            {
                ShiftNum = getSystemInfo2.SystemInfo.CommonShift.ShiftNum,
                CreateTime = getSystemInfo2.SystemInfo.CommonShift.ShiftStartTime,
                Restaurant = restaurant,
                Waiters = waiters,
                Visits = visits
            };

            CreateLogRecord(shift.ToJson());

            var mongoDB = new MongoDBInteface(settings.MongoConnectionString);
            mongoDB.SaveShift(shift);
            return true;
        }
    }

    public partial class RKReciever : TaskBase
    {
        public override ExecutionDelegate ExecutuionMethod => ExecuteProcess;
        public override IEnumerable<Type> TaskSettings => new[] { typeof(Settings.RKRecieverSettings) };
    }
}
