﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Messages.Tx
{
    public class ToneTime : TxMessage
    {
        #pragma warning disable 1591
        public ToneTime()
            : base()
        {
        }

        public ToneTime(byte[] vals)
            : base(vals)
        {
        }

        public override TxType Type { get { return TxType.TONE_TIME; } }
        public override byte Size { get { return 3; } }

        public UInt16 Milliseconds
        {
            get
            {
                UInt16 result = 0;
                UInt16 msb = (UInt16) this[TxField.INFO_1];
                UInt16 lsb = (UInt16) this[TxField.INFO_2];
                result = (UInt16)((msb << 8) + lsb); 
                return result;
            }
            set
            {
                byte msb = (byte) (value >> 8);
                byte lsb = (byte) (value % (256));
                this[TxField.INFO_1] = msb;
                this[TxField.INFO_2] = lsb;
            }
        }

        public override object Clone()
        {
            return CloneTo(new ToneTime());
        }
        #pragma warning restore 1591
    }
}
