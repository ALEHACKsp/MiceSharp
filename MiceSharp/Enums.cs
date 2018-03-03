using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiceSharp.Enums
{

    public enum AllocationProtectEnum : uint
    {
        PAGE_EXECUTE = 16u,

        PAGE_EXECUTE_READ = 32u,

        PAGE_EXECUTE_READWRITE = 64u,

        PAGE_EXECUTE_WRITECOPY = 128u,

        PAGE_NOACCESS = 1u,

        PAGE_READONLY,

        PAGE_READWRITE = 4u,

        PAGE_WRITECOPY = 8u,

        PAGE_GUARD = 256u,

        PAGE_NOCACHE = 512u,

        PAGE_WRITECOMBINE = 1024u
    }


    public enum StateEnum : uint
    {

        MEM_COMMIT = 4096u,

        MEM_FREE = 65536u,

        MEM_RESERVE = 8192u
    }


    public enum TypeEnum : uint
    {

        MEM_IMAGE = 16777216u,

        MEM_MAPPED = 262144u,

        MEM_PRIVATE = 131072u
    }


}
