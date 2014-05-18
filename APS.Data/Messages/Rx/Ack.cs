using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Messages.Rx
{
    public class Ack : RxMessage
    {
        public Ack()
            : base()
        {
        }

        public Ack(byte[] vals)
            : base(vals)
        {
        }

        public override RxType Type { get { return RxType.ACK; } }
        public override byte Size { get { return 1; } }

        public override object Clone()
        {
            return CloneTo(new Ack());
        }
    }
}
