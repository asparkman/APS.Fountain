using APS.Data;
using APS.Data.Messages.Rx;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace APS.Arduino
{
    /// <summary>
    /// Allows for communication with an Arduino device over a serial port.
    /// </summary>
    public class Arduino
    {
        /// <summary>
        /// The number of attempts to read that should be performed in order to 
        /// identify a last sent sequence from a previous execution of the 
        /// program.
        /// </summary>
        public static readonly int NUM_SEQ_NR_ATTEMPTS = 3;

        /// <summary>
        /// The number of times the identification sequence is attempted.
        /// </summary>
        private static readonly int NUM_ID_ATTEMPTS = 5;

        /// <summary>
        /// The number of times Read() is attempted after an 
        /// <c>APS.Data.Messages.Tx.Identify</c> is sent.
        /// </summary>
        private static readonly int NUM_ID_READS_PER_SEND = 3;

        /// <summary>
        /// Initializes a communication interface with the Arduino with an 
        /// implementation of <c>ISerialPort</c>, and a possible specific value 
        /// for <c>Random</c>.
        /// </summary>
        /// <param name="port">The implementation of <c>ISerialPort</c> that 
        /// message will be sent over to communicate with the Arduino.</param>
        /// <param name="rand">The random number generator used to generate 
        /// <c>Identify</c> requests.</param>
        public Arduino(ISerialPort port, Random rand = null)
        {
            Rand = rand ?? new Random();
            Port = port;
            LastSentSeqNr = false;
        }

        /// <summary>
        /// The random number generator used to create <c>Identify</c> 
        /// messages.
        /// </summary>
        public virtual Random Rand { get; set; }
        /// <summary>
        /// The implementation of <c>ISerialPort</c> used to communicate with 
        /// the Arduino.
        /// </summary>
        public virtual ISerialPort Port { get; set; }

        /// <summary>
        /// The last sequence number sent to the Arduino.
        /// </summary>
        protected virtual bool LastSentSeqNr { get; set; }
        /// <summary>
        /// The last message sent to the Arduino.
        /// </summary>
        protected volatile TxMessage SentMessage;
        /// <summary>
        /// The next message to send to the Arduino.
        /// </summary>
        protected volatile TxMessage NextMessageToSend;

        /// <summary>
        /// Waits until it send before returning.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual async Task<bool> SendAsync(TxMessage message)
        {
            NextMessageToSend = message;

            var task = new Task<bool>(Run);
            task.Start();

            return await task;
        }

        /// <summary>
        /// Performs the identification sequence against the given serial port.
        /// </summary>
        /// <param name="port">The port to examine for compliance.</param>
        /// <param name="rand">The random number generator to use to generate 
        /// Random0, and Random1 numbers.</param>
        /// <returns>An Arduino object since it has been identified.</returns>
        public static Arduino IsArduino(ISerialPort port, Random rand = null)
        {
            var candidate = new Arduino(port, rand);

            // Tries to initialize the candidate's last sent sequence number
            // according to what the candidate is sending.
            InitializeLastSeqNr(candidate);

            // We flip this, so it will be correct after the Write().
            candidate.LastSentSeqNr = !candidate.LastSentSeqNr;

            // If identified return the initialized candidate.
            return TryIdentify(candidate) ? candidate : null;
        }

        /// <summary>
        /// Tries to identify the arduino.
        /// </summary>
        /// <param name="candidate">The candidate to determine if it is an 
        /// Arduino.</param>
        /// <returns>Whether or not the candidate was identified as an Arduino.
        /// </returns>
        private static bool TryIdentify(Arduino candidate)
        {
            // Setup the random numbers we will be sending.
            var identifyRequest = new APS.Data.Messages.Tx.Identify();
            identifyRequest.Random0 = (byte) candidate.Rand.Next();
            identifyRequest.Random1 = (byte) candidate.Rand.Next();
            // Sets the sequence, so it matches LastSentSeqNr.  The 
            // LastSentSeqNr should already be set, so it is correct after a 
            // Write().
            identifyRequest.Sequence = (byte)(candidate.LastSentSeqNr ? 1 : 0);

            // Position the identify request for sending.
            candidate.NextMessageToSend = identifyRequest;

            // Set the number of attempts we will allow to write.
            var numIdAttemptsLeft = NUM_ID_ATTEMPTS;
            var sent = false;
            var received = false;
            candidate.Port.ReadExisting();
            // Do this until we run out of attempts, or until both are sent 
            // and received.
            while (numIdAttemptsLeft > 0 && !(sent && received))
            {
                try
                {
                    if (numIdAttemptsLeft == NUM_ID_ATTEMPTS) 
                        candidate.Write(); 
                    else 
                        candidate.ReWrite(); // Write it.
                    sent = true; // Mark that we wrote it.
                }
                catch (TimeoutException)
                {
                    // If we time out, then the sent variable isn't going 
                    // to be set, and it will be resent.
                }

                // Set the number of attempts we will allow to read for this 
                // write.
                var numIdReadAttemptsLeft = NUM_ID_READS_PER_SEND;
                // Read X times or until we have identified.
                while (numIdReadAttemptsLeft > 0 && !received)
                {
                    try
                    {
                        RxMessage receivedMessage = null;
                        while (receivedMessage == null) // Read it, until it finds a complete message.
                            receivedMessage = candidate.Read();
                        // Check to make sure we got the correct sequence number back.
                        var seqNrMatch = receivedMessage.Sequence == 0 && !candidate.LastSentSeqNr
                            || receivedMessage.Sequence == 1 && candidate.LastSentSeqNr;
                        // Make sure the sequence numbers match, and the received 
                        // is message matches what is expected.
                        if (seqNrMatch && IsExpected(identifyRequest, receivedMessage))
                        {
                            received = true; // Mark that we successfully identified.
                            // Changes the last sequence number, so that it 
                            // will represent what was last sent after the next Write().
                            candidate.LastSentSeqNr = !candidate.LastSentSeqNr;
                        }
                    }
                    catch (TimeoutException)
                    {
                        // If we time out, then the sent variable isn't going 
                        // to be set, and it will be resent.
                    }

                    numIdReadAttemptsLeft--;
                }

                numIdAttemptsLeft--;
            }

            return sent && received;
        }

        /// <summary>
        /// This is used to initialize the last sequence number accepted by 
        /// the Arduino.
        /// </summary>
        /// <param name="candidate">The candidate <c>Arduino</c> object to 
        /// perform the initialization.</param>
        private static void InitializeLastSeqNr(Arduino candidate)
        {
            // So we start with a Read in contrast to the Run() method.  This 
            // is because we can always expect the Arduino to be sending.  If
            // the program restarts, it will continue to send an ack for its 
            // last received message, or an identify for its last detected 
            // identify.
            int seqNrAttemptsLeft = NUM_SEQ_NR_ATTEMPTS;
            bool identifiedSeqNr = false;
            while (seqNrAttemptsLeft > 0 && !identifiedSeqNr)
            {
                try
                {
                    RxMessage receivedMessage = null;
                    // Read it, until it finds a complete message, or it times out.
                    while (receivedMessage == null)
                        receivedMessage = candidate.Read();
                    // Set the last sent sequence number according to what the 
                    // Arduino is sending.
                    candidate.LastSentSeqNr = receivedMessage.Sequence > 0;
                    identifiedSeqNr = true;
                }
                catch (TimeoutException)
                {
                    // If we time out, then the sent variable isn't going 
                    // to be set, and it will be resent.
                }
                seqNrAttemptsLeft--;
            }
        }

        /// <summary>
        /// Holds the rules for determining if the Tx.Identify matches the 
        /// Rx.Identify response.  The identifyResponse needs to be a 
        /// <c>RxType.IDENTIFY</c>.  The Arduino is expected to increment both 
        /// the Random0, and Random1 properties before sending it back.
        /// </summary>
        /// <param name="identifyRequest">The request for identification sent 
        /// to the candidate Arduino.</param>
        /// <param name="identifyResponse">The response for identification sent 
        /// to the candidate Arduino.</param>
        /// <returns>Whether or not the rules for determining a match are correct.</returns>
        private static bool IsExpected(
            APS.Data.Messages.Tx.Identify identifyRequest, 
            APS.Data.RxMessage identifyResponse
        )
        {
            bool result = false;

            if(identifyResponse.Type.Equals(RxType.IDENTIFY))
            {
                var conv = (APS.Data.Messages.Rx.Identify) identifyResponse;
                var random0Match = identifyRequest.Random0.Equals((byte)((conv.Random0 - 1) % 256));
                var random1Match = identifyRequest.Random1.Equals((byte)((conv.Random1 - 1) % 256));

                result = random0Match && random1Match;
            }

            return result;
        }

        /// <summary>
        /// This should be run in a seperate thread.
        /// </summary>
        public virtual bool Run()
        {
            // Sets the sequence, so after the Write it will have the 
            // correct value in the LastSentSeqNr.
            NextMessageToSend.Sequence = (byte)(LastSentSeqNr ? 1 : 0);

            bool sent = false;
            bool received = false;
            bool hasWritten = false;
            while (!(sent && received))
            {
                try
                {
                    if (!hasWritten) 
                        Write(); 
                    else 
                        ReWrite(); // Write it.
                    sent = true; // Mark that we wrote it.
                }
                catch (TimeoutException)
                {
                    // If we time out, then the sent variable isn't going 
                    // to be set, and it will be resent.
                }

                hasWritten = true;

                try
                {
                    RxMessage receivedMessage = null;
                    while (receivedMessage == null) // Read it, until it finds a complete message.
                        receivedMessage = Read();
                    if ( // Check to make sure we got the correct sequence number back.
                        receivedMessage.Sequence == 0 && !LastSentSeqNr
                        || receivedMessage.Sequence == 1 && LastSentSeqNr
                        )
                    {
                        received = true; // Mark that we got an ack for the appropriate one.
                        // Changes the last sequence number, so that it 
                        // will represent what was last sent after the next Write().
                        LastSentSeqNr = !LastSentSeqNr; 
                    }
                }
                catch (TimeoutException)
                {
                    // If we time out, then the sent variable isn't going 
                    // to be set, and it will be resent.
                }
            }

            return true;
        }

        /// <summary>
        /// Resend the <c>SentMessage</c>.
        /// </summary>
        protected virtual void ReWrite()
        {
            Port.Write(SentMessage.Bytes, 0, SentMessage.Bytes.Length);
        }

        /// <summary>
        /// Write the <c>NextMessageToSend</c> to the Arduino, and move it to 
        /// the <c>SentMessage</c> property.
        /// </summary>
        protected virtual void Write()
        {
            Port.Write(NextMessageToSend.Bytes, 0, NextMessageToSend.Bytes.Length);
            SentMessage = NextMessageToSend;
            NextMessageToSend = null;
        }


        private static int _ReceiveBufferSize = Enum.GetValues(typeof(RxField)).Length;
        private byte[] _ReceiveBuffer = new byte[_ReceiveBufferSize];
        /// <summary>
        /// Read a single message from the Arduino, and clear the buffer if any 
        /// other bytes remain in it.
        /// </summary>
        /// <returns>The first available message in the buffer.</returns>
        protected virtual RxMessage Read()
        {
            RxMessage result = null;
            _ReceiveBuffer = Enumerable.Repeat<byte>(0, _ReceiveBufferSize).ToArray();
            while ((_ReceiveBuffer[0] = (byte)Port.ReadByte()) != RxMessage._START);
            
            int counter = 1;
            while (
                counter < _ReceiveBufferSize // Make sure we don't overrun the buffer.
                && ((_ReceiveBuffer[counter] = (byte)Port.ReadByte()) != RxMessage._END) // Make sure we don't hit an end byte.
                && _ReceiveBuffer[counter] != RxMessage._START // Make sure we don't hit the start of another frame.
            )
                counter++;

            if(
                    counter == _ReceiveBufferSize - 1 // Did the counter reach the end of the buffer?
                    && _ReceiveBuffer[0] == RxMessage._START // Did the start involve a START byte?
                    && _ReceiveBuffer[_ReceiveBufferSize - 1] == RxMessage._END // Did the end involve an end byte?
                )
            {
                var ack = new Ack(_ReceiveBuffer);
                var rxType = ack[RxField.INFO_0];
                switch(rxType)
                {
                    case (int) RxType.IDENTIFY:
                        result = new Identify(_ReceiveBuffer);
                        break;
                    case (int) RxType.ACK:
                        result = ack;
                        break;
                }

                var bytesToRead = Port.BytesToRead;
                if (bytesToRead > 100)
                    Port.ReadExisting();
                
            }

            return result;
        }
    }
}
