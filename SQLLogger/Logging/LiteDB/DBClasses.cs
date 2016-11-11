using System;
using System.Collections.Generic;
using System.Diagnostics;
using LiteDB;

namespace Sisyphus.Logging.LiteDB
{
    
    public class Task
    {
        [BsonId]
        public int Id { get; set; }
        public string TaskName { get; set; }
        public List<MessageGroup> MessageGroups { get; set; }

        public Task() { }
    }

    public class MessageGroup
    {
        [BsonId]
        public int Id { get; set; }
        public Task Task { get; set; }
        public DateTime DateTime { get; set; }
        public bool MessageSent { get; set; }
        public bool IsErrorLog { get; set; }
        public List<Message> Messages { get; set; }
        public EventLogEntryType EntryType { get; set; }

        public MessageGroup() { }
    }

    public class Message
    {
        [BsonId]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public MessageGroup MessageGroup { get; set; }
        public string MessageText { get; set; }
        public EventLogEntryType EntryType { get; set; }

        public Message() { }
    }
}
