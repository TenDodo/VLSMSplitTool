using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VLSMSplitTool
{
    class Program
    {
        const int classAhosts = 16_777_216;
        const int classBhosts = 65_536;
        const int classChosts = 256;

        public static string decToBin(int num)
        {
            return Convert.ToString(num, 2);
        }

        public static int binToDec(string num)
        {
            return Convert.ToInt32(num, 2);
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to NetworkSplitTool by Arkadiusz Kozłowski");
            Start();
        }

        static void Start()
        {
            List<Subnet> subnets;
            List<int[]> hostsCountList = new List<int[]>();
            char _networkClass = default(char);
            int hostsLeft = default(int);
        netClass:
            Console.WriteLine("Type network class letter (A, B or C)");
            string temp = Console.ReadLine();
            if (temp.Length > 1 || temp.Length < 1)
            {
                Console.WriteLine("Try again");
                goto netClass;
            }
            else if (temp.ToUpper() == "D" || temp.ToUpper() == "E")
            {
                Console.WriteLine("Classes \'D\' and \'E\' are not supported");
                goto netClass;
            }
            else if (temp.ToUpper() != "A" && temp.ToUpper() != "B" && temp.ToUpper() != "C")
            {
                Console.WriteLine("Wrong number, try again");
                goto netClass;
            }
            else
            {
                _networkClass = temp.ToUpper()[0];
            }
            if (_networkClass == 'A') hostsLeft = classAhosts;
            if (_networkClass == 'B') hostsLeft = classBhosts;
            if (_networkClass == 'C') hostsLeft = classChosts;
            int loop = 1;
            while (hostsLeft != 0)
            {
            backLoop:
                Console.WriteLine("Type number of hosts in " + numberGenerator(loop) + " subnet (" + (hostsLeft - 2).ToString() + " addresses left, leave empty if you want to use all available spaces)");
                string noa = Console.ReadLine();
                int parseHelper1;
                try
                {
                    parseHelper1 = int.Parse(noa);
                }
                catch
                {
                    parseHelper1 = 1;
                }
                if (!noa.All(char.IsDigit) || parseHelper1 < 1 || parseHelper1 > hostsLeft -2 || hostsNumberGenerator(parseHelper1) > hostsLeft)
                {
                    Console.WriteLine("Wrong number, try again");
                    goto backLoop;
                }
                else if (noa == string.Empty)
                {
                    while (hostsLeft != 0)
                    {
                        if (hostsNumberGenerator(hostsLeft) > hostsLeft)
                        {
                            hostsCountList.Add(new int[] { hostsNumberGenerator(hostsLeft / 2) - 2, hostsNumberGenerator(hostsLeft / 2) - 2 });
                            hostsLeft = hostsLeft - hostsNumberGenerator(hostsLeft / 2);
                            loop++;
                        }
                        else
                        {
                            hostsCountList.Add(new int[] { hostsNumberGenerator(hostsLeft) - 2, hostsNumberGenerator(hostsLeft) - 2 });
                            hostsLeft = 0;
                            loop++;
                        }                       
                    }
                    
                }
                else
                {
                    hostsCountList.Add(new int[] { hostsNumberGenerator(parseHelper1) - 2, parseHelper1 });                    
                    hostsLeft = hostsLeft - hostsNumberGenerator(parseHelper1);
                    loop++;
                }
                
            }
            subnets = subnetList(hostsCountList, _networkClass);
            int l = 1;
            string space = "     ";
            Console.Clear();
            double totalNeeded = 0;
            double totalAvailable = 0;
            foreach (Subnet subnet in subnets)
            {
                Console.WriteLine(numberGenerator(l) + " subnet:");
                Console.WriteLine(space + "Subnet address: " + subnet.iPAddress.octet1 + "." + subnet.iPAddress.octet2 + "." + subnet.iPAddress.octet3 + "." + subnet.iPAddress.octet4);
                Console.WriteLine(space + "Subnet mask: " + subnet.subnetMaskIP.octet1 + "." + subnet.subnetMaskIP.octet2 + "." + subnet.subnetMaskIP.octet3 + "." + subnet.subnetMaskIP.octet4 + "   (/" + subnet.subnetMask + ")");
                Console.WriteLine(space + "Assignable Range: " + subnet.assignableRange);
                Console.WriteLine(space + "Broadcast: " + subnet.broadcast.octet1 + "." + subnet.broadcast.octet2 + "." + subnet.broadcast.octet3 + "." + subnet.broadcast.octet4);
                Console.WriteLine(space + "Needed/Allocated: " + subnet.neededHosts + "/" + subnet.availableHosts);
                Console.WriteLine();
                l++;
                totalNeeded += subnet.neededHosts;
                totalAvailable += subnet.availableHosts;
            }
            double percent = totalNeeded / totalAvailable * 100;
            Console.WriteLine("Summary: " + percent + "% of subnetted network will be used by needed addresses");
            Console.WriteLine("\nWould you like to do other calculations? (y/n)");
            string answer = Console.ReadLine();
            if (answer.ToLower() == "y" || answer.ToLower() == "yes" || answer.ToLower() == "t" || answer.ToLower() == "tak")
            {
                Console.Clear();
                Start();
            }
            else
            {
                Console.WriteLine("VLSMSplitTool by Arkadiusz Kozłowski\npress any key to exit");
                Console.ReadKey();
            }
            
        }

      

        static string numberGenerator(int num)
        {
            if (num == 1)
            {
                return "1st";
            }
            else if (num == 2)
            {
                return "2nd";
            }
            else if (num == 3)
            {
                return "3rd";
            }
            else
            {
                return num.ToString() + "th";
            }
        }

        static int hostsNumberGenerator(int hosts) {
            if (hosts == 1 || hosts == 2) return 4;
            if (hosts > 2 && hosts <= 6) return 8;
            if (hosts > 6 && hosts <= 14) return 16;
            if (hosts > 14 && hosts <= 30) return 32;
            if (hosts > 30 && hosts <= 62) return 64;
            if (hosts > 62 && hosts <= 126) return 128;
            if (hosts > 126 && hosts <= 254) return 256;
            if (hosts > 254 && hosts <= 510) return 512;
            if (hosts > 510 && hosts <= 1022) return 1024;
            if (hosts > 1022 && hosts <= 2046) return 2048;
            if (hosts > 2046 && hosts <= 4094) return 4096;
            if (hosts > 4094 && hosts <= 8190) return 8192;
            if (hosts > 8190 && hosts <= 16382) return 16384;
            if (hosts > 16382 && hosts <= 32766) return 32768;
            if (hosts > 32766 && hosts <= 65534) return 65536;
            if (hosts > 65534 && hosts <= 131070) return 131072;
            if (hosts > 131070 && hosts <= 262142) return 262144;
            if (hosts > 262142 && hosts <= 524286) return 524288;
            if (hosts > 524286 && hosts <= 1048574) return 1048576;
            if (hosts > 1048574 && hosts <= 2097150) return 2097152;
            if (hosts > 2097150 && hosts <= 4194302) return 4194304;
            if (hosts > 4194302 && hosts <= 8388606) return 8388608;
            if (hosts > 8388606 && hosts <= 16777214) return 16777216;
            return default(int);
        }


        static string hostsNumToIp(int hosts)
        {

            return string.Join(".", BitConverter.GetBytes(hosts).Reverse());


        }

        static int iPToHostsNum(IPAddress ip, char netclass)
        {
            if (netclass == 'A')
            {
                return ip.octet2 * 256 * 256 + ip.octet3 * 256 + ip.octet4;
            }
            else if (netclass == 'B')
            {
                return ip.octet3 * 256 + ip.octet4;
            }
            else
            {
                return ip.octet4;
            }
        }

        

        
        static List<Subnet> subnetList(List<int[]> receivedData, char netclass)
        {
            List<Subnet> final = new List<Subnet>();
            IPAddress previousIP;
            if (netclass == 'A')
            {
                previousIP = new IPAddress(10, 0, 0, 0);
            }
            else if (netclass == 'B')
            {
                previousIP = new IPAddress(172, 16, 0, 0);
            }
            else
            {
                previousIP = new IPAddress(192, 168, 0, 0);
            }
            foreach (int[] data in receivedData)
            {
                IPAddress actualIP;
                int subnIP1Octet = default(int);
                int subnIP2Octet = default(int);
                int subnIP3Octet = default(int);
                int subnIP4Octet = default(int);
                
                if (netclass == 'A')
                {
                    int n = iPToHostsNum(previousIP, netclass);
                    n += data[0] + 2;
                    string[] st = hostsNumToIp(n).Split('.');
                    actualIP = new IPAddress(previousIP.octet1, int.Parse(st[1]), int.Parse(st[2]), int.Parse(st[3]));
                    final.Add(new Subnet(previousIP, 31 - (int)Math.Log(data[0], 2), data[1]));
                    previousIP = actualIP;
                }
                else if(netclass == 'B')
                {
                    int n = iPToHostsNum(previousIP, netclass);
                    n += data[0] + 2;
                    string[] st = hostsNumToIp(n).Split('.');
                    actualIP = new IPAddress(previousIP.octet1, previousIP.octet2, int.Parse(st[2]), int.Parse(st[3]));
                    final.Add(new Subnet(previousIP, 31 - (int)Math.Log(data[0], 2), data[1]));
                    previousIP = actualIP;
                }
                else
                {
                    int n = iPToHostsNum(previousIP, netclass);
                    n += data[0] + 2;
                    string[] st = hostsNumToIp(n).Split('.');
                    actualIP = new IPAddress(previousIP.octet1, previousIP.octet2, previousIP.octet3, int.Parse(st[3]));
                    final.Add(new Subnet(previousIP, 31 - (int)Math.Log(data[0], 2), data[1]));
                    previousIP = actualIP;
                }
            }
            return final;
        }
    }
}
