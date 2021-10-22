using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Simple.Format
{
    [StructLayout(LayoutKind.Sequential, Pack =1)]
    public struct BMPHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] ID;
        public UInt32 FileSize;
        public UInt16 AppSpec1;
        public UInt16 AppSpec2;
        public UInt32 PixelOffset;
    }

    public class BMP
    {
        public static BMPHeader GetBMPHeader(byte[] buffer)
        {
            return Utility.ByteArrayToStruct<BMPHeader>(buffer);
        }

        public static BMPHeader GetBMPHeader(string path)
        {
            var bytes = File.ReadAllBytes(path);
            return GetBMPHeader(bytes);
        }
    }
}
