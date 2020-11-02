#pragma once
#include "stdafx.h"

extern "C" __declspec(dllexport)
int Add(int, int);

extern "C" __declspec(dllexport)
char* AddStr(char*, char*);

extern "C" __declspec(dllexport)
char GetChar(char*, int);

extern "C" __declspec(dllexport)
void ToUpper(char* str, int len);

extern "C" __declspec(dllexport)
void ToLower(char* str, int len);


void Func_Simple_Test();