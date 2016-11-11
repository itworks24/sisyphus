using System.ComponentModel;
using Settings;

namespace Sisyphus.Settings
{
    public class ReportMailSettings : SettingsRepresent
    {
        [Category("SMTP Server")]
        [DisplayName("SMTP Server Address")]
        public string SmtpServer { get; set; } = "smtp.yandex.ru";

        [Category("SMTP Server")]
        [DisplayName("SMTP port")]
        public int SmtpPort { get; set; } = 25;

        [Category("SMTP Server")]
        [DisplayName("Use SSL")]
        public bool UseSmtpssl { get; set; } = true;

        [Category("SMTP Server")]
        [DisplayName("User name")]
        public string UserName { get; set; }

        [Category("SMTP Server")]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Category("Messages")]
        [DisplayName("Mail address to send notifications")]
        public string MailAddress { get; set; } = "admin@itworks24.ru";

        [Category("Messages")]
        [DisplayName("Project name")]
        public string Project { get; set; } = System.Environment.MachineName;

        [Category("Messages")]
        [DisplayName("Header pattern")]
        public string ReportHeaderPattern { get; set; } = "{Project}, {TaskResult} report, {ObjectName}, {CurrentDateTime}";

        [Category("Messages")]
        [DisplayName("Report pattern")]
        public string ReportPattern { get; set; } = "Project: <b>{Project}</b><br/>Task result: <b>{TaskResult}</b><br/>Task name: <b>{ObjectName}</b><br/>Report date: <b>{CurrentDateTime}</b><br/>";

        [Category("Messages")]
        [DisplayName("Log pattern")]
        public string LogMessagePattern { get; set; } = "<br/>Type: <b>{MessageType}</b>, Date: {MessageDateTime}<br/><i>{Represent}</i><br/>";

        public ReportMailSettings(string groupName) : base(groupName)
        {
        }
    }
}
