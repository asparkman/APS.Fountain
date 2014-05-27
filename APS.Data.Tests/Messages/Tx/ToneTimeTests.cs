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
    public class ToneTimeTests : TxMessageTests<ToneTime>
    {
        private readonly byte[] SPECIAL_BYTES = new byte[] { 0, 255, 0 };

        public override IEnumerable<ToneTime> IterareThroughChanges(ToneTime obj)
        {
            var target = (ToneTime)obj.Clone();

            foreach (var byt in SPECIAL_BYTES.Skip(1))
            {
                target = (ToneTime)target.Clone();
                checked
                {
                    target.Milliseconds = (UInt16)(byt << ((byte)8));
                    Assert.IsTrue(target.Milliseconds == 0 || target.Milliseconds > 255);
                }
                yield return target;
            }

            foreach (var byt in SPECIAL_BYTES.Skip(1))
            {
                target = (ToneTime)target.Clone();
                checked
                {
                    target.Milliseconds = (UInt16)byt;
                    Assert.IsTrue(target.Milliseconds <= 255);
                }
                yield return target;
            }

            yield break;
        }

        public override IEnumerable<TxField> FieldThatChanged()
        {
            foreach (var byt in SPECIAL_BYTES.Skip(1))
                yield return TxField.INFO_1;

            foreach (var byt in SPECIAL_BYTES.Skip(1))
                yield return TxField.INFO_2;

            yield break;
        }
    }
}
