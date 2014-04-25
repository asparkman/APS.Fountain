using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data
{
    public class Identify : BaseMessage, IBytable
    {
        public override byte[] Position()
        {
            return GetBasePositionArray();
        }

        public override byte Type { get { return (byte)MessageType.Identify; } }
        public override int NonControlBytes { get { return 0; } }
    }
}
