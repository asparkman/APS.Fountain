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
    public class LeapListener : Listener
    {
        public LeapListener(MotionToNotes motionToNotes, APS.Arduino.Arduino arduino)
        {
            MotionToNotes = motionToNotes;
            Arduino = arduino;
        }

        public virtual MotionToNotes MotionToNotes { get; set; }
        public virtual APS.Arduino.Arduino Arduino { get; set; }
        public virtual Task<bool> ArduinoSendTask { get; set; }

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
