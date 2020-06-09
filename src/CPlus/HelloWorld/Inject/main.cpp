#include "../stdafx.h"
#include "main.h"
#include "ProcessModifer.h"

main::main()
{
}

main::~main()
{
}

//传值调用
void Swap_Value(int a, int b)
{
	a = a ^ b;
	b = a ^ b;
	a = a ^ b;
	return;
}

//引用调用
void Swap_Ref(int &a, int &b)
{
	a = a ^ b;
	b = a ^ b;
	a = a ^ b;
	return;
}

//指针调用
void Swap_Point(int *a, int *b)
{
	*a = *a ^ *b;
	*b = *a ^ *b;
	*a = *a ^ *b;
	return;
}

void TestMethodCall()
{
	int a = 10, b = 20;
	int a1 = 10, b1 = 20;
	int a2 = 10, b2 = 20;

	printf("A:%d,B:%d\n", a, b);
	printf("Call Swap_Value\n");
	Swap_Value(a, b);
	printf("A:%d,B:%d\n", a, b);

	printf("A1:%d,B1:%d\n", a1, b1);
	printf("Call Swap_Ref\n");
	Swap_Ref(a1, a1);
	printf("A1:%d,B1:%d\n", a1, b1);

	printf("A2:%d,B2:%d\n", a2, b2);
	printf("Call Swap_Point\n");
	Swap_Point(&a2, &b2);
	printf("A2:%d,B2:%d\n", a2, b2);
}
/*
A:10,B:20
Call Swap_Value
A:10,B:20
A1:10,B1:20
Call Swap_Ref
A1:0,B1:20
A2:10,B2:20
Call Swap_Point
A2:20,B2:10
*/

int main(int argc, _TCHAR* argv[])
{
	TestMethodCall();
	//char name[] = "text.exe";
	//ProcessModifer pr = ProcessModifer(name);
	//pr.Begin();
}