using System;
using System.Collections.Generic;
using Sisyphus.Settings;
using Settings;
using Sisyphus.Mail;

namespace Sisyphus
{
    class Core : Tasks.TaskBase
    {
        public new string TaskName => "Core";
        public override ExecutionDelegate ExecutuionMethod => ExecuteProcess;

        private bool ExecuteProcess()
        {
            CreateLogRecord("Sending report.");
            ReportMailSender.SendLog(this);
            return true;
        }

        public override IEnumerable<Type> TaskSettings => new[] { typeof(ServiceSettings), typeof(ReportMailSettings), };
    }
}
