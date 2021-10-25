using System;
using System.IO;
using Simple.Format;

namespace Test.ReadBMPHeader
{
    class Program
    {
        static readonly string assetsName = "sample_256_126.bmp";
        static readonly string copyName = "sample_256_126_copy.bmp";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Read BMP Header!");
            try
            {
                if (File.Exists(copyName))
                {
                    File.Delete(copyName);
                }
                File.Copy(assetsName, copyName);
                var bmpFileHeader = BMP.GetBMPFileHeader(copyName);
                var bmpInfoHeader = BMP.GetBMPInfoHeader(copyName);
                Console.WriteLine(bmpFileHeader.ToString());
                Console.WriteLine(bmpInfoHeader.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }
        }
    }
}
