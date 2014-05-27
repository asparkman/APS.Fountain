using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Messages.Tx
{
    public class Note : TxMessage
    {
        #pragma warning disable 1591
        public Note()
            : base()
        {
        }

        public Note(byte[] vals)
            : base(vals)
        {
        }

        public override TxType Type { get { return TxType.NOTE; } }
        public override byte Size { get { return 5; } }

        public byte Pin { get { return this[TxField.INFO_1]; } set { this[TxField.INFO_1] = value; } }
        public byte Step { get { return this[TxField.INFO_2]; } set { this[TxField.INFO_2] = value; } }
        public byte NoteLength { get { return this[TxField.INFO_3]; } set { this[TxField.INFO_3] = value; } }
        public byte RepeatLength { get { return this[TxField.INFO_4]; } set { this[TxField.INFO_4] = value; } }

        public override object Clone()
        {
            return CloneTo(new Note());
        }
        #pragma warning restore 1591
    }
}
