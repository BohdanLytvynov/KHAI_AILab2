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
   
   #Init environment
   python -m venv venv
   .\venv\Scripts\activate
   
   ```
   Then install all required ML libs:
   ```python
   
   # Update installer
   python -m pip install --upgrade pip
   
   # TensorFlow та Keras
   pip install tensorflow

   # Processing and visualizations
   pip install numpy seaborn matplotlib

   # Additional libs
   pip install kagglehub scikit-learn
   # Convertion to ONNX model
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
   We need to get exe file first to understand where to put the *.onnx model. 
   


   

   
