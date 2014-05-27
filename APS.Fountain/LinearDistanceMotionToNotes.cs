using APS.Data;
using APS.Data.Messages.Tx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Fountain
{
    public class LinearDistanceMotionToNotes : MotionToNotes
    {
        public LinearDistanceMotionToNotes()
        {
            MaxDistance = 0.0f;
        }

        protected static readonly TxMessage DEFAULT_LAST = new Pause()
                {
                    Pin = 8,
                    NoteLength = 1,
                    RepeatLength = 1
                };
        protected TxMessage Last { get; set; }

        protected double MaxDistance { get; set; }

        protected long LastFingerRecordTime { get; set; }

        public override Data.TxMessage Convert(Leap.Frame frame)
        {
            TxMessage result = null;
            if(frame.Fingers.Any())
            {
                #region FIGURES OUT STEP
                var avgX = frame.Fingers.Average(x => x.TipPosition.x);
                var avgY = frame.Fingers.Average(x => x.TipPosition.y);
                var avgZ = frame.Fingers.Average(x => x.TipPosition.z);

                var sumOfSquares = 
                    Math.Pow(avgX, 2.0) 
                    + Math.Pow(avgY, 2.0) 
                    + Math.Pow(avgZ, 2.0);

                var distance =
                    Math.Sqrt(sumOfSquares);

                if(distance > MaxDistance)
                    MaxDistance = distance;

                var step = (byte) (Math.Round((distance / MaxDistance) * 20.0, 0) + 40.0);
                #endregion

                // Saves the step.
                result = new Note()
                {
                    Pin = 8,
                    NoteLength = 1,
                    RepeatLength = 1,
                    Step = step
                };

                Console.WriteLine("{0:D3}", step);

                LastFingerRecordTime = DateTime.Now.Ticks;
            }
            else
            {
                if (DateTime.Now.Ticks - LastFingerRecordTime <= 2000000)
                    result = Last ?? DEFAULT_LAST;
                else
                    result = DEFAULT_LAST;
            }
                
            return Last = result;
        }
    }
}
