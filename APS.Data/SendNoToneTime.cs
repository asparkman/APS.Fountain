using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data
{
    public class SendNoToneTime : BaseMessage, IBytable
    {
        public override byte[] Position()
        {
            var result = GetBasePositionArray();

            result[2] = (byte)(Milliseconds >> 8);
            result[3] = (byte)(Milliseconds % 256);

            return result;
        }

        public override byte Type { get { return (byte)MessageType.SendNoToneTime; } }
        public override int NonControlBytes { get { return 2; } }

        public virtual UInt16 Milliseconds { get; set; }
    }
}
