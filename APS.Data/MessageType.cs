using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data
{
    public enum MessageType : byte
    {
        Identify = 0x1,
        SendNote = 0x2,
        SendNoToneTime = 0x3,
        SendPause = 0x4,
        SendToneTime = 0x5
    }
}
