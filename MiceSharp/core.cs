using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using MiceSharp.Welcome;
using MiceSharp.Hooks;
using MiceSharp.Bypass;

namespace MiceSharpCore
{
    class Core
    {

        [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern uint GetLastError();

        public void Initialize()
        {
            Bypass bypass = new Bypass();
            Welcome welcomeprint = new Welcome();
            welcomeprint.printwelcome();

            Hook hookfunc = new Hook();
            Process Mice = hookfunc.hooktransformice();


            Console.WriteLine("=========================================");
            Console.WriteLine("             Initializing...             ");
            Console.WriteLine("=========================================");
            DotNetScanMemory_SmoLL scan = new DotNetScanMemory_SmoLL();
            bypass.dobypass();

            List<string> Aobs = new List<string>();

            List<int> AddToAddy = new List<int>();

            List<string> Trocas = new List<string>();

            List<IntPtr[]> AobsInject = new List<IntPtr[]>();


            try
            {
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


                        AobsInject.Add(scan.ScanArray(Mice, split[0]));
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
            catch
            {
                Console.WriteLine("Core failed to initialize. \n Check your Aobs.JSON");
                Console.ReadLine();
            }
        }







    }

}