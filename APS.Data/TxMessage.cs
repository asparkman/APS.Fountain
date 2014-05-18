using System;
using System.Collections.Generic;
using System.Globalization;
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
            this[TxField.INFO_0] = (byte)Type;
            this[TxField.ESCAPE] = (byte)(3 << 6);
        }

        public TxMessage(byte[] vals)
            : base(vals)
        {
        }

        public override byte this[TxField index]
        {
            get
            {
                byte result = _Bytes[(int) index];
                if (
                        // Positioned in a place that needs unescaping.
                        (int)index > (int)TxField.ESCAPE && (int)index != (int)TxField.STOP
                    )
                {
                    var escapeBitIndex = (int)index - (int)TxField.ESCAPE - 1;
                    var readingMask = (byte)(byte.MinValue + (byte)(1 << ((byte)escapeBitIndex)));
                    var isEscaped = (_Bytes[(int)TxField.ESCAPE] & readingMask) > 0;
                    if(isEscaped)
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
                        (int)index > (int)TxField.ESCAPE && (int)index != (int)TxField.STOP
                        // Needs escaping
                        && (value == _START || value == _END)
                    )
                {
                    var escapeBitIndex = (int)index - (int)TxField.ESCAPE - 1;
                    var preservationMask = (byte)(byte.MaxValue - (byte)(1 << ((byte)escapeBitIndex)));
                    _Bytes[(int)TxField.ESCAPE] = (byte)((byte)(preservationMask & _Bytes[(int)TxField.ESCAPE]) + (byte)(1 << ((byte)escapeBitIndex)));
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
                return (byte)((this[TxField.SIZE_SEQ] << ((byte)3)) >> ((byte)3));
            }
            set
            {
                this[TxField.SIZE_SEQ] = (byte)((Size << ((byte)5)) + value);
            }
        }
    }
}
