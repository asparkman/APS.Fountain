using APS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace APS.Fountain
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var controller = new ArduinoController())
            {
                if(controller.SetComPort())
                {
                    var sendToneTime = new SendToneTime()
                    {
                        Milliseconds = 100
                    };
                    var sendNoToneTime = new SendNoToneTime()
                    {
                        Milliseconds = 25
                    };
                    controller.DoCommand(sendToneTime, "MT_SEND_TONE_TIME");
                    controller.DoCommand(sendNoToneTime, "MT_SEND_NO_TONE_TIME");

                    var notes = new int[] { -1, -1, -2, -2, -3, -3 };
                    var rand = new Random(5);
                    ArduinoController.SLEEP_CONSTANT = 100;
                    while (true)
                    {
                        sendToneTime = new SendToneTime()
                        {
                            Milliseconds = (ushort)(100)
                        };
                        sendNoToneTime = new SendNoToneTime()
                        {
                            Milliseconds = (ushort)(25)
                        };
                        controller.DoCommand(sendToneTime, "MT_SEND_TONE_TIME");
                        controller.DoCommand(sendNoToneTime, "MT_SEND_NO_TONE_TIME");

                        foreach (var note in notes)
                        {
                            var sendNote = new SendNote();
                            sendNote.Pin = 8;
                            sendNote.NoteLength = 1;
                            sendNote.RepeatLength = 5;
                            sendNote.Note = 3 + 57;

                            controller.DoCommand(sendNote, "MT_SEND_NOTE");
                            var noteTime = sendNote.NoteLength * sendToneTime.Milliseconds + (sendNote.NoteLength - 1) * sendNoToneTime.Milliseconds;
                            var repeatTime = sendNote.RepeatLength * noteTime + (sendNote.RepeatLength - 1) * sendNoToneTime.Milliseconds;
                            Thread.Sleep(repeatTime);
                            Console.WriteLine("SLEEP_TIME = {0}", repeatTime);
                        }
                    }

                }
            }

            Console.ReadKey();
        }
    }
}
