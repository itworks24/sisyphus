using System;

namespace Sysiphus.Tasks.RKBot
{
    internal class Order
    {
        public Order()
        {
        }

        public DateTime BillTime { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Dessert { get; set; }
        public bool Finished { get; set; }
        public DateTime FinishTime { get; set; }
        public int Id { get; set; }
        public string OrderName { get; set; }
        public int OrderSum { get; set; }
        public object Table { get; set; }
        public int Version { get; set; }
    }
}