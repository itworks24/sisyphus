using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LiteDB;
using Sisyphus.Logging.LiteDB;
using Sisyphus.Mail;

namespace Sisyphus.Logging
{

    public static class SqlLogger
    {
        private static readonly string DbFolderPath =
            Path.Combine(new string[] {
                                        Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                                        "itworks",
                                        "Sisyphus",
                                        "conf"
                                      }
                        );

        private static readonly string DbFileName = Path.Combine(DbFolderPath,
            "log.lite.db");

        private static LiteDatabase Connection
            => new LiteDatabase($"{DbFileName}");

        private static Task GetTask(LiteDatabase connection, string taskName)
        {
            var tasks = connection.GetCollection<Task>(nameof(Task));
            var task = tasks.FindOne(t => t.TaskName == taskName);
            if (task != null) return task;
            task = new Task() { TaskName = taskName };
            tasks.Insert(task);
            tasks.EnsureIndex(t => t.TaskName);
            return task;
        }

        private static MessageGroup AddLogGroup(LiteDatabase connection, TaskLogging taskLogging, Task task)
        {
            var messageGroups = connection.GetCollection<MessageGroup>(nameof(MessageGroup));
            var messageGroup = new MessageGroup()
            {
                Task = task,
                DateTime = DateTime.Now,
                IsErrorLog = taskLogging.ErrorAccured(),
                MessageSent = false,
                EntryType = EventLogEntryType.Information
            };
            messageGroups.Insert(messageGroup);
            messageGroups.EnsureIndex(t => t.DateTime);
            return messageGroup;
        }

        private static Message AddLogRecord(LiteDatabase connection, LogMessage logMessage, MessageGroup messageGroup)
        {
            var messages = connection.GetCollection<Message>(nameof(Message));
            var message = new Message()
            {
                MessageGroup = messageGroup,
                DateTime = DateTime.Now,
                EntryType = logMessage.EntryType,
                MessageText = logMessage.Message
            };
            if (messageGroup.EntryType > message.EntryType)
            {
                messageGroup.EntryType = message.EntryType;
                connection.GetCollection<MessageGroup>(nameof(MessageGroup)).Update(messageGroup);
            }
            messages.Insert(message);
            messages.EnsureIndex(t => t.DateTime);
            return message;
        }

        public static void ExcludeUnnecessaryLogGroups(MessageGroup messageGroup, EventLogEntryType ReportInformationLevel = EventLogEntryType.Error)
        {
            using (var connection = Connection)
            {
                if (messageGroup.EntryType <= ReportInformationLevel) return;
                messageGroup.MessageSent = true;
                connection.GetCollection<MessageGroup>(nameof(MessageGroup)).Update(messageGroup);
            }
        }

        public static MessageGroup AddLogRecords(TaskLogging taskLogging, string taskName)
        {
            using (var connection = Connection)
            {
                var task = GetTask(connection, taskName);
                var logGroup = AddLogGroup(connection, taskLogging, task);
                foreach (var logMessage in taskLogging.Where(t => !t.IsSaved))
                {
                    AddLogRecord(connection, logMessage, logGroup);
                    logMessage.IsSaved = true;
                }
                return logGroup;
            }
        }

        private static DateTime GetLastGroupReportSent(Task task)
        {
            using (var connection = Connection)
            {
                var messageGroups = connection.GetCollection<MessageGroup>(nameof(MessageGroup));
                var messageGroup = messageGroups.Find(t => t.Task.Id == task.Id && t.MessageSent).OrderByDescending(t => t.DateTime).FirstOrDefault();
                return messageGroup?.DateTime ?? DateTime.MinValue;
            }
        }

        public static void SendLogRecords(string taskName, int sendReportIntervalInSeconds, EventLogEntryType ReportInformationLevel = EventLogEntryType.Error)
        {
            using (var connection = Connection)
            {
                var task = GetTask(connection, taskName);
                var lastReportSentDateTime = GetLastGroupReportSent(task);
                if (lastReportSentDateTime.AddSeconds(sendReportIntervalInSeconds) > DateTime.Now) return;
                var messageGroups = connection.GetCollection<MessageGroup>(nameof(MessageGroup)).FindAll().Where(t => t.Task.Id == task.Id && t.EntryType <= ReportInformationLevel && !t.MessageSent);
                var taskLoggingList = (from messageGroup in messageGroups
                                       select connection.GetCollection<Message>(nameof(Message)).FindAll().Where(t => t.MessageGroup.Id == messageGroup.Id && t.EntryType <= ReportInformationLevel).OrderBy(t => t.DateTime)
                    into messagesList
                                       select messagesList.Select(t => new LogMessage.MessageStruct()
                                       {
                                           Message = $"ID {t.Id} {t.MessageText}",
                                           EntryType = t.EntryType,
                                           DateTime = t.DateTime
                                       })
                    into messages
                                       select TaskLogging.GetTaskLogging(task.TaskName, messages)).ToList();
                if (taskLoggingList.Count == 0) return;
                var result = ReportMailSender.SendLogArray(taskLoggingList);

                if (!result) return;

                foreach (var messageGroup in messageGroups)
                {
                    messageGroup.MessageSent = true;
                    connection.GetCollection<MessageGroup>(nameof(MessageGroup)).Update(messageGroup);
                }
            }
        }
    }
}
