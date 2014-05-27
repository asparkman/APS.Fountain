using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data
{
    /// <summary>
    /// Represents a message received from the Arduino.
    /// </summary>
    public abstract class RxMessage : Message<RxType, RxField>
    {
        /// <summary>
        /// Creates a bare-bones message representing a message received from 
        /// the Arduino.
        /// </summary>
        public RxMessage()
            : base()
        {
            this[RxField.SIZE_SEQ] = (byte) (Size << ((byte)5));
            this[RxField.INFO_0] = (byte)Type;
            this[RxField.ESCAPE] = (byte)(3 << 6);
        }

        /// <summary>
        /// Creates a representation of a message received by the Arduino, 
        /// given the raw bytes passed to it.
        /// </summary>
        /// <param name="vals">The raw bytes.</param>
        public RxMessage(byte[] vals)
            : base(vals)
        {
        }

        /// <summary>
        /// Gets or sets the raw byte value for the given receive message 
        /// field.
        /// </summary>
        /// <param name="index">The given receive message field.</param>
        /// <returns>The raw byte value for the given receive message field.
        /// </returns>
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

        /// <summary>
        /// Gets or sets the sequence according to the sliding window protocol 
        /// being used to communicate the message.
        /// </summary>
        public override byte Sequence
        { 
            get 
            {
                return (byte)(((byte)(this[RxField.SIZE_SEQ] << ((byte)3))) >> ((byte)3)); 
            } 
            set 
            {
                this[RxField.SIZE_SEQ] = (byte)((byte)(Size << ((byte)5)) + value); 
            } 
        }

    }
}
