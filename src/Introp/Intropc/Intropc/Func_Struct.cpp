#include "stdafx.h"
#include "Func_Struct.h"

typedef struct IntroStruct
{
	const char* charsA;
	const char* charsB;
	int C;
	bool A;
	bool B;
	char D;
};

#pragma region 结构体相关

extern "C" __declspec(dllexport)
int ImportStruct(IntroStruct* mystr)
{
	return 0;
}

extern "C" __declspec(dllexport)
int ExportStruct(IntroStruct* mystr)
{
	return 0;
}

extern "C" __declspec(dllexport)
int ImportStructs(IntroStruct** mystr, int count)
{
	return 0;
}

extern "C" __declspec(dllexport)
int ExportStructs(IntroStruct** mystr, int* count)
{
	return 0;
}

#pragma endregion

