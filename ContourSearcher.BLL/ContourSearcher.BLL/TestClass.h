#pragma once

#ifdef CS_EXPORTS
#define CS_API __declspec(dllexport)
#else
#define CS_API __declspec(dllimport)
#endif

struct TestClass
{
	TestClass(int a, std::string str);

	int getA() const;

	std::string getStr() const;

private:
	int a;
	std::string m_string;
};

