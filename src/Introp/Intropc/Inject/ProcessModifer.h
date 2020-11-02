#include <Windows.h>

#pragma once
class ProcessModifer
{
private:
	char wName[];
public:

	ProcessModifer(char * wName);
	~ProcessModifer();

public:

	HWND FindWindow_Ex(LPCTSTR ipClassName, LPCTSTR ipWindowName);
	DWORD GetWindowThreadProcessId_Ex(HWND hWnd, LPDWORD & lpdwProcessId);
	HANDLE OpenProcess_Ex(DWORD dwDesiredAccess, BOOL bInheritHandle, DWORD dwProcessId);
	bool WriteProcessMemory_Ex(HANDLE hProcess, LPVOID lpBaseAddress, LPVOID lpBuffer, DWORD nSize, SIZE_T* lpNumberOfBytesWritten);
	void Begin();
};

