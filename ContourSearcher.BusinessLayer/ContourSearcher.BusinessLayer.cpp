#include "pch.h"

#include "ContourSearcher.BusinessLayer.h"
#include"Enums.h"
#using <System.dll>

std::string ContourSearcherBusinessLayer::OpenCV::MarshalManagedString(String^ input)
{
	return msclr::interop::marshal_as<std::string>(input);
}

String^ ContourSearcherBusinessLayer::OpenCV::MarshalUnmanagedString(std::string& input)
{
	return msclr::interop::marshal_as<String^>(input);
}

void ContourSearcherBusinessLayer::OpenCV::FreeResources()
{
	auto ptr_deref = *m_ImageNameMap;

	for (auto r : ptr_deref)
	{
		r.second.release();
	}

	cvDestroyAllWindows();
}

ContourSearcherBusinessLayer::OpenCV::OpenCV()
{
	m_ImageNameMap = new std::map<std::string, cv::Mat>();

#ifndef NDEBUG
	OutputDebugStringA("OpenCV Initialized....");
#endif
}

ContourSearcherBusinessLayer::OpenCV::~OpenCV()
{
	if (!m_disposed)
	{
		OpenCV::FreeResources();
		delete m_ImageNameMap;
		m_disposed = true;
	}

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
		
		auto img = cv::imread(imgPath_str.c_str(), color);

		auto curr = m_ImageNameMap->find(imgName_str);

		if (curr == m_ImageNameMap->end())//Image Not Exists
		{
			m_ImageNameMap->insert(std::pair<std::string, cv::Mat>(imgName_str, img));
		}

#ifndef NDEBUG
		OutputDebugStringA("Image Loaded To OpenCV...");
#endif
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

	auto img = m_ImageNameMap->find(imgName_str);

	if (img != m_ImageNameMap->end())
	{
		cv::namedWindow(windowName_str, CV_WINDOW_AUTOSIZE);

		cv::imshow(windowName_str.c_str(), img->second);

		cv::waitKey(0);
	}
}

void ContourSearcherBusinessLayer::OpenCV::CloneImage(String^ imgName, String^ newImgName)
{
	try
	{
		std::string imgName_str = MarshalManagedString(imgName);
		std::string newImgName_str = MarshalManagedString(newImgName);

		auto img = m_ImageNameMap->find(imgName_str);

		if (img != m_ImageNameMap->end())
		{
			auto newImg = img->second.clone();

			m_ImageNameMap->insert(std::pair<std::string, cv::Mat>(newImgName_str, newImg));
		}

#ifndef NDEBUG
		std::string msg = "Image <" + std::string(imgName_str) + "> Cloned...";
		OutputDebugStringA(msg.c_str());
#endif
	}
	catch (System::Exception ex())
	{
		throw;
	}
}

void ContourSearcherBusinessLayer::OpenCV::SmoothImage(String^ imgToSmooth,
	Int32 smoothType, Int32 size1, Int32 size2, Double sigma1, Double sigma2)
{
	/*try
	{
		std::string imgToSmooth_str = MarshalManagedString(imgToSmooth);

		auto img = m_ImageNameMap->find(imgToSmooth_str);

		if (img != m_ImageNameMap->end())
		{
			auto imgCloned = img->second.clone();

			(img->second, imgCloned, smoothType, size1, size2, sigma1, sigma2);

			(*m_ImageNameMap)[imgToSmooth_str] = imgCloned;

			cvShowImage(imgToSmooth_str.c_str(), imgCloned);
			cvWaitKey(0);
		}
	}
	catch (System::Exception ex())
	{
		throw;
	}*/
}

void ContourSearcherBusinessLayer::OpenCV::DisplayImageInExistingWindow(String^ imgToDisplay, String^ existingWindow)
{
	try
	{
		std::string imgToDisplay_str = MarshalManagedString(imgToDisplay);
		std::string existingWindow_str = MarshalManagedString(existingWindow);

		auto img = m_ImageNameMap->find(imgToDisplay_str);
		cv::imshow(existingWindow_str.c_str(), img->second);
		cv::waitKey(0);
	}
	catch (System::Exception ex())
	{
		throw;
	}
}

