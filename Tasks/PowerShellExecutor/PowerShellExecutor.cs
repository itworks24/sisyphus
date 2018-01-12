using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;
using System.Threading;
using Sisyphus.Tasks;
using Sisyphus.Tasks.Settings;
using System.Linq;

namespace Sisyphus.Tasks
{
    public partial class PowershellExecutor
    {

        public bool ExecuteProcess()
        {
            var settings = GetSettings(typeof(PowershellExecutorSettings)) as Settings.PowershellExecutorSettings;
            using (var PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.Runspace.SessionStateProxy.Path.SetLocation(settings.WorkDirectory);
                if (!string.IsNullOrEmpty(settings.ScriptPath))
                {
                    var script = File.ReadAllText(settings.ScriptPath);
                    PowerShellInstance.AddScript(script);
                }
                else
                    PowerShellInstance.AddCommand(settings.SingleCommand);

                if (!string.IsNullOrEmpty(settings.Arguments))
                    PowerShellInstance.AddArgument(settings.Arguments);

                var result = PowerShellInstance.Invoke();
                result.ToList().ForEach(t => CreateLogRecord(t.ToString()));

                foreach (var verbose in PowerShellInstance.Streams.Verbose)
                    CreateLogRecord(verbose.Message);

                foreach (var error in PowerShellInstance.Streams.Error)
                    CreateLogRecord(error.Exception.Message, System.Diagnostics.EventLogEntryType.Error);

                return PowerShellInstance.Streams.Error.Count == 0;
            }
        }
    }

    public partial class PowershellExecutor : TaskBase
    {
        public override ExecutionDelegate ExecutuionMethod => ExecuteProcess;
        public override IEnumerable<Type> TaskSettings => new[] { typeof(PowershellExecutorSettings) };
    }
}
