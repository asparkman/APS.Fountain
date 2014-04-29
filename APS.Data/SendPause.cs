using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data
{
    public class SendPause : BaseMessage, IBytable
    {
        public override byte[] Position()
        {
            var result = GetBasePositionArray();

            result[2] = (byte)((Pin << 4) + (NoteLength >> 2));
            result[3] = (byte)(((NoteLength % 4) << 6) + RepeatLength);

            return result;
        }

        public override byte Type { get { return (byte)MessageType.SendPause; } }
        public override int NonControlBytes { get { return 2; } }

        [Range(0, 15)]
        public virtual int Pin { get; set; }

        [Range(0, 1023)]
        public virtual int NoteLength { get; set; }

        [Range(0, 1023)]
        public virtual int RepeatLength { get; set; }
    }
}