void ContourSearcherBusinessLayer::OpenCV::FreeImage(String^ imgName)
{
	std::string img_name = MarshalManagedString(imgName);

	auto img = m_ImageNameMap->find(img_name);
	if (img != m_ImageNameMap->end())
	{
		img->second.release();
	}

	m_ImageNameMap->erase(img_name);

#ifndef NDEBUG
	std::string msg = "Image <" + std::string(img_name) + "> Released...";
	OutputDebugStringA(msg.c_str());
#endif
}

void ContourSearcherBusinessLayer::OpenCV::DestroyWindow(String^ windowName)
{
	std::string winName = MarshalManagedString(windowName);
	if (!winName.empty() && cvGetWindowHandle(winName.c_str()) != nullptr)
	{
		cv::destroyWindow(winName);

#ifndef NDEBUG
		std::string msg = "Image <" + std::string(winName) + "> Destroyed...";
		OutputDebugStringA(msg.c_str());
#endif
	}
}

cv::Mat ContourSearcherBusinessLayer::OpenCV::computeHistogram(cv::Mat* srcImgs, int imgNumber)
{
	using namespace cv;

	Mat histogram;
	int channels[] = { 0 };
	int histSize[] = { 256 };
	float range[] = { 0, 256 };
	const float* ranges[] = { range };

	if (srcImgs != nullptr)
	{
		calcHist(srcImgs, imgNumber, channels, Mat(), histogram, 1, histSize,
			ranges);
	}

	return histogram;
}

cv::Scalar ContourSearcherBusinessLayer::OpenCV::getPlotColor(int channelIndex)
{
	switch (channelIndex)
	{
	case 0:
		return cv::Scalar(255, 0, 0);
	case 1:
		return cv::Scalar(0, 255, 0);
	case 2:
		return cv::Scalar(0, 0, 255);
	default:
		return cv::Scalar(255, 255, 255);
	}
}

void ContourSearcherBusinessLayer::OpenCV::plotRectHistogram(
	cv::Mat& hist,
	cv::Mat& canvas,
	cv::Scalar color,
	int rectType)
{
	int plotHeight = canvas.rows;
	int binWidth = (canvas.cols / hist.rows);
	normalize(hist, hist, 0, canvas.rows, cv::NormTypes::NORM_MINMAX);

	for (int i = 1; i < hist.rows; ++i) {
		rectangle(
			canvas,
			cv::Point((binWidth * (i - 1)), (plotHeight -
				cvRound(hist.at<float>(i - 1, 0)))),
			cv::Point(binWidth * i, plotHeight),
			color,
			rectType
		);
	}
}

void ContourSearcherBusinessLayer::OpenCV::plotLineHistogram(
	cv::Mat& hist,
	cv::Mat& canvas, 
	cv::Scalar color,
	int thickness,
	int lineType,
	int offset)
{
	using namespace cv;

	int plotWidth = canvas.cols;
	int plotHeight = canvas.rows;
	int binWidth = (plotWidth / hist.rows);
	normalize(hist, hist, 0, plotHeight, NormTypes::NORM_MINMAX);

	for (int i = 1; i < hist.rows; ++i) 
	{
		line(
			canvas,
			Point((binWidth * (i - 1)), (plotHeight -
				cvRound(hist.at<float>(i - 1, 0)))),
			Point(binWidth * i, (plotHeight -
				cvRound(hist.at<float>(i, 0)))),
			color, thickness, lineType, offset
		);
	}
}

void ContourSearcherBusinessLayer::OpenCV::BuildKernel(cv::Mat* res, List<List<Double>^>^ matrix)
{
	cv::Mat m(matrix->Count, matrix[0]->Count, CV_8UC1);

	for (int i = 0; i < matrix->Count; i++)
	{
		uchar* image_row = m.ptr<uchar>(i);
		for (int j = 0; j < matrix[i]->Count; j++)
		{
			image_row[j] = matrix[i][j];
		}
	}

	*res = m;
}

