#ifndef COMM_CPP
#define COMM_CPP

#include "Arduino.h"
#include "comm.h"

// ----------------------------------------------------------------------------
// Start internal variable declarations
// ----------------------------------------------------------------------------
unsigned char expected_seq_nr = 0;

// ----------------------------------------------------------------------------
// End internal variable declarations
// ----------------------------------------------------------------------------

// ----------------------------------------------------------------------------
// Start Helper Function DEFINITIONS
// ----------------------------------------------------------------------------
boolean readTxMessage(unsigned char *arr)
{
	boolean result = false;
	
	int last = millis();
	do
	{
		if(Serial.peek() != -1 && Serial.peek() != _START)
			Serial.read();
		else if(Serial.peek() == _START && Serial.available() >= TX_SIZE)
		{
			arr[0] = Serial.read();
			int bytes_read = Serial.readBytesUntil((char) _END, (char*)(arr + 1), TX_SIZE - 1);
			if(TX_SIZE - 1 == bytes_read && arr[TX_SIZE - 1] == _END)
				result = true;
			break;
		}
	} while(millis() - last < 100);
		
	return result;
}

boolean writeRxMessage(unsigned char* arr)
{
	boolean result = false;
	if(Serial.write((uint8_t*)arr, RX_SIZE))
		result = true;
	
	return result;
}

void waitForMessages()
{
	// Turn off the timeout.
	Serial.setTimeout(100);
	
	// Ack for packet that hasn't been sent.
	RxIdentify* rxIdentify = new RxIdentify();
	rxIdentify->setRandom0(0);
	rxIdentify->setRandom1(0);
	rxIdentify->setSeqNr(1);
			
	Ack* rxAck = new Ack();
	rxAck->setSeqNr(1);
	
	boolean wasMessageRcvd = false;
	unsigned char *bytes = new unsigned char [TX_SIZE];
	while(true)
	{
		for(int i = 0; i < TX_SIZE; i++)
			bytes[i] = (unsigned char) 0;
	
		wasMessageRcvd = readTxMessage(bytes);
		// If the first message was received, and it is a TX_IDENTIFY.
		if(wasMessageRcvd) {
			TxIdentify* txIdentify = new TxIdentify(bytes); 
			Note* note = new Note(bytes);
			Pause* pause = new Pause(bytes);
			ToneTime* toneTime = new ToneTime(bytes);
			NoToneTime* noToneTime = new NoToneTime(bytes);
			switch(bytes[TX_INFO_0])
			{
				case TX_IDENTIFY:
					if(txIdentify->getSeqNr() == expected_seq_nr)
					{
						// Set the acknowledgement packet.
						rxIdentify->setRandom0((unsigned char)((txIdentify->getRandom0() + 1) % 256));
						rxIdentify->setRandom1((unsigned char)((txIdentify->getRandom1() + 1) % 256));
						rxIdentify->setSeqNr(txIdentify->getSeqNr());
					}
					break;
				case PAUSE:
					if(pause->getSeqNr() == expected_seq_nr)
					{
						// Set the acknowledgement packet.
						rxAck->setSeqNr(pause->getSeqNr());
					}
					break;
				case NOTE:
					if(note->getSeqNr() == expected_seq_nr)
					{
						// Set the acknowledgement packet.
						rxAck->setSeqNr(note->getSeqNr());
						double a = pow(2.0, 1.0 / 12.0);
						int t = (int) (pow(a, (double) ((int)(note->getStep()) - 57)) * 440.0);
						if(t < 30)
							t = 30;
						else if(t > 3000)
							t = 3000;
						tone(note->getPin(), t, 100);
					}
					break;
				case TONE_TIME:
					if(toneTime->getSeqNr() == expected_seq_nr)
					{
						// Set the acknowledgement packet.
						rxAck->setSeqNr(toneTime->getSeqNr());
					}
					break;
				case NO_TONE_TIME:
					if(noToneTime->getSeqNr() == expected_seq_nr)
					{
						// Set the acknowledgement packet.
						rxAck->setSeqNr(noToneTime->getSeqNr());
					}
					break;
			}
			
			delete txIdentify; 
			delete pause;
			delete note;
			delete noToneTime;
			delete toneTime;
		}
		
		RxMessage* rxMessage;
		rxMessage = bytes[TX_INFO_0] == TX_IDENTIFY ? ((RxMessage*) rxIdentify) : ((RxMessage*) rxAck);
		if(writeRxMessage(rxMessage->getBytes()) && wasMessageRcvd)
			expected_seq_nr = expected_seq_nr ? 0 : 1;
	}
	
	delete rxIdentify;
	delete rxAck;
}

