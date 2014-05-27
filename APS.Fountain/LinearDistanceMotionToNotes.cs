using APS.Data;
using APS.Data.Messages.Tx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Fountain
{
    /// <summary>
    /// Converts the distance between the average finger tip position and the 
    /// sensor.
    /// </summary>
    public class LinearDistanceMotionToNotes : MotionToNotes
    {
        /// <summary>
        /// Initializes a <c>MotionToNotes</c> object that converts linearly
        /// using the distance between the average finger tip positions and the 
        /// sensor.
        /// </summary>
        public LinearDistanceMotionToNotes()
        {
            MaxDistance = 0.0f;
        }

        /// <summary>
        /// The default <c>Arduino</c> message to be sent before one has been 
        /// created from the <c>Convert(Leap.Frame)</c> method.
        /// </summary>
        protected static readonly TxMessage DEFAULT_LAST = new Pause()
                {
                    Pin = 8,
                    NoteLength = 1,
                    RepeatLength = 1
                };
        /// <summary>
        /// The last resultant conversion from the <c>Convert(Leap.Frame)</c> 
        /// method.
        /// </summary>
        protected TxMessage Last { get; set; }

        /// <summary>
        /// The maximum distance captured by the Leap Motion between the 
        /// average of the finger tip positions and the sensor.
        /// </summary>
        protected double MaxDistance { get; set; }

        /// <summary>
        /// Provides the last time a finger was found in the Leap Motion data 
        /// sent to <c>Convert(Leap.Frame)</c>.
        /// </summary>
        protected long LastFingerRecordTime { get; set; }

        /// <summary>
        /// Takes the distance between the average finger tip position, and the 
        /// Leap Motion sensor, and converts it to a <c>Note</c> to play.
        /// </summary>
        /// <param name="frame">The leap motion data containing the finger 
        /// information.</param>
        /// <returns>Will return a <c>Note</c> if at least one finger has been 
        /// detected within so long of its last call.  Otherwise it will return
        /// a <c>Pause</c>.</returns>
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
