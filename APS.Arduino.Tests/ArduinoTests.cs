using APS.Data.Messages.Tx;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace APS.Arduino.Tests
{
    [TestFixture]
    public class ArduinoTests
    {
        [Test]
        public void TryIdentify()
        {
            using (var serialPort = new ExtSerialPort("COM4", 115200, Parity.Odd, 8, StopBits.One))
            {
                serialPort.ReadTimeout = 10000;
                serialPort.WriteTimeout = 10000;
                serialPort.Open();
                Assert.IsTrue(serialPort.IsOpen);
                Assert.IsNotNull(Arduino.IsArduino(serialPort, new Random(1)));
                serialPort.Close();
            }

            using (var serialPort = new ExtSerialPort("COM4", 115200, Parity.Odd, 8, StopBits.One))
            {
                serialPort.ReadTimeout = 10000;
                serialPort.WriteTimeout = 10000;
                serialPort.Open();
                Assert.IsTrue(serialPort.IsOpen);
                Assert.IsNotNull(Arduino.IsArduino(serialPort, new Random(3)));
                serialPort.Close();
            }
        }
        [Test]
        public async void TrySending()
        {
            using (var serialPort = new ExtSerialPort("COM4", 115200, Parity.Odd, 8, StopBits.One))
            {
                serialPort.ReadTimeout = 10000;
                serialPort.WriteTimeout = 10000;
                serialPort.Open();
                Assert.IsTrue(serialPort.IsOpen);
                var arduino = Arduino.IsArduino(serialPort, new Random(4));
                
                Assert.IsNotNull(arduino);

                for (int i = 0; i < 10; i++)
                {
                    Assert.IsTrue(await arduino.SendAsync(new Note()
                        {
                            NoteLength = 1,
                            Pin = 8,
                            RepeatLength = 1,
                            Step = 1
                        }));
                }

                serialPort.Close();
            }

        }
        [Test]
        public async void LoadTests()
        {
            using (var serialPort = new ExtSerialPort("COM4", 115200, Parity.Odd, 8, StopBits.One))
            {
                serialPort.ReadTimeout = 100;
                serialPort.WriteTimeout = 100;
                serialPort.Open();
                Assert.IsTrue(serialPort.IsOpen);
                var rand = new Random(7);
                var arduino = Arduino.IsArduino(serialPort, rand);

                Assert.IsNotNull(arduino);

                const int BUFFER_SIZE = 10;
                var readBufferRatio = new double[BUFFER_SIZE];
                var writeBufferRatio = new double[BUFFER_SIZE];
                for (int i = 0; i < 100000; i++)
                {
                    Assert.IsTrue(await arduino.SendAsync(new Note()
                        {
                            NoteLength = 1,
                            Pin = 8,
                            RepeatLength = 1,
                            Step = (byte) rand.Next(i % 64)
                        }));


                    Console.WriteLine("WriteBufferSize = {0}, ReadBufferSize = {1}, BytesToWrite = {2}, BytesToRead = {3}", serialPort.WriteBufferSize, serialPort.ReadBufferSize, serialPort.BytesToWrite, serialPort.BytesToRead);

                    //Assert.IsFalse(serialPort.BytesToRead > 100);

                }

                serialPort.Close();
            }
        }
    }
}
