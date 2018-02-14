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
            DotNetScanMemory_SmoLL scan = new DotNetScanMemory_SmoLL();
            List<string> Aobs = new List<string>();

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
                    // Console.WriteLine($"[AOB] {split[0]}\n[TROCA] {split[1]}");
                }

                for (int a = 0; a < AobsInject.Count; a++)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    scan.WriteArray(AobsInject[a][0], Trocas[a]);
                    Console.WriteLine($"[{AobsInject[a][0]}] Troca: " + Trocas[a] + " => Sucess.");
                    
                }
                
                Console.Beep();
                Console.ReadLine();
            }
        }
    }
}
