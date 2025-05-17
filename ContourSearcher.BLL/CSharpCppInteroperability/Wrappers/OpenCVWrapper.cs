using System.Runtime.InteropServices;

namespace CSharpCppInteroperability.Wrappers
{
    public static class OpenCVWrapper
    {
        private const string m_dllname = "ContourSearcher.BLL.dll";

        [DllImport(m_dllname, 
            CallingConvention = CallingConvention.Cdecl,
            SetLastError = true)]
        public extern static void LoadAndDisplayImage(
            [MarshalAs(UnmanagedType.LPStr)]string path, 
            int color);

        [DllImport(m_dllname, 
            CallingConvention = CallingConvention.Cdecl,
            SetLastError = true)]
        public extern static void FreeResources();

    }
}
