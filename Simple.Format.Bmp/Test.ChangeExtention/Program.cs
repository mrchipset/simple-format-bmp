using System;
using System.IO;

namespace Test.ChangeExtention
{
    class Program
    {
        static readonly string assetsName = "sample_256_126.bmp";
        static readonly string copyName = "sample_256_126.jpg";
        static void Main(string[] args)
        {
            Console.WriteLine("Change the extension from bmp to jpeg!");
            if (File.Exists(copyName))
            {
                File.Delete(copyName);
            }

            File.Copy(assetsName, copyName);
        }
    }
}
