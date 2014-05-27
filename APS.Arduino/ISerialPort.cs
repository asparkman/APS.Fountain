﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.Arduino
{
    /// <summary>
    /// This is an interface that copies <c>SerialPort</c>.  It is intended to 
    /// improve testability.
    /// </summary>
    public interface ISerialPort
    {
        #pragma warning disable 1591
        Stream BaseStream { get; }
        int BaudRate { get; set; }
        bool BreakState { get; set; }
        int BytesToRead { get; }
        int BytesToWrite { get; }
        bool CDHolding { get; }
        bool CtsHolding { get; }
        int DataBits { get; set; }
        bool DiscardNull { get; set; }
        bool DsrHolding { get; }
        bool DtrEnable { get; set; }
        Encoding Encoding { get; set; }
        Handshake Handshake { get; set; }
        bool IsOpen { get; }
        string NewLine { get; set; }
        Parity Parity { get; set; }
        byte ParityReplace { get; set; }
        string PortName { get; set; }
        int ReadBufferSize { get; set; }
        int ReadTimeout { get; set; }
        int ReceivedBytesThreshold { get; set; }
        bool RtsEnable { get; set; }
        StopBits StopBits { get; set; }
        int WriteBufferSize { get; set; }
        int WriteTimeout { get; set; }

        event SerialDataReceivedEventHandler DataReceived;
        event SerialErrorReceivedEventHandler ErrorReceived;
        event SerialPinChangedEventHandler PinChanged;

        void Close();
        void DiscardInBuffer();
        void DiscardOutBuffer();
        void Open();
        int Read(byte[] buffer, int offset, int count);
        int Read(char[] buffer, int offset, int count);
        int ReadByte();
        int ReadChar();
        string ReadExisting();
        string ReadLine();
        string ReadTo(string value);
        void Write(string text);
        void Write(byte[] buffer, int offset, int count);
        void Write(char[] buffer, int offset, int count);
        void WriteLine(string text);
        #pragma warning restore 1591
    }
}
