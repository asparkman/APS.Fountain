using APS.Data.Messages.Tx;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Tests.Messages.Tx
{
    [TestFixture]
    public class NoteTests : TxMessageTests<Note>
    {
        public override IEnumerable<Note> IterareThroughChanges(Note obj)
        {
            var target = (Note) obj.Clone();

            foreach (var byt in BYTES)
            {
                target = (Note)target.Clone();
                target.Pin = byt;
                yield return target;
            }
            foreach (var byt in BYTES)
            {
                target = (Note)target.Clone();
                target.Step = byt;
                yield return target;
            }
            foreach (var byt in BYTES)
            {
                target = (Note)target.Clone();
                target.NoteLength = byt;
                yield return target;
            }
            foreach (var byt in BYTES)
            {
                target = (Note)target.Clone();
                target.RepeatLength = byt;
                yield return target;
            }

            yield break;
        }

        public override IEnumerable<TxField> FieldThatChanged()
        {
            var target = new Identify();

            foreach (var byt in BYTES)
                yield return TxField.INFO_1;
            foreach (var byt in BYTES)
                yield return TxField.INFO_2;
            foreach (var byt in BYTES)
                yield return TxField.INFO_3;
            foreach (var byt in BYTES)
                yield return TxField.INFO_4;

            yield break;
        }

        [Test]
        public void Escaping_Tx()
        {
            var original = new Note();
            var target = original.Clone();
            TestField(TxField.START, original, false);
            TestField(TxField.ESCAPE, original, false);
            TestField(TxField.SIZE_SEQ, original, true);
            TestField(TxField.INFO_0, original, true);
            TestField(TxField.INFO_1, original, true);
            TestField(TxField.INFO_2, original, true);
            TestField(TxField.INFO_3, original, true);
            TestField(TxField.INFO_4, original, true);
            TestField(TxField.INFO_5, original, true);
            TestField(TxField.STOP, original, false);
        }

        public void TestField(TxField field, Note original, bool expectEscaped)
        {
            ZeroTest(field, original);
            EscapeTest(field, original, expectEscaped);
        }

        public void ZeroTest(TxField field, Note original)
        {
            var target = (Note)original.Clone();
            target[field] = 0;
            Assert.AreEqual(0, target.Bytes[(int)field]);
        }

        public void EscapeTest(TxField field, Note original, bool expectEscaped)
        {
            var target = (Note)original.Clone();
            target[field] = Note._START;
            if (expectEscaped)
                Assert.AreEqual(Note._START - 1, target.Bytes[(int)field]);
            else
                Assert.AreEqual(Note._START, target.Bytes[(int)field]);

            target[field] = Note._END;
            if (expectEscaped)
                Assert.AreEqual(Note._END - 1, target.Bytes[(int)field]);
            else
                Assert.AreEqual(Note._END, target.Bytes[(int)field]);
        }
    }
}
