// Fakehacker.cpp : main project file.

#include "stdafx.h"

using namespace System;

int main(array<System::String ^> ^args)
{
	int num;
	num = 10000000;
	while(num > 0) {
		Console::Write(num + "-");
		num-=1;
	}
}
