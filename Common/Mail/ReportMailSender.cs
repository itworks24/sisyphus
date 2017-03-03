using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using Sisyphus.Common;
using Sisyphus.Logging;

namespace Sisyphus.Mail
{
    public static class ReportMailSender
    {
        private static readonly TaskLogging TaskLogging = new TaskLogging();
        private static Settings.ReportMailSettings Settings => new Settings.ReportMailSettings("");

        public static bool SendReportMail(string subject, string body, object source = null)
        {
            return SendMail(Settings.MailAddress, subject, body, source);
        }

        private static StringBuilder GetLogMessageText(TaskLogging taskLogging)
        {
            var messageText = new StringBuilder();
            var taskRepresent = new { Settings.Project, TaskResult = taskLogging.ErrorAccured() ? "ERROR" : "SUCCESS", ObjectName = taskLogging.Source, CurrentDateTime = DateTime.Now.ToString(CultureInfo.CurrentCulture) };
            var body = Settings.ReportPattern.FormatWith(taskRepresent);
            messageText.Append(body);
            messageText.Append("<ul>");
            foreach (var message in taskLogging)
            {
                messageText.Append("<li>");
                var messageRepresent = new { MessageType = message.EntryType.ToString(), MessageDateTime = message.DateTime.ToString(CultureInfo.CurrentCulture), Represent = message.Message };
                messageText.Append(Settings.LogMessagePattern.FormatWith(messageRepresent));
                messageText.Append("</li>");
            }
            messageText.Append("</ul>");
            return messageText;
        }

        public static bool SendLogArray(List<TaskLogging> taskLoggingArray)
        {
            Debug.Assert(taskLoggingArray != null, "taskLoggingArray != null");
            var taskRepresent = new { Settings.Project, TaskResult = taskLoggingArray.Any(t => t.ErrorAccured()) ? "ERROR" : "SUCCESS", ObjectName = taskLoggingArray.FirstOrDefault().Source, CurrentDateTime = DateTime.Now.ToString(CultureInfo.CurrentCulture) };
            var header = Settings.ReportHeaderPattern.FormatWith(taskRepresent);
            var messageText = new StringBuilder();
            foreach (var taskLogging in taskLoggingArray)
            {
                messageText.Append(GetLogMessageText(taskLogging));
                messageText.Append("<br/><br/>");
            }
            try
            {
                return SendReportMail(header, messageText.ToString(), null);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static void SendLog(TaskLogging taskLogging)
        {
            var taskRepresent = new { Settings.Project, TaskResult = taskLogging.ErrorAccured() ? "ERROR" : "SUCCESS", ObjectName = taskLogging.Source, CurrentDateTime = DateTime.Now.ToString(CultureInfo.CurrentCulture) };
            var header = Settings.ReportHeaderPattern.FormatWith(taskRepresent);
            var messagesRepresent = GetLogMessageText(taskLogging);
            try
            {
                SendReportMail(header, messagesRepresent.ToString(), taskLogging.Source);
            }
            catch (Exception e)
            {
                taskLogging.CreateLogRecord(e);
            }
        }

        public static bool SendMail(string recepientAddress, string subject, string body, object source = null)
        {
            var client = new SmtpClient(Settings.SmtpServer, Settings.SmtpPort)
            {
                Credentials = new NetworkCredential(Settings.UserName, Settings.Password),
                EnableSsl = Settings.UseSmtpssl,
            };

            var messageBody = source == null ? body : body.FormatWith(source);

            var mailMessage = new MailMessage(Settings.UserName,
                recepientAddress,
                subject,
                messageBody)
            { IsBodyHtml = true };

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception e)
            {
                TaskLogging.CreateLogRecord(e);
                return false;
            }
        }
    }
}
