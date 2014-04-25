using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Tests
{
    public abstract class TestBaseMessage<T> : TestIBytable<T> where T : BaseMessage
    {
        [Test]
        public virtual void GetBasePositionArray_All()
        {
            var target = GetTestTarget();

            var basePositionArray = target.GetBasePositionArray();
            Assert.AreEqual(BaseMessage.START, basePositionArray.First());
            Assert.AreEqual((byte) target.Type, (byte) (basePositionArray[1] >> 5));
            Assert.AreEqual(BaseMessage.END, basePositionArray.Last());
        }

        [Test]
        public virtual void Position_Start()
        {
            var target = GetTestTarget();

            var positioned = target.Position();
            Assert.IsTrue(positioned.Any());
            Assert.AreEqual(BaseMessage.START, positioned[0]);
        }

        [Test]
        public virtual void Position_End()
        {
            var target = GetTestTarget();

            var positioned = target.Position();
            Assert.IsTrue(positioned.Any());
            Assert.AreEqual(BaseMessage.END, positioned[positioned.Length - 1]);
        }

        [Test]
        public override void Escape_All()
        {
            var target = GetTestTarget();

            var positioned = target.Position();
            byte[] startBytes = positioned.Take(2).ToArray();
            byte[] endBytes = positioned.Skip(positioned.Length - 1).ToArray();
            Dictionary<byte, byte> escapes = new Dictionary<byte, byte>()
            {
                { (byte)4, (byte)3},
                { (byte)16, (byte)15},
            };
            byte replacementByte = 5;
            for(int escapeIndex = 2; escapeIndex < positioned.Length - 1; escapeIndex++)
            {
                var candidate = positioned.ToArray();
                // Set before bytes
                for(int beforeIndex = 2; beforeIndex < escapeIndex; beforeIndex++)
                {
                    candidate[beforeIndex] = replacementByte;
                }

                // Set after bytes
                for (int afterIndex = escapeIndex + 1; afterIndex < positioned.Length - 1; afterIndex++)
                {
                    candidate[afterIndex] = replacementByte;
                }

                // Make a test case for each possible escapable character and test it.
                foreach(var key in escapes.Keys)
                {
                    var keyCandidate = candidate.ToArray();
                    keyCandidate[escapeIndex] = key;

                    target.Escape(keyCandidate);

                    for(int i = 2; i < keyCandidate.Length - 1; i++)
                    {
                        if(i != escapeIndex)
                        {
                            Assert.AreEqual(false, keyCandidate[1].GetBit(i));
                            Assert.AreEqual(replacementByte, keyCandidate[i]);
                        }
                        else
                        {
                            Assert.AreEqual(true, keyCandidate[1].GetBit(i));
                            Assert.AreEqual(escapes[key], keyCandidate[i]);
                        }
                    }
                }

            }
        }
    }
}
