using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data
{
    public abstract class TxMessage : Message<TxType, TxField>
    {
        public TxMessage()
            : base()
        {
            this[TxField.SIZE_SEQ] = (byte)(Size << ((byte)5));
            this[TxField.INFO_0] = (byte) Type;
        }

        public override byte Sequence
        {
            get
            {
                return (byte)((this[TxField.SIZE_SEQ] << ((byte)3)) >> ((byte)3));
            }
            set
            {
                this[TxField.SIZE_SEQ] = (byte)((Size << ((byte)5)) + value);
            }
        }
    }
}
