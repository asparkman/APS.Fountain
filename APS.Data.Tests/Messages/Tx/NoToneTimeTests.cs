using APS.Data.Messages.Tx;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Tests.Messages.Tx
{
    /// <summary>
    /// Holds unit tests for <c>NoToneTime</c> transfer messages.
    /// </summary>
    [TestFixture]
    public class NoToneTimeTests : TxMessageTests<NoToneTime>
    {
        /// <summary>
        /// Special bytes that need to be used with 
        /// <c>IterateThroughChanges</c>, because of the two bytes used for 
        /// <c>Milliseconds</c>.
        /// </summary>
        private readonly byte[] SPECIAL_BYTES = new byte[] { 0, 255, 0 };

        /// <summary>
        /// Iterates through common changes in information bytes.
        /// </summary>
        /// <returns>A enumerable that changes an information byte singularly 
        /// for each custom information byte.</returns>
        public override IEnumerable<NoToneTime> IterareThroughChanges(NoToneTime obj)
        {
            var target = (NoToneTime) obj.Clone();

            foreach (var byt in SPECIAL_BYTES.Skip(1))
            {
                target = (NoToneTime) target.Clone();
                checked
                {
                    target.Milliseconds = (UInt16) (byt << ((byte) 8));
                    Assert.IsTrue(target.Milliseconds == 0 || target.Milliseconds > 255);
                }
                yield return target;
            }

            foreach (var byt in SPECIAL_BYTES.Skip(1))
            {
                target = (NoToneTime)target.Clone();
                checked
                {
                    target.Milliseconds = (UInt16)byt;
                    Assert.IsTrue(target.Milliseconds <= 255);
                }
                yield return target;
            }

            yield break;
        }

        /// <summary>
        /// Iterates along side <c>IterateThroughCommonChanges</c>, so to 
        /// specify what raw byte field changed.
        /// </summary>
        /// <returns>The raw byte field that changed.</returns>
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
