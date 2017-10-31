using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sysiphus.Tasks.Roseltorg
{
    public class File
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Procedure
    {
        public string name { get; set; }
        public string code { get; set; }
        public string type { get; set; }
        public string contractor { get; set; }
        public string contactName { get; set; }
        public string contactInfo { get; set; }
        public string organizer { get; set; }
        public List<File> files { get; set; }

        public Procedure()
        {
            files = new List<File>();
        }
    }
}
