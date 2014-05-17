using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data
{
    public abstract class Message<TTypeEnum, TFieldEnum> : ICloneable
        where TTypeEnum : struct, IConvertible
        where TFieldEnum : struct, IConvertible
    {
        public static readonly byte _START = 16;
        public static readonly byte _END = 4;

        public Message()
        {
            _Bytes = Enumerable.Repeat((byte)0, Enum.GetValues(typeof(TFieldEnum)).Length).ToArray();
            _Bytes[0] = (byte)_START;
            _Bytes[_Bytes.Length - 1] = (byte)_END;
        }

        public abstract byte Size { get; }
        public abstract byte Sequence { get; set; }

        public abstract TTypeEnum Type { get; }
        private byte[] _Bytes { get; set; }
        public byte[] Bytes { get { return _Bytes; } }
        public byte this[TFieldEnum index]
        {
            get
            {
                byte result = _Bytes[index.ToInt32(CultureInfo.CurrentCulture)];

                return result;
            }
            set
            {
                _Bytes[index.ToInt32(CultureInfo.CurrentCulture)] = value;
            }
        }

        public object CloneTo(Message<TTypeEnum, TFieldEnum> message)
        {
            message._Bytes = new Byte[this._Bytes.Length];
            for (int i = 0; i < message._Bytes.Length; i++)
                message._Bytes[i] = this._Bytes[i];
            return message;
        }

        public abstract object Clone();

        public override string ToString()
        {
            return string.Join(" ", _Bytes.Select(x => x.ToString("D3")));
        }
    }
}
