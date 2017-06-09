using System;
using System.Diagnostics;
using System.Management;
using System.Net;
using System.ServiceProcess;
using System.Text;
using Sisyphus.CashSystems;
using Sisyphus.Common;

namespace Sisyphus.Services
{
    public enum StartupMode { Auto, Manual, Disabled }
    public static class ServiceControll
    {
        private static readonly CashSystem CurrentCashSystem = new CashSystem();
        internal static bool SeparatedProcess { get; set; }
        internal static bool ForceElevate { get; set; } = true;
        public static string StartupModeToString(StartupMode value)
        {
            switch (value)
            {
                case StartupMode.Auto:
                    return "Automatic";
                case StartupMode.Manual:
                    return "Manual";
                case StartupMode.Disabled:
                    return "Disabled";
                default:
                    return "Disabled";
            }
        }

        public static StartupMode StringToStartupMode(string value)
        {
            switch (value.ToLower())
            {
                case "auto":
                    return StartupMode.Auto;
                case "automatic":
                    return StartupMode.Auto;
                case "manual":
                    return StartupMode.Manual;
                case "disabled":
                    return StartupMode.Disabled;
                default:
                    return StartupMode.Disabled;
            }
        }
        private static ManagementObject GetServiceManagementObject(string serviceName)
        {
            var sQuery = new SelectQuery($"select * from Win32_Service where name = '{serviceName}'");
            using (var mgmtSearcher = new ManagementObjectSearcher(sQuery))
            {
                foreach (var o in mgmtSearcher.Get())
                {
                    var service = (ManagementObject)o;
                    return service;
                }
            }
            return null;
        }
        private static ManagementObject GetCashedServiceManagmentObject(string serviceName)
        {
            return (ManagementObject)CurrentCashSystem.GetCashedValue(serviceName, () => GetServiceManagementObject(serviceName));
        }
        public static bool ServiceExist(string serviceName)
        {
            return GetCashedServiceManagmentObject(serviceName) != null;
        }
        public static bool ServiceCanStop(string serviceName)
        {
            if (!ServiceExist(serviceName)) return false;
            var service = GetCashedServiceManagmentObject(serviceName);
            return (bool)service[$"AcceptStop"];
        }
        public static bool ServiceCanStart(string serviceName)
        {
            if (!ServiceExist(serviceName)) return false;
            var serviceStartMode = GetServiceStartMode(serviceName);
            if (serviceStartMode == StartupMode.Disabled) return false;
            var service = GetCashedServiceManagmentObject(serviceName);
            return !((bool)service[$"Started"]);
        }
        public static bool ServiceCanRestart(string serviceName)
        {
            if (!ServiceExist(serviceName)) return false;
            var serviceStartMode = GetServiceStartMode(serviceName);
            if (serviceStartMode == StartupMode.Disabled) return false;
            var service = GetCashedServiceManagmentObject(serviceName);
            return ((bool)service[$"Started"]);
        }
        public static string GetServiceState(string serviceName)
        {
            if (!ServiceExist(serviceName)) return "";
            var service = GetCashedServiceManagmentObject(serviceName);
            return service[$"State"].ToString();
        }
        public static string GetServiceLogonName(string serviceName)
        {
            if (!ServiceExist(serviceName)) return "";
            var service = GetCashedServiceManagmentObject(serviceName);
            return service[$"startname"].ToString();
        }
        public static StartupMode GetServiceStartMode(string serviceName)
        {
            if (!ServiceExist(serviceName)) return StartupMode.Disabled;
            var service = GetCashedServiceManagmentObject(serviceName);
            return StringToStartupMode(service[$"StartMode"].ToString());
        }
        public static bool SetServiceStartupMode(string serviceName, StartupMode startupMode, out string errorRepresent)
        {
            errorRepresent = "";
            if (!SeparatedProcess && (ForceElevate || !UacHelper.IsProcessElevated))
            {  
                var result = Program.RunElevated(serviceName, out errorRepresent, startupMode);
                FlushCash();
                return result;
            }
            if (!ServiceExist(serviceName)) return false;
            try
            {
                var parameters = new object[1];
                parameters[0] = StartupModeToString(startupMode);
                var result = (uint)GetCashedServiceManagmentObject(serviceName).InvokeMethod("ChangeStartMode", parameters);
                FlushCash();
                return result == 0;
            }
            catch (Exception e) { errorRepresent = e.Message; FlushCash(); return false; }
        }
        public static bool SetServiceLogonCredentials(string serviceName, NetworkCredential credential, out uint errorСode)
        {
            errorСode = 200;
            string errorRepresent;
            if (!SeparatedProcess && (ForceElevate || !UacHelper.IsProcessElevated))
            {
                var result = Program.RunElevated(serviceName, out errorRepresent);
                FlushCash();
                return result;
            }
            if (!ServiceExist(serviceName)) return false;
            var wmiParams = new object[11];
            var sb = new StringBuilder();
            credential.Domain = credential.UserName.GetDomain();
            credential.UserName = credential.UserName.GetUserName();
            sb.Append(credential.Domain == "" ? "." : credential.Domain);
            sb.Append("\\");
            sb.Append(credential.UserName);
            wmiParams[6] = credential.UserName == "" ? null : sb.ToString();
            wmiParams[7] = credential.UserName == "" ? null : credential.Password;
            errorСode = (uint)GetCashedServiceManagmentObject(serviceName).InvokeMethod("Change", wmiParams);
            FlushCash();
            return errorСode == 0;
        }
        public static bool SetServiceLogon(string serviceName, out uint errorСode)
        {
            errorСode = 200;
           
            if (!SeparatedProcess && (ForceElevate || !UacHelper.IsProcessElevated))
            {
                string errorRepresent;
                var result = Program.RunElevated(serviceName, out errorRepresent);
                FlushCash();
                return result;
            }
            NetworkCredential credential;
            CommonFunctions.GetCredentialsVistaAndUp(out credential);
            return credential != null && SetServiceLogonCredentials(serviceName, credential, out errorСode);
        }
        private static void ManageService(string serviceName, string command)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = serviceName + ".exe",
                Arguments = command,
                UseShellExecute = true,
                Verb = "runas"
            };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit(1000 * 60 * 5);
            FlushCash();
        }
        public static void InstallService(string serviceName)
        {
            ManageService(serviceName, "-install");
        }
        public static void UninstallService(string serviceName)
        {
            ManageService(serviceName, "-uninstall");
        }
        public static bool StartService(string serviceName, out string errorRepresent)
        {
            if (!SeparatedProcess && (ForceElevate || !UacHelper.IsProcessElevated))
            {
                var result = Program.RunElevated(serviceName, out errorRepresent);
                FlushCash();
                return result;
            }
            errorRepresent = "";
            if (!ServiceExist(serviceName)) return false;
            if (!ServiceCanStart(serviceName)) return false;
            var serviceController = new ServiceController(serviceName);
            try
            {
                serviceController.Start();
                FlushCash();
                return true;
            }
            catch (Exception e) { errorRepresent = e.Message; FlushCash(); return false; }
        }
        public static bool StopService(string serviceName, out string errorRepresent)
        {
            errorRepresent = "";
            if (!SeparatedProcess && (ForceElevate || !UacHelper.IsProcessElevated))
            {
                var result = Program.RunElevated(serviceName, out errorRepresent);
                FlushCash();
                return result;
            }
            if (!ServiceExist(serviceName)) return false;
            if (!ServiceCanStop(serviceName)) return false;
            var serviceController = new ServiceController(serviceName);
            try
            {
                serviceController.Stop();
                FlushCash();
                return true;
            }
            catch (Exception e) { errorRepresent = e.Message; FlushCash(); return false; }
        }

        private static void FlushCash()
        {
            CurrentCashSystem.FlushCash();
        }
    }

}
