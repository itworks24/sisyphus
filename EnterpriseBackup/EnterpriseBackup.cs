using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Sisyphus.Common;
using Sisyphus.Logging;
using Sisyphus.Settings;
using Sisyphus.Tasks;
using V83;
using Settings;

namespace Sisyphus
{
    internal class BaseRepresent
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        public static string GetWindowText(IntPtr hWnd)
        {
            var size = GetWindowTextLength(hWnd);
            if (size <= 0) return string.Empty;
            var builder = new StringBuilder(size + 1);
            GetWindowText(hWnd, builder, builder.Capacity);
            return builder.ToString();
        }

        public enum BackupState
        {
            Nothing,
            NotApplicable,
            Queued,
            InProgress,
            Success,
            Error
        }

        public enum BaseType
        {
            Server,
            File,
            WebServer,
            BadPath
        }

        private string _baseName;

        private List<string> _logResult;

        private string _serverName;

        public BaseRepresent(string newName, string newPath)
        {
            Name = CommonFunctions.RemoveInvalidCharacters(newName);
            Path = newPath;
        }

        public string Name { get; }

        public string Date => DateTime.Now.ToString(CultureInfo.InvariantCulture);

        public string ServerName => _serverName;

        public string BaseName => _baseName;

        private string _path;

        private string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                CurrentBaseType = GetBaseType(Path, out _serverName, out _baseName);
            }
        }

        private BaseType _currentBaseType;

        public BaseType CurrentBaseType
        {
            get { return _currentBaseType; }
            set
            {
                _currentBaseType = value;
                if (_currentBaseType == BaseType.BadPath)
                {
                    LogResult = $"Bad connection string for base {Name}\nConnection string: {_path}";
                    CurrentBackUpState = BackupState.Error;
                }
                if (_currentBaseType == BaseType.WebServer)
                {
                    LogResult = $"Base '{Name}' is a web server defenition. Cant backup it";
                    CurrentBackUpState = BackupState.NotApplicable;
                }
                if ((_currentBaseType == BaseType.File) || (_currentBaseType == BaseType.Server))
                {
                    LogResult =
                        $@"Server name: {ServerName}
                                Base name: {BaseName}
                                Path: {Path}
                                ";
                    CurrentBackUpState = BackupState.Queued;
                }
                else
                    CurrentBackUpState = BackupState.Nothing;
            }
        }

        private string _logFileName;

        private string LogFileName
        {
            get { return _logFileName; }
            set
            {
                _logFileName = value;
                LogResult = File.ReadAllText(LogFileName);
            }
        }

        public string LogResult
        {
            private get
            {
                var result = new StringBuilder();
                foreach (var log in _logResult)
                    result.AppendLine(log);
                return result.ToString();
            }
            set
            {
                if (_logResult == null) _logResult = new List<string>();
                Console.WriteLine(value);
                _logResult.Add(value);
            }
        }

        private BackupState CurrentBackupState { get; set; }

        public BackupState CurrentBackUpState
        {
            get { return CurrentBackupState; }
            set
            {
                if (value == BackupState.Success) LogResult = $"Base '{Name}' successfuly backed up.";
                CurrentBackupState = value;
            }
        }

        private IWorkingProcessConnection GetWorkingProcessConnection()
        {
            var connector = new COMConnector();
            var agentConnection = connector.ConnectAgent(ServerName);
            agentConnection.AuthenticateAgent("", "");
            var cluster = agentConnection.GetClusters().GetValue(0) as IClusterInfo;
            agentConnection.Authenticate(cluster, "", "");
            var workingProcess = agentConnection.GetWorkingProcesses(cluster).GetValue(0) as IWorkingProcessInfo;
            if (workingProcess != null)
            {
                var connectString = workingProcess.HostName + ":" + workingProcess.MainPort;
                var workingProcessConnection = connector.ConnectWorkingProcess(connectString);
                workingProcessConnection.AddAuthentication("", "");
                return workingProcessConnection;
            }
            else
            {
                return null;
            }
        }

        private void SetSheduledJobsStatus(bool scheduledJobsDenied)
        {
            try
            {
                var workingProcessConnection = GetWorkingProcessConnection();
                foreach (IInfoBaseInfo ibDesc in workingProcessConnection.GetInfoBases())
                    if (ibDesc.Name == BaseName)
                    {
                        ibDesc.ScheduledJobsDenied = scheduledJobsDenied;
                        workingProcessConnection.UpdateInfoBase(ibDesc);
                        return;
                    }
                LogResult = "Base not found";
            }
            catch (Exception e)
            {
                LogResult = e.Message;
            }
        }

        private bool DissconnectConnections()
        {
            var workingProcessConnection = GetWorkingProcessConnection();
            var ibDesc = workingProcessConnection.CreateInfoBaseInfo();
            ibDesc.Name = BaseName;
            var connections = workingProcessConnection.GetInfoBaseConnections(ibDesc);
            foreach (var connection in connections)
            {
                var infoBaseConnectionInfo = connection as IInfoBaseConnectionInfo;
                if (infoBaseConnectionInfo != null && infoBaseConnectionInfo.AppID != "COMConsole")
                    workingProcessConnection.Disconnect(connection as IInfoBaseConnectionInfo);
            }
            return (from object connection in connections select connection as IInfoBaseConnectionInfo).All(infoBaseConnectionInfo => infoBaseConnectionInfo == null || infoBaseConnectionInfo.AppID == "COMConsole");
        }

        private void KickAllUsers()
        {
            if (CurrentBaseType == BaseType.File)
            {
                var process = new Process();
                var startInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = true,
                    FileName = "cmd.exe",
                    Arguments = "taskkill /f /t /fi \"IMAGENAME eq 1cv8*\"",
                    Verb = "runas"
                };
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit(5000);
                process.Kill();
            }
            else
            {
                var maxErrorCount = 4;
                var allConnectionsDisconected = false;
                while (maxErrorCount > 0 && !allConnectionsDisconected)
                {
                    try
                    {
                        allConnectionsDisconected = DissconnectConnections();
                    }
                    catch (Exception e)
                    {
                        LogResult = e.Message;
                    }
                    maxErrorCount -= 1;
                }
            }
        }

        private static string RemoveIllegelPathSymbols(string path)
        {
            var regexSearch = new string(System.IO.Path.GetInvalidFileNameChars()) + new string(System.IO.Path.GetInvalidPathChars());
            var r = new Regex($"[{Regex.Escape(regexSearch)}]");
            return r.Replace(path, "");
        }

        public void CreateBackup(string enterprisePath, string destinationPath, string fileNameFormat = "{name}_{date}",
            int whaitForBackUp = 1000 * 60 * 60 * 4)
        {
            if (CurrentBackUpState != BackupState.Queued)
                return;

            LogResult = $"Starting backup process for base {BaseName}";

            var startInfo = new ProcessStartInfo();
            string backupFileName;
            string dumpResultFileName;
            string logFileName;

            try
            {
                var fileName = RemoveIllegelPathSymbols(fileNameFormat.FormatWith(this));
                var logDirectory = System.IO.Path.Combine(destinationPath, "log");
                if (!Directory.Exists(logDirectory)) Directory.CreateDirectory(logDirectory);
                backupFileName = System.IO.Path.Combine(destinationPath, fileName + ".dt");
                logFileName = System.IO.Path.Combine(logDirectory, fileName + ".log");
                dumpResultFileName = System.IO.Path.Combine(logDirectory, fileName + "-dumpresult.log");
                var commandArgs =
                    $"DESIGNER /IBConnectionString{Path} /DumpIB \"{backupFileName}\" /DumpResult \"{dumpResultFileName}\" /Out \"{logFileName}\"";

                startInfo.WindowStyle = ProcessWindowStyle.Normal;
                startInfo.FileName = System.IO.Path.Combine(enterprisePath, "1cv8.exe");
                startInfo.RedirectStandardOutput = false;
                startInfo.RedirectStandardError = false;
                startInfo.UseShellExecute = true;
                startInfo.Arguments = commandArgs;

                if (CurrentBaseType == BaseType.Server) SetSheduledJobsStatus(true);

                KickAllUsers();
            }
            catch (Exception e)
            {
                LogResult = "Process ended whith error";
                LogResult = e.Message;
                CurrentBackUpState = BackupState.Error;
                return;
            }

            using (var backupProcess = Process.Start(startInfo))
            {
                if (backupProcess == null) return;
                LogResult = $"Process started, PID: {backupProcess.Id}";
                CurrentBackUpState = BackupState.InProgress;
                while (!backupProcess.HasExited)
                {
                    try
                    {
                        if (backupProcess.MainWindowHandle != IntPtr.Zero && GetWindowText(backupProcess.MainWindowHandle) != string.Empty)
                        {
                            backupProcess.Kill();
                            LogResult = "Auth failed"; break;
                        }
                        System.Threading.Thread.Sleep(1000);
                    }
                    catch (Exception e)
                    {
                        // ignored
                    }
                }
                //exeProcess.WaitForExit(whaitForBackUp);
                var exitCode = backupProcess.ExitCode;
                int dumpResult;
                try
                {
                    dumpResult = int.TryParse(File.ReadAllText(dumpResultFileName), out dumpResult) ? dumpResult : -1;
                }
                catch (Exception e)
                {
                    LogResult = e.Message;
                    dumpResult = -2;
                }

                if (CurrentBaseType == BaseType.Server)
                    SetSheduledJobsStatus(false);

                if (!File.Exists(backupFileName) || dumpResult != 0 || exitCode != 0)
                {
                    CurrentBackUpState = BackupState.Error;
                    LogResult = "Process ended whith error";
                    if (dumpResult != -2) LogFileName = logFileName;
                }
                else
                {
                    CurrentBackUpState = BackupState.Success;
                }
            }
        }

        public void AddLogRecord(TaskLogging logger)
        {
            if (CurrentBackUpState == BackupState.Error)
                logger.CreateLogRecord(LogResult, EventLogEntryType.Error);
            if (CurrentBackUpState == BackupState.NotApplicable)
                logger.CreateLogRecord(LogResult, EventLogEntryType.Warning);
            if (CurrentBackUpState == BackupState.Success)
                logger.CreateLogRecord(LogResult);
        }

        private BaseType GetBaseType(string basePath, out string serverName, out string baseName)
        {
            serverName = "";
            baseName = "";
            try
            {
                var regexObj = new Regex("(?<Server>srvr)?(?<File>file)?(?<WS>ws)?=.*",
                    RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ExplicitCapture);
                var matchResult = regexObj.Match(basePath);
                while (matchResult.Success)
                {
                    if (matchResult.Groups["Server"].Value != "")
                    {
                        regexObj = new Regex(@"srvr=""(?<Server>[\w]+)"";ref=""(?<BaseName>[\w]+)""",
                            RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ExplicitCapture);
                        matchResult = regexObj.Match(basePath);
                        if (matchResult.Success)
                        {
                            serverName = matchResult.Groups["Server"].Value;
                            baseName = matchResult.Groups["BaseName"].Value;
                        }
                        return BaseType.Server;
                    }
                    if (matchResult.Groups["File"].Value != "")
                    {
                        regexObj = new Regex(@"file=""(?<BaseName>.*)"";",
                            RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ExplicitCapture);
                        matchResult = regexObj.Match(basePath);
                        if (matchResult.Success)
                            baseName = matchResult.Groups["BaseName"].Value;
                        return BaseType.File;
                    }
                    if (matchResult.Groups["WS"].Value == "") continue;
                    regexObj = new Regex(@"ws=""(?<Server>.*)/(?<BaseName>.*);",
                        RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ExplicitCapture);
                    matchResult = regexObj.Match(basePath);
                    if (matchResult.Success)
                    {
                        serverName = matchResult.Groups["Server"].Value;
                        baseName = matchResult.Groups["BaseName"].Value;
                    }
                    return BaseType.WebServer;
                }
            }
            catch (ArgumentException ex)
            {
                serverName = "";
                baseName = "";
                LogResult = ex.Message;
                return BaseType.BadPath;
            }
            return BaseType.BadPath;
        }
    }

    public sealed partial class EnterpriseBackup
    {
        public bool ExecuteProcess()
        {
            var options = GetSettings(typeof(BackupSettings)) as BackupSettings;

            var baseList = new Ini(options.ListFile);
            baseList.Load();

            if (options.Restart)
            {
                string command = $"sc \\\\{options.Server} restart \"{options.ServiceName}\"";
                var process = new Process();
                var startInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = command
                };
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit(1000 * 60 * 5);
            }

            var baseRepresentList = new List<BaseRepresent>();

            foreach (var currentBase in baseList.GetSections())
                baseRepresentList.Add(new BaseRepresent(currentBase, baseList.GetValue("Connect", currentBase)));

            foreach (var currentBase in baseRepresentList)
            {
                try
                {
                    currentBase.CreateBackup(options.EnterprisePath, options.DestinationPath, options.OutputFormat);
                }
                catch (Exception e)
                {
                    currentBase.LogResult = "Process ended whith error";
                    currentBase.LogResult = e.Message;
                    currentBase.CurrentBackUpState = BaseRepresent.BackupState.Error;
                }
            }

            if (options.UseLog)
                foreach (var currentBase in baseRepresentList)
                    currentBase.AddLogRecord(this);

            return baseRepresentList.All(t => t.CurrentBackUpState != BaseRepresent.BackupState.Error);
        }
    }

    public sealed partial class EnterpriseBackup : TaskBase
    {
        public override ExecutionDelegate ExecutuionMethod => ExecuteProcess;
        public override IEnumerable<Type> TaskSettings => new[] { typeof(BackupSettings) };
    }
}