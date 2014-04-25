using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Tests
{
    [TestFixture]
    public class TestByteExtensions
    {
        [Test]
        public void GetBit()
        {
            for(int i = 0; i < 8; i++)
            {
                var target = (byte)(1 << i);
                Assert.AreEqual(true, target.GetBit(i));
                target = (byte)(~((int)target));
                Assert.AreNotEqual(true, target.GetBit(i));
            }
        }
    }
}
