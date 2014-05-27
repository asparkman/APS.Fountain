using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Tests
{
    public abstract class RxMessageTests<TTypeClass> : MessageTests<TTypeClass, RxType, RxField>
        where TTypeClass : Message<RxType, RxField>, new()
    {
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

        public override IEnumerable<RxField> CommonFieldThatChanged()
        {
            foreach (var byt in BYTES)
                yield return RxField.SIZE_SEQ;

            yield break;
        }
    }
}
