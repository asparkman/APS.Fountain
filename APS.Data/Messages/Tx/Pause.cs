using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Messages.Tx
{
    /// <summary>
    /// Represents a pause to be taken by a piezo connected to the Arduino.
    /// </summary>
    public class Pause : TxMessage
    {
        /// <summary>
        /// Creates a bare-bone message of the <c>Pause</c> type.
        /// </summary>
        public Pause()
            : base()
        {
        }

        /// <summary>
        /// Creates an object that represents a <c>Pause</c> message from 
        /// raw bytes.
        /// </summary>
        /// <param name="vals">Raw bytes representing a <c>Pause</c> 
        /// message.</param>
        public Pause(byte[] vals)
            : base(vals)
        {
        }

        /// <summary>
        /// The transfer message type.
        /// </summary>
        public override TxType Type { get { return TxType.PAUSE; } }
        /// <summary>
        /// The number of information bytes that the transfer message uses.
        /// </summary>
        public override byte Size { get { return 4; } }

        /// <summary>
        /// The pin that this note should be written to.
        /// </summary>
        public byte Pin { get { return this[TxField.INFO_1]; } set { this[TxField.INFO_1] = value; } }
        /// <summary>
        /// The pin will be silent for this length of time.
        /// <c>ToneTime.Milliseconds</c> * <c>NoteLength</c> 
        /// + <c>NoToneTime.Milliseconds</c> * (<c>NoteLength</c> - 1)
        /// 
        /// Pauses between multiple pauses will be seperated by 
        /// <c>NoToneTime.Milliseconds</c>.
        /// </summary>
        public byte NoteLength { get { return this[TxField.INFO_2]; } set { this[TxField.INFO_2] = value; } }
        /// <summary>
        /// The number of times to repeat the <c>Pause</c> each time playing it 
        /// a length specified in the description of <c>NoteLength</c>.
        /// </summary>
        public byte RepeatLength { get { return this[TxField.INFO_3]; } set { this[TxField.INFO_3] = value; } }

        /// <summary>
        /// Creates a clone of this transfer message.
        /// </summary>
        /// <returns>An exact copy of the transfer message.</returns>
        public override object Clone()
        {
            return CloneTo(new Pause());
        }
    }
}
