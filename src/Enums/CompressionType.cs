using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM
{
    public enum CompressionType
    {
        NULL_BACKGROUND = 0,
        NULL_FOREGROUND = 1,
        T6 = 2,        
        T4_1 = 3,        
        T4_2 = 4,        
        BITMAP = 5,
        RUN_LENGTH = 6,
        BASELINE_JPEG = 7,
        LZW = 8,        
        PNG = 9
    }
}
