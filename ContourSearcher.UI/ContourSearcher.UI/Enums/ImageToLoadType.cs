using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContourSearcher.UI.Enums
{
    internal enum ImageToLoadType : sbyte
    {
        /* 8bit, color or not */
        [Description("Load Image Unchanged")]
        CV_LOAD_IMAGE_UNCHANGED = -1,
        /* 8bit, gray */
        [Description("Load Image Grayscale")]
        CV_LOAD_IMAGE_GRAYSCALE = 0,
        /* ?, color */
        [Description("Load Image Color")]
        CV_LOAD_IMAGE_COLOR = 1,
        /* any depth, ? */
        [Description("Load Image AnyDepth")]
        CV_LOAD_IMAGE_ANYDEPTH = 2,
        /* ?, any color */
        [Description("Load Image Anycolor")]
        CV_LOAD_IMAGE_ANYCOLOR = 4
    }
}
