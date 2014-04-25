using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Tests
{
    [TestFixture]
    public class TestSendToneTime : TestBaseMessage<SendToneTime>
    {
        [Test]
        public override void Position_All()
        {
            var target = GetTestTarget();
            target.Milliseconds = 1 << 8 + 1;
            var positioned = target.Position();

            Assert.IsNotNull(positioned);

            var byte2 = (byte)(target.Milliseconds >> 8);
            var byte3 = (byte)(target.Milliseconds % 256);

            Assert.AreEqual(byte2, positioned[2]);
            Assert.AreEqual(byte3, positioned[3]);
        }

        public override SendToneTime GetTestTarget()
        {
            return new SendToneTime();
        }
    }
}
