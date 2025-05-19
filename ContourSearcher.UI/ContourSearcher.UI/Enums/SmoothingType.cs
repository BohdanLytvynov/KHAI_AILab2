using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContourSearcher.UI.Enums
{
    internal enum SmoothingType
    {
        /** linear convolution with \f$\texttt{size1}\times\texttt{size2}\f$ box kernel (all 1's). If
   you want to smooth different pixels with different-size box kernels, you can use the integral
   image that is computed using integral */
        CV_BLUR_NO_SCALE = 0,
        /** linear convolution with \f$\texttt{size1}\times\texttt{size2}\f$ box kernel (all
        1's) with subsequent scaling by \f$1/(\texttt{size1}\cdot\texttt{size2})\f$ */
        CV_BLUR = 1,
        /** linear convolution with a \f$\texttt{size1}\times\texttt{size2}\f$ Gaussian kernel */
        CV_GAUSSIAN = 2,
        /** median filter with a \f$\texttt{size1}\times\texttt{size1}\f$ square aperture */
        CV_MEDIAN = 3,
        /** bilateral filter with a \f$\texttt{size1}\times\texttt{size1}\f$ square aperture, color
        sigma= sigma1 and spatial sigma= sigma2. If size1=0, the aperture square side is set to
        cvRound(sigma2\*1.5)\*2+1. See cv::bilateralFilter */
        CV_BILATERAL = 4
    }
}
