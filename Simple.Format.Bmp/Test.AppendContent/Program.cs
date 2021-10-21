using System;
using System.IO;

namespace Test.AppendContent
{
    class Program
    {
        static readonly string assetsName = "sample_256_126.bmp";
        static readonly string copyName = "sample_256_126_append.bmp";
        static void Main(string[] args)
        {
            Console.WriteLine("Append Hello World to bmp data!");
            try
            {
                if (File.Exists(copyName))
                {
                    File.Delete(copyName);
                }
                File.Copy(assetsName, copyName);
                StreamWriter streamWriter = File.AppendText(copyName);
                for (int i = 0; i < 1000; ++i)
                    streamWriter.WriteLine("Hello World!");
                streamWriter.Close();
            } catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }
        }
    }
}
