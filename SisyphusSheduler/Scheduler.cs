using System;
using System.ServiceProcess;
using Sisyphus.Mail;

namespace Sisyphus
{
    public partial class Scheduler : ServiceBase
    {
        public Scheduler()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                InitializeScheduler();
                StartShceduler();
                _serviceLogger.CreateLogRecord("Service started");
            }
            catch (Exception e)
            {
                _serviceLogger.CreateLogRecord(e);
                ReportMailSender.SendLog(_serviceLogger);
                throw;
            }  
        }

        protected override void OnStop()
        {
            StopShceduler();
        }
    }
}
