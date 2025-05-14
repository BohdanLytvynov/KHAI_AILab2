#include "pch.h"
#include "TestClass.h"

TestClass::TestClass(int a, std::string str)
{
	this->a = a;
	this->m_string = str;
}

int TestClass::getA() const
{
	return a;
}

std::string TestClass::getStr() const
{
	return m_string;
}


