// This is the main DLL file.

#include <windows.h>
#include "stdafx.h"
//#include <stdio.h>
//#include <tchar.h>
//#include "NativeDll.h"
//#include "MSCorEE.h"
//#include "metahost.h"

DWORD CALLBACK StartTheDotNetRuntime(LPVOID lp)
{

	//HRESULT hr = S_OK;
	//ICLRMetaHost    *m_pMetaHost = NULL;
	//ICLRRuntimeInfo *m_pRuntimeInfo = NULL;
	//ICLRRuntimeHost    *pClrHost = NULL;
 //      
	//hr = CLRCreateInstance(CLSID_CLRMetaHost, IID_ICLRMetaHost, (LPVOID*) &m_pMetaHost);
	//if (hr != S_OK) 
	//	return hr;
	//hr = m_pMetaHost->GetRuntime (L"v4.0.30319", IID_ICLRRuntimeInfo, (LPVOID*) &m_pRuntimeInfo);
	//if (hr != S_OK)
	//	return hr;
	//hr = m_pRuntimeInfo->GetInterface(CLSID_CLRRuntimeHost, IID_ICLRRuntimeHost, (LPVOID*) &pClrHost );
	//	   if (FAILED(hr)) return hr;
	//HRESULT hrStart = pClrHost->Start();

 //   DWORD dwRet = 0;
 //   hr = pClrHost->ExecuteInDefaultAppDomain(
 //       L"d:\\ManagedDll.dll",
 //       L"ManagedDll.Class1", L"Start", L"nothing to post", &dwRet);

 //   hr = pClrHost->Stop();

 //   pClrHost->Release();
	MessageBox(NULL, L"test", L"test", 0);
	return S_OK;
}


BOOL WINAPI DllMain(HINSTANCE hinstDLL,DWORD fdwReason,LPVOID lpvReserved)
{
	switch(fdwReason)
    {
      case DLL_PROCESS_ATTACH:
		 CreateThread(0,0,StartTheDotNetRuntime,0,0,0);
		 //MessageBox(NULL, L"DllMain test", L"Dll Main test", 0);
		 break;
      case DLL_PROCESS_DETACH:
        break;
	  case DLL_THREAD_ATTACH:
		 break;
	  case DLL_THREAD_DETACH:
		 break;
      default:
        break;
    }
	return true;
}
