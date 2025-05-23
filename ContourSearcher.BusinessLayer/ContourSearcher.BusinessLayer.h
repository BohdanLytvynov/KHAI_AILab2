#ifndef CSBLL_H
#define CSBLL_H


using namespace System;

class OpenCV
{
#pragma region Fields
	std::map<std::string, IplImage*>* m_ImageStorage;
#pragma endregion

#pragma region Ctor
	OpenCV();
#pragma endregion

#pragma region Destructor
	~OpenCV();
#pragma endregion

#pragma region Functions

#pragma endregion


};

namespace ContourSearcherBusinessLayer {
	public ref class OpenCVWrapper
	{
		
	};
}

#endif
