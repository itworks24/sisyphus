using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Serialization;
using LiteDB;
using MongoDB.Bson;
using Newtonsoft.Json;
using Sisyphus.Tasks;
using Sysiphus.Tasks.RKBot.Settings;

namespace Sysiphus.Tasks.RKBot
{
    public partial class RKReciever
    {

        public bool ExecuteProcess()
        {
            var settings = GetSettings(typeof(RKRecieverSettings)) as Settings.RKRecieverSettings;
            var rkInterface = new RKInterface();
            rkInterface.SetCLientSettings(settings.RK7ApiAddress, settings.RK7ApiUser, settings.RK7ApiPassword);
            var getWaiterListResult = rkInterface.GetWaiterList(true);
            CreateLogRecord(JsonConvert.SerializeObject(getWaiterListResult, Formatting.Indented));
            var getOrderListResult = rkInterface.GetOrderList();
            CreateLogRecord(JsonConvert.SerializeObject(getOrderListResult, Formatting.Indented));
            var getSystemInfo2 = rkInterface.GetSystemInfo2();
            CreateLogRecord(JsonConvert.SerializeObject(getSystemInfo2, Formatting.Indented));
            return true;
        }
    }

    public partial class RKReciever : TaskBase
    {
        public override ExecutionDelegate ExecutuionMethod => ExecuteProcess;
        public override IEnumerable<Type> TaskSettings => new[] { typeof(Settings.RKRecieverSettings) };
    }
}