void waitUntilIdentified()
{
	// Turn off the timeout.
	Serial.setTimeout(0);
	unsigned char *bytes = new unsigned char [TX_SIZE];
	
	// Ack for packet that hasn't been sent.
	RxIdentify* rxIdentify = new RxIdentify();
	rxIdentify->setRandom0(0);
	rxIdentify->setRandom1(0);
	rxIdentify->setSeqNr(1);
	
	boolean wasFirstTxMessageRcvd = false;
	while(!wasFirstTxMessageRcvd)
	{
		wasFirstTxMessageRcvd = readTxMessage(bytes);
		// If the first message was received, and it is a TX_IDENTIFY.
		if(wasFirstTxMessageRcvd && bytes[TX_INFO_0] == TX_IDENTIFY) {
			// Place the message from the computer into an object.
			TxIdentify* txIdentify = new TxIdentify(bytes); 
			
			if(txIdentify->getSeqNr() == expected_seq_nr)
			{
				// Set the acknowledgement packet.
				rxIdentify->setRandom0((unsigned char)((txIdentify->getRandom0() + 1) % 256));
				rxIdentify->setRandom1((unsigned char)((txIdentify->getRandom1() + 1) % 256));
				rxIdentify->setSeqNr(txIdentify->getSeqNr());
			}
			
			delete txIdentify;
		}
		
		if(writeRxMessage(rxIdentify->getBytes()) && wasFirstTxMessageRcvd)
			expected_seq_nr = expected_seq_nr ? 0 : 1;
	}
	
	delete rxIdentify;
	delete [] bytes;
}
// ----------------------------------------------------------------------------
// End Helper Function DEFINITIONS
// ----------------------------------------------------------------------------



// ----------------------------------------------------------------------------
// Start Message DEFINITIONS
// ----------------------------------------------------------------------------
Message::Message()
{
}
Message::~Message()
{
	delete [] bytes;
}

unsigned char Message::getType()
{
	return bytes[TX_INFO_0];
}

unsigned char Message::getSize()
{
	return (unsigned char) (bytes[TX_SIZE_SEQ] >> ((unsigned char)5));
}

unsigned char Message::getSeqNr()
{
	return (unsigned char)(((unsigned char)(bytes[TX_SIZE_SEQ] << ((unsigned char)3))) >> ((unsigned char)3));
}

void Message::setSeqNr(unsigned char val)
{
	bytes[TX_SIZE_SEQ] = (unsigned char)(((unsigned char)(this->getSize() << ((unsigned char)5))) + val);
}

unsigned char* Message::getBytes()
{
	return this->bytes;
}

void Message::setBytes(unsigned char *arr)
{
}

void Message::setType(unsigned char val)
{
	bytes[TX_INFO_0] = (unsigned char)val;
}

void Message::setSize(unsigned char val)
{
	bytes[TX_SIZE_SEQ] = (unsigned char) (val << ((unsigned char)5));
}
// ----------------------------------------------------------------------------
// End Message DEFINITIONS
// ----------------------------------------------------------------------------



// ----------------------------------------------------------------------------
// Start RxMessage DEFINITIONS
// ----------------------------------------------------------------------------
RxMessage::RxMessage()
	: Message()
{
	bytes = new unsigned char[RX_SIZE];
	bytes[RX_START] = _START;
	bytes[RX_STOP] = _END;
	bytes[RX_ESCAPE] = (unsigned char)(3 << 6);
}
RxMessage::RxMessage(unsigned char arr[])
	: Message()
{
	bytes = new unsigned char[RX_SIZE];
	setBytes(arr);
}
RxMessage::~RxMessage()
{
}

