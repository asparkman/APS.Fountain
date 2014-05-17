using APS.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace APS.Fountain.Tests
{
    [TestFixture]
    public class TestArduinoController
    {
        public static void Main()
        {
            new TestArduinoController().Timer_Success();
            Thread.Sleep(1000);
        }

        [Test]
        public void Timer_Success()
        {
            CurrentNote = 0;
            Arduino = new ArduinoController();
            Assert.IsTrue(Arduino.SetComPort());
            NoteSender = CreateTimer();

            NoteSender.Start();

            Console.WriteLine("Press any key to continue!");
            Console.ReadKey();
        }

        public virtual System.Timers.Timer NoteSender { get; set; }
        public virtual ArduinoController Arduino { get; set; }

        public virtual int CurrentNote { get; set; }

        public System.Timers.Timer CreateTimer()
        {
            var timer = new System.Timers.Timer(10000);
            timer.Elapsed += new ElapsedEventHandler(NoteSenderHandler);
            timer.AutoReset = true;
            timer.Interval = 95;

            return timer;
        }

        public void NoteSenderHandler(object source, ElapsedEventArgs e)
        {
            var message = new SendNote()
            {
                Pin = 8,
                Note = CurrentNote,
                NoteLength = 1,
                RepeatLength = 1
            };
            CurrentNote++;

            if (CurrentNote == 10)
                CurrentNote = 0;

            bool result = false;
            lock (Arduino)
            {
                result = Arduino.DoCommand(message);
            }
        }
    }
}
