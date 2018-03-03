using System;

namespace MiceSharp.MemStruct
{
    class MemoryStruct
    {
        // the struct needs to be out of the class c:
    }

    struct MEMORY_BASIC_INFORMATION
    {

        public IntPtr BaseAddress;


        public IntPtr AllocationBase;


        public uint AllocationProtect;


        public uint RegionSize;


        public uint State;


        public uint Protect;

        public uint Type;
    }
}
