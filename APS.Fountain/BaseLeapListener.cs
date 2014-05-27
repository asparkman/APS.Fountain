using Leap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Fountain
{
    /// <summary>
    /// Base of Leap Motion listener.
    /// </summary>
    public abstract class BaseLeapListener : Listener
    {
        /// <summary>
        /// Used for thread-safe writing to the <c>Console</c> window.
        /// </summary>
        private Object thisLock = new Object();

        /// <summary>
        /// Safely writes to the <c>Console</c> window.
        /// </summary>
        /// <param name="line"></param>
        public void SafeWriteLine(String line)
        {
            lock (thisLock)
            {
                Console.WriteLine(line);
            }
        }

        /// <summary>
        /// Shows that this method has been called via the console.
        /// </summary>
        /// <param name="controller">The Leap Motion <c>Controller</c> that 
        /// this listener is attached to.</param>
        public override void OnInit(Controller controller)
        {
            SafeWriteLine("Initialized");
        }

        /// <summary>
        /// Shows that this method has been called via the console.
        /// </summary>
        /// <param name="controller">The Leap Motion <c>Controller</c> that 
        /// this listener is attached to.</param>
        public override void OnConnect(Controller controller)
        {
            SafeWriteLine("Connected");
        }

        /// <summary>
        /// Shows that this method has been called via the console.
        /// </summary>
        /// <param name="controller">The Leap Motion <c>Controller</c> that 
        /// this listener is attached to.</param>
        public override void OnDisconnect(Controller controller)
        {
            //Step: not dispatched when running in a debugger.
            SafeWriteLine("Disconnected");
        }

        /// <summary>
        /// Shows that this method has been called via the console.
        /// </summary>
        /// <param name="controller">The Leap Motion <c>Controller</c> that 
        /// this listener is attached to.</param>
        public override void OnExit(Controller controller)
        {
            SafeWriteLine("Exited");
        }
    }
}
