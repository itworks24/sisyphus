using System;
using System.Collections.Generic;
using System.Threading;
using Sisyphus.Tasks;

namespace SampleTask
{
    public partial class SampleTask
    {
        public bool ExecuteProcess()
        {
            var settings = this.GetSettings(typeof(SampleTaskSettings)) as SampleTaskSettings;
            Thread.Sleep((settings.WaitTime > 10 ? 10 : settings.WaitTime) * 1000);

            if (settings.AddErrorInformationLogRecord) CreateLogRecord($"Executed!\nSampleTaskSettings.SampleProperty: {settings.SampleProperty}", System.Diagnostics.EventLogEntryType.Information);
            if (settings.AddErrorLogRecord) CreateLogRecord($"This is error report!", System.Diagnostics.EventLogEntryType.Error);

            return true;

        }
    }

    public partial class SampleTask : TaskBase
    {
        public override ExecutionDelegate ExecutuionMethod => ExecuteProcess;
        public override IEnumerable<Type> TaskSettings => new[] { typeof(SampleTaskSettings) };
    }
}
