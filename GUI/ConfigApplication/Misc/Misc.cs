using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConfigApplication
{
    static class Misc
    {
        [DllImport("msvcr110.dll", CallingConvention = CallingConvention.Cdecl)]
        private  static extern int _fpreset();

        public static int FPReset()
        {
            return _fpreset();
        }
    }
}
