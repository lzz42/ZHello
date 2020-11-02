#define V1

#include "stdafx.h"
#include "Func_Simple.h"
#include <cstring>
#include <ctype.h>

#ifdef __CPLUSPLUS__
	
#endif // 



#pragma region ¼òµ¥º¯Êý

extern "C" __declspec(dllexport)
int Add(int a, int b)
{
#ifdef V1
	return (a + b) * 10;
#endif // V1
#ifdef V2
	return (a + b) * 100;
#endif // V2
	return a + b;
}

char a1[10] = "12340678K";

extern "C" __declspec(dllexport)
char * GetString(int a)
{
#ifdef V1
	a1[0] = 'V';
	//c = "V1abcdefV1";
#endif // V1
#ifdef V2
	a1[0] = 'B';
	//c = "V2abcdefV2";
#endif // V2
	return a1;
}

extern "C" __declspec(dllexport)
int GetStringLen(char* str)
{
	int len = strlen(str);
#ifdef V1
	len *= 10;
#endif // V1
#ifdef V2
	len *= 100;
#endif // V2
	return len;
}

extern "C" __declspec(dllexport)
char GetChar()
{
	char c = '0';
#ifdef V1
	c = 'G';
#endif // V1
#ifdef V2
	c = 'G';
#endif // V2
	return c;
}

extern "C" __declspec(dllexport)
char GetASCIIChar(int a)
{
	if (a + 1 >= 48 && a + 1 <= 126)
	{
#ifdef V1
		return toascii(a);
#endif // V1
#ifdef V2
		return toascii(a + 1);
#endif // V2
	}
	else
	{
		return '!';
	}
	return '\n';
}

#pragma endregion
