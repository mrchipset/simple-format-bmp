using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Simple.Format
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BMPFileHeader
    {
        /// <summary>
        /// BMP Identification code "BM"
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] ID;
        /// <summary>
        /// the size of bmp file
        /// </summary>
        public uint FileSize;
        /// <summary>
        /// reserved commonly zero
        /// </summary>
        public ushort AppSpec1;
        /// <summary>
        /// reserved commonly zero
        /// </summary>
        public ushort AppSpec2;
        /// <summary>
        /// offset of pixel data
        /// </summary>
        public uint PixelOffset;

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
        /// <summary>
        /// Size of the info header
        /// </summary>
        public uint Size;
        /// <summary>
        /// width of bitmap in pixels
        /// </summary>
        public int Width;
        /// <summary>
        /// height of bitmap in pixels
        /// </summary>
        public int Height;
        /// <summary>
        /// No. of planes for the target device, always 1
        /// </summary>
        public ushort Planes;
        /// <summary>
        /// No. of bits per pixels
        /// </summary>
        public ushort BitCount;
        public uint Compression;
        /// <summary>
        /// size of the image data
        /// </summary>
        public uint SizeImage;
        /// <summary>
        /// ppm in x
        /// </summary>
        public int PixelsPerMeterInX;
        /// <summary>
        /// ppm in y
        /// </summary>
        public int PixelsPerMeterInY;
        /// <summary>
        /// No. color indexes in the color table. use 0 for max number
        /// </summary>
        public uint ColorsUsed;
        /// <summary>
        /// No. of colors used for displaying the bitmap
        /// </summary>
        public uint ColorsImportant;

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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Pixel
    {
        public byte B;
        public byte G;
        public byte R;

        public override string ToString()
        {
            return $"({B}, {G}, {R})";
        }
    }

    public class BMPFile
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

        public BMPFileHeader FileHeader { get; set; }
        public BMPInfoHeader InfoHeader { get; set; }
        public Pixel[,] Pixels { get; set; }
        public BMPFile(string path)
        {
            var bytes = File.ReadAllBytes(path);
            FileHeader = GetBMPFileHeader(bytes);
            if (!FileHeader.IsValidBmp())
            {
                throw new InvalidOperationException("Not a valid BMP file");
            }

            int offset = Marshal.SizeOf(FileHeader);
            byte[] buffer = new byte[bytes.Length - offset];
            Array.Copy(bytes, offset, buffer, 0, buffer.Length);
            InfoHeader = GetBMPInfoHeader(buffer);

            // Parse the pixel data
            byte[] pixelBuffer = new byte[InfoHeader.SizeImage];
            Array.Copy(bytes, FileHeader.PixelOffset, pixelBuffer, 0, InfoHeader.SizeImage);
            Pixels = new Pixel[InfoHeader.Height, InfoHeader.Width];
            uint pixelSize = 3;
            uint rowStride = (uint)(pixelSize * InfoHeader.Width);
            for (int row = 0; row < InfoHeader.Height; ++row)
            {
                for (int col = 0; col < InfoHeader.Width; ++col)
                {
                    uint pxOffset = (uint)(row * rowStride + col * pixelSize);
                    Pixels[row, col].B = pixelBuffer[pxOffset];
                    Pixels[row, col].G = pixelBuffer[pxOffset + 1];
                    Pixels[row, col].R = pixelBuffer[pxOffset + 2];
                }
            }
        }
    }
}
