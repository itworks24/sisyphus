using Rlc.Cron;
using System;
using System.Collections.Generic;
using Sisyphus.Settings;
using Sisyphus.Tasks;

namespace Sisyphus
{
    partial class Scheduler
    {
        private List<CronObject> _cronObjectList;
        private ServiceSettings _serviceSettings;
        private Logging.TaskLogging _serviceLogger;

        private struct CronObjectHelper
        {
            internal Type TaskType;
            internal string GroupName;
        }

        private CronObject CreateCronObject(Type newTaskType, string groupName, string cronExpression)
        {
            var cronSchedule = CronSchedule.Parse(cronExpression);
            var cronSchedules = new List<CronSchedule> { cronSchedule };

            var dc = new CronObjectDataContext
            {
                Object = new CronObjectHelper() { TaskType = newTaskType, GroupName = groupName },
                CronSchedules = cronSchedules,
                LastTrigger = DateTime.Now
            };

            var cron = new CronObject(dc);

            cron.OnCronTrigger += Cron_OnCronTrigger;
            cron.OnThreadAbort += Cron_OnThreadAbort;
            return cron;
        }

        private void Cron_OnThreadAbort(CronObject cronObject)
        {
            _serviceLogger.CreateLogRecord($"Thread aboreted. Task type {cronObject.Object}", System.Diagnostics.EventLogEntryType.Warning);
        }

        private void Cron_OnCronTrigger(CronObject cronObject)
        {
            if (cronObject == null) return;
            // ReSharper disable once AssignNullToNotNullAttribute
            try
            {
                var cronObjectHelper = (CronObjectHelper)cronObject.Object;
                var task = Activator.CreateInstance(cronObjectHelper.TaskType) as TaskBase;
                task?.ExecuteTask(cronObjectHelper.GroupName,
                    (bool)_serviceSettings.GetPropertyValue(ServiceSettings.GetSettingNameReport(task, cronObjectHelper.GroupName)),
                    (int)_serviceSettings.GetPropertyValue(ServiceSettings.GetSettingNameSendReportInterval(task, cronObjectHelper.GroupName)) * 60,
                    (System.Diagnostics.EventLogEntryType)_serviceSettings.GetPropertyValue(ServiceSettings.GetSettingNameReportInformationLevel(task, cronObjectHelper.GroupName)),
                    (uint)_serviceSettings.GetPropertyValue(ServiceSettings.GetSettingNameMaxErrorCount(task, cronObjectHelper.GroupName)));
            }
            catch (Exception e) { _serviceLogger.CreateLogRecord(e); }
        }

        private void InitializeScheduler()
        {
            _cronObjectList = new List<CronObject>();
            _serviceSettings = new ServiceSettings("");
            _serviceLogger = new Logging.TaskLogging();
            if (!_serviceSettings.EnableScheduler)
            {
                _serviceLogger.CreateLogRecord("Shceduller start forbidden by ServiceSettings.enableScheduler");
                return;
            }

            foreach (var type in Tasks.TasksEnumerator.GetTaskEnumertor())
            {
                var task = Activator.CreateInstance(type) as TaskBase;
                foreach (var group in new SettingsGroupsCollection(task.TaskSettings))
                {
                    if (!(bool)_serviceSettings.GetPropertyValue(ServiceSettings.GetSettingNameEnable(task, group)))
                        continue;
                    try
                    {
                        _cronObjectList.Add(CreateCronObject(type, group, (string)_serviceSettings.GetPropertyValue(ServiceSettings.GetSettingNameShedule(task, group))));
                    }
                    catch (Exception e)
                    {
                        _serviceLogger.CreateLogRecord(e);
                    }
                }
            }
        }

        private void StartShceduler()
        {
            System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            _serviceSettings = new ServiceSettings("");
            _cronObjectList.ForEach(x => x.Start());
        }

        private void StopShceduler()
        {
            _cronObjectList.ForEach(x => x.Stop());
        }
    }
}
