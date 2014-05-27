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
    public class PauseTests : TxMessageTests<Pause>
    {
        public override IEnumerable<Pause> IterareThroughChanges(Pause obj)
        {
            var target = (Pause) obj.Clone();

            foreach (var byt in BYTES)
            {
                target = (Pause)target.Clone();
                target.Pin = byt;
                yield return target;
            }
            foreach (var byt in BYTES)
            {
                target = (Pause)target.Clone();
                target.NoteLength = byt;
                yield return target;
            }
            foreach (var byt in BYTES)
            {
                target = (Pause)target.Clone();
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

            yield break;
        }
    }
}
