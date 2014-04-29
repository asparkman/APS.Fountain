#ifndef MUSIC_H
#define MUSIC_H

#include "Arduino.h"

const double F0 = 440.0;
const double A = pow(2.0, 1.0 / 12.0);

double genNote(int stepCnt) {
	return F0 * pow(A, ((double) stepCnt));
}

#endif
