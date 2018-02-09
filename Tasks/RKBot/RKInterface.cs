using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sysiphus.Tasks.RKBot
{
    class RKInterface
    {

        private HttpClient Client;
        private const string InterfacePath = "/rk7api/v0/xmlinterface.xml";

        public RKInterface()
        {
            Client = new HttpClient();
        }

        public void SetCLientSettings(string rkHost, string rkUserName, string rkPassword)
        {
            var url = new Uri(rkHost);
            Client.BaseAddress = url;
            var byteArray = Encoding.ASCII.GetBytes($"{rkUserName}:{rkPassword}");
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        private string GetXMLRequest(RK7Query request)
        {
            using (var textWriter = new StringWriter())
            {
                var serializer = new XmlSerializer(request.GetType());
                serializer.Serialize(textWriter, request);
                return textWriter.ToString();
            }
        }

        private RK7QueryResult SendRequest(string requestHost, RK7QueryRK7CMD requestObject, Type requestResultType)
        {
            // curl -k -u user:pass -i -X POST -H "Content-Type: text/xml" -d @comand.xml "http://xxx.xxx.xxx.xxx.:2237/rk7api/v0/xmlinterface.xml"

            var query = new RK7Query { RK7CMD = requestObject };

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestHost)
            {
                Content = new StringContent(GetXMLRequest(query),
                    Encoding.GetEncoding(1251),
                    "text/xml")
            };

            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            var responce = Client.SendAsync(httpRequest);
            responce.Wait();
            var responceContent = responce.Result.Content;

            var serializer = new XmlSerializer(requestResultType);
            var streamTask = responceContent.ReadAsStreamAsync();
            streamTask.Wait();
            return serializer.Deserialize(streamTask.Result) as RK7QueryResult;
        }

        public GetWaiterListQueryResult GetWaiterList(bool registeredOnly)
        {
            var getWaiterListRequest = new GetWaiterListCMD { registeredOnly = registeredOnly };
            return SendRequest(InterfacePath, getWaiterListRequest, typeof(GetWaiterListQueryResult)) as GetWaiterListQueryResult;
        }

        public GetOrderListQueryResult GetOrderList()
        {
            var getOrderListRequest = new GetOrderListCMD { needNames = 1 };
            return SendRequest(InterfacePath, getOrderListRequest, typeof(GetOrderListQueryResult)) as GetOrderListQueryResult;
        }

        public GetSystemInfo2QueryResult GetSystemInfo2()
        {
            var getOrderListRequest = new GetSystemInfo2CMD();
            return SendRequest(InterfacePath, getOrderListRequest, typeof(GetSystemInfo2QueryResult)) as GetSystemInfo2QueryResult;
        }
    }
}
