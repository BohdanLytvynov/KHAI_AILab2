#include "pch.h"

#include "ContourSearcher.BusinessLayer.h"

OpenCV::OpenCV()
{
	m_ImageStorage = new std::map<std::string, IplImage*>();
}

OpenCV::~OpenCV()
{
	delete m_ImageStorage;
}
