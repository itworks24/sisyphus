using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Sisyphus.Mail;
using Sisyphus.Settings;

namespace Sisyphus.Tasks
{
    public abstract class TaskBase : Logging.TaskLogging
    {
        public string TaskName => GetType().Name;

        public string Version => FileVersionInfo.GetVersionInfo(GetType().Assembly.Location).FileVersion;

        public delegate bool ExecutionDelegate();
        public abstract ExecutionDelegate ExecutuionMethod { get; }

        public abstract IEnumerable<Type> TaskSettings { get; }

        private string GroupName { get; set; }

        private SettingsGroupRepresentDictionary SettingsGroupDict { get; set; }

        protected object GetSettings(Type settingsType)
        {
            object setting;
            return SettingsGroupDict.TryGetValue(settingsType.Name, out setting) ? setting : Activator.CreateInstance(settingsType);
        }

        public void ExecuteTask(string groupName = "", bool createEmailReport = true, int sendReportIntervalInSeconds = 0, EventLogEntryType reportInformationLevel = EventLogEntryType.Error, uint maxErrorCount = 0)
        {
            var errorAccured = false;
            GroupName = groupName;
            SettingsGroupDict = new SettingsGroupRepresentDictionary(GroupName, TaskSettings);
            var lastErrorLogsCount = this.Count(t => t.IsErrorMessage);
            while (maxErrorCount + 1 > 0)
            {
                try
                {
                    ExecutuionMethod();
                    if (lastErrorLogsCount == this.Count(t => t.IsErrorMessage)) break;
                }
                catch (Exception e)
                {
                    CreateLogRecord(e);
                    errorAccured = true;
                }
                lastErrorLogsCount = this.Count(t => t.IsErrorMessage);
                maxErrorCount--;
            }
            if (!createEmailReport)
                return;
            {
                try
                {
                    var messageGroup = Logging.SqlLogger.AddLogRecords(this, $"{TaskName}.{GroupName}");
                    Logging.SqlLogger.ExcludeUnnecessaryLogGroups(messageGroup, reportInformationLevel);
                }
                catch (Exception e)
                {
                    ReportMailSender.SendLog(this);
                    CreateLogRecord(e);
                    return;
                }
                try
                {
                    Logging.SqlLogger.SendLogRecords($"{TaskName}.{GroupName}", sendReportIntervalInSeconds, reportInformationLevel);
                }
                catch (Exception e)
                {
                    CreateLogRecord(e);
                }
            }
        }
    }
}