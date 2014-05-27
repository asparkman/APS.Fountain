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
    /// Takes Leap Motion data, and maps a single note to each grid.
    /// </summary>
    public class GriddedMotionToNotes : MotionToNotes
    {
        /// <summary>
        /// Creates a <c>MotionToNotes</c> object that takes the average of the 
        /// available finger-tip positions, and converts them into notes.  
        /// Scales may be added.
        /// </summary>
        /// <param name="scaleX">Initializes <c>ScaleX</c>.</param>
        /// <param name="scaleY">Initializes <c>ScaleY</c>.</param>
        /// <param name="scaleZ">Initializes <c>ScaleZ</c>.</param>
        public GriddedMotionToNotes(float scaleX = 1.0f, float scaleY = 8.0f, float scaleZ = 64.0f)
        {
            ScaleX = scaleX;
            ScaleY = scaleY;
            ScaleZ = scaleZ;
        }

        // The following contains dimensional boundaries for the Leap 
        // Motion.  As of its writing, these are magic numbers.  The Leap 
        // Motion API does not provide meta data about its bounds.
        #region DIMENSIONAL BOUNDARIES
        /// <summary>
        /// The maximum value in the X-dimension.
        /// </summary>
        protected static readonly float MAX_X = 256.0f;
        /// <summary>
        /// The minimum value in the X-dimension.
        /// </summary>
        protected static readonly float MIN_X = -256.0f;
        /// <summary>
        /// The maximum value in the Y-dimension.
        /// </summary>
        protected static readonly float MAX_Y = 256.0f;
        /// <summary>
        /// The minimum value in the Y-dimension.
        /// </summary>
        protected static readonly float MIN_Y = 0.0f;
        /// <summary>
        /// The maximum value in the Z-dimension.
        /// </summary>
        protected static readonly float MAX_Z = 256.0f;
        /// <summary>
        /// The minimum value in the Y-dimension.
        /// </summary>
        protected static readonly float MIN_Z = -256.0f;
        #endregion

        #region DIMENSIONAL RESOLUTIONS
        /// <summary>
        /// How many lengths the X-dimension is split into.
        /// </summary>
        protected static readonly float RESOLUTION_X = 8.0f;
        /// <summary>
        /// How many lengths the Y-dimension is split into.
        /// </summary>
        protected static readonly float RESOLUTION_Y = 8.0f;
        /// <summary>
        /// How many lengths the Z-dimension is split into.
        /// </summary>
        protected static readonly float RESOLUTION_Z = 4.0f;
        #endregion

        #region DIMENSIONAL SCALES
        /// <summary>
        /// How much to scale the tone by in reference to the x-th set of 
        /// grids the position falls in.
        /// </summary>
        protected float ScaleX { get; set; }
        /// <summary>
        /// How much to scale the tone by in reference to the y-th set of 
        /// grids the position falls in.
        /// </summary>
        protected float ScaleY { get; set; }
        /// <summary>
        /// How much to scale the tone by in reference to the z-th set of 
        /// grids the position falls in.
        /// </summary>
        protected float ScaleZ { get; set; }
        #endregion

        /// <summary>
        /// The default note to play, when no note has been selected using the 
        /// Leap Motion device.
        /// </summary>
        protected static readonly TxMessage DEFAULT_LAST = new Note()
                {
                    Pin = 8,
                    NoteLength = 1,
                    RepeatLength = 1,
                    Step = 1
                };

        /// <summary>
        /// The last note that was converted.
        /// </summary>
        protected TxMessage Last { get; set; }

        /// <summary>
        /// The conversion changes the Leap Motion space into a set of cubes.  
        /// It then calculates the average of the finger tips detected by the 
        /// Leap Motion.  The cube that contains that average position is 
        /// located, and used to determine a note to send to the Arduino.
        /// </summary>
        /// <param name="frame">The leap motion data containing the finger 
        /// information.</param>
        /// <returns>A note to send to the Arduino.  This will always return a 
        /// <c>Note</c>.</returns>
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

                var scaledX = Math.Round(unitX * RESOLUTION_X, 0) * ScaleX;
                var scaledY = Math.Round(unitY * RESOLUTION_Y, 0) * ScaleY;
                var scaledZ = Math.Round(unitZ * RESOLUTION_Z, 0) * ScaleZ;

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
