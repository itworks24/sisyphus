using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Sisyphus.Logging
{
    public class LogMessage
    {
        public struct MessageStruct
        {
            public EventLogEntryType EntryType { get; set; }
            public DateTime DateTime { get; set; }
            public string Message { get; set; }
        }
        
        public EventLogEntryType EntryType { get; set; }
        public string Message { get; set; }
        private readonly string _source;
        public DateTime DateTime { get; set; }

        public bool IsErrorMessage => EntryType == EventLogEntryType.Error;
        
        public bool IsSaved { get; set; }

        public string Represent()
        {
            return
                $"Source: {_source}. Type: {EntryType.ToString()}. Date: {DateTime.ToString(CultureInfo.InvariantCulture)}. Message: {Message}.";
        }

        public LogMessage(string newSource, string newMessage, EventLogEntryType newEntryType, DateTime? dateTime = null)
        {
            Message = newMessage;
            _source = newSource;
            EntryType = newEntryType;
            DateTime = dateTime ?? DateTime.Now;
        }
    }

    public class TaskLogging : IEnumerable<LogMessage>
    {
        private readonly List<LogMessage> _logMessageList;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _logMessageList.GetEnumerator();
        }

        private readonly bool _writeSystemLog;
        public object Source { get; private set; }

        public bool ErrorAccured()
        {
            return _logMessageList.ToArray().Count(x => x.IsErrorMessage) > 0;
        }

        public void CreateLogRecord(string message, EventLogEntryType entryType = EventLogEntryType.Information) { _logMessageList.Add(Logger.CreateLogRecord(Source, message, entryType, _writeSystemLog)); }
        public void CreateLogRecord(Exception e) { _logMessageList.Add(Logger.CreateLogRecord(Source, e, _writeSystemLog)); }

        public IEnumerator<LogMessage> GetEnumerator()
        {
            return _logMessageList.GetEnumerator();
        }

        public TaskLogging(bool newWriteSystemLog = true)
        {
            Source = this;
            _writeSystemLog = newWriteSystemLog;
            _logMessageList = new List<LogMessage>();
        }

        public string Represent
        {
            get
            {
                var stringBuilder = new StringBuilder();
                _logMessageList.ForEach(t => stringBuilder.Append($"{t.Represent()}\n"));
                return stringBuilder.ToString();
            }
        }

        public static TaskLogging GetTaskLogging(string source, IEnumerable<LogMessage.MessageStruct> messages)
        {
            var taskLogging = new TaskLogging() {Source = source};
            foreach (var message in messages)
            {
                taskLogging._logMessageList.Add(new LogMessage(source, message.Message, message.EntryType, message.DateTime));
            }
            return taskLogging;
        }
    }

    internal static class Logger
    {
        public static LogMessage CreateLogRecord(Type source, string log, EventLogEntryType entryType = EventLogEntryType.Information, bool writeSystemLog = true)
        {
            return CreateLogRecord(source.Name, log, entryType, writeSystemLog);
        }

        public static LogMessage CreateLogRecord(object source, string log, EventLogEntryType entryType = EventLogEntryType.Information, bool writeSystemLog = true)
        {
            return CreateLogRecord(source.GetType().Name, log, entryType, writeSystemLog);
        }

        public static LogMessage CreateLogRecord(object source, Exception e, bool writeSystemLog = true)
        {
            return CreateLogRecord(source.ToString(), $"Error occurred. Message: {e.Message}\nDescription: {e}", EventLogEntryType.Error, writeSystemLog);
        }

        public static LogMessage CreateLogRecord(string source, Exception e, bool writeSystemLog = true)
        {
            return CreateLogRecord(source, $"Error occurred. Message: {e.Message}", EventLogEntryType.Error, writeSystemLog);
        }

        private static string GetEventLogSource(string source) { return "Sisyphus_" + source; }
        private static string LogName => "Application";

        private static LogMessage CreateLogRecord(string source, string log, EventLogEntryType entryType = EventLogEntryType.Information, bool writeSystemLog = true)
        {
            var sSource = GetEventLogSource(source);
            if (!writeSystemLog) return new LogMessage(sSource, log, entryType);
            try
            {
                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, LogName);
                EventLog.WriteEntry(sSource, log, entryType);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Cant wrtie event log. Reason: {e.Message}");
            }
            return new LogMessage(sSource, log, entryType);
        }
    }
}
