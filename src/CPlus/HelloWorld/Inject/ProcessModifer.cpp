#include "../stdafx.h"
#include "ProcessModifer.h"
#include <Windows.h>
#include <stdio.h>


ProcessModifer::ProcessModifer(char * name)
{
}


ProcessModifer::~ProcessModifer()
{
}

//查找window 返回窗口句柄
HWND ProcessModifer::FindWindow_Ex(LPCTSTR ipClassName,LPCTSTR ipWindowName)
{
	return ::FindWindow(ipClassName, ipWindowName);
}
//通过窗口句柄获取窗口所在进程ID和线程ID
DWORD ProcessModifer::GetWindowThreadProcessId_Ex(HWND hWnd, LPDWORD &lpdwProcessId)
{
	return ::GetWindowThreadProcessId(hWnd, lpdwProcessId);
}

//打开已存在的进程，返回进行句柄
HANDLE ProcessModifer::OpenProcess_Ex(DWORD dwDesiredAccess, BOOL bInheritHandle, DWORD dwProcessId)
{
	return ::OpenProcess(dwDesiredAccess, bInheritHandle, dwProcessId);
}

//进行区域写入数据
bool ProcessModifer::WriteProcessMemory_Ex(HANDLE hProcess, LPVOID lpBaseAddress, LPVOID lpBuffer, DWORD nSize, LPDWORD lpNumberOfBytesWritten)
{
	return ::WriteProcessMemory(hProcess, lpBaseAddress, lpBuffer, nSize, lpNumberOfBytesWritten);
}

void ProcessModifer::Begin()
{
	LPCTSTR name = LPCTSTR("test.exe");
	HWND h = FindWindow_Ex(NULL, name);
	LPDWORD lpid;
	DWORD res = GetWindowThreadProcessId_Ex(h, lpid);
	HANDLE pidh;
	pidh = OpenProcess_Ex(PROCESS_ALL_ACCESS, FALSE, *lpid);
	if (pidh == 0) {
		printf("打开进程失败");
		return;
	}
	else {
		printf("打开进程成功");
		DWORD hp = 300;
		LPCVOID addr = (LPCVOID)0xFFFF;
		DWORD res2 = WriteProcessMemory_Ex(pidh, (LPVOID)addr, &hp, 4, 0);
	}
}

