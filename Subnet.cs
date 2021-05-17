using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLSMSplitTool
{
    class Subnet
    {
        public Subnet(IPAddress SubnetIP, int SubnetMask, int NeededSize)
        {
            iPAddress = SubnetIP;
            subnetMask = SubnetMask;
            neededHosts = NeededSize;
            string binMask = new string('1', SubnetMask) + new string('0', 32 - SubnetMask);
            subnetMaskIP = new IPAddress(Program.binToDec(binMask.Substring(0, 8)), Program.binToDec(binMask.Substring(8, 8)), Program.binToDec(binMask.Substring(16, 8)), Program.binToDec(binMask.Substring(24, 8)));
            broadcast = new IPAddress(~Convert.ToByte(subnetMaskIP.octet1) + 256 + iPAddress.octet1, ~Convert.ToByte(subnetMaskIP.octet2) + 256 + iPAddress.octet2, ~Convert.ToByte(subnetMaskIP.octet3) + 256 + iPAddress.octet3, ~Convert.ToByte(subnetMaskIP.octet4) + 256 + iPAddress.octet4);
            minHost = new IPAddress(iPAddress.octet1, iPAddress.octet2, iPAddress.octet3, iPAddress.octet4 + 1);
            maxHost = new IPAddress(broadcast.octet1, broadcast.octet2, broadcast.octet3, broadcast.octet4 - 1);
            availableHosts = (int)Math.Pow(2, 32 - SubnetMask) - 2;
            assignableRange = minHost.octet1 + "." + minHost.octet2 + "." + minHost.octet3 + "." + minHost.octet4 + " - " + maxHost.octet1 + "." + maxHost.octet2 + "." + maxHost.octet3 + "." + maxHost.octet4;
        }
        public IPAddress iPAddress;
        public int subnetMask;
        public IPAddress subnetMaskIP;
        public IPAddress broadcast;
        public IPAddress minHost;
        public IPAddress maxHost;
        public int neededHosts;
        public int availableHosts;
        public string assignableRange;


    }
}
