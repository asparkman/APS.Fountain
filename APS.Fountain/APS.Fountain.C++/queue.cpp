#ifndef QUEUE_CPP
#define QUEUE_CPP

#include "queue.h"
#include "Constants.h"

template<class TEMPLATE_CLASS>
queue<TEMPLATE_CLASS>::queue(unsigned int size, const unsigned int maxActualCount)
{
	this->size = size;
	first = 0;
	last = -1;
	actualCount = 0;
	arr = new TEMPLATE_CLASS[size];
	#ifndef ENABLE_EXCEPTIONS
	lastError = 0;
	#endif

	if (maxActualCount == 0)
		MAX_ACTUAL_COUNT = size;
	else
		MAX_ACTUAL_COUNT = maxActualCount;

	if (size == 0)
		#ifndef ENABLE_EXCEPTIONS
		lastError = 0;
		#endif
		#ifdef ENABLE_EXCEPTIONS
		throw - 1;
		#endif
}

template<class TEMPLATE_CLASS>
queue<TEMPLATE_CLASS>::~queue()
{
	delete[] arr;
}

template<class TEMPLATE_CLASS>
TEMPLATE_CLASS queue<TEMPLATE_CLASS>::pop()
{
	if (actualCount <= 0)
		#ifndef ENABLE_EXCEPTIONS
		lastError = 0;
		#endif
		#ifdef ENABLE_EXCEPTIONS
		throw -1;
		#endif

	actualCount--;
	TEMPLATE_CLASS result = arr[last];
	last = (last - 1) % size;
	return result;
}

template<class TEMPLATE_CLASS>
TEMPLATE_CLASS queue<TEMPLATE_CLASS>::peek()
{
	if (actualCount <= 0)
		#ifndef ENABLE_EXCEPTIONS
		lastError = 0;
		#endif
		#ifdef ENABLE_EXCEPTIONS
		throw - 1;
		#endif

	return arr[last];
}

template<class TEMPLATE_CLASS>
void queue<TEMPLATE_CLASS>::push(TEMPLATE_CLASS var)
{
	if (actualCount + 1 > MAX_ACTUAL_COUNT)
		#ifndef ENABLE_EXCEPTIONS
		lastError = 0;
		#endif
		#ifdef ENABLE_EXCEPTIONS
		throw -1;
		#endif

	actualCount++;
	last = (last + 1) % size;
	arr[last] = var;
}

template<class TEMPLATE_CLASS>
TEMPLATE_CLASS queue<TEMPLATE_CLASS>::popHead()
{
	if (actualCount <= 0)
		#ifndef ENABLE_EXCEPTIONS
		lastError = 0;
		#endif
		#ifdef ENABLE_EXCEPTIONS
		throw -1;
		#endif

	actualCount--;
	TEMPLATE_CLASS result = arr[first];
	first = (first + 1) % size;
	return result;
}

template<class TEMPLATE_CLASS>
TEMPLATE_CLASS queue<TEMPLATE_CLASS>::peekHead()
{
	if (actualCount <= 0)
		#ifndef ENABLE_EXCEPTIONS
		lastError = 0;
		#endif
		#ifdef ENABLE_EXCEPTIONS
		throw - 1;
		#endif

	return arr[first];
}

template<class TEMPLATE_CLASS>
void queue<TEMPLATE_CLASS>::pushHead(TEMPLATE_CLASS var)
{
	if (actualCount + 1 > MAX_ACTUAL_COUNT)
		#ifndef ENABLE_EXCEPTIONS
		lastError = 0;
		#endif
		#ifdef ENABLE_EXCEPTIONS
		throw - 1;
		#endif

	actualCount++;
	first = (first - 1) % size;
	arr[first] = var;
}

template<class TEMPLATE_CLASS>
unsigned int queue<TEMPLATE_CLASS>::count()
{
	return actualCount;
}

#endif QUEUE_CPP