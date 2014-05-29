using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Messages.Rx
{
    /// <summary>
    /// Acknowledges the receipt of a message transfered from the computer.  
    /// This message contains no special bytes, because the acknowledgement 
    /// happens in the SIZE_SEQ byte.
    /// </summary>
    public class Ack : RxMessage
    {
        /// <summary>
        /// Creates a bare-bone message of the <c>Ack</c> type.
        /// </summary>
        public Ack()
            : base()
        {
        }

        /// <summary>
        /// Creates an object that represents a <c>Ack</c> message from 
        /// raw bytes.
        /// </summary>
        /// <param name="vals">Raw bytes representing a <c>Ack</c> 
        /// message.</param>
        public Ack(byte[] vals)
            : base(vals)
        {
        }

        /// <summary>
        /// The receive message type.
        /// </summary>
        public override RxType Type { get { return RxType.ACK; } }
        /// <summary>
        /// The number of information bytes that the receive message uses.
        /// </summary>
        public override byte Size { get { return 1; } }

        /// <summary>
        /// Creates a clone of this receive message.
        /// </summary>
        /// <returns>An exact copy of the receive message.</returns>
        public override object Clone()
        {
            return CloneTo(new Ack());
        }
    }
}
