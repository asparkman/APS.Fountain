using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Tests
{
    [TestFixture]
    public class TestIdentify : TestBaseMessage<Identify>
    {
        [Test]
        public override void Position_All()
        {
            var target = GetTestTarget();
            var positioned = target.Position();

            Assert.IsNotNull(positioned);
        }

        public override Identify GetTestTarget()
        {
            return new Identify();
        }
    }
}
