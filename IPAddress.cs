using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLSMSplitTool
{
    class IPAddress
    {
        public IPAddress(int Octet1, int Octet2, int Octet3, int Octet4)
        {
            octet1 = Octet1;
            octet2 = Octet2;
            octet3 = Octet3;
            octet4 = Octet4;
            address = Octet1.ToString() + "." + Octet2.ToString() + "." + Octet3.ToString() + "." + Octet4.ToString();
        }

        public int octet1;
        public int octet2;
        public int octet3;
        public int octet4;
        public string address;
        
    }
}
