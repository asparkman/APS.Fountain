using APS.Data;
using APS.Data.Messages.Tx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Fountain
{
    public class LinearMotionToNotes : MotionToNotes
    {
        public LinearMotionToNotes(float scaleX = 1.0f, float scaleY = 8.0f, float scaleZ = 64.0f)
        {
            ScaleX = scaleX;
            ScaleY = scaleY;
            ScaleZ = scaleZ;
        }

        protected static readonly float MAX_X = 256.0f;
        protected static readonly float MIN_X = -256.0f;
        protected static readonly float MAX_Y = 256.0f;
        protected static readonly float MIN_Y = 0.0f;
        protected static readonly float MAX_Z = 256.0f;
        protected static readonly float MIN_Z = -256.0f;

        protected float ScaleX { get; set; }
        protected float ScaleY { get; set; }
        protected float ScaleZ { get; set; }

        protected static readonly TxMessage DEFAULT_LAST = new Note()
                {
                    Pin = 8,
                    NoteLength = 1,
                    RepeatLength = 1,
                    Step = 1
                };
        protected TxMessage Last { get; set; }

        public override Data.TxMessage Convert(Leap.Frame frame)
        {
            TxMessage result = null;
            if(frame.Fingers.Any())
            {
                #region FIGURES OUT STEP
                var avgX = frame.Fingers.Average(x => x.TipPosition.x);
                var avgY = frame.Fingers.Average(x => x.TipPosition.y);
                var avgZ = frame.Fingers.Average(x => x.TipPosition.z);

                var unitX = (avgX - MIN_X) / (MAX_X - MIN_X);
                var unitY = (avgY - MIN_Y) / (MAX_Y - MIN_Y);
                var unitZ = (avgZ - MIN_Z) / (MAX_Z - MIN_Z);

                var scaledX = Math.Round(unitX * 8.0f, 0) * ScaleX;
                var scaledY = Math.Round(unitY * 8.0f, 0) * ScaleY;
                var scaledZ = Math.Round(unitZ * 4.0f, 0) * ScaleZ;

                var step = (byte) (scaledX + scaledY + scaledZ);
                #endregion

                // Saves the step.
                result = new Note()
                {
                    Pin = 8,
                    NoteLength = 1,
                    RepeatLength = 1,
                    Step = step
                };
            }
            else
            {
                result = Last ?? DEFAULT_LAST;
            }
                
            return Last = result;
        }
    }
}
