#include "stdafx.h"

/*
参数实验

*/

void Swap1(int a, int b)
{
	a = a ^ b;
	b = a ^ b;
	a = a ^ b;
}

void Swap2(int* a, int* b)
{
	*a = *a ^ *b;
	*b = *a ^ *b;
	*a = *a ^ *b;
}
void Swap3(int& a, int& b)
{
	a = a ^ b;
	b = a ^ b;
	a = a ^ b;
}

void ParamTest()
{
	int a1 = 10, b1 = 20;
	int a2 = 10, b2 = 20;
	int a3 = 10, b3 = 20;

	printf("a1:%d,b1:%d\n", a1, b1);
	Swap1(a1, b1);
	printf("a1:%d,b1:%d\n", a1, b1);
	
	printf("--------\n");

	printf("a2:%d,b2:%d\n", a2, b2);
	Swap2(&a2, &b2);
	printf("a2:%d,b2:%d\n", a2, b2);
	
	printf("--------\n");

	printf("a3:%d,b3:%d\n", a3, b3);
	Swap3(a3, b3);
	printf("a3:%d,b3:%d\n", a3, b3);
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
void Swap_Ref(int& a, int& b)
{
	a = a ^ b;
	b = a ^ b;
	a = a ^ b;
	return;
}

//指针调用
void Swap_Point(int* a, int* b)
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
