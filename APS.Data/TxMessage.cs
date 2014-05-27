using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data
{
    /// <summary>
    /// Represents a message sent to the Arduino.
    /// </summary>
    public abstract class TxMessage : Message<TxType, TxField>
    {
        /// <summary>
        /// Creates a bare bones message that may be sent to the Arduino.
        /// </summary>
        public TxMessage()
            : base()
        {
            this[TxField.SIZE_SEQ] = (byte)(Size << ((byte)5));
            this[TxField.INFO_0] = (byte)Type;
            this[TxField.ESCAPE] = (byte)(3 << 6);
        }

        /// <summary>
        /// Creates a representation of a message sent to the Arduino, given 
        /// the raw bytes passed to it.
        /// </summary>
        /// <param name="vals">The raw bytes.</param>
        public TxMessage(byte[] vals)
            : base(vals)
        {
        }

        /// <summary>
        /// Gets or sets the raw byte value for the given transfer message 
        /// field.
        /// </summary>
        /// <param name="index">The given transfer message field.</param>
        /// <returns>The raw byte value for the given transfer message field.
        /// </returns>
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

        /// <summary>
        /// Gets or sets the sequence according to the sliding window protocol 
        /// being used to communicate the message.
        /// </summary>
        public override byte Sequence
        {
            get
            {
                return (byte)(((byte)(this[TxField.SIZE_SEQ] << ((byte)3))) >> ((byte)3)); 
            }
            set
            {
                this[TxField.SIZE_SEQ] = (byte)((byte)(Size << ((byte)5)) + value);
            }
        }
    }
}
