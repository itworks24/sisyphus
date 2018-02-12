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

            var shift = new Shift
            {
                ShiftNum = getSystemInfo2.SystemInfo.CommonShift.ShiftNum,
                CreateTime = getSystemInfo2.SystemInfo.CommonShift.ShiftStartTime,
                Restaurant = new Restaurant
                {
                    Id = new MongoDB.Bson.ObjectId(getSystemInfo2.SystemInfo.Restaurant.id),
                    Name = getSystemInfo2.SystemInfo.Restaurant.AnyAttr[0].Value
                },
                Waiters = getWaiterListResult.Waiters.Select(waiter => new Waiter()
                {
                    Code = waiter.Code,
                    Id = new MongoDB.Bson.ObjectId(waiter.ID),
                    Name = getOrderListResult.Visit.Select(visit => visit.Orders.Select(order => new { name = order.WaiterName, code = order.WaiterCode }).FirstOrDefault())
                                                   .FirstOrDefault(t => t.code == waiter.Code).name,
                }).ToList(),
                Visits = getOrderListResult.Visit.Select(visit => new Visit()
                {
                    Id = new MongoDB.Bson.ObjectId(visit.VisitID),
                    GuestCount = int.Parse(visit.GuestsCount),
                    Orders = visit.Orders.Select(order => new Order()
                    {
                        Id = new MongoDB.Bson.ObjectId(order.OrderID),
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
                            Id = new MongoDB.Bson.ObjectId(order.TableID),
                            Name = order.TableName
                        },
                        Version = int.Parse(order.Version)
                    }).ToList()
                }).ToList()
            };

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
