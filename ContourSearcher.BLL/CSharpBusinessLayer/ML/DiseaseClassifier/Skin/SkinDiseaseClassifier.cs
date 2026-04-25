using CSharpBusinessLayer.ML.DiseaseClassifier.Base;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System.Diagnostics;

namespace CSharpBusinessLayer.ML.DiseaseClassifier.Skin
{
    public class SkinDiseaseClassifier : IDiseaseClassifier
    {
        #region Fields
        private string m_PathToScannerModule;
        private InferenceSession? m_inferenceSession;
        private string[] m_labels;
        private string m_inputName;
        private int m_xSize;
        private int m_ySize;
        #endregion

        #region Properties
        public string[] Labels { get => m_labels; }
        #endregion

        #region Ctor
        public SkinDiseaseClassifier(string pathToScannerModule, string[] labels,
            int xSize, int ySize, string inputName)
        {
            m_inputName = inputName;

            m_xSize = xSize;
            m_ySize = ySize;

            m_PathToScannerModule = pathToScannerModule ?? throw new ArgumentNullException(nameof(pathToScannerModule));
            InitModel();
            if (labels == null || labels.Length == 0)
                throw new Exception("There were no labels for diseases!");
            m_labels = labels;
            m_inputName = inputName ?? throw new Exception("Input name wasn't set!");
        }

        public void Dispose()
        {
            m_inferenceSession?.Dispose();
#if DEBUG
            Debug.WriteLine("Model (Skin Disease Scanner) disposed...");
#endif
        }
        #endregion

        #region Methods

        public float[] Predict(byte[] rgbPixels)
        {
            int[] dimensions = { 1, m_xSize, m_ySize, 3 };
            var inputTensor = new DenseTensor<float>(dimensions);

            for (int i = 0; i < rgbPixels.Length / 3; i++)
            {
                int y = i / m_ySize;
                int x = i % m_xSize;

                inputTensor[0, y, x, 0] = (float)rgbPixels[i * 3 + 0]; // R
                inputTensor[0, y, x, 1] = (float)rgbPixels[i * 3 + 1]; // G
                inputTensor[0, y, x, 2] = (float)rgbPixels[i * 3 + 2]; // B
            }

            var inputs = new List<NamedOnnxValue>
            {
                 NamedOnnxValue.CreateFromTensor(m_inputName, inputTensor)
            };

            float[] output = null;

            using (var results = m_inferenceSession.Run(inputs))
            {
                output = results.First().AsEnumerable<float>().ToArray();
            }

            return output;
        }

        #region Private Methods
        private void InitModel()
        {
            try
            {
                m_inferenceSession = new InferenceSession(m_PathToScannerModule);
#if DEBUG
                Debug.Write("Model (Skin disease scanner) Loaded ...");
#endif
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #endregion
    }
}