std::string ContourSearcherBusinessLayer::OpenCV::ChannelIndexToString(int index)
{
	switch (index)
	{
		case 0:
			return "Blue";
		case 1:
			return "Green";
		case 2:
			return "Red";
		case 3:
			return "Alpha";
		default:
			return "Not supported Channel!";
	}
}

void ContourSearcherBusinessLayer::OpenCV::UpdateImageStorage(std::string imgName, cv::Mat img)
{
	auto res = m_ImageNameMap->find(imgName);

	if (m_ImageNameMap->empty())
	{
		m_ImageNameMap->insert(std::pair<std::string, cv::Mat>(imgName, img));
		return;
	}

	if (res == m_ImageNameMap->end())
	{
		m_ImageNameMap->insert(std::pair<std::string, cv::Mat>(imgName, img));
		return;
	}
	else
	{
		m_ImageNameMap->at(imgName) = img;
	}
}

void ContourSearcherBusinessLayer::OpenCV::Draw2DHistogram(
	String^ imgSrcName,
	String^ histName,
	Int32 width,
	Int32 height,
	Int32 channels,
	Int32 amountOfChannels,
	Int32 drawingMode)
{
	using namespace cv;
	using namespace std;

	std::string imgSrcName_str = MarshalManagedString(imgSrcName);
	std::string histName_str = MarshalManagedString(histName);

	auto src = m_ImageNameMap->find(imgSrcName_str);

	if (src != m_ImageNameMap->end())
	{
		Mat img = src->second;
		int channelsCount = img.channels();

		if (channelsCount == 1)//GrayScale Image
		{
			Mat hist = computeHistogram(&img, 1);
			Mat canvas(height, width, CV_8UC3, Scalar(0, 0, 0));
			plotRectHistogram(hist, canvas, CV_RGB(200, 200, 200), CV_FILLED);
			namedWindow(histName_str, CV_WINDOW_AUTOSIZE);
			imshow(histName_str, canvas);
			waitKey(0);
		}
		else//Colored Image
		{
			vector<Mat> channelsMat;
			split(img, channelsMat);

			bool mode = (1 & drawingMode) != 0;//0 - draw all separately 1 - all in one plot

			if (mode)//All in one
			{
				Mat canvas(height, width, CV_8UC3, Scalar(0,0,0));

				for (int i = 0; i < amountOfChannels; i++)
				{
					if (((1 << i) & channels) != 0)
					{
						histName_str += "_" + ChannelIndexToString(i);
						Mat hist = computeHistogram(&channelsMat[i], 1);
						plotLineHistogram(hist, canvas, getPlotColor(i), 2, CV_AA, 0);
					}
				}
				
				namedWindow(histName_str, CV_WINDOW_AUTOSIZE);
				imshow(histName_str, canvas);
				waitKey(0);
			}
			else//Draw in separate Plots
			{
				for (int i = 0; i < amountOfChannels; i++)
				{
					if (((1 << i) & channels) != 0)
					{
						Mat canvas(height, width, CV_8UC3, Scalar(0, 0, 0));
						Mat hist = computeHistogram(&channelsMat[i], 1);
						plotRectHistogram(hist, canvas, getPlotColor(i), CV_FILLED);
						std::string winName = histName_str + "_" + ChannelIndexToString(i);
						namedWindow(winName, CV_WINDOW_AUTOSIZE);
						imshow(winName, canvas);
					}
				}

				waitKey(0);
			}
		}
	}
}

void ContourSearcherBusinessLayer::OpenCV::Simple_Equalize(String^ SrcImg, String^ EqualizeResultName)
{
	using namespace cv;
	std::string srcImgName_str = MarshalManagedString(SrcImg);
	std::string equalizedResultName_str = MarshalManagedString(EqualizeResultName);

	auto img = m_ImageNameMap->find(srcImgName_str);

	if (img != m_ImageNameMap->end())
	{
		Mat src = img->second;
		int channelCount = src.channels();
		if (channelCount == 1)
		{
			Mat dest = src.clone();
			equalizeHist(src, dest);
			m_ImageNameMap->insert(std::pair<std::string, Mat>(equalizedResultName_str, dest));
		}
	}
}

