using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data
{
    public class SendNote : BaseMessage, IBytable
    {
        public override byte[] Position()
        {
            var result = GetBasePositionArray();

            result[2] = (byte)(Pin << 4 + Note >> 4);
            result[3] = (byte)((Note % 1024) << 4 + NoteLength >> 2);
            result[4] = (byte)((NoteLength % 4) << 6 + RepeatLength);

            return result;
        }

        public override byte Type { get { return (byte)MessageType.SendNote; } }
        public override int NonControlBytes { get { return 3; } }
        
        [Range(0, 15)]
        public virtual int Pin { get; set; }

        [Range(0, 255)]
        public virtual int Note { get; set; }

        [Range(0, 1023)]
        public virtual int NoteLength { get; set; }

        [Range(0, 1023)]
        public virtual int RepeatLength { get; set; }
    }
}