void RxMessage::setBytes(unsigned char *arr)
{
	for(int i = 0; i < RX_SIZE; i++)
	{
		bytes[i] = arr[i];
	}
}
// ----------------------------------------------------------------------------
// End RxMessage DEFINITIONS
// ----------------------------------------------------------------------------



// ----------------------------------------------------------------------------
// Start RxIdentify DEFINITIONS
// ----------------------------------------------------------------------------
RxIdentify::RxIdentify()
	: RxMessage()
{
	setType(RX_IDENTIFY);
	setSize(3);
}
RxIdentify::RxIdentify(unsigned char arr[])
	: RxMessage(arr)
{
}
RxIdentify::~RxIdentify()
{
}

unsigned char RxIdentify::getRandom0()
{
	return bytes[RX_INFO_1];
}
void RxIdentify::setRandom0(unsigned char val)
{
	bytes[RX_INFO_1] = val;
}

unsigned char RxIdentify::getRandom1()
{
	return bytes[RX_INFO_2];
}
void RxIdentify::setRandom1(unsigned char val)
{
	bytes[RX_INFO_2] = val;
}
// ----------------------------------------------------------------------------
// End RxIdentify DEFINITIONS
// ----------------------------------------------------------------------------



// ----------------------------------------------------------------------------
// Start Ack DEFINITIONS
// ----------------------------------------------------------------------------
Ack::Ack()
	: RxMessage()
{
	setType(ACK);
	setSize(1);
}
Ack::Ack(unsigned char arr[])
	: RxMessage(arr)
{
}
Ack::~Ack()
{
}
// ----------------------------------------------------------------------------
// End Ack DEFINITIONS
// ----------------------------------------------------------------------------



// ----------------------------------------------------------------------------
// Start TxMessage DEFINITIONS
// ----------------------------------------------------------------------------
TxMessage::TxMessage()
	: Message()
{
	bytes = new unsigned char[TX_SIZE];
	bytes[TX_START] = _START;
	bytes[TX_STOP] = _END;
	bytes[TX_ESCAPE] = (unsigned char)(3 << 6);
}
TxMessage::TxMessage(unsigned char arr[])
	: Message()
{
	bytes = new unsigned char[TX_SIZE];
	setBytes(arr);
}
TxMessage::~TxMessage()
{
}

void TxMessage::setBytes(unsigned char *arr)
{
	for(int i = 0; i < TX_SIZE; i++)
	{
		bytes[i] = arr[i];
	}
}
// ----------------------------------------------------------------------------
// End TxMessage DEFINITIONS
// ----------------------------------------------------------------------------



// ----------------------------------------------------------------------------
// Start TxIdentify DEFINITIONS
// ----------------------------------------------------------------------------
TxIdentify::TxIdentify()
	: TxMessage()
{
	setType(TX_IDENTIFY);
	setSize(3);
}
TxIdentify::TxIdentify(unsigned char arr[])
	: TxMessage(arr)
{
}
TxIdentify::~TxIdentify()
{
}

unsigned char TxIdentify::getRandom0()
{
	return bytes[TX_INFO_1];
}
void TxIdentify::setRandom0(unsigned char val)
{
	bytes[TX_INFO_1] = val;
}

unsigned char TxIdentify::getRandom1()
{
	return bytes[TX_INFO_2];
}
void TxIdentify::setRandom1(unsigned char val)
{
	bytes[TX_INFO_2] = val;
}
// ----------------------------------------------------------------------------
// End TxIdentify DEFINITIONS
// ----------------------------------------------------------------------------



// ----------------------------------------------------------------------------
// Start Note DEFINITIONS
// ----------------------------------------------------------------------------
Note::Note()
	: TxMessage()
{
	setType(NOTE);
	setSize(5);
}
Note::Note(unsigned char arr[])
	: TxMessage(arr)
{
}
Note::~Note()
{
}

