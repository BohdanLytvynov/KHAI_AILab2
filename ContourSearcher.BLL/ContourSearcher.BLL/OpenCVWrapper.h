
#ifndef EXPORTER
#define OPENCV_API extern "C" __declspec(dllexport)
#else
#define OPENCV_API extern "C" __declspec(dllimport)
#endif

OPENCV_API void LoadAndDisplayImage(const char* path, int color);

OPENCV_API void FreeResources();

OPENCV_API void FindContours();

#pragma region Private functions

void FindContoursInternal();

#pragma endregion
