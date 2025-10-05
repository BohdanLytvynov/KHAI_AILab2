#ifndef CSBLL_H
#define CSBLL_H
#include <debugapi.h>
#include <crtdbg.h>
#include <functional>
#include "LUT.h"

using namespace System;
using namespace System::Collections::Generic;

namespace ContourSearcherBusinessLayer {
	
	public interface class ICVSystem
	{
	public:
		void LoadToOpenCV(String^ path, String^ imgName, Int32 color);

		void DisplayImageInWindow(String^ imgName, String^ windowName);

		void FreeImage(String^ imgName);

		void CloneImage(String^ imgName, String^ newImgName);

		void SmoothImage(String^ imgToSmooth, Int32 smoothType, Int32 size1, Int32 size2, Double sigma1, Double sigma2);

		void DisplayImageInExistingWindow(String^ imgToDisplay, String^ existingWindow);

		void DestroyWindow(String^ windowName);
		
		void Draw2DHistogram(String^ imgSrcName, 
			String^ histName,
			Int32 width, 
			Int32 height,
			Int32 channels,
			Int32 amountOfChannels,
			Int32 drawingMode);

		void Simple_Equalize(String^ SrcImg, String^ EqualizeResultName);
		void CLAHE_Equalize(String^ SrcImg, String^ EqualizeResultName, Double clipLimit, Int32 tileWidth, Int32 tileHeight);
		void Separate_Equalize(String^ SrcImg, String^ EqualizeResultName);
		void YCrCb_Equalize(String^ SrcImg, String^ EqualizeResultName);
		List<String^>^ GetActiveWindows();
	};


	public ref class OpenCV : public IDisposable, public ICVSystem
	{
	private:
		std::map<std::string, cv::Mat>* m_ImageNameMap;		
		bool m_disposed;

#pragma region Internal Private Functions
		std::string MarshalManagedString(String^ input);
		String^ MarshalUnmanagedString(std::string& input);

		void FreeResources();
		
		cv::Mat computeHistogram(cv::Mat* srcImgs, int imgNumber);

		cv::Scalar getPlotColor(int channelIndex);

		void plotRectHistogram(
			cv::Mat& hist,
			cv::Mat& canvas,
			cv::Scalar color,
			int rectType); 

		void plotLineHistogram(
			cv::Mat& hist,
			cv::Mat& canvas,
			cv::Scalar color,
			int thickness,
			int lineType,
			int offset);

		std::string ChannelIndexToString(int index);

#pragma endregion

	public:
#pragma region Ctor

		OpenCV();

#pragma endregion

#pragma region Destructor

		~OpenCV();

		!OpenCV();
#pragma endregion
		
#pragma region Functions To Call From Outside

		virtual void LoadToOpenCV(String^ path, String^ imgName, Int32 color) override;
		virtual void DisplayImageInWindow(String^ imgName, String^ windowName) override;
		virtual void CloneImage(String^ imgName, String^ newImgName) override;
		virtual void SmoothImage(String^ imgToSmooth, Int32 smoothType, Int32 size1, Int32 size2, Double sigma1, Double sigma2) override;
		virtual void DisplayImageInExistingWindow(String^ imgToDisplay, String^ existingWindow) override;
		virtual void FreeImage(String^ imgName) override;
		virtual void DestroyWindow(String^ windowName) override;
		virtual void Draw2DHistogram(
			String^imgSrcName, 
			String^ histName, 
			Int32 width,
			Int32 height, 
			Int32 channels,
			Int32 amountOfChannels,
			Int32 drawingMode) override;

		virtual void Simple_Equalize(String^ SrcImg, String^ EqualizeResultName) override;
		virtual void CLAHE_Equalize(String^ SrcImg, String^ EqualizeResultName, Double clipLimit, Int32 tileWidth, Int32 tileHeight) override;
		virtual void Separate_Equalize(String^ SrcImg, String^ EqualizeResultName);
		virtual void YCrCb_Equalize(String^ SrcImg, String^ EqualizeResultName);
		virtual List<String^>^ GetActiveWindows() override;
#pragma endregion
	};
}

#endif
