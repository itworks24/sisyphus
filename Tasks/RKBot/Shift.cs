using System;
using System.Collections.Generic;

namespace Sysiphus.Tasks.RKBot
{
    public class Shift
    {
        public DateTime CreateTime { get; internal set; }
        public int ShiftNum { get; internal set; }
        internal Restaurant Restaurant { get; set; }
        internal List<Visit> Visits { get; set; }
        internal List<Waiter> Waiters { get; set; }
    }
}