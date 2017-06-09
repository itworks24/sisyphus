using System;
using System.Windows.Controls;

namespace Sisyphus.Common
{
    public static class Extensions
    {
        private static string[] separators => new[] { "\\", "@" };

        private static int SeparatorPosition(string source)
        {
            foreach (var separator in separators)
            {
                var index = source.IndexOf(separator);
                if (index > 0) return index;
            }
            return -1;
        }

        public static string GetDomain(this string identity)
        {
            var oldStyle = identity.Contains("\\");
            var splitArray = oldStyle ? identity.Split('\\') : identity.Split('@');
            var index = oldStyle ? 0 : 1;
            return index < splitArray.Length ? splitArray[index] : "";
        }

        public static string GetUserName(this string identity)
        {
            var oldStyle = identity.Contains("\\");
            var splitArray = oldStyle ? identity.Split('\\') : identity.Split('@');
            var index = oldStyle ? 1 : 0;
            return index < splitArray.Length ? splitArray[index] : "";
        }
    }
}