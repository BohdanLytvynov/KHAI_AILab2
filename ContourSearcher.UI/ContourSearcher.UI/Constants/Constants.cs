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

        public const string ORIGINAL_IMAGE_NAME = "Original";
        public const string SMOOTHED_IMAGE_NAME = "Smoothed Image";
        public const string DEFAULT_INPUT_VALUE = "";

        //Don't touch this section!
        public const string LOADED_IMAGE_LIST_COLLECTION = "LoadedImages";
        public const string IMAGES_LOADED_TO_PIPELINE = "ImagesInPipeline";
    }
}
