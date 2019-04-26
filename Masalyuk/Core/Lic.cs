using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Masalyuk.Core
{
    class Lic
    {
        public static Dictionary<string, string> ids =
            new Dictionary<string, string>();

       public static ManagementObjectSearcher searcher;
        public static string lol { get; set; }
        public string GetPay()
        {
            searcher = new ManagementObjectSearcher("root\\CIMV2",
                "SELECT UUID FROM Win32_ComputerSystemProduct");
            foreach (ManagementObject queryObj in searcher.Get())
                ids.Add("UUID", queryObj["UUID"].ToString());
            lol = ids["UUID"];
            lol = lol.Replace("-", "");
            return lol;
        }
    }
}
