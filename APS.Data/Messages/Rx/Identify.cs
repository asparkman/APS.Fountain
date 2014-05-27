using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Messages.Rx
{
    public class Identify : RxMessage
    {
        #pragma warning disable 1591
        public Identify()
            : base()
        {
        }

        public Identify(byte[] vals)
            : base(vals)
        {
        }

        public override RxType Type { get { return RxType.IDENTIFY; } }
        public override byte Size { get { return 3; } }

        public byte Random0 { get { return this[RxField.INFO_1]; } set { this[RxField.INFO_1] = value; } }
        public byte Random1 { get { return this[RxField.INFO_2]; } set { this[RxField.INFO_2] = value; } }

        public override object Clone()
        {
            return CloneTo(new Identify());
        }
        #pragma warning restore 1591
    }
}
