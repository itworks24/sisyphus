using System;
using System.Collections.Generic;
using System.Threading;
using Sisyphus.Tasks;

namespace Sysiphus.Tasks
{
    public partial class SampleTask
    {
        public bool ExecuteProcess()
        {
            var settings = GetSettings(typeof(Settings.SampleTaskSettings)) as Settings.SampleTaskSettings;
            Thread.Sleep((settings.WaitTime > 10 ? 10 : settings.WaitTime) * 1000);

            if (settings.AddErrorInformationLogRecord) CreateLogRecord($"Executed!\nSampleTaskSettings.SampleProperty: {settings.SampleProperty}");
            if (settings.AddErrorLogRecord) CreateLogRecord($"This is error report!", System.Diagnostics.EventLogEntryType.Error);

            return true;

        }
    }

    public partial class SampleTask : TaskBase
    {
        public override ExecutionDelegate ExecutuionMethod => ExecuteProcess;
        public override IEnumerable<Type> TaskSettings => new[] { typeof(Settings.SampleTaskSettings) };
    }
}
