using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MiceSharp.Hooks
{
    public class Hook
    {
        [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern uint GetLastError();
        public Process hooktransformice()
        {
            string ProcessName;
            Process Mice;
            ProcessName = "Transformice";
            Mice = Process.GetProcessesByName(ProcessName)[0];
            return Mice;
        }
    }


}