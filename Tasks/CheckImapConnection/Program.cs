using System;
using System.Threading;

namespace Sisyphus
{

    static class Program
    {
        static void Main()
        {

            try
            {
                while (true)
                {
                    new CheckImapConnection().ExecuteProcess();
                    Thread.Sleep(5000);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
