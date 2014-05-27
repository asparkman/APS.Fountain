using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Arduino
{
    /// <summary>
    /// An extension of <c>SerialPort</c> that implements <c>ISerialPort</c>.  
    /// This was done to allow a Fake to be inserted into the Arduino test 
    /// code, if needed in the future.
    /// </summary>
    public class ExtSerialPort : SerialPort, ISerialPort
    {
        #pragma warning disable 1591
        public ExtSerialPort() : base() {}
        public ExtSerialPort(IContainer container) : base(container) {}
        public ExtSerialPort(string portName) : base(portName) {}
        public ExtSerialPort(string portName, int baudRate) : base(portName, baudRate) {}
        public ExtSerialPort(string portName, int baudRate, Parity parity) : base(portName, baudRate, parity) {}
        public ExtSerialPort(string portName, int baudRate, Parity parity, int dataBits) : base(portName, baudRate, parity, dataBits) {}
        public ExtSerialPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits) : base(portName, baudRate, parity, dataBits, stopBits) { }
        #pragma warning restore 1591
    }
}
