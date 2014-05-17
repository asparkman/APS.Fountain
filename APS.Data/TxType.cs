using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data
{
    public enum TxType : byte
    {
        IDENTIFY = 0,
        NOTE = 1,
        PAUSE = 2,
        NO_TONE_TIME = 3,
        TONE_TIME = 4
    }
}
