using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data
{
    /// <summary>
    /// Provides a common implementation of 
    /// <c>IBytable&lt;T&gt;.Escape(byte[] positionedBytes)</c>.
    /// </summary>
    public abstract class BaseMessage : IBytable
    {
        public static readonly byte START = (byte)(16);
        public static readonly byte END = (byte)(4);
        public static readonly byte CONTROL_BYTES = 3;

        public abstract byte[] Position();

        public void Escape(byte[] positionedBytes)
        {
            for(int i = CONTROL_BYTES - 1; i < positionedBytes.Length - 1; i++)
            {
                if(positionedBytes[i] == START || positionedBytes[i] == END)
                {
                    // Escape byte
                    positionedBytes[i] -= 1;

                    // Update adder bits
                    positionedBytes[1] = (byte) (((int)positionedBytes[1]) | (1 << i));
                }
            }
        }

        public abstract byte Type { get; }

        public abstract int NonControlBytes { get; }

        public virtual byte[] GetBasePositionArray()
        {
            var result = new byte[CONTROL_BYTES + NonControlBytes];

            result[0] = START;
            result[1] = (byte)(Type << 5);
            result[CONTROL_BYTES + NonControlBytes - 1] = END;

            return result;
        }
    }
}
