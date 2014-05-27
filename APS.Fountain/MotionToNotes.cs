using APS.Data;
using Leap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Fountain
{
    public abstract class MotionToNotes
    {
        public abstract TxMessage Convert(Frame frame);
    }
}