void ContourSearcherBusinessLayer::OpenCV::CLAHE_Equalize(
	String^ SrcImg, 
	String^ EqualizeResultName, 
	Double clipLimit, 
	Int32 tileWidth, 
	Int32 tileHeight)
{
	using namespace cv;
	using namespace std;
	std::string src_str = MarshalManagedString(SrcImg);
	std::string equalResult_str = MarshalManagedString(EqualizeResultName);

	auto res = m_ImageNameMap->find(src_str);

	if (res != m_ImageNameMap->end())
	{
		Mat srcImg = res->second;
		int channelCount = srcImg.channels();
		Size tileGridSize = Size(tileWidth, tileHeight);
		Ptr<cv::CLAHE> clahePtr = cv::createCLAHE(clipLimit, tileGridSize);
		Mat dest;
		if (channelCount == 1)//GrayScaleImg
		{
			clahePtr->apply(srcImg, dest);
		}
		else
		{
			Mat labImg;
			//1) Convert the BGR to LAB color space
			cvtColor(srcImg, labImg, CV_Lab2BGR);
			//2) Extract L-Channels lab_planes[0] - Lightness [1] - a [2] - b
			vector<Mat> lab_planes(3);
			split(labImg, lab_planes);
			//3) Apply CLAHE			
			clahePtr->apply(lab_planes[0], lab_planes[0]);
			//4) Merge equalized Lightness back to LAB channels
			merge(lab_planes, labImg);
			//5) Convert back to BGR
			cvtColor(labImg, dest, COLOR_Lab2BGR);
		}
		m_ImageNameMap->insert(std::pair<std::string, Mat>(equalResult_str, dest));
	}
}

void ContourSearcherBusinessLayer::OpenCV::Separate_Equalize(String^ SrcImg, String^ EqualizeResultName)
{
	using namespace cv;
	using namespace std;
	std::string src_str = MarshalManagedString(SrcImg);
	std::string equalResult_str = MarshalManagedString(EqualizeResultName);

	auto res = m_ImageNameMap->find(src_str);
	if (res != m_ImageNameMap->end())
	{
		Mat dest;
		Mat srcImg = res->second;
		vector<Mat> channels;
		split(srcImg, channels);
		int channelsCount = channels.size();
		if (channelsCount > 1)
		{
			for (int i = 0; i < channelsCount; i++)
			{
				equalizeHist(channels[i], channels[i]);
			}

			merge(channels, dest);
			m_ImageNameMap->insert(pair<string, Mat>(equalResult_str, dest));
		}
	}
}

void ContourSearcherBusinessLayer::OpenCV::YCrCb_Equalize(String^ SrcImg, String^ EqualizeResultName)
{
	using namespace std;
	using namespace cv;
	string srcImgName = MarshalManagedString(SrcImg);
	string equalResultName = MarshalManagedString(EqualizeResultName);

	auto res = m_ImageNameMap->find(srcImgName);
	if (res != m_ImageNameMap->end())
	{
		Mat srcImg = res->second;
		int channelCount = srcImg.channels();
		if (channelCount > 1)
		{
			//1) Convert to the yCrCb colorspace
			Mat yCrCb;
			Mat dest;
			cvtColor(srcImg, yCrCb, COLOR_BGR2YCrCb);

			//2)Split the image into Y, Cr, and Cb channels
			vector<Mat> channels;
			split(yCrCb, channels);
			//3) Luminance Equalization
			equalizeHist(channels[0], channels[0]);
			//4)Merge the equalized Y channel with the original Cr and Cb channels
			merge(channels, yCrCb);
			//5) Convert back to BGR
			cvtColor(yCrCb, dest, COLOR_YCrCb2BGR);
			m_ImageNameMap->insert(pair<string, Mat>(equalResultName, dest));
		}
	}
}

