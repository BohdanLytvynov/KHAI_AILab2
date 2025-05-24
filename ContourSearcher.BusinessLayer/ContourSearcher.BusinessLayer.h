#ifndef CSBLL_H
#define CSBLL_H
#include <debugapi.h>

using namespace System;
using namespace System::Collections::Generic;

namespace ContourSearcherBusinessLayer {
	public interface class ICVSystem
	{
	public:
		void LoadToOpenCV(String^ path, String^ imgName, Int32 color);

		void DisplayImageInWindow(String^ imgName, String^ windowName);

		List<String^>^ CallCleanUp();
	};


	public ref class OpenCV : public IDisposable, public ICVSystem
	{
	private:
		std::map<std::string, IplImage*>* m_ImageStorage;
		bool m_disposed;

#pragma region Internal Private Functions
		std::string MarshalManagedString(String^ input);
		String^ MarshalUnmanagedString(std::string& input);
		
		std::vector<std::pair<std::string, IplImage*>> FindNotUsedImages();

		void FreeResources();
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

		virtual List<String^>^ CallCleanUp();
#pragma endregion


	};
}

#endif
