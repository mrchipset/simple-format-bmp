using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Simple.Format
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BMPFileHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] ID;   // BMP Identification code "BM"
        public uint FileSize;   // the size of bmp file
        public ushort AppSpec1; // reserved commonly zero
        public ushort AppSpec2; // reserved commonly zero
        public uint PixelOffset;    // offset of pixel data

        public override string ToString()
        {
            return string.Format("Identification: {0}\n" +
                "File Size: {1}\n" +
                "App Spec Reserved1: {2}\n" +
                "App Spec Reserved2: {3}\n" +
                "Pixel Offset: {4}\n",
                Encoding.UTF8.GetString(ID),
                FileSize,
                AppSpec1,
                AppSpec2,
                PixelOffset
               );
        }

        public bool IsValidBmp() => ID[0] == 0x42 && ID[1] == 0x4d;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BMPInfoHeader
    {
        public uint Size;   // Size of the info header
        public int Width; // width of bitmap in pixels
        public int Height;    // height of bitmap in pixels
        public ushort Planes;   // No. of planes for the target device, always 1
        public ushort BitCount; // No. of bits per pixels
        public uint Compression;
        public uint SizeImage;    // size of the image data
        public int PixelsPerMeterInX; // ppm in x
        public int PixelsPerMeterInY; // ppm in y
        public uint ColorsUsed;   // No. color indexes in the color table. use 0 for max number
        public uint ColorsImportant;  // No. of colors used for displaying the bitmap

        public override string ToString()
        {
            return string.Format("Header Size: {0}\nPixel Width: {1}\nPixel Height: {2}\n" +
                "Planes: {3}\nBitCount: {4}\nCompression: {5}\n" +
                "Size Image: {6}\nPixelsPerMeterInX: {7}\nPixelsPerMeterInX: {8}\n" +
                "ColorsUsed: {8}\nColorsImportant: {9}",
                Size,
                Width,
                Height,
                Planes,
                BitCount,
                Compression,
                SizeImage,
                PixelsPerMeterInX,
                PixelsPerMeterInY,
                ColorsUsed,
                ColorsImportant
                );
        }
    }

    public class BMP
    {
        public static BMPFileHeader GetBMPFileHeader(byte[] buffer)
        {
            return Utility.ByteArrayToStruct<BMPFileHeader>(buffer);
        }

        public static BMPFileHeader GetBMPFileHeader(string path)
        {
            var bytes = File.ReadAllBytes(path);
            return GetBMPFileHeader(bytes);
        }

        public static BMPInfoHeader GetBMPInfoHeader(byte[] buffer)
        {
            return Utility.ByteArrayToStruct<BMPInfoHeader>(buffer);
        }

        public static BMPInfoHeader GetBMPInfoHeader(string path)
        {
            var bytes = File.ReadAllBytes(path);
            var bmpFileHeader = GetBMPFileHeader(bytes);
            if (!bmpFileHeader.IsValidBmp())
            {
                return new BMPInfoHeader();
            }

            int offset = Marshal.SizeOf(bmpFileHeader);
            var buffer = new byte[bytes.Length - offset];
            Array.Copy(bytes, offset, buffer, 0, buffer.Length);
            return GetBMPInfoHeader(buffer);
        }
    }
}
