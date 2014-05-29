using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Tests
{
    /// <summary>
    /// Abstract class to house the common receive message tests.
    /// </summary>
    /// <typeparam name="TTypeClass">The type of receive message that is 
    /// tested in the more derived class.</typeparam>
    public abstract class RxMessageTests<TTypeClass> : MessageTests<TTypeClass, RxType, RxField>
        where TTypeClass : Message<RxType, RxField>, new()
    {
        /// <summary>
        /// Iterates through common changes in information bytes.
        /// </summary>
        /// <returns>An enumerable that changes an information byte singularly 
        /// for each custom information byte.</returns>
        public override IEnumerable<TTypeClass> IterateThroughCommonChanges()
        {
            var target = new TTypeClass();

            foreach(var byt in BYTES)
            {
                target.Sequence = byt;
                yield return target;
            }

            yield break;
        }

        /// <summary>
        /// Iterates along side <c>IterateThroughCommonChanges</c>, so to 
        /// specify what raw byte field changed.
        /// </summary>
        /// <returns>The raw byte field that changed.</returns>
        public override IEnumerable<RxField> CommonFieldThatChanged()
        {
            foreach (var byt in BYTES)
                yield return RxField.SIZE_SEQ;

            yield break;
        }
    }
}
