#include "stdafx.h"
#include "MemoryAlignment.h"
#include "ParamTest.h"
#include "PointerTest.h"
#include "Intropc\Func_Simple.h"
//#include "Inject\InjectDemo.h"

int main(int argc, char* argv) {

	ParamTest();
	//PointerTest();
	MemoryAlignmentTest();

	Func_Simple_Test();

	return 0;
}