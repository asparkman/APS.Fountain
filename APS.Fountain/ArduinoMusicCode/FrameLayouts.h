#ifndef FRAME_LAYOUTS_H
#define FRAME_LAYOUTS_H

#include "Arduino.h"

int toneTime = 100;
int noToneTime = 25;

const int MAX_FRAME_SIZE = 6;
char lastRead[MAX_FRAME_SIZE] = { (char) 0, (char) 0, (char) 0, (char) 0, (char) 0, (char) 0 };

const char START_CHAR = (char) 16;
const char END_CHAR = (char) 4;

const byte MT_IDENTIFY = 1;
const byte MT_SEND_NOTE = 2;
const byte MT_SEND_NO_TONE_TIME = 3;
const byte MT_SEND_PAUSE = 4;
const byte MT_SEND_TONE_TIME = 5;

struct Identify {
};

struct SendNote {
	byte pin;
	byte note;
	byte noteLength;
	byte repeatLength;
};

struct SendPause {
	byte pin;
	byte noteLength;
	byte repeatLength;
};

struct SendToneTime {
	unsigned int milliseconds;
};

struct SendNoToneTime {
	unsigned int milliseconds;
};

void unescape(char lastRead[], int length) {
	byte adderByte = ((byte)lastRead[1]);
	byte adderBit = 0;

	int i = 2;
	while(lastRead[i] != 4) {
		adderBit = (adderByte >> (i - 2)) % (2 << (i - 2));
		lastRead[i] += adderBit; 
		i++;
	}
}

bool verify(char lastRead[], int length) {
        return lastRead[0] == START_CHAR;
}

byte determineType(char lastRead[], int length) {
	return lastRead[1] >> 5;
}

void read_Identify(char lastRead[], int length, Identify &val) {

}

void read_SendNote(char lastRead[], int length, SendNote &val) {
	val.pin = ((byte)lastRead[2]) >> 4;
	val.note = (int)((((((byte)lastRead[2]) % 16) << 4) + ((byte)lastRead[3]) >> 4));
	val.noteLength = ((((byte)lastRead[3]) % 16) << 2) + (((byte)lastRead[4]) >> 6);
	val.repeatLength = ((byte)lastRead[4]) % 64;
}

void read_SendPause(char lastRead[], int length, SendPause &val) {
	val.pin = ((byte)lastRead[2]) >> 4;
	val.noteLength = ((((byte)lastRead[2]) % 16) << 2) + (((byte)lastRead[3]) >> 6);
	val.repeatLength = ((byte)lastRead[3]) % 64;
}

void read_SendToneTime(char lastRead[], int length, SendToneTime &val) {
	val.milliseconds = (((unsigned int)lastRead[2]) << 8) + ((byte)lastRead[2]);
}

void read_SendNoToneTime(char lastRead[], int length, SendNoToneTime &val) {
	val.milliseconds = (((unsigned int)lastRead[2]) << 8) + ((byte)lastRead[2]);
}
#endif
