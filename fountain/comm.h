#ifndef COMM_H
#define COMM_H

#include "Arduino.h"



// The field names of the TX messages that are received by the Arduino.
enum TxField { 
    TX_START, TX_ESCAPE, TX_SIZE_SEQ, 
    TX_INFO_0, TX_INFO_1, TX_INFO_2, 
    TX_INFO_3, TX_INFO_4, TX_INFO_5, 
    TX_STOP,
    TX_SIZE
}; 

// The type of TX messages that can be received by the Arduino.
enum TxType {
    TX_IDENTIFY,
    NOTE,
    PAUSE,
    NO_TONE_TIME,
    TONE_TIME
};

// The field names of the RX messages that are received by the computer.
enum RxField { 
    RX_START, RX_ESCAPE, RX_SIZE_SEQ, 
    RX_INFO_0, RX_INFO_1, RX_INFO_2, 
    RX_STOP,
    RX_SIZE
}; 

// The type of RX messages that are received by the computer.
enum RxType {
    RX_IDENTIFY,
    ACK
};

// The start byte of every message.
const unsigned char _START = 16;

// The end byte of every message.
const unsigned char _END = 4;

// Reads a TX message into the byte arr argument.  The array should be of 
// TX_SIZE length.  Returns false if a timeout, or framing error occurred.
boolean readTxMessage(unsigned char *arr);

// Writes an RX message to Serial.  The array should be of RX_SIZE length.
// Returns false if a timeout, or framing error occurred.
boolean writeRxMessage(unsigned char* arr);

// Manages the messages sent to the Arduino.
void waitForMessages();

// Blocks until the identify handshake is completed.
void waitUntilIdentified();


// Base defintion of messages sent between the computer.
class Message {
	public:
		unsigned char getType();
		
		unsigned char getSize();
		
		unsigned char getSeqNr();
		void setSeqNr(unsigned char val);
		
		unsigned char* getBytes();
		virtual void setBytes(unsigned char *arr);
		
	protected:
		Message();
		~Message();
		
		void setType(unsigned char val);
		
		void setSize(unsigned char val);
		
		unsigned char* bytes;
};

// Base definition for messages sent from the Arduino.
class RxMessage : public Message{
	public:
		~RxMessage();
		
		void setBytes(unsigned char *arr);
	protected:
		RxMessage();
		RxMessage(unsigned char arr[]);
};

// A message sent from the Arduino to identify that it is in fact an Arduino.
class RxIdentify : public RxMessage {
	public:
		RxIdentify();
		RxIdentify(unsigned char arr[]);
		~RxIdentify();
		
		unsigned char getRandom0();
		void setRandom0(unsigned char val);
		
		unsigned char getRandom1();
		void setRandom1(unsigned char val);
};
// An empty message sent from the Arduino to indicate that it has received a 
// message.
class Ack : public RxMessage {
	public:
		Ack();
		Ack(unsigned char arr[]);
		~Ack();
};

// Base definition for messages sent from the computer.
class TxMessage : public Message {
	public:
		~TxMessage();
		
		void setBytes(unsigned char *arr);
		
	protected:
		TxMessage();
		TxMessage(unsigned char arr[]);
};

// A message sent from the computer to identify if the device on the other end 
// of the serial port is an Arduino.
class TxIdentify : public TxMessage {
	public:
		TxIdentify();
		TxIdentify(unsigned char arr[]);
		~TxIdentify();
		
		unsigned char getRandom0();
		void setRandom0(unsigned char val);
		
		unsigned char getRandom1();
		void setRandom1(unsigned char val);
};

// Message for playing notes for a total of NoteLength counts a total of 
// RepeatLength times.
class Note : public TxMessage {
	public:
		Note();
		Note(unsigned char arr[]);
		~Note();
		
		unsigned char getPin();
		void setPin(unsigned char val);
		
		unsigned char getStep();
		void setStep(unsigned char val);
		
		unsigned char getNoteLength();
		void setNoteLength(unsigned char val);
		
		unsigned char getRepeatLength();
		void setRepeatLength(unsigned char val);
};

// Message for pausing for a number of counts.
class NoToneTime : public TxMessage {
	public:
		NoToneTime();
		NoToneTime(unsigned char arr[]);
		~NoToneTime();
		
		unsigned int getMilliseconds();
		void setMilliseconds(unsigned int val);
};

// Message for setting the down time between each note.
class Pause : public TxMessage {
	public:
		Pause();
		Pause(unsigned char arr[]);
		~Pause();
		
		unsigned char getPin();
		void setPin(unsigned char val);
		
		unsigned char getNoteLength();
		void setNoteLength(unsigned char val);
		
		unsigned char getRepeatLength();
		void setRepeatLength(unsigned char val);
};

// Message for setting the play time for each note.
class ToneTime : public TxMessage {
	public:
		ToneTime();
		ToneTime(unsigned char arr[]);
		~ToneTime();
		
		unsigned int getMilliseconds();
		void setMilliseconds(unsigned int val);
};

#endif
