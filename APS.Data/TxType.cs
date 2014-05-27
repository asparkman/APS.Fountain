using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data
{
    /// <summary>
    /// The possible types a message transfered to the Arduino may be.
    /// </summary>
    public enum TxType : byte
    {
        /// <summary>
        /// A message that tells the Arduino it needs to confirm its identity.
        /// </summary>
        IDENTIFY = 0,
        /// <summary>
        /// A message that tells the Arduino to play a note for a certain 
        /// number of repetitions.
        /// </summary>
        NOTE = 1,
        /// <summary>
        /// A message that tells the Arduino to stop playing notes.
        /// </summary>
        PAUSE = 2,
        /// <summary>
        /// A message that indicates how the pause between each note should 
        /// last.
        /// </summary>
        NO_TONE_TIME = 3,
        /// <summary>
        /// A message that indicates how long each note should last.
        /// </summary>
        TONE_TIME = 4
    }
}
