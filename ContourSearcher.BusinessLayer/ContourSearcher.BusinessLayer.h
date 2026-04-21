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

		void AverageImage(String^ SrcImg, String^ AveragingResultName,
			Int32 dataType, Int32 kernelRows, Int32 kernelColumns, 
			Int32 anchorX, Int32 anchorY, bool normalize, Int32 borderTye);
		void Blur(String^ SrcImg, String^ BluringResultName, Int32 kernelRows, Int32 kernelColumns,
			Int32 anchorX, Int32 anchorY, Int32 borderType);
		void GaussianBlur(String^ SrcImg, String^ GaussianBlurResultName, Int32 kernelRows, Int32 kernelColumns,
			Double sig1, Double sig2, Int32 borderType);
		void CustomFilter(String^ SrcImg, String^ CustomFilterResultName,
			List<List<Double>^>^ kernel, Int32 depth, Int32 anchorX, Int32 anchorY, Double delta,
			Int32 borderType);
		void BilateralFilter(String^ SrcImg, String^ BilateralFilterResultName,
			Int32 d, Double sigmaColor, Double sigmaSpace, Int32 borderType);		
		void SobelEdgeDetect(String^ srcImg, String^ dstImg, Double threshold, int kernelSize, Double scale, Double delta, int borderType);
		void CannyEdgeDetect(String^ srcImg, String^ dstImg, Double threshold1, Double threshold2, int kernelSize, bool L2Flag);
		void LaplacianEdgeDetect(String^ srcImg, String^ dstImg, int kernelSize, Double scale, Double Delta, int borderType);

		void BlobDetect(String^ srcImg, String^ dstImg, bool filterByArea,
			float minArea, float maxArea, bool filterByCircularity,
			float minCircularity, bool filterByColor, unsigned char color);
		Double GetVarianceOfLaplacian(String^ srcImg);
		List<String^>^ GetActiveWindows();
		array<Byte>^ GetImageForSkinDiseaseScanner(String^ imgName, int sizeX, int sizeY, bool useDebug, String^ debugImgName);
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

		void BuildKernel(cv::Mat* res, List<List<Double>^>^ matrix);
		std::string ChannelIndexToString(int index);
		void UpdateImageStorage(std::string imgName, cv::Mat img);
		cv::Mat GetGradientMagnitude(cv::Mat xGrad, cv::Mat yGrad);
		float getMean(cv::Mat input);
		float getVariance(cv::Mat input);

		array<Byte>^ Mat2ByteArray(cv::Mat mat);

		void SaveDebugImageForSkinDiseaseScanner(const std::string& imgName, cv::Mat mat);
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
		virtual void Separate_Equalize(String^ SrcImg, String^ EqualizeResultName) override;
		virtual void YCrCb_Equalize(String^ SrcImg, String^ EqualizeResultName) override;
		virtual void AverageImage(String^ SrcImg, String^ AveragingResultName,
			Int32 dataType, Int32 kernelRows, Int32 kernelColumns,
			Int32 anchorX, Int32 anchorY, bool normalize, Int32 borderTye) override;
		virtual void Blur(String^ SrcImg, String^ BluringResultName, Int32 kernelRows, Int32 kernelColumns,
			Int32 anchorX, Int32 anchorY, Int32 borderType) override;
		virtual void GaussianBlur(String^ SrcImg, String^ GaussianBlurResultName, Int32 kernelRows, Int32 kernelColumns,
			Double sig1, Double sig2, Int32 borderType) override;
		virtual void CustomFilter(String^ SrcImg, String^ CustomFilterResultName, 
			List<List<Double>^>^ kernel, Int32 depth, Int32 anchorX, Int32 anchorY, Double delta,
			Int32 borderType) override;
		virtual void BilateralFilter(String^ SrcImg, String^ BilateralFilterResultName, 
			Int32 d, Double sigmaColor, Double sigmaSpace, Int32 borderType) override;
		virtual void SobelEdgeDetect(String^ srcImg, String^ dstImg, Double threshold, int kernelSize, Double scale, Double delta, int borderType) override;
		virtual void CannyEdgeDetect(String^ srcImg, String^ dstImg, Double threshold1, Double threshold2, int kernelSize, bool L2Flag) override;
		virtual void LaplacianEdgeDetect(String^ srcImg, String^ dstImg, int kernelSize, Double scale, Double Delta, int borderType) override;
		virtual void BlobDetect(String^ srcImg, String^ dstImg, bool filterByArea,
			float minArea, float maxArea, bool filterByCircularity,
			float minCircularity, bool filterByColor, unsigned char color) override;
		virtual List<String^>^ GetActiveWindows() override;
		virtual Double GetVarianceOfLaplacian(String^ srcImg) override;
		virtual array<Byte>^ GetImageForSkinDiseaseScanner(String^ imgName, int sizeX, int sizeY, bool useDebug, String^ debugImgName) override;
#pragma endregion
	};
}

#endif
