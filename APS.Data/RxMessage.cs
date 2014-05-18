using System;
using System.Collections.Generic;
using System.Globalization;
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
            this[RxField.ESCAPE] = (byte)(3 << 6);
        }

        public RxMessage(byte[] vals)
            : base(vals)
        {
        }

        public override byte this[RxField index]
        {
            get
            {
                byte result = _Bytes[(int)index];
                if (
                    // Positioned in a place that needs unescaping.
                        (int)index > (int)RxField.ESCAPE && (int)index != (int)RxField.STOP
                    )
                {
                    var escapeBitIndex = (int)index - (int)RxField.ESCAPE - 1;
                    var readingMask = (byte)(byte.MinValue + (byte)(1 << ((byte)escapeBitIndex)));
                    var isEscaped = (_Bytes[(int)RxField.ESCAPE] & readingMask) > 0;
                    if (isEscaped)
                        result = (byte)(_Bytes[(int)index] + ((byte)1));
                    else
                        result = _Bytes[(int)index];
                }

                return result;
            }
            set
            {
                if (
                    // Positioned in a place that needs escaping.
                        (int)index > (int)RxField.ESCAPE && (int)index != (int)RxField.STOP
                    // Needs escaping
                        && (value == _START || value == _END)
                    )
                {
                    var escapeBitIndex = (int)index - (int)RxField.ESCAPE - 1;
                    var preservationMask = (byte)(byte.MaxValue - (byte)(1 << ((byte)escapeBitIndex)));
                    _Bytes[(int)RxField.ESCAPE] = (byte)((byte)(preservationMask & _Bytes[(int)RxField.ESCAPE]) + (byte)(1 << ((byte)escapeBitIndex)));
                    _Bytes[(int)index] = (byte)(value - 1);
                }
                else
                    _Bytes[(int)index] = value;
            }
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
