using System;

namespace MiceSharp.Bypass
{
    public class Bypass
    {
   
        public void dobypass()
        {
            DotNetScanMemory_SmoLL scan = new DotNetScanMemory_SmoLL();
            IntPtr[] adobeairaddy;
            string adobe_air_dll_check = "6E 00 0A 00 76 00";
            string bypass_adobe_air = "11 13 00 00 60 AB 08 66 85 32 60 D1 08 66 BC 40 61 BC 08 10 62 00 00";
            adobeairaddy = scan.ScanArray(scan.GetPID("Transformice"), adobe_air_dll_check);
            scan.WriteArray(adobeairaddy[0], bypass_adobe_air);
        }
    }


}