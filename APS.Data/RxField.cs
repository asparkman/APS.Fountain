using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data
{
    /// <summary>
    /// Used to identify, and position received message bytes.
    /// </summary>
    public enum RxField : int
    {
        /// <summary>
        /// The start byte of a receive message.
        /// </summary>
        START = 0,
        /// <summary>
        /// The byte that escapes the bytes that neither start, nor stop the 
        /// receive message.
        /// </summary>
        ESCAPE = 1,
        /// <summary>
        /// The byte that contains the size and sequence information.  The size 
        /// is the number of useful data in the information section.  The 
        /// sequence is used by the sliding window protocol.
        /// </summary>
        SIZE_SEQ = 2,
        /// <summary>
        /// The 0th byte of the information section of a receive message.
        /// </summary>
        INFO_0 = 3,
        /// <summary>
        /// The 1st byte of the information section of a receive message.
        /// </summary>
        INFO_1 = 4,
        /// <summary>
        /// The 2nd byte of the information section of a receive message.
        /// </summary>
        INFO_2 = 5,
        /// <summary>
        /// The stop byte of a receive message.
        /// </summary>
        STOP = 6
    }
}
