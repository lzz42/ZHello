//#define V1

#include "stdafx.h"
#include "Func_Simple.h"
#include <cstring>
#include <ctype.h>

#ifdef __CPLUSPLUS__

#endif //

/// <summary>
/// 值传递
/// 返回整形
/// </summary>
/// <param name="a"></param>
/// <param name="b"></param>
/// <returns></returns>
int Add(int a, int b)
{
#ifdef V1
	return (a + b) * 10;
#else
#ifdef V2
	return (a + b) * 100;
#else
	return a + b;
#endif // V2
#endif // V1
}

/// <summary>
/// 引用传递
/// 返回一个字符串
/// </summary>
/// <param name="str1"></param>
/// <param name="str2"></param>
/// <returns></returns>
char* AddStr(char* str1, char* str2)
{
	int len1 = strlen(str1);
	int len2 = strlen(str2);
	if (len1 <= 0 && len2 <= 0)
	{
		return NULL;
	}
	char* rec = (char*)malloc(sizeof(char) * (len1 + len2 + 1));
	for (int i = 0; i < len1; i++)
	{
		rec[i] = str1[i];
	}
	for (int i = 0; i < len2; i++)
	{
		rec[i + len1] = str2[i];
	}
	rec[len1 + len2] = '\0';
	return rec;
}

/// <summary>
///
/// </summary>
/// <param name="str"></param>
/// <param name="a"></param>
/// <returns></returns>
char GetChar(char* str, int a)
{
	int len = strlen(str);
	if (a < 0)
		return NULL;
	if (a >= len)
		return NULL;
	return str[a];
}

/// <summary>
/// 引用传递修改传入值内容
/// </summary>
/// <param name="str"></param>
/// <param name="len"></param>
void ToUpper(char* str, int len)
{
	if (len <= 0)
		return;
	int d = 'A' - 'a';
	for (int i = 0; i < len; i++)
	{
		if (str[i] >= 'a' && str[i] <= 'z')
		{
			str[i] += d;
		}
	}
}

/// <summary>
///
/// </summary>
/// <param name="str"></param>
/// <param name="len"></param>
void ToLower(char* str, int len)
{
	if (len <= 0)
		return;
	int d = 'A' - 'a';
	for (int i = 0; i < len; i++)
	{
		char c = str[i];
		if (c >= 'A' && c <= 'Z')
		{
			str[i] = (char)(c - d);
		}
	}
}

void Func_Simple_Test()
{
	printf("导出函数测试\n");
	int a = 100, b = 200;
	int c = Add(a, b);

	printf("Add \t:%d\n", c);
	char* s1 = new char[5]{ "abcd" };
	char* s2 = new char[5]{ "1234" };
	char* s3 = AddStr(s1, s2);
	printf("Add \t:%s\n");
	free(s3);

	ToUpper(s1, 4);
	ToUpper(s2, 4);
	printf("ToUpper \t:%s\n", s1);
	printf("ToUpper \t:%s\n", s2);
	ToLower(s1, 4);
	ToLower(s2, 4);
	printf("ToLower \t:%s\n", s1);
	printf("ToLower \t:%s\n", s2);
}