using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Sisyphus.Tasks;

namespace Sisyphus.Settings
{
    public class GroupSettings
    {
        public string TaskName { get; private set; }
        public string GroupName { get; private set; }
        public bool RunMultipleThreads { get; set; }
        public int StopAfter { get; set; }

        [DisplayName(@"Enable")]
        public bool Enable { get; set; }

        [DisplayName(@"Sheduler in Cron-like pattern format (* * * * *) (minutes hours days months days_of_week)")]
        public string Shedule { get; set; }

        [Category("Reports")]
        [DisplayName(@"Send reports")]
        public bool Report { get; set; }

        [Category("Reports")]
        [DisplayName(@"Send report no more than (minutes)")]
        public int SendReportInterval { get; set; }

        [Category("Reports")]
        [DisplayName(@"Report information level")]
        public EventLogEntryType ReportInformationLevel { get; set; }

        [Category("Behavior")]
        [DisplayName(@"Repeate X times if error")]
        public uint MaxErrorCount { get; set; } = 0;

        private GroupSettings(string taskName, string groupName)
        {
            TaskName = taskName;
            GroupName = groupName;
        }

        public void SaveSettings()
        {
            var currentSettings = new ServiceSettings("");
            currentSettings.SetPropertyValue(ServiceSettings.GetSettingNameEnable(TaskName, GroupName), Enable);
            currentSettings.SetPropertyValue(ServiceSettings.GetSettingNameShedule(TaskName, GroupName), Shedule);
            currentSettings.SetPropertyValue(ServiceSettings.GetSettingNameReport(TaskName, GroupName), Report);
            currentSettings.SetPropertyValue(ServiceSettings.GetSettingNameRunMultipleThreads(TaskName, GroupName), RunMultipleThreads);
            currentSettings.SetPropertyValue(ServiceSettings.GetSettingNameStopAfter(TaskName, GroupName), StopAfter);
            currentSettings.SetPropertyValue(ServiceSettings.GetSettingNameRunMultipleThreads(TaskName, GroupName), RunMultipleThreads);
            currentSettings.SetPropertyValue(ServiceSettings.GetSettingNameSendReportInterval(TaskName, GroupName), SendReportInterval);
            currentSettings.SetPropertyValue(ServiceSettings.GetSettingNameReportInformationLevel(TaskName, GroupName), ReportInformationLevel);
            currentSettings.SetPropertyValue(ServiceSettings.GetSettingNameMaxErrorCount(TaskName, GroupName), MaxErrorCount);
            currentSettings.SetSettings();
        }

        public static GroupSettings GetGroupSettings(TaskBase task, string groupName)
        {
            var currentSettings = new ServiceSettings("");
            var result = new GroupSettings(task.TaskName, groupName)
            {
                Enable =
                    (bool)
                        currentSettings.GetPropertyValue(ServiceSettings.GetSettingNameEnable(task, groupName),
                            typeof(bool)),
                Shedule =
                    (string)
                        currentSettings.GetPropertyValue(ServiceSettings.GetSettingNameShedule(task, groupName),
                            typeof(string)),
                Report =
                    (bool)
                        currentSettings.GetPropertyValue(ServiceSettings.GetSettingNameReport(task, groupName),
                            typeof(bool)),
                RunMultipleThreads =
                    (bool)
                        currentSettings.GetPropertyValue(
                            ServiceSettings.GetSettingNameRunMultipleThreads(task, groupName), typeof(bool)),
                StopAfter =
                    (int)
                        currentSettings.GetPropertyValue(ServiceSettings.GetSettingNameStopAfter(task, groupName),
                            typeof(int)),
                SendReportInterval =
                    (int)
                        currentSettings.GetPropertyValue(
                            ServiceSettings.GetSettingNameSendReportInterval(task, groupName), typeof(int)),
                ReportInformationLevel =
                    (EventLogEntryType)
                        currentSettings.GetPropertyValue(
                            ServiceSettings.GetSettingNameReportInformationLevel(task, groupName),
                            typeof(EventLogEntryType)),
                MaxErrorCount = (uint)
                                    currentSettings.GetPropertyValue(
                                          ServiceSettings.GetSettingNameMaxErrorCount(task, groupName),
                                          typeof(uint)),
            };

            if (result.Shedule == string.Empty || result.Shedule == currentSettings.EmptyValue)
                result.Shedule = "0 0 * * *";

            if (result.ReportInformationLevel == 0)
                result.ReportInformationLevel = EventLogEntryType.Error;

            return result;
        }
    }

