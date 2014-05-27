using APS.Data;
using Leap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Fountain
{
    /// <summary>
    /// This class helps convert Leap Motion data into usable commands for the 
    /// Arduino.  It does not handle the sending, just the conversion.
    /// </summary>
    public abstract class MotionToNotes
    {
        /// <summary>
        /// Takes a piece of Leap Motion data, and converts it to something 
        /// the <c>Arduino</c> can use.
        /// </summary>
        /// <param name="frame">A piece of Leap Motion data representing the 
        /// hand(s) it senses at the time.</param>
        /// <returns>A usable piece of data that may be sent to the 
        /// <c>Arduino</c>.</returns>
        public abstract TxMessage Convert(Frame frame);
    }
}
