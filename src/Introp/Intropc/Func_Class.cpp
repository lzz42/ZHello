#include "stdafx.h"
#include "Func_Class.h"

class A
{
public:
	A();
	~A();

private:

};

A::A()
{
}

A::~A()
{
}


extern "C" __declspec(dllexport)
int ImportClass(A a)
{
	return 0;
}

extern "C" __declspec(dllexport)
int ImportClasss(A* a)
{
	return 0;
}

extern "C" __declspec(dllexport)
int ExportClass(A* a)
{
	return 0;
}

extern "C" __declspec(dllexport)
int ExportClasss(A** a)
{
	return 0;
}