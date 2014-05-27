using APS.Data;
using Leap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APS.Arduino;

namespace APS.Fountain
{
    /// <summary>
    /// Handles the flow of data between the Leap Motion, and the 
    /// <c>Arduino</c>.
    /// </summary>
    public class LeapListener : Listener
    {
        /// <summary>
        /// Creates a <c>LeapListener</c> that uses the given motion to notes 
        /// converter, and the given <c>Arduino</c> interface controller.
        /// </summary>
        /// <param name="motionToNotes">The object to use to convert the Leap 
        /// Motion data into notes.</param>
        /// <param name="arduino">The object to use to communicate with the 
        /// Arduino.</param>
        public LeapListener(MotionToNotes motionToNotes, APS.Arduino.Arduino arduino)
        {
            MotionToNotes = motionToNotes;
            Arduino = arduino;
        }

        /// <summary>
        /// Controls how to convert Leap Motion data into messages to be passed 
        /// to the Arduino.
        /// </summary>
        public virtual MotionToNotes MotionToNotes { get; set; }
        /// <summary>
        /// Controls the sending of data to the Arduino.
        /// </summary>
        public virtual APS.Arduino.Arduino Arduino { get; set; }
        /// <summary>
        /// Used to control the flow of messages to the Arduino.
        /// </summary>
        protected virtual Task<bool> ArduinoSendTask { get; set; }

        /// <summary>
        /// This takes every frame received by the Leap Motion controller, and 
        /// sends the Arduino a message using it.
        /// </summary>
        /// <param name="controller">The Leap Motion <c>Controller</c>.</param>
        public async override void OnFrame(Controller controller)
        {
            Frame frame = controller.Frame();

            var message = MotionToNotes.Convert(frame);

            if (ArduinoSendTask != null)
                await ArduinoSendTask;

            ArduinoSendTask = Arduino.SendAsync(message);
        }
    }
}
