#include "pch.h"
#include "OpenCVWrapper.h"
#include "opencv2/highgui/highgui.hpp"

IplImage* original = nullptr;

const char* originalName = "original";

OPENCV_API void LoadAndDisplayImage(const char* path, int color)
{
	using namespace cv;

	original = cvLoadImage(path, color);

	cvNamedWindow(originalName);

	cvShowImage(originalName, original);

	waitKey(0);
}

OPENCV_API void FindContours()
{
	using namespace cv;
		
	
}

void FindContoursInternal(IplImage* src)
{
	IplImage* gray = cvCreateImage(src->imageSize, 8, 1);
	
	auto storage = cvCreateMemStorage();

	CvSeq* contours = nullptr;
	cvCvtColor(src, gray, CV_BGR2GRAY);
	cvThreshold(gray, gray, 128, 255, CV_THRESH_BINARY);

	cvFindContours(gray, storage, &contours, sizeof(CvContour),
		CV_RETR_TREE, CV_CHAIN_APPROX_SIMPLE, cvPoint(0, 0)
	);

	if (contours != nullptr)
	{
		cvDrawContours(original, contours, CV_RGB(255, 0, 0), CV_RGB(0, 255, 0),
			2, 1, CV_AA, cvPoint(0, 0));

		cvShowImage(originalName, original);

		cv::waitKey(0);
	}	

	cvReleaseImage(&gray);
	cvReleaseMemStorage(&storage);	
}

OPENCV_API void FreeResources()
{
	cvReleaseImage(&original);
	
	cvDestroyAllWindows();
}


