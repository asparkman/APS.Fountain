using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Messages.Tx
{
    /// <summary>
    /// Represents the amount of time a note should be played.
    /// </summary>
    public class ToneTime : TxMessage
    {
        /// <summary>
        /// Creates a bare-bone message of the <c>ToneTime</c> type.
        /// </summary>
        public ToneTime()
            : base()
        {
        }

        /// <summary>
        /// Creates an object that represents a <c>ToneTime</c> message from 
        /// raw bytes.
        /// </summary>
        /// <param name="vals">Raw bytes representing a <c>ToneTime</c> 
        /// message.</param>
        public ToneTime(byte[] vals)
            : base(vals)
        {
        }

        /// <summary>
        /// The transfer message type.
        /// </summary>
        public override TxType Type { get { return TxType.TONE_TIME; } }
        /// <summary>
        /// The number of information bytes that the transfer message uses.
        /// </summary>
        public override byte Size { get { return 3; } }

        /// <summary>
        /// The number of milliseconds each note should be played.
        /// </summary>
        public UInt16 Milliseconds
        {
            get
            {
                UInt16 result = 0;
                UInt16 msb = (UInt16) this[TxField.INFO_1];
                UInt16 lsb = (UInt16) this[TxField.INFO_2];
                result = (UInt16)((msb << 8) + lsb); 
                return result;
            }
            set
            {
                byte msb = (byte) (value >> 8);
                byte lsb = (byte) (value % (256));
                this[TxField.INFO_1] = msb;
                this[TxField.INFO_2] = lsb;
            }
        }

        /// <summary>
        /// Creates a clone of this transfer message.
        /// </summary>
        /// <returns>An exact copy of the transfer message.</returns>
        public override object Clone()
        {
            return CloneTo(new ToneTime());
        }
    }
}
