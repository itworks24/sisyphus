using System;
using System.Collections.Generic;
using System.Net;
using RKeeperReporter;
using RKeeperReporter.RKeeperExchange;
using Sysiphus.Tasks.Settings;

namespace Sisyphus
{
    internal class EnterpriseWsWrapper
    {    
        private readonly RKeeperExchange _service;

        public bool SendData(Report[] reports, string reportId, ref string errorInfo) 
        {   
            return _service.ReceiveData(reports, reportId, ref errorInfo);
        }

        public void CloseConnection()
        {
            _service.Dispose();
        }

        public EnterpriseWsWrapper(RkeeperReporterSettings settings)
        {
            var myCredentials = new CredentialCache
            {
                {
                    new Uri(settings.EnterpriseWsPath), 
                    "Basic",
                    new NetworkCredential(settings.EnterpriseUserName, settings.EnterprisePassword)
                }
            };
            _service = new RKeeperExchange
            {
                SoapVersion = System.Web.Services.Protocols.SoapProtocolVersion.Soap12,
                Credentials = myCredentials,
                Url = settings.EnterpriseWsPath
            };
        }
    }
}
