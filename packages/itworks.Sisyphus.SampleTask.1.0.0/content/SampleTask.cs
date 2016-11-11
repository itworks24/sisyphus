using System;
using System.Collections.Generic;
using System.Threading;

namespace SampleTask
{
    public partial class  SampleTask
    {
        public bool ExecuteProcess()
        {
            var settings = this.GetSettings(typeof(SampleTaskSettings)) as SampleTaskSettings;
            Thread.Sleep((settings.WaitTime > 10 ? 10 : settings.WaitTime) * 1000);
            CreateLogRecord($"Executed!\nSampleTaskSettings.SampleProperty: {settings.SampleProperty}");
            return true;
            //test
        }
    }

    public partial  class SampleTask : Sisyphus.Tasks.TaskBase
    {
        public override string TaskName => "SampleTask";
        public override string Version => "1.0.0";

        public override ExecutionDelegate ExecutuionMethod => ExecuteProcess;
        public override IEnumerable<Type> TaskSettings => new[] { typeof(SampleTaskSettings) };
    }
}
