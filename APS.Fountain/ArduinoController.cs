using APS.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace APS.Fountain
{
    public class ArduinoController : IDisposable
    {
        public ArduinoController()
        {
            StreamWriter = new StreamWriter("t.txt", false, Encoding.ASCII);
        }

        public static int SLEEP_CONSTANT = 100;

        SerialPort currentPort;
        bool portFound;
        StreamWriter StreamWriter { get; set; }
        public bool SetComPort()
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports.Reverse())
                {
                    currentPort = new SerialPort(port, 115200);
                    Console.WriteLine("{0}{1}{2}", currentPort.DataBits, currentPort.Parity, currentPort.StopBits);
                    Console.WriteLine(currentPort.Encoding.EncodingName);
                    if (DetectArduino())
                    {
                        portFound = true;
                        break;
                    }
                    else
                    {
                        portFound = false;
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return portFound;
        }
        public bool DoCommand(IBytable bytable, string message = null)
        {
            var bytes = bytable.Position();
            bytable.Escape(bytes);
            return DoCommand(bytes, message);
        }
        public bool DoCommand(byte[] bytes, string message = null)
        {
            try
            {
                StreamWriter.WriteLine("C# Input Start");
                foreach(var byt in bytes)
                {
                    StreamWriter.WriteLine(byt);
                }
                StreamWriter.WriteLine("C# Input End");
                //streamWriter.Write(System.Text.Encoding.ASCII.GetString(bytes));

                //The below setting are for the Hello handshake
                int intReturnASCII = 0;
                char charReturnValue = (Char)intReturnASCII;
                if (!currentPort.IsOpen)
                    currentPort.Open();
                currentPort.Write(bytes, 0, bytes.Length);
                Thread.Sleep(SLEEP_CONSTANT);
                int count = currentPort.BytesToRead;
                string returnMessage = "";
                while (count > 0)
                {
                    intReturnASCII = currentPort.ReadByte();
                    returnMessage = returnMessage + Convert.ToChar(intReturnASCII);
                    count--;
                }
                if (message != null)
                {
                    if (returnMessage.Contains(message))
                    {
                        Console.WriteLine("CONTAINS {0}!!, <{1}>", message, returnMessage);
                        StreamWriter.WriteLine("Input from serial start");
                        StreamWriter.WriteLine(returnMessage);
                        StreamWriter.WriteLine("Input from serial end");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("DOES NOT CONTAIN {0}!!, <{1}>", message, returnMessage);
                        StreamWriter.WriteLine("Input from serial start");
                        StreamWriter.WriteLine(returnMessage);
                        StreamWriter.WriteLine("Input from serial end");
                        return false;
                    }
                }
                else
                    return true;
                
            }
            catch (Exception e)
            {
                return false;
            }
        }
        private bool DetectArduino()
        {
            var identify = new Identify();
            var bytes = identify.Position();
            identify.Escape(bytes);

            return DoCommand(bytes, "HELLO FROM ARDUINO");
        }

        public void Dispose()
        {
            if (currentPort != null && currentPort.IsOpen)
                currentPort.Close();

            if (StreamWriter != null)
                StreamWriter.Close();
        }
    }
}
