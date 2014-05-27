using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Tests
{
    public abstract class TxMessageTests<TTypeClass> : MessageTests<TTypeClass, TxType, TxField>
        where TTypeClass : Message<TxType, TxField>, new()
    {
        public override IEnumerable<TTypeClass> IterateThroughCommonChanges()
        {
            var target = new TTypeClass();

            foreach (var byt in BYTES)
            {
                target.Sequence = byt;
                yield return target;
            }

            yield break;
        }

        public override IEnumerable<TxField> CommonFieldThatChanged()
        {
            foreach (var byt in BYTES)
                yield return TxField.SIZE_SEQ;

            yield break;
        }
    }
}
