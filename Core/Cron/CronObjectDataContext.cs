using System;
using System.Collections.Generic;

namespace Rlc.Cron
{
	public class CronObjectDataContext
	{
		public object Object { get; set; }
		public DateTime LastTrigger { get; set; }
		public List<CronSchedule> CronSchedules { get; set; }
	}
}