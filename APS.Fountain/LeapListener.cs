using APS.Data;
using Leap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Fountain
{
    public class LeapListener : Listener
    {
        private Object thisLock = new Object();

        private DateTime Start { get; set; }
        private DateTime LastStart { get; set; }
        private DateTime End { get; set; }

        private TimeSpan Max { get; set; }
        private TimeSpan Min { get; set; }

        public void SafeWriteLine(String line)
        {
            lock (thisLock)
            {
                Console.WriteLine(line);
            }
        }

        public override void OnInit(Controller controller)
        {
            SafeWriteLine("Initialized");

            Min = new TimeSpan(0, 0, 0, 1);
        }

        public override void OnConnect(Controller controller)
        {
            SafeWriteLine("Connected");
        }

        public override void OnDisconnect(Controller controller)
        {
            //Step: not dispatched when running in a debugger.
            SafeWriteLine("Disconnected");
        }

        public override void OnExit(Controller controller)
        {
            SafeWriteLine("Exited");
        }

        public override void OnFrame(Controller controller)
        {
            // Get the most recent frame and report some basic information
            Frame frame = controller.Frame();

            if (!frame.Hands.IsEmpty)
            {
                // Get the first hand
                Hand hand = frame.Hands[0];

                // Check if the hand has any fingers
                FingerList fingers = hand.Fingers;
                if (!fingers.IsEmpty)
                {
                    // Calculate the hand's average finger tip position
                    Vector avgPos = Vector.Zero;
                    foreach (Finger finger in fingers)
                    {
                        avgPos += finger.TipPosition;
                    }
                    avgPos /= fingers.Count;
                }


                // Get the hand's normal vector and direction
                Vector normal = hand.PalmNormal;
                Vector direction = hand.Direction;
            }


            //Message = null;
            //new SendNote()
            //{
            //    NoteLength = 1,
            //    Pin = 8,
            //    RepeatLength = 1,
            //    Note = frame.Hands.IsEmpty ? 0 : 1
            //};

            LastStart = Start;
            Start = DateTime.Now;



            if (LastStart != default(DateTime))
            {
                var timeSpan = (Start - LastStart);
                if (timeSpan > Max)
                    Max = timeSpan;
                if (timeSpan < Min)
                    Min = timeSpan;

                SafeWriteLine(string.Format("Elapsed: {0}, Min: {1}, Max: {2}", timeSpan.Milliseconds, Min.Milliseconds, Max.Milliseconds));
            }
        }
    }
}
