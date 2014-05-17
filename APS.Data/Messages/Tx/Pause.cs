﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Messages.Tx
{
    public class Pause : TxMessage
    {
        public override TxType Type { get { return TxType.PAUSE; } }
        public override byte Size { get { return 4; } }

        public byte Pin { get { return this[TxField.INFO_1]; } set { this[TxField.INFO_1] = value; } }
        public byte NoteLength { get { return this[TxField.INFO_2]; } set { this[TxField.INFO_2] = value; } }
        public byte RepeatLength { get { return this[TxField.INFO_3]; } set { this[TxField.INFO_3] = value; } }

        public override object Clone()
        {
            return CloneTo(new Pause());
        }
    }
}
