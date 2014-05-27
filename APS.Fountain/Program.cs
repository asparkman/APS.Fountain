using APS.Arduino;
using Leap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Fountain
{
    public class Program
    {
        static void Main(string[] args)
        {
            using(var serialPort = new ExtSerialPort("COM4", 115200, System.IO.Ports.Parity.Odd, 8, System.IO.Ports.StopBits.One))
            {
                serialPort.ReadTimeout = 1000;
                serialPort.WriteTimeout = 1000;
                serialPort.Open();
                var arduino = APS.Arduino.Arduino.IsArduino(serialPort);

                if (arduino != null)
                {
                    Console.WriteLine("ARDUINO FOUND!");

                    // Create a sample listener and controller
                    var listener = new LeapListener(new LinearDistanceMotionToNotes(), arduino);
                    var controller = new Controller();

                    // Have the sample listener receive events from the controller
                    controller.AddListener(listener);

                    // Keep this process running until Enter is pressed
                    Console.WriteLine("Press Enter to quit...");
                    Console.ReadLine();

                    // Remove the sample listener when done
                    controller.RemoveListener(listener);
                    controller.Dispose();
                }
                else
                {
                    Console.WriteLine("ARDUINO NOT FOUND!");

                    Console.WriteLine("Press Enter to quit...");
                    Console.ReadLine();
                }

                serialPort.Close();
            }
        }
    }
}
