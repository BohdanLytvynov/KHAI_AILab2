namespace ContourSearcher.UI.Constant
{
    internal static class Constants
    {
        internal static class Histogram
        {
            public const string Name = "Histograms:";
            public const int Width = 256;
            public const int Height = 256;
        }

        internal static class Equalizer
        {
            public const string Name = "Equalizer:";
            public const double ClipLimit = 2.0;
            public const int GridTileWidth = 8;
            public const int GridTileHeight = 8;
        }

        internal static class Filtering
        {
            public const string Name = "Filtering:";
        }

        internal static class EdgeDetection
        {
            public const string Name = "Edge Detection";
        }

        internal static class BlobDetection
        {
            public const string Name = "Blod Detection";
        }

        internal static class BlurDetection
        {
            public const string Name = "Blur Detection";
        }

        internal static class SkinDiseaseDetection
        {
            public const string Name = "Skin Disease Detection";
            public const string DiagnosisDefault = "Not estimated yet.";
        }

        public const string ORIGINAL_IMAGE_NAME = "Original";
        public const string SMOOTHED_IMAGE_NAME = "Smoothed Image";
        public const string DEFAULT_INPUT_VALUE = "";
        public const string MODULE_ERROR_MSG = "An error occured during the processing! Error: {0}";

        //Don't touch this section!
        public const string LOADED_IMAGE_LIST_COLLECTION = "LoadedImages";
        public const string IMAGES_LOADED_TO_PIPELINE = "ImagesInPipeline";
    }
}
