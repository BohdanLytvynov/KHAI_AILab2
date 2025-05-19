using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace CSharpCppInteroperability.Wrappers
{
    public static class OpenCVWrapper
    {
        private const string m_dllname = "ContourSearcher.BLL.dll";

        [DllImport(m_dllname, 
            CallingConvention = CallingConvention.Cdecl,
            SetLastError = true, CharSet = CharSet.Ansi)]
        public extern static void LoadImageToOpenCV(
            [MarshalAs(UnmanagedType.LPStr)] string path,
            [MarshalAs(UnmanagedType.LPStr)] string imgName,
            int color);

        [DllImport(m_dllname, 
            CallingConvention = CallingConvention.Cdecl,
            SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern void DisplayImageInWindow(
            [MarshalAs(UnmanagedType.LPStr)] string imgName,
            [MarshalAs(UnmanagedType.LPStr)] string windowName
            );

        [DllImport(m_dllname, 
            CallingConvention = CallingConvention.StdCall,
            SetLastError = true)]
        public extern static void PerformCleanUp();

        [DllImport(m_dllname,
            CallingConvention = CallingConvention.Cdecl,
            CharSet = CharSet.Ansi)]
        public extern static void CallImagesCleanUp(
            [MarshalAs(UnmanagedType.LPStr)]string pathToImagesTempFile);

        [DllImport(m_dllname,
            CallingConvention = CallingConvention.Cdecl,
            SetLastError = true,
            CharSet = CharSet.Ansi)]
        public extern static void SmoothImage(
            [MarshalAs(UnmanagedType.LPStr)] string srcimageName,
            [MarshalAs(UnmanagedType.LPStr)] string dstimageName,
            int smoothType,
            int size1,
            int size2,
            double sigma1,
            double sigma2
            );

    }
}
