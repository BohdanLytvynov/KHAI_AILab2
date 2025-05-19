#include "pch.h"
#include "OpenCVWrapper.h"
#include<map>
#include<algorithm>

static std::map<std::string, IplImage*> m_ImageStorage;

void LoadImageToOpenCV(const char* path, const char* imgName, int color)
{
	auto img = cvLoadImage(path, color);

	if (img != nullptr && m_ImageStorage.find(imgName) == m_ImageStorage.end())
		m_ImageStorage[imgName] = img;
}

void DisplayImageInWindow(const char* imgName, const char* windowName)
{
	auto img = m_ImageStorage.find(imgName);

	cvNamedWindow(windowName, CV_WINDOW_AUTOSIZE);

	if (img != m_ImageStorage.end())
	{
		cvShowImage(windowName, img->second);

		cvWaitKey(0);
	}
}

void SmoothImage(const char* srcImgName, const char* dstImgName,
	int smoothType, int size1, int size2, double sigma1, double sigma2)
{
	//Get the source image from the storage
	auto srcImg = m_ImageStorage.find(std::string(srcImgName));

	if (srcImg != m_ImageStorage.end())//If source image was found
	{
		auto smoothImg = cvCloneImage(srcImg->second);

		cvSmooth(srcImg->second, smoothImg, smoothType, size1, size2, sigma1, sigma2);

		m_ImageStorage[dstImgName] = smoothImg;
	}
}

//OPENCV_API void FindContours()
//{
//	using namespace cv;
//		
//	
//}
//
//void FindContoursInternal(IplImage* src)
//{
//	IplImage* gray = cvCreateImage(src->imageSize, 8, 1);
//	
//	auto storage = cvCreateMemStorage();
//
//	CvSeq* contours = nullptr;
//	cvCvtColor(src, gray, CV_BGR2GRAY);
//	cvThreshold(gray, gray, 128, 255, CV_THRESH_BINARY);
//
//	cvFindContours(gray, storage, &contours, sizeof(CvContour),
//		CV_RETR_TREE, CV_CHAIN_APPROX_SIMPLE, cvPoint(0, 0)
//	);
//
//	if (contours != nullptr)
//	{
//		cvDrawContours(original, contours, CV_RGB(255, 0, 0), CV_RGB(0, 255, 0),
//			2, 1, CV_AA, cvPoint(0, 0));
//
//		cvShowImage(originalName, original);
//
//		cv::waitKey(0);
//	}	
//
//	cvReleaseImage(&gray);
//	cvReleaseMemStorage(&storage);	
//}

void PerformCleanUp()
{
	for (auto r : m_ImageStorage)
	{
		cvReleaseImage(&r.second);
	}

	cvDestroyAllWindows();
}

void FreeImage(const char* imgName)
{
	auto img = m_ImageStorage.find(imgName);

	if (img != m_ImageStorage.end())
		cvReleaseImage(&img->second);
}

void CallImagesCleanUp(const char* pathToImgFiles)
{
	std::string images;

	auto imgs = FindNotUsedImages();
	size_t len = imgs.size();
	int i = 0;

	for (auto img : imgs)
	{
		m_ImageStorage.erase(img.first);
		cvReleaseImage(&img.second);

		if (i == len - 1)//Last element
		{
			images.append(img.first);
		}
		else
		{
			images.append(img.first);
			images.append(",");
		}

		i++;
	}

	if (images.size() > 0)
	{
		std::fstream fs;
		fs.open(pathToImgFiles, std::ios::out | std::ios::trunc);
		fs << images;
		fs.close();
	}
}

std::vector<std::pair<const std::string, IplImage*>> FindNotUsedImages()
{
	std::vector<std::pair<const std::string, IplImage*>> result;

	for (auto w : m_ImageStorage)
	{
		if (cvGetWindowHandle(w.first.c_str()) == nullptr)
		{
			result.push_back(w);
		}
	}

	return result;
}




