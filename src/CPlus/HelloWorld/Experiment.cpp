#include "stdafx.h"
#include <iostream>
#include <stdio.h>
#include "Func_Struct.h"

#define AUDIO32_t 0x7fffffff
#define Random(x)(rand()%x)
#define V1

#ifndef NULL
#ifdef __cplusplus
#define NULL    0
#else
#define NULL    ((void *)0)
#endif
#endif

void PointerTest();
void StructAndPointer();
void TestStructMemoryAllocation();
void ParamTest();
void Swap1(int, int);
void Swap2(int*, int*);
void Swap3(int&, int&);

int main(int argc, char* argv[])
{
	//PointerTest();
	ParamTest();
	return 0;
}

#pragma region 指针实验

void PointerTest()
{
	//Declare pointer
	int* konwnTypePtr;
	int *p1, *p12;
	void* unknownPtr;

	//Operation
	// &取地址符 *取值
	int int_a = 128;
	printf("%d\n", int_a);
	int* pInt_a = &int_a;
	int int_b = *pInt_a;
	printf("%p\n", pInt_a);
	printf("%d\n", *pInt_a);
	printf("%d\n", int_b);

	//+ & - 指针前后位移

	//指针数组
	int a, b, c;
	int *pIntArry[3] = { &a,&b,&c };

	//指针与数组
	int ar[3] = { 1,2,3 };
	int* pAr = ar;
	int* pAr2 = &ar[0];
	printf("%p\n", pAr);
	printf("%p\n", pAr2);

	//函数指针
	//指向函数 void PointerTest()
	void(*ptrPonterTest)();
	ptrPonterTest = PointerTest;
	//指向函数 int main(int argc, char* argv[])
	int(*ptrMain)(int, char*[]);
	ptrMain = main;

	//结构体指针
	IntroStruct structA;
	IntroStruct* ptrStruct = &structA;
	bool ttrue = structA.A == ptrStruct->A;

	printf("Hello World");
	float ff = 1.2f;
	float *p_ch = &ff;
	int32_t sf = (int32_t)((*p_ch) * (float)AUDIO32_t);
	printf("%d", sf);

	int p[] = { 1,2,3,4,5 };
	int len = 5;
	int* ptr = p;
	for (size_t i = 0; i < len; i++)
	{
		ptr++;
		int sss = sizeof(ptr);
		int64_t s = (int64_t)p;
		s = s + sizeof(int);
		printf("%x\n", s);
	}
	int *pp = p;
	for (size_t i = 0; i < len; i++)
	{
		(*pp)++;
	}

	char ary[] = { 'a','b','c','d','e' };
	char* p2 = ary;
	char** ptr2 = &p2;
	ptr2++;
	std::cout << ptr << std::endl;
	std::cout << *ptr << std::endl;
	std::cout << **ptr2 << std::endl;

	int array[3] = { 123,456,789 };
	printf("array: %d\n", sizeof(array));
	printf("*array: %d\n", sizeof(*array));
	printf("*(array+0):%d\n", sizeof(*(array + 0)));
	printf("array+0:%d\n", sizeof(array + 0));
}

void Sort(int* begin, int* end, bool(*CompareFunc)(int, int))
{
	while (begin < end)
	{
		Sort(begin, begin + (end - begin) / 2, CompareFunc);
		Sort(begin + (end - begin) / 2, end, CompareFunc);
	}
}

#pragma endregion

#pragma region 参数实验

void ParamTest()
{
	int a1 = 10, b1 = 20;
	int a2 = 10, b2 = 20;
	int a3 = 10, b3 = 20;
	printf("a1:%d,b1:%d\n", a1, b1);
	Swap1(a1, b1);
	printf("a1:%d,b1:%d\n", a1, b1);
	printf("a2:%d,b2:%d\n", a2, b2);
	Swap2(&a2, &b2);
	printf("a2:%d,b2:%d\n", a2, b2);
	printf("a3:%d,b3:%d\n", a3, b3);
	Swap3(a3, b3);
	printf("a3:%d,b3:%d\n", a3, b3);
}

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

#pragma endregion

#pragma region  内存对齐

/*内存对齐

*/

struct MyStruct
{
	int a;//4
	char b; //1->4
	int c[3]; //3->4
	char d[3]; //3->4
	//4+4+4+4 = 16
};

struct MyStruct1
{
	int a;
	char b;
	int c[3];
	char d[3];
};

struct MyStruct2
{
	char b[5];//5->8
	int a;//4
	char c;//1->4
	//=8+4+4=16
};

struct MyStruct3
{
	char b[3]; //3->4
	int a;		//4
	char c;//1->4
	//=4+4+4=12
};

struct MyStruct4
{
	char b[13];//13->16
	int a;//4 ->8
	double d;//8
	char c;//1->8
	//= 16+8+8+8 = 40
};

struct MyStruct5
{
	double d;//8
	char b[13];//13->16
	int a;//4
	char c;//1->4
	// = 8+16+4+4=32
};

void StructAndPointer()
{
	struct MyStruct ms = { 22,'a',{123,456,789},{'x','y','z'} };
	printf("%d\n", ms.a);
	printf("%d\n", ms.b);
	printf("%d\n", ms.c);
	printf("%d\n", ms.d);
	struct MyStruct *p = &ms;
	printf("%d\n", p->a);
	printf("%d\n", p->b);
	printf("%d\n", p->c);
	printf("%d\n", p->d);
}

void TestStructMemoryAllocation()
{
	printf("_______________________\n");
	printf("%d\n", sizeof(MyStruct2));
	printf("%d\n", sizeof(MyStruct3));
	printf("%d\n", sizeof(MyStruct4));
	printf("%d\n", sizeof(MyStruct5));
	MyStruct2 s = { {'a','b','c','d','e'},9,'o' };
	MyStruct2 *p = &s;
	printf("%p\n", &p->b);
	printf("%p\n", &p->a);
	printf("%p\n", &p->c);
}

#pragma endregion