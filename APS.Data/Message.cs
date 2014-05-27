using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data
{
    /// <summary>
    /// Abstract class that reprsents all messages sent between the Arduino, 
    /// and the computer.
    /// </summary>
    /// <typeparam name="TTypeEnum">The enumeration for the message types.
    /// </typeparam>
    /// <typeparam name="TFieldEnum">The enumeration of the fields for the 
    /// message.</typeparam>
    public abstract class Message<TTypeEnum, TFieldEnum> : ICloneable
        where TTypeEnum : struct, IConvertible
        where TFieldEnum : struct, IConvertible
    {
        /// <summary>
        /// Represents the START byte value of all messages.
        /// </summary>
        public static readonly byte _START = 16;
        /// <summary>
        /// Represents the END byte value of all messages.
        /// </summary>
        public static readonly byte _END = 4;

        /// <summary>
        /// Creates a bare-bones messages.
        /// </summary>
        public Message()
        {
            _Bytes = Enumerable.Repeat((byte)0, Enum.GetValues(typeof(TFieldEnum)).Length).ToArray();
            _Bytes[0] = (byte)_START;
            _Bytes[_Bytes.Length - 1] = (byte)_END;
        }

        /// <summary>
        /// Creates a copy of a message using the given byte array.
        /// </summary>
        /// <param name="vals"></param>
        public Message(byte[] vals)
        {
            _Bytes = (byte[]) vals.Clone();
        }

        /// <summary>
        /// The size determined by the type of the message.
        /// </summary>
        public abstract byte Size { get; }
        /// <summary>
        /// The sequence number used by the sliding window protocol.
        /// </summary>
        public abstract byte Sequence { get; set; }

        /// <summary>
        /// The type of the message.
        /// </summary>
        public abstract TTypeEnum Type { get; }
        /// <summary>
        /// The raw bytes that represent the message.
        /// </summary>
        protected byte[] _Bytes { get; set; }
        /// <summary>
        /// A copy of the raw bytes that represent the message.
        /// </summary>
        public byte[] Bytes { get { return (byte[]) _Bytes.Clone(); } }
        /// <summary>
        ///  
        /// </summary>
        /// <param name="index">The field or index of the byte in its raw 
        /// representation.</param>
        /// <returns>Returns the raw byte value of the message for the given 
        /// field.</returns>
        public abstract byte this[TFieldEnum index] { get; set; }

        /// <summary>
        /// Copies this to the argument passed in the message.
        /// </summary>
        /// <param name="message">The message to be copied to.</param>
        /// <returns>The message to be copied to.</returns>
        public object CloneTo(Message<TTypeEnum, TFieldEnum> message)
        {
            message._Bytes = new Byte[this._Bytes.Length];
            for (int i = 0; i < message._Bytes.Length; i++)
                message._Bytes[i] = this._Bytes[i];
            return message;
        }

        /// <summary>
        /// Creates a clone of the message.
        /// </summary>
        /// <returns>A cloned object of the message.</returns>
        public abstract object Clone();

        /// <summary>
        /// A fixed-length string showing the values of the raw bytes array.
        /// </summary>
        /// <returns>A fixed-length string showing the values of the raw 
        /// bytes array.</returns>
        public override string ToString()
        {
            return string.Join(" ", _Bytes.Select(x => x.ToString("D3")));
        }
    }
}
