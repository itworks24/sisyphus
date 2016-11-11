using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Sisyphus.Common
{
    public static class LogonControll
    {
        public struct UserAccount
        {
            public bool Equals(UserAccount other)
            {
                return string.Equals(UserName, other.UserName) && string.Equals(Domain, other.Domain);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is UserAccount && Equals((UserAccount) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((UserName?.GetHashCode() ?? 0)*397) ^ (Domain?.GetHashCode() ?? 0);
                }
            }

            public string UserName { get; private set; }
            public string Domain { get; private set; }

            public string AccountName => $"{UserName}@{Domain}";

            public UserAccount(string accountNameRepresent)
            {
                Domain = accountNameRepresent.GetDomain().ToLower();
                UserName = accountNameRepresent.GetUserName().ToLower();
            }

            public static bool operator ==(UserAccount source, UserAccount dest)
            {
                return source.UserName == dest.UserName &&
                       (source.Domain.StartsWith(dest.Domain) || dest.Domain.StartsWith(source.Domain));
            }

            public static bool operator !=(UserAccount source, UserAccount dest)
            {
                return !(source == dest);
            }
        }
    }
}