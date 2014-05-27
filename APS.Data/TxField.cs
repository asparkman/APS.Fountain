using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data
{
    /// <summary>
    /// Used to identify, and position transfer message bytes.
    /// </summary>
    public enum TxField : int
    {
        /// <summary>
        /// The start byte of a transfer message.
        /// </summary>
        START = 0,
        /// <summary>
        /// The byte that escapes the bytes that neither start, nor stop the 
        /// transfer message.
        /// </summary>
        ESCAPE = 1,
        /// <summary>
        /// The byte that contains the size and sequence information.  The size 
        /// is the number of useful data in the information section.  The 
        /// sequence is used by the sliding window protocol.
        /// </summary>
        SIZE_SEQ = 2,
        /// <summary>
        /// The 0th byte of the information section of a transfer message.
        /// </summary>
        INFO_0 = 3,
        /// <summary>
        /// The 1st byte of the information section of a transfer message.
        /// </summary>
        INFO_1 = 4,
        /// <summary>
        /// The 2nd byte of the information section of a transfer message.
        /// </summary>
        INFO_2 = 5,
        /// <summary>
        /// The 3rd byte of the information section of a transfer message.
        /// </summary>
        INFO_3 = 6,
        /// <summary>
        /// The 4th byte of the information section of a transfer message.
        /// </summary>
        INFO_4 = 7,
        /// <summary>
        /// The 5th byte of the information section of a transfer message.
        /// </summary>
        INFO_5 = 8,
        /// <summary>
        /// The stop byte of a transfer message.
        /// </summary>
        STOP = 9
    }
}
