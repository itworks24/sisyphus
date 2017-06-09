using System;
using System.Security;

namespace Sisyphus.ActiveDirectory
{
    namespace Exceptions
    {
        public class ConnectionErrorException : Exception
        {

        }
    }

    public interface IActiveDirectory
    {
        string UserName { get; }
        string Domain { get; }
        string Password { get; }

        bool Connected { get; }

        void Connect(string domainName, string userName, string password);
    }

    public class ActiveDirecotoryHelper : IActiveDirectory
    {
        private bool _connected;
        public bool Connected => _connected;

        private string _domain;
        public string Domain => _domain;

        private string _password;
        public string Password => _password;

        private string _userName;
        public string UserName => _userName;

        internal bool ConnectToActiveDirectory()
        {
            return true;
        }

        public void Connect(string domain, string userName, string password)
        {
            _domain = domain;
            _userName = userName;
            _password = password;
            _connected = ConnectToActiveDirectory();
        }
    }
}