using Leap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace APS.Fountain
{
    public class Coordinator : IDisposable
    {
        public Coordinator()
        {
            LeapListener = new LeapListener();
            LeapController = new Controller();
            Arduino = new ArduinoController();
            Arduino.SetComPort();
            NoteSender = CreateTimer();

            ArduinoTimeLogger = new TimeLogger();
            LeapListenerTimeLogger = new TimeLogger();
            NoteSenderTimeLogger = new TimeLogger();
        }

        public virtual Timer NoteSender { get; set; }
        public virtual ArduinoController Arduino { get; set; }
        public virtual LeapListener LeapListener { get; set; }
        public virtual Controller LeapController { get; set; }

        public virtual TimeLogger ArduinoTimeLogger { get; set; }
        public virtual TimeLogger LeapListenerTimeLogger { get; set; }
        public virtual TimeLogger NoteSenderTimeLogger { get; set; }

        public Timer CreateTimer()
        {
            var timer = new System.Timers.Timer(10000);
            timer.Elapsed += new ElapsedEventHandler(NoteSenderHandler);
            timer.AutoReset = true;
            timer.Interval = 100;

            return timer;
        }

        public void NoteSenderHandler(object source, ElapsedEventArgs e)
        {
            NoteSenderTimeLogger.LastExecStart = NoteSenderTimeLogger.ExecStart;
            NoteSenderTimeLogger.ExecStart = DateTime.Now;

            //if (!Arduino.DoCommand(LeapListener.Message))
            //    Console.WriteLine("FAILURE!");

            NoteSenderTimeLogger.LastExecEnd = NoteSenderTimeLogger.ExecEnd;
            NoteSenderTimeLogger.ExecEnd = DateTime.Now;

            Console.WriteLine(NoteSenderTimeLogger);
        }

        public void Run()
        {
            NoteSender.Start();
            // Have the sample listener receive events from the controller
            LeapController.AddListener(LeapListener);


            // Keep this process running until Enter is pressed
            Console.WriteLine("Press Enter to quit...");
            Console.ReadLine();
        }

        public void Dispose()
        {
            LeapController.RemoveListener(LeapListener);
            LeapController.Dispose();
            Arduino.Dispose();
            NoteSender.Dispose();
        }
    }
}
