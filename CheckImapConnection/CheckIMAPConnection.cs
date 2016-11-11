using Sisyphus.Settings;
using AE.Net.Mail;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using Sisyphus.Mail;
using Sisyphus.Properties;
using Sisyphus.Tasks;

namespace Sisyphus
{
    internal class SymbolSetCollection
    {
        private List<SymbolSet> _symbolSetList;
        private List<SymbolSet> SymbolSetList => _symbolSetList ?? (_symbolSetList = new List<SymbolSet>());

        public int Count => SymbolSetList.Count;

        public void AddSymbolString(string symbolString)
        {
            SymbolSetList.Add(new SymbolSet() { SymbolString = symbolString });
        }

        public char GetRandomChar(Random rnd, bool allSets = true)
        {
            if (!allSets)
                foreach (var symbolSet in SymbolSetList)
                    if (symbolSet.Count == 0)
                        return symbolSet.GetRandomChar(rnd);
            int setIndex = rnd.Next(Count);
            return SymbolSetList.ElementAt(setIndex).GetRandomChar(rnd);
        }
    }

    class SymbolSet
    {
        public string SymbolString
        {
            set
            {
                _symbols = value.ToCharArray();
                _symbolsCount = value.Length;
                Count = 0;
            }
        }

        public int Count { get; private set; }

        private char[] _symbols;
        private int _symbolsCount;

        public char GetRandomChar(Random rnd)
        {
            Count += 1;
            int symbolIndex = rnd.Next(_symbolsCount);
            return _symbols[symbolIndex];
        }
    }

    internal sealed partial class CheckImapConnection : TaskBase
    {
        public override ExecutionDelegate ExecutuionMethod => ExecuteProcess;
        public override IEnumerable<Type> TaskSettings => new []{ typeof(ImapSettings)};
    }

