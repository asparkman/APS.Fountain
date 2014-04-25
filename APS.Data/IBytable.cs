using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Data
{
    /// <summary>
    /// Contains methods for converting the item into a COM port serializable 
    /// representation of bytes.
    /// </summary>
    public interface IBytable
    {
        /// <summary>
        /// Takes the object's properties and turns them into a byte array.
        /// </summary>
        /// <returns>An array of bytes that are positioned, but unescaped.</returns>
        byte[] Position();

        /// <summary>
        /// Takes an array of bytes and modifies them to be transmittable over 
        /// the serial port.
        /// </summary>
        /// <param name="positionedBytes">The output of <c>Position()</c></param>
        void Escape(byte[] positionedBytes);
    }
}
