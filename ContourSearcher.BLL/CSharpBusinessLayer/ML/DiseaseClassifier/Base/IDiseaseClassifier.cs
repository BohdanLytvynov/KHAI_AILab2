namespace CSharpBusinessLayer.ML.DiseaseClassifier.Base
{
    public interface IDiseaseClassifier : IDisposable
    {
        string[] Labels { get; }
        float[] Predict(byte[] rgbPixels);
    }
}