void ContourSearcherBusinessLayer::OpenCV::AverageImage(String^ SrcImg, String^ AveragingResultName,
	Int32 dataType, Int32 kernelRows, Int32 kernelColumns, Int32 anchorX, Int32 anchorY, bool normalize, Int32 borderTye)
{
	using namespace std;
	using namespace cv;
	string srcImgName = MarshalManagedString(SrcImg);
	string resultName = MarshalManagedString(AveragingResultName);

	auto res = m_ImageNameMap->find(srcImgName);
	if (res != m_ImageNameMap->end())
	{
		Mat srcImg = res->second;
		Mat output;
		boxFilter(srcImg, output, dataType, cv::Size(kernelColumns, kernelRows), cv::Point(anchorX, anchorY), normalize, borderTye);		
		UpdateImageStorage(resultName, output);
	}
}

void ContourSearcherBusinessLayer::OpenCV::Blur(String^ SrcImg, String^ BluringResultName, Int32 kernelRows, Int32 kernelColumns, Int32 anchorX, Int32 anchorY, Int32 borderType)
{
	using namespace std;
	using namespace cv;
	string srcImgName = MarshalManagedString(SrcImg);
	string resultName = MarshalManagedString(BluringResultName);

	auto res = m_ImageNameMap->find(srcImgName);
	if (res != m_ImageNameMap->end())
	{
		Mat srcImg = res->second;
		Mat output;
		blur(srcImg, output, cv::Size(kernelColumns, kernelRows), cv::Point(anchorX, anchorY), borderType);		
		UpdateImageStorage(resultName, output);
	}
}

void ContourSearcherBusinessLayer::OpenCV::GaussianBlur(String^ SrcImg, String^ GaussianBlurResultName, Int32 kernelRows, Int32 kernelColumns, Double sig1, Double sig2, Int32 borderType)
{
	using namespace std;
	using namespace cv;
	string srcImgName = MarshalManagedString(SrcImg);
	string resultName = MarshalManagedString(GaussianBlurResultName);

	auto res = m_ImageNameMap->find(srcImgName);
	if (res != m_ImageNameMap->end())
	{
		Mat srcImg = res->second;
		Mat output;
		cv::GaussianBlur(srcImg, output, cv::Size(kernelColumns, kernelRows), sig1, sig2, borderType);
		UpdateImageStorage(resultName, output);
	}
}

void ContourSearcherBusinessLayer::OpenCV::CustomFilter(String^ SrcImg, String^ CustomFilterResultName,
	List<List<Double>^>^ kernel, Int32 depth, Int32 anchorX, Int32 anchorY, Double delta,
	Int32 borderType)
{
	using namespace std;
	using namespace cv;
	string srcImgName = MarshalManagedString(SrcImg);
	string resultName = MarshalManagedString(CustomFilterResultName);

	auto res = m_ImageNameMap->find(srcImgName);
	if (res != m_ImageNameMap->end())
	{
		Mat srcImg = res->second;
		Mat output;
		Mat filterKernel;
		BuildKernel(&filterKernel, kernel);
		cv:filter2D(srcImg, output, depth, filterKernel, cv::Point(anchorX, anchorY), delta, borderType);
		UpdateImageStorage(resultName, output);
	}
}

void ContourSearcherBusinessLayer::OpenCV::BilateralFilter(String^ SrcImg, String^ BilateralFilterResultName,
	Int32 d, Double sigmaColor, Double sigmaSpace, Int32 borderType)
{
	using namespace std;
	using namespace cv;
	string srcImgName = MarshalManagedString(SrcImg);
	string resultName = MarshalManagedString(BilateralFilterResultName);

	auto res = m_ImageNameMap->find(srcImgName);
	if (res != m_ImageNameMap->end())
	{
		Mat srcImg = res->second;
		Mat output;		
		bilateralFilter(srcImg, output, d, sigmaColor, sigmaSpace, borderType);
		UpdateImageStorage(resultName, output);
	}
}

List<String^>^ ContourSearcherBusinessLayer::OpenCV::GetActiveWindows()
{
	if (m_disposed)
		return gcnew List<String^>();

	auto ImageNameMap = *m_ImageNameMap;	

	List<String^>^ result = gcnew List<String^>();
	
	for (auto item : ImageNameMap)
	{
		auto handle = cvGetWindowHandle(item.first.c_str());
		if (handle != nullptr)
		{
			auto name = item.first;
			result->Add(MarshalUnmanagedString(name));
		}
	}

	return result;
}