    internal sealed partial class CheckImapConnection
    {
        static string SecureStringToString(SecureString value)
        {
            var valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        static SecureString GeneratePassword(int length, bool useLowerSymbols = true, bool useUpperSymbols = true, bool useNumbers = true, bool useSpecialSymbols = true)
        {
            var symbolSetList = new SymbolSetCollection();
            if (useLowerSymbols)
                symbolSetList.AddSymbolString("abcdefghijklmnopqrstuvwxyz");
            if (useUpperSymbols)
                symbolSetList.AddSymbolString("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            if (useNumbers)
                symbolSetList.AddSymbolString("0123456789");
            if (useSpecialSymbols)
                symbolSetList.AddSymbolString("!№;%:?*()");

            var password = new SecureString();
            var rnd = new Random(DateTime.Now.Millisecond);
            rnd = new Random(rnd.Next(int.MaxValue));

            for (var i = 0; i < length - symbolSetList.Count; i++)
                password.AppendChar(symbolSetList.GetRandomChar(rnd));

            for (var i = 0; i < symbolSetList.Count; i++)
                password.AppendChar(symbolSetList.GetRandomChar(rnd, false));

            return password;
        }

        static bool TestMailSign(string dkimHeader, string spfHeader, string fromAddress, string primaryDomain, out string dkimDomain, out string spfMail)
        {
            var dkimPass = false;
            var spfPass = false;
            dkimDomain = "";
            spfMail = "";

            const string dkimPattern = @"dkim=pass.*?(@[A-Z0-9.-]+\.[A-Z]{2,6})";
            const string spfPattern = @"spf=pass.*?([A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}).*?$";

            foreach (Match match in Regex.Matches(dkimHeader, dkimPattern, RegexOptions.IgnoreCase))
            {
                dkimPass = true;
                dkimDomain = match.Groups[1].Value.ToLower().Trim();
                break;
            }

            foreach (Match match in Regex.Matches(spfHeader, spfPattern, RegexOptions.IgnoreCase))
            {
                spfMail = match.Groups[1].Value.ToLower().Trim();
                spfPass = spfMail == fromAddress;
                break;
            }

            if (!spfPass && !spfHeader.ToLower().Contains("spf="))
            {
                spfPass = true;
                spfMail = fromAddress;
            }

            var domainPass = (primaryDomain == "") || (dkimDomain.Contains(primaryDomain) || spfMail.Contains(primaryDomain)) && fromAddress.Contains(primaryDomain);

            return dkimPass && spfPass && domainPass;
        }

        private bool SetNewPassword(string emailAddress, SecureString newPassword, out string userName, out string accountName)
        {
            try
            {
                var connectionPrefix = "LDAP://" + Domain.GetCurrentDomain();
                var entry = new DirectoryEntry(connectionPrefix);
                var mySearcher = new DirectorySearcher(entry)
                {
                    Filter = "(&(objectClass=user)(mail=" + emailAddress + "))"
                };


                var result = mySearcher.FindOne();

                if (result == null)
                {
                    throw new NullReferenceException
                    ("unable to locate the user with mail " + emailAddress + " in the " + Domain.GetCurrentDomain() + " domain");
                }

                var directoryObject = result.GetDirectoryEntry();

                userName = directoryObject.Properties["DisplayName"].Value.ToString();
                accountName = directoryObject.Properties["sAMAccountName"].Value.ToString();

                CreateLogRecord($"Changing password for user {accountName}");

                directoryObject.Invoke("SetPassword", SecureStringToString(newPassword));

                entry.Close();
                entry.Dispose();
                mySearcher.Dispose();

                CreateLogRecord($"Password changed for user {accountName}");
                return true;
            }
            catch (Exception e)
            {
                CreateLogRecord(e);
                userName = "";
                accountName = "";
                return false;
            }
        }

        private static bool SendPassword(ImapSettings settings, SecureString newPassword, string userMail, string userName, string accountName)
        {
            return ReportMailSender.SendMail(userMail,
                                                "Важно! Установка нового пароля.",
                                                settings.MailPattern,
                                                new
                                                {
                                                    UserMail = userMail,
                                                    UserName = userName,
                                                    AccountName = accountName + "@" + Domain.GetCurrentDomain(),
                                                    UserPassword = SecureStringToString(newPassword), settings.AdminMailAddress
                                                });

        }

        public bool ExecuteProcess()
        {
            var settings = GetSettings(typeof(ImapSettings)) as ImapSettings;

            //if (settings.addTaskShedule) AddTaskShedule(String.Join(" ", args));

            var ic = new ImapClient(settings.ImapServer, settings.UserName, settings.Password,
                       AuthMethods.Login, settings.ImapPort, settings.UseImapssl);
            CreateLogRecord(Resources.CheckIMAPConnection_ExecuteProcess_);
            
            var result = ic.SearchMessages(settings.ToAddressSearchCondition != "" && settings.ToAddressSearchCondition != "none" ? SearchCondition.Unseen().And(SearchCondition.To(settings.ToAddressSearchCondition)) : SearchCondition.Unseen());

            var primaryDomain = settings.Domain;
            foreach (var entry in result)
            {
                var message = entry.Value;
                CreateLogRecord(string.Format(Resources.CheckIMAPConnection_ExecuteProcess_MESSAGE_UID__0___SUBJECT__1___FROM__2____3_, message.Uid, message.Subject, message.From.Address, message.Date.ToString(CultureInfo.CurrentCulture)));

                string dkimDomain;
                string spfMail;
                if (TestMailSign(message.Headers["Authentication-Results"].RawValue, message.Headers["Authentication-Results"].RawValue, message.From.Address, primaryDomain, out dkimDomain, out spfMail))
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(Resources.CheckIMAPConnection_ExecuteProcess_DKIM_and_SPF_tests_pass);
                    sb.AppendLine($"DKIM domain : {dkimDomain}");
                    sb.AppendLine($"SPF mail : {spfMail}");
                    CreateLogRecord(sb.ToString()); 
                    var newPassword = GeneratePassword(10);
                    string userName;
                    string accountName;
                    var setPasswordResult = SetNewPassword(spfMail, newPassword, out userName, out accountName);
                    if (!setPasswordResult) continue;
                    var mailSent = SendPassword(settings, newPassword, spfMail, userName, accountName);
                    if (mailSent)
                        ic.SetFlags(Flags.Seen, message);
                }
                else
                {
                    CreateLogRecord(Resources.CheckIMAPConnection_ExecuteProcess_DKIM_and_APF_tests_fails);
                }
            }
            ic.Dispose();
            CreateLogRecord(Resources.CheckIMAPConnection_ExecuteProcess_End_mail_parse_);

            return true;
        }
    }
}

