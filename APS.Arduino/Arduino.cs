using APS.Data;
using APS.Data.Messages.Rx;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Arduino
{
    public class Arduino
    {
        public Arduino(SerialPort port, Random rand = null)
        {
            Rand = rand ?? new Random();
            Port = port;
            KeepRunning = true;
            LastSentSeqNr = true;
        }

        public virtual Random Rand { get; set; }
        public virtual SerialPort Port { get; set; }

        protected virtual bool KeepRunning { get; set; }
        protected virtual bool LastSentSeqNr { get; set; }
        protected volatile TxMessage SentMessage;
        protected volatile TxMessage NextMessageToSend;

        /// <summary>
        /// Waits until it send before returning.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual bool Send(TxMessage message)
        {
            while (NextMessageToSend != null);

            NextMessageToSend = message;

            return true;
        }

        /// <summary>
        /// This should be run in a seperate thread.
        /// </summary>
        public virtual void Run()
        {
            while(true)
            {
                // Flip the sequence number.
                LastSentSeqNr = !LastSentSeqNr;
                // Wait for new message to arrive.
                while (NextMessageToSend == null);

                bool sent = false;
                bool received = false;
                while (!(sent && received))
                {
                    try
                    {
                        Write(); // Write it.
                        sent = true; // Mark that we wrote it.
                    }
                    catch (TimeoutException)
                    {
                        // If we time out, then the sent variable isn't going 
                        // to be set, and it will be resent.
                    }

                    try
                    {
                        RxMessage receivedMessage = null;
                        while (receivedMessage == null) // Read it, until it finds a complete message.
                            receivedMessage = Read();
                        if( // Check to make sure we got the correct sequence number back.
                            receivedMessage.Sequence == 0 && !LastSentSeqNr
                            || receivedMessage.Sequence == 1 && LastSentSeqNr
                            )
                            received = true; // Mark that we got an ack for the appropriate one.
                    }
                    catch (TimeoutException)
                    {
                        // If we time out, then the sent variable isn't going 
                        // to be set, and it will be resent.
                    }
                }
                
            }
        }

        protected virtual void Write()
        {
            Port.Write(NextMessageToSend.Bytes, 0, NextMessageToSend.Bytes.Length);
            SentMessage = NextMessageToSend;
            NextMessageToSend = null;
        }


        private static int _ReceiveBufferSize = Enum.GetValues(typeof(RxField)).Length;
        private byte[] _ReceiveBuffer = new byte[_ReceiveBufferSize];
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
                    counter == _ReceiveBufferSize // Did the counter reach the end of the buffer?
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
            }

            return result;
        }
    }
}
