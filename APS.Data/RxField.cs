using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data
{
    public enum RxField : int
    {
        START = 0,
        ESCAPE = 1,
        SIZE_SEQ = 2,
        INFO_0 = 3,
        INFO_1 = 4,
        INFO_2 = 5,
        STOP = 6
    }
}
