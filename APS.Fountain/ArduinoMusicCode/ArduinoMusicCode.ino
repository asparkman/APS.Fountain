#include "FrameLayouts.h"
#include "Music.h"

const int LED_PIN = 5;
const int PIEZO_PIN = 8;
int lastMessage = millis();

void read();

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
        byte readByte = START_CHAR;
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
	if(bytesReceived > 0)
	{
		unescape(lastRead, MAX_FRAME_SIZE);
		byte type = determineType(lastRead, MAX_FRAME_SIZE);

		switch(type)
		{
			case MT_IDENTIFY:
				Identify identify;
				read_Identify(lastRead, MAX_FRAME_SIZE, identify);
				
				write_Identify_Ack(identify);
				break;
			case MT_SEND_NOTE:
				SendNote sendNote;
				read_SendNote(lastRead, MAX_FRAME_SIZE, sendNote);
                                write_SendNote_Ack(sendNote);
  
				for(int i = 0; i < sendNote.repeatLength; i++) {
					int delayTime = sendNote.noteLength * toneTime + (sendNote.noteLength - 1) * noToneTime;
					tone(sendNote.pin, genNote(sendNote.note), delayTime);
					capt_delay(delayTime);
    				        noTone(sendNote.pin);
    				        capt_delay(noToneTime);
				}
				break;
			case MT_SEND_PAUSE:
				SendPause sendPause;
				read_SendPause(lastRead, MAX_FRAME_SIZE, sendPause);
                                write_SendPause_Ack(sendPause);

				noTone(sendPause.pin);
				capt_delay((toneTime + noToneTime) * sendPause.repeatLength);
				break;
			case MT_SEND_NO_TONE_TIME:
				SendNoToneTime sendNoToneTime;
				read_SendNoToneTime(lastRead, MAX_FRAME_SIZE, sendNoToneTime);
                                write_SendNoToneTime_Ack(sendNoToneTime);
				
				noToneTime = sendNoToneTime.milliseconds;
				break;
			case MT_SEND_TONE_TIME:
				SendToneTime sendToneTime;
				read_SendToneTime(lastRead, MAX_FRAME_SIZE, sendToneTime);
                                write_SendToneTime_Ack(sendToneTime);
				
				toneTime = sendToneTime.milliseconds;
				break;
                        default:
                                noTone(8);
                                break;
		}
                lastMessage = millis();
	}
        
}