unsigned char Note::getPin()
{
	return bytes[TX_INFO_1];
}
void Note::setPin(unsigned char val)
{
	bytes[TX_INFO_1] = val;
}


unsigned char Note::getStep()
{
	return bytes[TX_INFO_2];
}
void Note::setStep(unsigned char val)
{
	bytes[TX_INFO_2] = val;
}

unsigned char Note::getNoteLength()
{
	return bytes[TX_INFO_3];
}
void Note::setNoteLength(unsigned char val)
{
	bytes[TX_INFO_3] = val;
}

unsigned char Note::getRepeatLength()
{
	return bytes[TX_INFO_4];
}
void Note::setRepeatLength(unsigned char val)
{
	bytes[TX_INFO_4] = val;
}
// ----------------------------------------------------------------------------
// End Note DEFINITIONS
// ----------------------------------------------------------------------------



// ----------------------------------------------------------------------------
// Start NoToneTime DEFINITIONS
// ----------------------------------------------------------------------------
NoToneTime::NoToneTime()
	: TxMessage()
{
	setType(NO_TONE_TIME);
	setSize(4);
}
NoToneTime::NoToneTime(unsigned char arr[])
	: TxMessage(arr)
{
}
NoToneTime::~NoToneTime()
{
}

unsigned int NoToneTime::getMilliseconds()
{
	unsigned int result = 0;
	unsigned int msb = (unsigned int)bytes[TX_INFO_1];
	unsigned int lsb = (unsigned int)bytes[TX_INFO_2];
	result = (unsigned int)((msb << 8) + lsb);
	return result;
}
void NoToneTime::setMilliseconds(unsigned int val)
{
	unsigned char msb = (byte)(val >> 8);
	unsigned char lsb = (byte)(val % (256));
	bytes[TX_INFO_1] = msb;
	bytes[TX_INFO_2] = lsb;
}
// ----------------------------------------------------------------------------
// End NoToneTime DEFINITIONS
// ----------------------------------------------------------------------------



// ----------------------------------------------------------------------------
// Start Pause DEFINITIONS
// ----------------------------------------------------------------------------
Pause::Pause()
	: TxMessage()
{
	setType(PAUSE);
	setSize(2);
}
Pause::Pause(unsigned char arr[])
	: TxMessage(arr)
{
}
Pause::~Pause()
{
}

unsigned char Pause::getPin()
{
	return bytes[TX_INFO_1];
}
void Pause::setPin(unsigned char val)
{
	bytes[TX_INFO_1] = val;
}

unsigned char Pause::getNoteLength()
{
	return bytes[TX_INFO_2];
}
void Pause::setNoteLength(unsigned char val)
{
	bytes[TX_INFO_2] = val;
}


unsigned char Pause::getRepeatLength()
{
	return bytes[TX_INFO_3];
}
void Pause::setRepeatLength(unsigned char val)
{
	bytes[TX_INFO_3] = val;
}
// ----------------------------------------------------------------------------
// End Pause DEFINITIONS
// ----------------------------------------------------------------------------



// ----------------------------------------------------------------------------
// Start ToneTime DEFINITIONS
// ----------------------------------------------------------------------------
ToneTime::ToneTime()
	: TxMessage()
{
	setType(TONE_TIME);
	setSize(2);
}
ToneTime::ToneTime(unsigned char arr[])
	: TxMessage(arr)
{
}
ToneTime::~ToneTime()
{
}

unsigned int ToneTime::getMilliseconds()
{
	unsigned int result = 0;
	unsigned int msb = (unsigned int)bytes[TX_INFO_1];
	unsigned int lsb = (unsigned int)bytes[TX_INFO_2];
	result = (unsigned int)((msb << 8) + lsb);
	return result;
}
void ToneTime::setMilliseconds(unsigned int val)
{
	unsigned char msb = (byte)(val >> 8);
	unsigned char lsb = (byte)(val % (256));
	bytes[TX_INFO_1] = msb;
	bytes[TX_INFO_2] = lsb;
}
// ----------------------------------------------------------------------------
// End ToneTime DEFINITIONS
// ----------------------------------------------------------------------------


#endif
