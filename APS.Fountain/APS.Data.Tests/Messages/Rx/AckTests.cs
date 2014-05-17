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
    }
}
