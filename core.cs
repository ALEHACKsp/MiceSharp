using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AobAutoInjection
{
    class Program
    {


        static void Main(string[] args)
        {
            
           
            Console.Title = "MiceSharp";
            Console.WriteLine("=========================================");
            Console.WriteLine("             Mice Sharp Logger           ");
            Console.WriteLine("=========================================");
            DotNetScanMemory_SmoLL scan = new DotNetScanMemory_SmoLL();
            IntPtr[] adobeairaddy;
            string adobe_air_dll_check = "6E 00 0A 00 76 00";
            string bypass_adobe_air = "11 13 00 00 60 AB 08 66 85 32 60 D1 08 66 BC 40 61 BC 08 10 62 00 00";
            adobeairaddy = scan.ScanArray(scan.GetPID("Transformice"), adobe_air_dll_check);
            scan.WriteArray(adobeairaddy[0], bypass_adobe_air);

            List<string> Aobs = new List<string>();

            List<int> AddToAddy = new List<int>();

            List<string> Trocas = new List<string>();

            List<IntPtr[]> AobsInject = new List<IntPtr[]>();



            using (StreamReader sr = new StreamReader("aobs.json"))
            {
                Console.ForegroundColor = ConsoleColor.White;
                var Json = sr.ReadToEnd();
                JObject jsonObject = JObject.Parse(Json);

                for (int i = 1; i < int.Parse(jsonObject["countAobs"].ToString()) + 1; i++)
                {
                    Aobs.Add(jsonObject[$"{i}"].ToString());
                }

                for (int i = 0; i < Aobs.Count; i++)
                {

                    string[] split = Aobs[i].Split(',');


                    AobsInject.Add(scan.ScanArray(scan.GetPID("Transformice"), split[0]));
                    Trocas.Add(split[1]);
                    try
                    {
                        if (split[2] == null)
                        {
                            AddToAddy.Add(0);
                        }
                        else
                        {
                            string PlusAddy = split[2];
                            int addtoaddy = Int32.Parse(PlusAddy);
                            AddToAddy.Add(addtoaddy);
                        }
                    }
                    catch
                    {
                        AddToAddy.Add(0);
                    }
                    

                }

                for (int a = 0; a < AobsInject.Count; a++)
                {
                    int nulle = 0;
                    IntPtr NullePTR = new IntPtr(nulle);
                    

                    try
                    {
                        if (AobsInject[a][0] == NullePTR)
                        {
                            Console.WriteLine("[X] Pattern Scan Failed, falling back");
                        }
                        else
                        {
                            Console.WriteLine("[+] Address Logged =>" + $" [0x{AobsInject[a][0].ToString("X")}]");
                            Console.WriteLine("[+] Writing " + Trocas[a] + " to it.");
                            int plus = AddToAddy[0];
                            AobsInject[a][0] += plus;
                            scan.WriteArray(AobsInject[a][0], Trocas[a]);
                            
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Fatal Error, check your aobs.json!");
                    }
                    

                }

                Console.Beep();
                Console.ReadLine();
            }
        }
        
    }
}
