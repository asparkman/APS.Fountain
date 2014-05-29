using APS.Data.Messages.Rx;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Tests.Messages.Rx
{
    /// <summary>
    /// Holds unit tests for <c>Ack</c> receive messages.
    /// </summary>
    [TestFixture]
    public class AckTests : RxMessageTests<Ack>
    {
        /// <summary>
        /// Iterates through common changes in information bytes.
        /// </summary>
        /// <returns>A enumerable that changes an information byte singularly 
        /// for each custom information byte.</returns>
        public override IEnumerable<Ack> IterareThroughChanges(Ack obj)
        {
            yield break;
        }

        /// <summary>
        /// Iterates along side <c>IterateThroughCommonChanges</c>, so to 
        /// specify what raw byte field changed.
        /// </summary>
        /// <returns>The raw byte field that changed.</returns>
        public override IEnumerable<RxField> FieldThatChanged()
        {
            yield break;
        }

        /// <summary>
        /// Tests that escaping received messages is done through the 
        /// assignment of the raw bytes.
        /// </summary>
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

        /// <summary>
        /// Helper method for <c>Escaping_Rx</c>.  Used to help test every 
        /// possible field.
        /// </summary>
        /// <param name="field">The field to test.</param>
        /// <param name="original">The receive message to test.</param>
        /// <param name="expectEscaped">Whether escaping will be expected.</param>
        public void TestField(RxField field, Ack original, bool expectEscaped)
        {
            ZeroTest(field, original);
            EscapeTest(field, original, expectEscaped);
        }

        /// <summary>
        /// Makes sure that the field will be returned properly when a 
        /// non-escape byte, 0, is assigned to it.
        /// </summary>
        /// <param name="field">The field to test.</param>
        /// <param name="original">The receive message to test.</param>
        public void ZeroTest(RxField field, Ack original)
        {
            var target = (Ack)original.Clone();
            target[field] = 0;
            Assert.AreEqual(0, target.Bytes[(int)field]);
        }

        /// <summary>
        /// Makes sure that the field will be stored as escaped or not 
        /// escaped.  The test is performed for all escape characters.
        /// </summary>
        /// <param name="field">The field to test.</param>
        /// <param name="original">The receive message to test.</param>
        /// <param name="expectEscaped">Whether to expect it to be stored as 
        /// escaped.</param>
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