    public class ServiceSettings : SettingsRepresent
    {
        private Dictionary<string, Type> _extFields;

        public static string GetSettingNameEnable(TaskBase task, string groupName) { return GetSettingNameEnable(task.TaskName, groupName); }
        public static string GetSettingNameEnable(string taskName, string groupName) { return $"{taskName}.{groupName}Enable"; }
        public static string GetSettingNameShedule(TaskBase task, string groupName) { return GetSettingNameShedule(task.TaskName, groupName); }
        public static string GetSettingNameShedule(string taskName, string groupName) { return $"{taskName}.{groupName}Shedule"; }
        public static string GetSettingNameReport(TaskBase task, string groupName) { return GetSettingNameReport(task.TaskName, groupName); }
        public static string GetSettingNameReport(string taskName, string groupName) { return $"{taskName}.{groupName}Report"; }
        public static string GetSettingNameRunMultipleThreads(TaskBase task, string groupName) { return GetSettingNameRunMultipleThreads(task.TaskName, groupName); }
        public static string GetSettingNameRunMultipleThreads(string taskName, string groupName) { return $"{taskName}.{groupName}RunMultipleThreads"; }
        public static string GetSettingNameStopAfter(TaskBase task, string groupName) { return GetSettingNameStopAfter(task.TaskName, groupName); }
        public static string GetSettingNameStopAfter(string taskName, string groupName) { return $"{taskName}.{groupName}StopAfter"; }
        public static string GetSettingNameGroupName(TaskBase task, string groupName) { return GetSettingNameGroupName(task.TaskName, groupName); }
        public static string GetSettingNameGroupName(string taskName, string groupName) { return $"{taskName}.{groupName}GroupName"; }
        public static string GetSettingNameSendReportInterval(TaskBase task, string groupName) { return GetSettingNameSendReportInterval(task.TaskName, groupName); }
        public static string GetSettingNameSendReportInterval(string taskName, string groupName) { return $"{taskName}.{groupName}SendReportInterval"; }
        public static string GetSettingNameReportInformationLevel(TaskBase task, string groupName) { return GetSettingNameReportInformationLevel(task.TaskName, groupName); }
        public static string GetSettingNameReportInformationLevel(string taskName, string groupName) { return $"{taskName}.{groupName}ReportInformationLevel"; }
        public static string GetSettingNameMaxErrorCount(TaskBase task, string groupName) { return GetSettingNameMaxErrorCount(task.TaskName, groupName); }
        public static string GetSettingNameMaxErrorCount(string taskName, string groupName) { return $"{taskName}.{groupName}MaxErrorCount"; }

        protected override Dictionary<string, Type> ExtFields
        {
            get
            {
                if (_extFields != null) return _extFields;
                _extFields = new Dictionary<string, Type>();
                foreach (var type in TasksEnumerator.GetTaskEnumertor())
                {
                    var task = Activator.CreateInstance(type) as TaskBase;
                    var groupsCollection = new SettingsGroupsCollection(task.TaskSettings);
                    foreach (var group in groupsCollection)
                    {
                        _extFields.Add(GetSettingNameEnable(task, group), typeof(bool));
                        _extFields.Add(GetSettingNameShedule(task, group), typeof(string));
                        _extFields.Add(GetSettingNameReport(task, group), typeof(bool));
                        _extFields.Add(GetSettingNameRunMultipleThreads(task, group), typeof(bool));
                        _extFields.Add(GetSettingNameStopAfter(task, group), typeof(int));
                        _extFields.Add(GetSettingNameGroupName(task, group), typeof(string));
                        _extFields.Add(GetSettingNameSendReportInterval(task, group), typeof(int));
                        _extFields.Add(GetSettingNameReportInformationLevel(task, group), typeof(EventLogEntryType));
                        _extFields.Add(GetSettingNameMaxErrorCount(task, group), typeof(EventLogEntryType));
                    }
                }
                return _extFields;
            }
        }

        public bool EnableScheduler { get; set; } = true;
        public bool EnableLogging { get; set; } = true;

        public ServiceSettings(string groupName) : base(groupName)
        {
        }
    }
}
