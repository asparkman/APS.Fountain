using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data
{
    public abstract class RxMessage : Message<RxType, RxField>
    {
        public RxMessage()
            : base()
        {
            this[RxField.SIZE_SEQ] = (byte) (Size << ((byte)5));
            this[RxField.INFO_0] = (byte)Type;
        }

        public override byte Sequence
        { 
            get 
            {
                return (byte) ((this[RxField.SIZE_SEQ] << ((byte)3)) >> ((byte)3)); 
            } 
            set 
            {
                this[RxField.SIZE_SEQ] = (byte)((Size << ((byte)5)) + value); 
            } 
        }

    }
}
