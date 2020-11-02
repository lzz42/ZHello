#include "stdafx.h"
#include <iostream>
#include <stdio.h>

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


typedef struct IntroStruct
{
	const char* charsA;
	const char* charsB;
	int C;
	bool A;
	bool B;
	char D;
};

void Sort(int* begin, int* end, bool(*CompareFunc)(int, int))
{
	while (begin < end)
	{
		Sort(begin, begin + (end - begin) / 2, CompareFunc);
		Sort(begin + (end - begin) / 2, end, CompareFunc);
	}
}

void PointerTest()
{
	printf("指针测试 \n");
	//声明指针
	int* konwnTypePtr;
	int *p1, *p12;
	//声明未知类型指针
	void* unknownPtr;

	//指针操作
	// &取地址符 *取值
	int ac = 128;
	printf("%d\n", ac);
	//声明指针pinta 指向变量int_a地址
	int* pinta = &ac;
	//声明变量 int_b 其值为指针pinta的地址的值
	int int_b = *pinta;
	printf("打印指针pinta的内容,即指向的变量a的地址：%p\n", pinta);
	printf("打印指针pinta指向的地址的内容，即指向的变量变量a的值%d\n", *pinta);
	printf("打印变量b值：%d\n", int_b);
	ac++;
	printf("打印指针pinta的内容,即指向的变量a的地址：%p\n", pinta);
	printf("打印指针pinta指向的地址的内容，即指向的变量变量a的值%d\n", *pinta);
	printf("打印变量b值：%d\n", int_b);

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
	void(*ptrMain)(int* , int* , bool(*CompareFunc)(int, int));
	ptrMain = Sort;

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

