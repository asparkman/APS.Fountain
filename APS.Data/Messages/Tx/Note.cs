﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Messages.Tx
{
    /// <summary>
    /// Represents a note to be played by a piezo connected to the Arduino.
    /// </summary>
    public class Note : TxMessage
    {
        /// <summary>
        /// Creates a bare-bone message of the <c>Note</c> type.
        /// </summary>
        public Note()
            : base()
        {
        }

        /// <summary>
        /// Creates an object that represents a <c>Note</c> message from 
        /// raw bytes.
        /// </summary>
        /// <param name="vals">Raw bytes representing a <c>Note</c> 
        /// message.</param>
        public Note(byte[] vals)
            : base(vals)
        {
        }

        /// <summary>
        /// The transfer message type.
        /// </summary>
        public override TxType Type { get { return TxType.NOTE; } }
        /// <summary>
        /// The number of information bytes that the transfer message uses.
        /// </summary>
        public override byte Size { get { return 5; } }

        /// <summary>
        /// The pin that this note should be written to.
        /// </summary>
        public byte Pin { get { return this[TxField.INFO_1]; } set { this[TxField.INFO_1] = value; } }
        /// <summary>
        /// The step or note that should be played.  The frequency written to 
        /// the PWM module on the Arduino is calculated as follows.
        /// 
        /// freq = (2 ^ ((step - offset) / 12) ) * base_freq
        /// 
        /// where:
        ///     offset - an arbitrary offset specified on the Arduino that is 
        ///         used to range in the available frequencies to be played.
        ///     base_freq - the frequency when (step - offset) = 0.
        /// </summary>
        public byte Step { get { return this[TxField.INFO_2]; } set { this[TxField.INFO_2] = value; } }
        /// <summary>
        /// Each note will be played for this length of time.
        /// <c>ToneTime.Milliseconds</c> * <c>NoteLength</c> 
        /// + <c>NoToneTime.Milliseconds</c> * (<c>NoteLength</c> - 1)
        /// 
        /// Pauses between multiple notes set to be repeated will be seperated 
        /// by <c>NoToneTime.Milliseconds</c>.
        /// </summary>
        public byte NoteLength { get { return this[TxField.INFO_3]; } set { this[TxField.INFO_3] = value; } }
        /// <summary>
        /// The number of times to repeat the <c>Step</c> each time playing it 
        /// a length specified in the description of <c>NoteLength</c>.
        /// </summary>
        public byte RepeatLength { get { return this[TxField.INFO_4]; } set { this[TxField.INFO_4] = value; } }

        /// <summary>
        /// Creates a clone of this transfer message.
        /// </summary>
        /// <returns>An exact copy of the transfer message.</returns>
        public override object Clone()
        {
            return CloneTo(new Note());
        }
    }
}
