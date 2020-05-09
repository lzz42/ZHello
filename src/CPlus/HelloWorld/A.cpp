#include "stdafx.h"
#include "A.h"

A::A()
{
	M = 9;
}


A::~A()
{
}

int A::Add(int a, int b)
{
	return (a + b) * 100 + M;
}
