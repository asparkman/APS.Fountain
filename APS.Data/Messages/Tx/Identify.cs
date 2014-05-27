using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Messages.Tx
{
    public class Identify : TxMessage
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

        public override TxType Type { get { return TxType.IDENTIFY; } }
        public override byte Size { get { return 3; } }

        public byte Random0 { get { return this[TxField.INFO_1]; } set { this[TxField.INFO_1] = value; } }
        public byte Random1 { get { return this[TxField.INFO_2]; } set { this[TxField.INFO_2] = value; } }

        public override object Clone()
        {
            return CloneTo(new Identify());
        }
        #pragma warning restore 1591
    }
}
