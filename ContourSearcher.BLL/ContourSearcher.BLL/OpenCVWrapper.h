
#ifndef OPENCVWRAPPER_H
#define OPENCVWRAPPER_H

#ifndef EXPORTER
#define OPENCV_API extern "C" __declspec(dllexport)
#else
#define OPENCV_API extern "C" __declspec(dllimport)
#endif

OPENCV_API void LoadImageToOpenCV(const char* path, const char*, int color);

OPENCV_API void DisplayImageInWindow(const char* imgName, const char* windowName);

OPENCV_API void CallCleanUp();

OPENCV_API void FreeImage(const char* imgName);

OPENCV_API void CallImagesCleanUp(const char* pathToImgTempFiles);

OPENCV_API void SmoothImage(const char* srcImageName, const char* destImgName,
	int smoothType, int size1, int size2, double sigma1, double sigma2);

//OPENCV_API void FindContours();

#pragma region Private functions

std::vector<std::pair<const std::string, IplImage*>> FindNotUsedImages();

#pragma endregion

#endif