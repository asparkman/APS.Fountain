using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data.Tests
{
    public abstract class TestIBytable<T> where T : IBytable
    {
        [Test]
        public abstract void Position_All();

        /// <summary>
        /// Makes sure the number of adder bits allows the length of the 
        /// position returned of the byte array.
        /// </summary>
        [Test]
        public void Position_Length()
        {
            Assert.IsTrue(GetTestTarget().Position().Length <= 7);
            Assert.IsTrue(GetTestTarget().Position().Length >= 3);
        }

        public abstract T GetTestTarget();

        [Test]
        public abstract void Escape_All();
    }
}
