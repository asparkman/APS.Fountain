using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Tests
{
    [TestFixture]
    public class TestSendPause : TestBaseMessage<SendPause>
    {
        [Test]
        public override void Position_All()
        {
            var target = GetTestTarget();
            target.Pin = 1;
            target.NoteLength = 1;
            target.RepeatLength = 1;
            var positioned = target.Position();

            Assert.IsNotNull(positioned);

            var byte2 = (byte)((target.Pin << 4) + (target.NoteLength >> 2));
            var byte3 = (byte)(((target.NoteLength % 4) << 6) + target.RepeatLength);

            Assert.AreEqual(byte2, positioned[2]);
            Assert.AreEqual(byte3, positioned[3]);
        }

        public override SendPause GetTestTarget()
        {
            return new SendPause();
        }
    }
}
