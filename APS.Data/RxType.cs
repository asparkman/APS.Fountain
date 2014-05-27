using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data
{
    /// <summary>
    /// The possible types a message transfered from the Arduino may be.
    /// </summary>
    public enum RxType : byte
    {
        /// <summary>
        /// A message that confirms the identity of the Arduino.
        /// </summary>
        IDENTIFY = 0,

        /// <summary>
        /// A message that confirms the receipt of a transfer message on the 
        /// Arduino.
        /// </summary>
        ACK = 1
    }
}
