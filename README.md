# Contour Searcher Project
## Description
This program has a multifunctional possibilities. It has a modules for Image processing and Image Analyzing. But the main functionality is the Skin Disease Skanner.

## Technologies used:
- WPF
- C++ CLI
- OpenCV
- Python TensorFlow.

## This app is trained to recognize 5 skin diseases:
- Eksem
- Panu
- Acne
- Rosacea
- Herpes

## Deploymment
# Main requirements:
  - OpenCV 3.1.0
  - Visual Studio 2022 + C++ CLI Support
  - Visual Studio Code or another IDE for Python.
  - Python 3.12.4

Main deployment Steps
1. OpenCV 3.1.0: That is the core of the Image Processing pipeline.
   Link to OpenCV: https://sourceforge.net/projects/opencvlibrary/files/opencv-win/3.0.0/opencv-3.0.0.exe/download
   Links to tutorials for OpenCV install:
     - https://progtpoint.blogspot.com/2016/12/tutorial-1.html
     - https://progtpoint.blogspot.com/2016/12/tutorial-1-settup-opencv-in-visual.html
   This is highly required for correct build of the OpenCV Core
2. Visual Studio 2022 requirments. We use C++ CLI for interconection between WPF Application and C++ OpenCV core. So you need:
   <img width="1066" height="286" alt="image" src="https://github.com/user-attachments/assets/cd3dde30-ca48-496d-8673-cb951db1fbe9" />
   And the:
   <img width="746" height="551" alt="image" src="https://github.com/user-attachments/assets/2d1966a7-96bc-4ebd-8da8-951b4d8b8e80" />
3. Python - https://www.python.org/downloads/release/python-3124/
4. ML Learning libs:
   During installation most of the libs that are required for ML will try to mutate files and folders on a system drive. That can lead to install errors.
   So we recommend to use the Virtual Environment:
   ```python
   
   REM Init environment
   python -m venv venv
   .\venv\Scripts\activate
   
   ```
   Then install all required ML libs:
   ```python
   
   REM Update installer
   python -m pip install --upgrade pip
   
   REM TensorFlow та Keras
   pip install tensorflow

   REM Processing and visualizations
   pip install numpy seaborn matplotlib

   REM Additional libs
   pip install kagglehub scikit-learn
   REM Convertion to ONNX model
   pip install tf2onnx onnx
   ```
   If you recieve some additional errors, like the lack of some libs, you can install it using
   ```python
   pip install <lib>
   ```
   Then we need to navigate to the ../ContourSearcher.ML.Core
   There will be the DiseaseScanner.py that is the file that is responsible for compiling converting and exporting the model to the *.onnx
   If you want to train ML core for the new disease the new */onnx model must be compiled.
   Or you can use the precompiled disease_scanner.onnx file
   
   ## Compiling Project in VS 2022
   We need to get exe file first to understand where to put the *.onnx model. So open the ContourSearcher.UI.sln which is located at the ../ContourSearcher.UI/ContourSearcher.UI/
   Open it using Visual Studio 2022. Configure the configuration of the output Debug or Release. And build it. Also it can be built by using dotnet tools:
   ```cmd
   REM Restore the solution
   dotnet restore ContourSearcher.UI.sln
   ```
   Then do this command:
   ```cmd
   REM Build solution
   msbuild ContourSearcher.UI.sln /p:Configuration=<Configuration (Debug | Release)> /p:Platform=<Platform (x32 | x64 | x86)>
   ```
   When the project is build successfully you will get an exe file at the ..\ContourSearcher.UI\ContourSearcher.UI\bin\<Configuration>\net8.0-windows.
   There you should create the floder ..\ContourSearcher.UI\ContourSearcher.UI\bin\Debug\net8.0-windows\ML\DiseaseScanner\Skin and put there our generated or precompiled diesease_scaner.onnx file. We recommend such file structure to keep all things structured. But if you want to store the *.onnx file in other location it's up to you. But in that case you have to modify App.config file: ..\ContourSearcher.UI\ContourSearcher.UI\App.config
   <img width="1252" height="164" alt="image" src="https://github.com/user-attachments/assets/59bf4295-97fc-4677-9476-ccc96ff0830e" />
   Here we have to modify the relativePath to model property but with respect to *.exe file location.
  Also when the model will be compiled you will recieve the correct order of the diseases. That order must be preserved and inserted to the labels field.
  The input parameter can be found out by using Netron website: https://netron.app/ Insert there *.onnx file and find out the name of the input model:
  <img width="442" height="128" alt="image" src="https://github.com/user-attachments/assets/f788c2d1-9f96-4705-9742-955d9babf058" />
  As you can see we plug the name of the input node to the input parameter of the configuration.
## First Run
Suppose you set up all that we need. Run application either via *.exe file or VisualStudio 2022. If all is ok you will see this:
<img width="982" height="806" alt="image" src="https://github.com/user-attachments/assets/74aa8f94-76e6-475a-8669-7a5ccd534aa4" />

## How to Use
1. Press Open Button on Image Loading Tab
   <img width="985" height="823" alt="image" src="https://github.com/user-attachments/assets/04045410-8e66-45df-8156-d6cd237c6897" />
   And choose image with a skin disease
2. Choose the name for the image
   <img width="1196" height="811" alt="image" src="https://github.com/user-attachments/assets/2a43b684-8a12-49de-8e2c-c2b731ae76ef" />
3. Load image to OpenCV pipeline:
   - Choose your image. If you see nothing in a combobox click on the update button then choose your image and press Load to Pipeline
     <img width="1188" height="806" alt="image" src="https://github.com/user-attachments/assets/47743a03-834a-4854-a847-e42a884c1090" />
     If you see small window, that mean that image is loaded
     <img width="1692" height="892" alt="image" src="https://github.com/user-attachments/assets/a6b388f8-fea3-4f10-87a3-0d0f87c0b739" />
4. You can navigate via Tabs and perform Image Processing and Detection if you need it.
5. Navigate to the Disease Scanner Tab
   <img width="1191" height="798" alt="image" src="https://github.com/user-attachments/assets/46b5ca99-8791-4fc0-b9ef-9600d59c7f82" />
6. Choose your image. Again if combobox is empty just press update button. Then press calculate. The enable dubug checkbox enables the display of the image that was sent to the model. 
   <img width="1191" height="797" alt="image" src="https://github.com/user-attachments/assets/0909056e-c0e5-4ab1-93f5-03cd3109d0ce" />
7. After you press Calculate button you will see:
   <img width="1195" height="806" alt="image" src="https://github.com/user-attachments/assets/90929eab-00f5-4981-9d9d-7d362085e3b2" />
   Here you can see the probability distributions for the possible disgnosis.










   


   

   
