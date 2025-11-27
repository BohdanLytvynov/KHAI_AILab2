using System.ComponentModel;

namespace ContourSearcher.UI.Enums
{
    internal enum ImageToLoadType : sbyte
    {
        [Description("If set, return the loaded image as is (with alpha channel, otherwise it gets cropped")]
        IMREAD_UNCHANGED = -1,
        [Description("If set, always convert image to the single channel grayscale image.")]
        IMREAD_GRAYSCALE = 0,
        [Description("If set, always convert image to the 3 channel BGR color image.")]
        IMREAD_COLOR = 1,
        [Description("If set, return 16-bit/32-bit image when the input has the corresponding depth, otherwise convert it to 8-bit.")]
        IMREAD_ANYDEPTH = 2,
        [Description("If set, the image is read in any possible color format.")]
        IMREAD_ANYCOLOR = 4,
        [Description("Gdal driver for loading the image")]
        IMREAD_LOAD_GDAL = 8,
        [Description("If set, always convert image to the single channel grayscale image and the image size reduced 1/2")]
        IMREAD_REDUCED_GRAYSCALE_2 = 16,
        [Description("If set, always convert image to the 3 channel BGR color image and the image size reduced 1/2.")]
        IMREAD_REDUCED_COLOR_2 = 17,
        [Description("If set, always convert image to the single channel grayscale image and the image size reduced 1/4.")]
        IMREAD_REDUCED_GRAYSCALE_4 = 32,
        [Description("If set, always convert image to the 3 channel BGR color image and the image size reduced 1/4.")]
        IMREAD_REDUCED_COLOR_4 = 33,
        [Description("If set, always convert image to the single channel grayscale image and the image size reduced 1/8.")]
        IMREAD_REDUCED_GRAYSCALE_8 = 64,
        [Description("If set, always convert image to the 3 channel BGR color image and the image size reduced 1/8.")]
        IMREAD_REDUCED_COLOR_8 = 65
    }   
}
