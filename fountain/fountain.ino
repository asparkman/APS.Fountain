#include "Arduino.h"
#include "comm.h"

void setup()
{
    Serial.begin(115200, SERIAL_8O1); pinMode(3, OUTPUT);
    Serial.setTimeout(100);
    digitalWrite(3, HIGH); tone(8, 294, 200);
    delay(100);
    digitalWrite(3, LOW);
}

void loop()
{
    waitForMessages();
}
