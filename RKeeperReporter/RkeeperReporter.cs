using System;
using System.Collections.Generic;
using System.Threading;
using Sisyphus.Tasks;

namespace Sysiphus.Tasks
{
    public partial class RkeeperReporter
    {
        public bool ExecuteProcess()
        {
            var settings = GetSettings(typeof(Settings.RkeeperReporterSettings)) as Settings.RkeeperReporterSettings;
            Thread.Sleep((settings.WaitTime > 10 ? 10 : settings.WaitTime) * 1000);

            if (settings.AddErrorInformationLogRecord) CreateLogRecord($"Executed!\nSampleTaskSettings.SampleProperty: {settings.SampleProperty}");
            if (settings.AddErrorLogRecord) CreateLogRecord($"This is error report!", System.Diagnostics.EventLogEntryType.Error);

            return true;

        }
    }

    public partial class RkeeperReporter : TaskBase
    {
        public override ExecutionDelegate ExecutuionMethod => ExecuteProcess;
        public override IEnumerable<Type> TaskSettings => new[] { typeof(Settings.RkeeperReporterSettings) };
    }
}
