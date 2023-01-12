#pragma once

#ifdef __DLLEXPORT
#define __DLL_EXP _declspec(dllexport) 
#else
#define __DLL_EXP _declspec(dllimport) 
#endif 

extern "C" __DLL_EXP int Add(int left, int right);