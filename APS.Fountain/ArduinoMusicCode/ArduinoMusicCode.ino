#include "FrameLayouts.h"
#include "Music.h"

const int LED_PIN = 5;
const int PIEZO_PIN = 8;

void setup() {
	Serial.begin(115200, SERIAL_8N1);
	pinMode(LED_PIN, OUTPUT);
	digitalWrite(LED_PIN, HIGH);
	delay(100);
	digitalWrite(LED_PIN, LOW);

	tone(PIEZO_PIN, 294, 200);
}

void loop() {
	read();
        delay(10);
}

void read() {
	int bytesReceived = 0;
        int startPosition = 0;
        boolean hitStart = false;
	while(Serial.available() > 0)
	{
                byte readByte = Serial.read();
                if(readByte == START_CHAR)
                {
                      startPosition = bytesReceived;
                      hitStart = true;
                }
                if(hitStart)
                {
		      lastRead[bytesReceived - startPosition] = readByte;
                }
		bytesReceived++;
                if(readByte == END_CHAR && hitStart)
                  break;
	}
	if(bytesReceived != 0)
	{
		unescape(lastRead, MAX_FRAME_SIZE);
		byte type = determineType(lastRead, MAX_FRAME_SIZE);

		switch(type)
		{
			case MT_IDENTIFY:
				Identify identify;
				read_Identify(lastRead, MAX_FRAME_SIZE, identify);
				
				Serial.print("HELLO FROM ARDUINO");
				break;
			case MT_SEND_NOTE:
                                Serial.print("MT_SEND_NOTE");
				SendNote sendNote;
				read_SendNote(lastRead, MAX_FRAME_SIZE, sendNote);
  
				for(int i = 0; i < sendNote.repeatLength; i++) {
					int delayTime = sendNote.noteLength * toneTime + (sendNote.noteLength - 1) * noToneTime;
					tone(sendNote.pin, genNote(sendNote.note), delayTime);
					delay(delayTime);
    				        noTone(sendNote.pin);
    				        delay(noToneTime);
				}
				break;
			case MT_SEND_PAUSE:
                                Serial.print("MT_SEND_PAUSE");
				SendPause sendPause;
				read_SendPause(lastRead, MAX_FRAME_SIZE, sendPause);

				noTone(sendPause.pin);
				delay((toneTime + noToneTime) * sendPause.repeatLength);
				break;
			case MT_SEND_NO_TONE_TIME:
                                Serial.print("MT_SEND_NO_TONE_TIME");
				SendNoToneTime sendNoToneTime;
				read_SendNoToneTime(lastRead, MAX_FRAME_SIZE, sendNoToneTime);
				
				noToneTime = sendNoToneTime.milliseconds;
				break;
			case MT_SEND_TONE_TIME:
                                Serial.print("MT_SEND_TONE_TIME");
				SendToneTime sendToneTime;
				read_SendToneTime(lastRead, MAX_FRAME_SIZE, sendToneTime);
				
				toneTime = sendToneTime.milliseconds;
				break;
                        default:
                                Serial.print("default");
                                break;
		}
	}
}
