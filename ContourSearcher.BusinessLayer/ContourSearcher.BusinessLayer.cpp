#include "pch.h"

#include "ContourSearcher.BusinessLayer.h"
#using <System.dll>

std::string ContourSearcherBusinessLayer::OpenCV::MarshalManagedString(String^ input)
{
	return msclr::interop::marshal_as<std::string>(input);
}

String^ ContourSearcherBusinessLayer::OpenCV::MarshalUnmanagedString(std::string& input)
{
	return msclr::interop::marshal_as<String^>(input);
}

std::vector<std::pair<std::string, IplImage*>> ContourSearcherBusinessLayer::OpenCV::FindNotUsedImages()
{
	std::vector<std::pair<std::string, IplImage*>> result;
	auto deref_ptr = *m_ImageStorage;
	for (auto w : deref_ptr)
	{
		if (cvGetWindowHandle(w.first.c_str()) == nullptr)
		{
			result.push_back(w);
		}
	}

	return result;
}

void ContourSearcherBusinessLayer::OpenCV::FreeResources()
{
	auto ptr_deref = *m_ImageStorage;

	for (auto r : ptr_deref)
	{
		cvReleaseImage(&r.second);
	}

	cvDestroyAllWindows();
}

ContourSearcherBusinessLayer::OpenCV::OpenCV()
{
	m_ImageStorage = new std::map<std::string, IplImage*>();

#ifndef NDEBUG
	OutputDebugStringA("OpenCV Initialized....");
#endif
}

ContourSearcherBusinessLayer::OpenCV::~OpenCV()
{
	OpenCV::FreeResources();

	delete m_ImageStorage;

#ifndef NDEBUG
	OutputDebugStringA("OpenCV Resources Released....");
#endif
}

ContourSearcherBusinessLayer::OpenCV::!OpenCV()
{
	this->~OpenCV();
}

void ContourSearcherBusinessLayer::OpenCV::LoadToOpenCV(String^ path, String^ imgName, Int32 color)
{
	try
	{
		std::string imgPath_str = MarshalManagedString(path);
		std::string imgName_str = MarshalManagedString(imgName);

		auto img = cvLoadImage(imgPath_str.c_str(), color);

		if (img != nullptr)
		{
			auto curr = m_ImageStorage->find(imgName_str);

			if (curr == m_ImageStorage->end())//Image Not Exists
			{
				m_ImageStorage->insert(std::pair<std::string, IplImage*>(imgName_str, img));
			}
		}
	}
	catch (System::Exception ex())
	{
		throw ex;
	}
}

void ContourSearcherBusinessLayer::OpenCV::DisplayImageInWindow(String^ imgName, String^ windowName)
{
	auto imgName_str = MarshalManagedString(imgName);
	auto windowName_str = MarshalManagedString(windowName);

	auto img = m_ImageStorage->find(imgName_str);

	if (img != m_ImageStorage->end())
	{
		cvNamedWindow(windowName_str.c_str(), CV_WINDOW_AUTOSIZE);

		cvShowImage(windowName_str.c_str(), img->second);

		cvWaitKey(0);
	}
}

List<String^>^ ContourSearcherBusinessLayer::OpenCV::CallCleanUp()
{
	List<String^>^ result = gcnew List<String^>();

	auto imgs = FindNotUsedImages();

	if (imgs.size() > 0)
	{
		for (auto img : imgs)
		{
			cvReleaseImage(&img.second);
			String^ str = MarshalUnmanagedString(img.first);
			result->Add(str);
		}
	}

	return result;
}

void ContourSearcherBusinessLayer::OpenCV::CloneImage(String^ imgName, String^ newImgName)
{
	try
	{
		std::string imgName_str = MarshalManagedString(imgName);
		std::string newImgName_str = MarshalManagedString(newImgName);

		auto img = m_ImageStorage->find(imgName_str);

		if (img != m_ImageStorage->end())
		{
			auto newImg = cvCloneImage(img->second);

			if (newImg != nullptr)
			{
				m_ImageStorage->insert(std::pair<std::string, IplImage*>(newImgName_str, newImg));
			}
		}
	}
	catch (System::Exception ex())
	{
		throw;
	}
}

void ContourSearcherBusinessLayer::OpenCV::SmoothImage(String^ imgToSmooth, Int32 smoothType, Int32 size1, Int32 size2, Double sigma1, Double sigma2)
{
	std::string imgToSmooth_str = MarshalManagedString(imgToSmooth);

	auto img = m_ImageStorage->find(imgToSmooth_str);

	if (img != m_ImageStorage->end())
	{
		auto dest = cvCloneImage(img->second);

		cvSmooth(img->second, dest, smoothType, size1, size2, sigma1, sigma2);

		cvShowImage(img->first.c_str(), img->second);

		cvWaitKey(0);
	}
}
