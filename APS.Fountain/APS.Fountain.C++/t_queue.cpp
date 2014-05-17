#ifndef T_QUEUE_H
#define T_QUEUE_H

#include <iostream>
#include "t_queue.h"
#include "queue.cpp"
#include "Constants.h"

using namespace std;

void queue_tests()
{

	const int SIZE = 12;
	bool tests[SIZE] = { false, false, false, false, false, false, false, false, false, false, false, false };


	try
	{
		tests[0] = t_last_0();
	}
	catch (int i)
	{
		cout << "UNEXPECTED ERROR " << i << endl;
	}
	try
	{
		tests[1] = t_last_1();
	}
	catch (int i)
	{
		cout << "UNEXPECTED ERROR " << i << endl;
	}
	try
	{
		tests[2] = t_last_2();
	}
	catch (int i)
	{
		cout << "UNEXPECTED ERROR " << i << endl;
	}
	try
	{
		tests[3] = t_first_0();
	}
	catch (int i)
	{
		cout << "UNEXPECTED ERROR " << i << endl;
	}
	try
	{
		tests[4] = t_first_1();
	}
	catch (int i)
	{
		cout << "UNEXPECTED ERROR " << i << endl;
	}
	try
	{
		tests[5] = t_first_2();
	}
	catch (int i)
	{
		cout << "UNEXPECTED ERROR " << i << endl;
	}
	try
	{
		tests[6] = t_firstAndLast_0();
	}
	catch (int i)
	{
		cout << "UNEXPECTED ERROR " << i << endl;
	}
	try
	{
		tests[7] = t_firstAndLast_1();
	}
	catch (int i)
	{
		cout << "UNEXPECTED ERROR " << i << endl;
	}
	try
	{
		tests[8] = t_firstAndLast_2();
	}
	catch (int i)
	{
		cout << "UNEXPECTED ERROR " << i << endl;
	}
	try
	{
		tests[9] = t_firstAndLast_3();
	}
	catch (int i)
	{
		cout << "UNEXPECTED ERROR " << i << endl;
	}
	try
	{
		tests[10] = t_firstAndLast_4();
	}
	catch (int i)
	{
		cout << "UNEXPECTED ERROR " << i << endl;
	}
	try
	{
		tests[11] = t_count_0();
	}
	catch (int i)
	{
		cout << "UNEXPECTED ERROR " << i << endl;
	}

	bool failed = false;
	for (int i = 0; i < SIZE; i++)
	{
		if (tests[i])
			cout << i << " PASSED" << endl;
		else
		{
			cout << i << " FAILED" << endl;
			failed = true;
		}
	}

	if (failed)
		cout << "At least one test failed!" << endl;
}

bool t_last_0()
{
	queue<unsigned short> *q = new queue<unsigned short>(2);

	q->push(2);
	q->push(1);

	bool result = true;
	if (q->pop() != 1 || q->pop() != 2)
		result = false;

	delete q;

	return result;
}

bool t_last_1()
{
	queue<unsigned short> *q = new queue<unsigned short>(1);

	q->push(3);

	bool result = true;
	if (q->pop() != 3)
		result = false;

	delete q;

	return result;
}

bool t_last_2()
{
	queue<unsigned short> *q = new queue<unsigned short>(1);

	bool result = true;
	q->push(4);
	try
	{
		q->push(5);
		result = false;
	}
	catch (int i)
	{

	}

	delete q;

	return result;
}

bool t_first_0()
{
	queue<unsigned short> *q = new queue<unsigned short>(2);

	q->pushHead(2);
	q->pushHead(1);

	bool result = true;
	if (q->popHead() != 1 || q->popHead() != 2)
		result = false;

	delete q;

	return result;
}

bool t_first_1()
{
	queue<unsigned short> *q = new queue<unsigned short>(1);

	q->pushHead(3);

	bool result = true;
	if (q->popHead() != 3)
		result = false;

	delete q;

	return result;
}

bool t_first_2()
{
	queue<unsigned short> *q = new queue<unsigned short>(1);

	bool result = true;
	q->pushHead(4);
	try
	{
		q->pushHead(5);
		result = false;
	}
	catch (int i)
	{

	}

	delete q;

	return result;
}

bool t_firstAndLast_0()
{
	queue<unsigned short> *q = new queue<unsigned short>(2);

	q->push(2);
	q->pushHead(1);

	bool result = true;
	if (q->pop() != 2 || q->pop() != 1)
		result = false;

	delete q;

	return result;
}

bool t_firstAndLast_1()
{
	queue<unsigned short> *q = new queue<unsigned short>(2);

	q->push(2);
	q->pushHead(1);

	bool result = true;
	if (q->popHead() != 1 || q->popHead() != 2)
		result = false;

	delete q;

	return result;
}

bool t_firstAndLast_2()
{
	queue<unsigned short> *q = new queue<unsigned short>(2);

	q->push(2);
	q->pushHead(1);

	bool result = true;
	try
	{
		q->push(1);
		result = false;
	}
	catch (int i)
	{

	}

	delete q;

	return result;
}

bool t_firstAndLast_3()
{
	queue<unsigned short> *q = new queue<unsigned short>(2);

	q->pushHead(2);
	q->push(1);

	bool result = true;
	try
	{
		q->pushHead(1);
		result = false;
	}
	catch (int i)
	{

	}

	delete q;

	return result;
}

bool t_firstAndLast_4()
{
	queue<unsigned short> *q = new queue<unsigned short>(2);

	bool result = true;
	try
	{
		q->pushHead(1);
		q->pushHead(2);
		q->pop();
		q->pushHead(3);
		q->pop();
		q->pushHead(4);
		q->pop();
	}
	catch (int i)
	{
		result = false;
	}

	delete q;

	return result;
}

bool t_count_0()
{

	queue<unsigned short> *q = new queue<unsigned short>(6);

	bool result = true;

	if (q->count() != 0)
		result = false;
	q->push(5);
	if (q->count() != 1)
		result = false;
	q->pop();
	if (q->count() != 0)
		result = false;

	q->pushHead(1);
	if (q->count() != 1)
		result = false;
	q->popHead();
	if (q->count() != 0)
		result = false;


	q->pushHead(1);
	if (q->count() != 1)
		result = false;

	q->pushHead(2);
	if (q->count() != 2)
		result = false;

	q->pushHead(3);
	if (q->count() != 3)
		result = false;

	q->pushHead(4);
	if (q->count() != 4)
		result = false;

	q->pushHead(5);
	if (q->count() != 5)
		result = false;

	q->pushHead(6);
	if (q->count() != 6)
		result = false;


	delete q;

	return result;
}

#endif T_QUEUE_H