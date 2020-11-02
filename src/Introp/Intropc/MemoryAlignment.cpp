#include "stdafx.h"
#include "MemoryAlignment.h"



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
	struct MyStruct* p = &ms;
	printf("%d\n", p->a);
	printf("%d\n", p->b);
	printf("%d\n", p->c);
	printf("%d\n", p->d);
}

void MemoryAlignmentTest()
{
	printf("内存对齐测试\n");
	printf("%d\n", sizeof(MyStruct2));
	printf("%d\n", sizeof(MyStruct3));
	printf("%d\n", sizeof(MyStruct4));
	printf("%d\n", sizeof(MyStruct5));
	MyStruct2 s = { {'a','b','c','d','e'},9,'o' };
	MyStruct2* p = &s;
	printf("%p\n", &p->b);
	printf("%p\n", &p->a);
	printf("%p\n", &p->c);
}
