using APS.Data.Messages.Rx;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Tests.Messages.Rx
{
    [TestFixture]
    public class AckTests : RxMessageTests<Ack>
    {
        public override IEnumerable<Ack> IterareThroughChanges(Ack obj)
        {
            yield break;
        }

        public override IEnumerable<RxField> FieldThatChanged()
        {
            yield break;
        }

        [Test]
        public void Escaping_Rx()
        {
            var original = new Ack();
            var target = original.Clone();
            TestField(RxField.START, original, false);
            TestField(RxField.ESCAPE, original, false);
            TestField(RxField.SIZE_SEQ, original, true);
            TestField(RxField.INFO_0, original, true);
            TestField(RxField.INFO_1, original, true);
            TestField(RxField.INFO_2, original, true);
            TestField(RxField.STOP, original, false);
        }

        public void TestField(RxField field, Ack original, bool expectEscaped)
        {
            ZeroTest(field, original);
            EscapeTest(field, original, expectEscaped);
        }

        public void ZeroTest(RxField field, Ack original)
        {
            var target = (Ack)original.Clone();
            target[field] = 0;
            Assert.AreEqual(0, target.Bytes[(int)field]);
        }

        public void EscapeTest(RxField field, Ack original, bool expectEscaped)
        {
            var target = (Ack)original.Clone();
            target[field] = Ack._START;
            if (expectEscaped)
                Assert.AreEqual(Ack._START - 1, target.Bytes[(int)field]);
            else
                Assert.AreEqual(Ack._START, target.Bytes[(int)field]);

            target[field] = Ack._END;
            if (expectEscaped)
                Assert.AreEqual(Ack._END - 1, target.Bytes[(int)field]);
            else
                Assert.AreEqual(Ack._END, target.Bytes[(int)field]);
        }
    }
}
