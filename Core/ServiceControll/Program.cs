using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Sisyphus.Services;
using CommandLine;
using CommandLine.Text;
using System.Reflection;

namespace Sisyphus
{
    internal static class Program
    {
        class Options
        {
            [Option('m', "method", Required = true,
                HelpText = "Method to execute elevated.")]
            public string MethodName { get; set; }

            [Option('s', "servicename", Required = true,
                HelpText = "Service name.")]
            public string ServiceName { get; set; }

            [Option('d', "startupmode", Required = false,
                HelpText = "Startup mode.")]
            public string StartupMode { get; set; }

            [Option('o', "output", Required = false,
                HelpText = "Output file.")]
            public string Output { get; set; } = "";

            [Option('g', "debug", Required = false,
                HelpText = "Debug on.")]
            public bool debug { get; set; }

            [ParserState]
            public IParserState LastParserState { get; set; }

            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this,
                    (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            }
        }

        public static bool RunElevated(string serviceName, out string errorRepresent, StartupMode startupMode = StartupMode.Auto,
            [CallerMemberName] string methodName = "")
        {
            uint temp;
            return RunElevated(serviceName, out errorRepresent, out temp, startupMode, methodName);
        }
        public static bool RunElevated(string serviceName, out string errorRepresent, out uint errorCode, StartupMode startupMode = StartupMode.Auto,
            [CallerMemberName] string methodName = "")
        {
            errorRepresent = "";
            errorCode = 0;
            var tempFileName = Path.GetTempFileName();
            var startInfo = new ProcessStartInfo
            {
                FileName = Assembly.GetCallingAssembly().Location,
                WorkingDirectory = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location) ?? "",
                Verb = "runas",
                CreateNoWindow = true,
                UseShellExecute = true,
                Arguments = $"-m {methodName} -s {serviceName} -o {tempFileName} -d {ServiceControll.StartupModeToString(startupMode)}"
            };
            var process = new Process { StartInfo = startInfo };
            process.Start();
            process.WaitForExit();
            try
            {
                var resultFileContent = File.ReadAllLines(tempFileName);
                errorCode = Convert.ToUInt16(resultFileContent[1]);
                errorRepresent = Convert.ToString(resultFileContent[2]);
                return Convert.ToBoolean(resultFileContent[0]);
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Consume them
        static void Main(string[] args)
        {
            var options = new Options();
            if (!Parser.Default.ParseArguments(args, options)) return;
            if (options.debug) Console.ReadLine();
            ServiceControll.SeparatedProcess = true;
            if (options.MethodName == null) return;
            FileStream fs = null;
            if (options.Output != "")
            {
                try
                {
                    fs = new FileStream(options.Output, FileMode.OpenOrCreate, FileAccess.Write);
                    var sw = new StreamWriter(fs);
                    Console.SetOut(sw);
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            var errorRepresent = "";
            var result = false;
            uint errorCode = 0;
            try
            {
                switch (options.MethodName)
                {
                    case nameof(ServiceControll.StartService):
                        {
                            result = ServiceControll.StartService(options.ServiceName, out errorRepresent);
                            break;
                        }
                    case nameof(ServiceControll.StopService):
                        {
                            result = ServiceControll.StopService(options.ServiceName, out errorRepresent);
                            break;
                        }
                    case nameof(ServiceControll.InstallService):
                        {
                            ServiceControll.InstallService(options.ServiceName);
                            break;
                        }
                    case nameof(ServiceControll.UninstallService):
                        {
                            ServiceControll.UninstallService(options.ServiceName);
                            break;
                        }
                    case nameof(ServiceControll.SetServiceLogon):
                        {
                            result = ServiceControll.SetServiceLogon(options.ServiceName, out errorCode);
                            break;
                        }
                    case nameof(ServiceControll.SetServiceStartupMode):
                        {
                            result = ServiceControll.SetServiceStartupMode(options.ServiceName,
                                ServiceControll.StringToStartupMode(options.StartupMode), out errorRepresent);
                            break;
                        }
                    default:
                    {
                        result = false;
                        errorRepresent = $"Method doesn't exist. {options.MethodName}";
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                errorRepresent = e.Message;
            }
            Console.WriteLine(result);
            Console.WriteLine(errorCode);
            Console.WriteLine(errorRepresent);
            Console.WriteLine(options.MethodName);
            Console.Out.Close();
            fs?.Close();
        }
    }
}
