#ifndef QUEUE_H
#define QUEUE_H
#include "Constants.h"

#pragma once
template<class TEMPLATE_CLASS>
class queue
{
public:
	queue(unsigned int size, const unsigned int maxActualCount = 0);
	~queue();
	
	TEMPLATE_CLASS popHead();

	TEMPLATE_CLASS peekHead();

	void pushHead(TEMPLATE_CLASS var);

	TEMPLATE_CLASS pop();

	TEMPLATE_CLASS peek();

	void push(TEMPLATE_CLASS var);


	
	unsigned int count();

private:
	unsigned int first;
	unsigned int last;
	unsigned int actualCount;
	TEMPLATE_CLASS *arr;
	unsigned int size;

	unsigned int MAX_ACTUAL_COUNT;

	#ifndef ENABLE_EXCEPTIONS
	unsigned int lastError;
	#endif
};

#endif QUEUE_H