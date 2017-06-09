using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

namespace Sisyphus
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.  
        /// </summary>
        static void Main(string[] args)
        {
            if (System.Environment.UserInteractive)
            {
                if (args.Length <= 0) return;
                switch (args[0])
                {
                    case "-install":
                    {
                        ManagedInstallerClass.InstallHelper(new[] { Assembly.GetExecutingAssembly().Location });
                        break;
                    }
                    case "-uninstall":
                    {
                        ManagedInstallerClass.InstallHelper(new[] { "/u", Assembly.GetExecutingAssembly().Location });
                        break;
                    }
                }
            }
            else
            {
                var servicesToRun = new ServiceBase[]
                {
                    new Scheduler()
                };
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
