using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Tests
{
    [TestFixture]
    public class TestMessageType
    {
        [Test]
        public void Enum_NonZero()
        {
            var messageTypes = Enum.GetValues(typeof(MessageType)).Cast<MessageType>();

            foreach(var messageType in messageTypes)
            {
                Assert.AreNotEqual(0x00, (byte)messageType);
            }
        }
    }
}
