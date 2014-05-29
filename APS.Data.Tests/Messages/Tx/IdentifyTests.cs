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
    /// Holds unit tests for <c>Identify</c> transfer messages.
    /// </summary>
    [TestFixture]
    public class IdentifyTests : TxMessageTests<Identify>
    {
        /// <summary>
        /// Iterates through common changes in information bytes.
        /// </summary>
        /// <returns>A enumerable that changes an information byte singularly 
        /// for each custom information byte.</returns>
        public override IEnumerable<Identify> IterareThroughChanges(Identify obj)
        {
            var target = (Identify) obj.Clone();
            
            foreach(var byt in BYTES)
            {
                target = (Identify)target.Clone();
                target.Random0 = byt;
                yield return target;
            }

            foreach (var byt in BYTES)
            {
                target = (Identify)target.Clone();
                target.Random1 = byt;
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
            var target = new Identify();

            foreach (var byt in BYTES)
                yield return TxField.INFO_1;

            foreach (var byt in BYTES)
                yield return TxField.INFO_2;

            yield break;
        }
    }
}
