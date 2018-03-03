using System;
using System.Collections.Generic;
using System.Linq;
using MiceSharp.Kernel;
using MiceSharp.Enums;
using MiceSharp.MemStruct;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MiceSharp.Engine
{
    public class MemoryEngine
    {





        private List<MEMORY_BASIC_INFORMATION> MappedMemory { get; set; }


        public static string GetSystemMessage(uint errorCode)
        {
            return new Win32Exception((int)errorCode).Message;
        }


        protected void MemInfo(IntPtr pHandle)
        {
            IntPtr intPtr = (IntPtr)((long)this.InicioScan);
            while ((long)intPtr <= (long)this.FimScan)
            {
                MEMORY_BASIC_INFORMATION memory_BASIC_INFORMATION = default(MEMORY_BASIC_INFORMATION);
                if (Kernel__.VirtualQueryEx(pHandle, intPtr, out memory_BASIC_INFORMATION, Marshal.SizeOf(memory_BASIC_INFORMATION)) == 0)
                {
                    break;
                }
                if ((memory_BASIC_INFORMATION.State & 4096u) != 0u && (memory_BASIC_INFORMATION.Protect & 256u) != 256u)
                {
                    this.MappedMemory.Add(memory_BASIC_INFORMATION);
                }
                intPtr = new IntPtr(memory_BASIC_INFORMATION.BaseAddress.ToInt32() + (int)memory_BASIC_INFORMATION.RegionSize);
            }
        }

        protected IntPtr ScanInBuff(IntPtr Address, byte[] Buff, string[] StrMask)
        {
            int num = Buff.Length;
            int num2 = StrMask.Length;
            int num3 = num2 - 1;
            byte[] array = new byte[num2];
            for (int i = 0; i < num2; i++)
            {
                if (StrMask[i] == "??")
                {
                    array[i] = 0;
                }
                else
                {
                    array[i] = Convert.ToByte(StrMask[i], 16);
                }
            }
            for (int j = 0; j <= num - num2 - 1; j++)
            {
                if (Buff[j] == array[0])
                {
                    int num4 = num3;
                    while (StrMask[num4] == "??" || Buff[j + num4] == array[num4])
                    {
                        if (num4 == 0)
                        {
                            if (this.StopTheFirst)
                            {
                                return new IntPtr(j);
                            }
                            if ((long)(Address.ToInt32() + j) >= (long)this.InicioScan && (long)(Address.ToInt32() + j) <= (long)this.FimScan)
                            {
                                this.AddressList.Add((IntPtr)(Address.ToInt32() + j));
                                break;
                            }
                            break;
                        }
                        else
                        {
                            num4--;
                        }
                    }
                }
            }
            return IntPtr.Zero;
        }

        public Process GetPID(string ProcessName)
        {
            try
            {
                return Process.GetProcessesByName(ProcessName)[0];
            }
            catch
            {
            }
            return null;
        }

        public IntPtr[] ScanArray(Process P, string ArrayString)
        {
            EnablePrivileges.GoDebugPriv();
            IntPtr[] array = new IntPtr[1];
            if (P == null)
            {
                return new IntPtr[1];
            }
            this.Attacked = Process.GetProcessById(P.Id);
            string[] array2 = ArrayString.Split(new char[]
            {
            " "[0]
            });
            for (int i = 0; i < array2.Length; i++)
            {
                if (array2[i] == "?")
                {
                    array2[i] = "??";
                }
            }
            this.MappedMemory = new List<MEMORY_BASIC_INFORMATION>();
            this.MemInfo(this.Attacked.Handle);
            for (int j = 0; j < this.MappedMemory.Count; j++)
            {
                byte[] array3 = new byte[this.MappedMemory[j].RegionSize];
                Kernel__.ReadProcessMemory(this.Attacked.Handle, this.MappedMemory[j].BaseAddress, array3, this.MappedMemory[j].RegionSize, 0);
                IntPtr value = IntPtr.Zero;
                if (array3.Length != 0)
                {
                    value = this.ScanInBuff(this.MappedMemory[j].BaseAddress, array3, array2);
                }
                if (this.StopTheFirst && value != IntPtr.Zero)
                {
                    array = new IntPtr[0];
                    array[0] = (IntPtr)(this.MappedMemory[j].BaseAddress.ToInt32() + value.ToInt32());
                    return array;
                }
            }
            if (!this.StopTheFirst && this.AddressList.Count > 0)
            {
                array = new IntPtr[this.AddressList.Count];
                for (int k = 0; k < this.AddressList.Count; k++)
                {
                    array[k] = this.AddressList[k];
                }
                this.AddressList.Clear();
                return array;
            }
            return array;
        }

        public bool WriteArray(IntPtr address, string ArrayString)
        {
            if (this.Attacked == null)
            {
                return false;
            }
            string[] array = ArrayString.Split(new char[]
            {
            " "[0]
            });
            byte[] array2 = new byte[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == "?" || array[i] == "??")
                {
                    array2[i] = 0;
                }
                else
                {
                    array2[i] = Convert.ToByte(array[i], 16);
                }
            }
            return Kernel__.WriteProcessMemory((int)this.Attacked.Handle, address.ToInt32(), array2, array2.Length, 0);
        }




        private uint PROCESS_ALL_ACCESS = 127231u;


        public ulong InicioScan;


        public ulong FimScan = 4294967295;


        private bool StopTheFirst;


        private Process Attacked;


        private List<IntPtr> AddressList = new List<IntPtr>();

    }

}
