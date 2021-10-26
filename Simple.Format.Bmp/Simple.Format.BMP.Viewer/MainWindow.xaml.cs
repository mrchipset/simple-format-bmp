using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Simple.Format;

namespace Simple.Format.BMP.Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Bitmap File (*.bmp)|*.bmp";
            if (openFileDialog.ShowDialog() == true)
            {
                Path.Text = openFileDialog.FileName;
                OpenImage(openFileDialog.FileName);
                DrawImageOnCanvas(openFileDialog.FileName);
            }
        }

        private void OpenImage(string path)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(path);
            bitmap.EndInit();
            WPFImage.Source = bitmap;
        }

        private void DrawImageOnCanvas(string path)
        {
            BMPFile bmp = new BMPFile(path);
            Pixel[,] pixels = bmp.Pixels;
            WriteableBitmap writeableBitmap =
                new WriteableBitmap(
                    bmp.InfoHeader.Width,
                    bmp.InfoHeader.Height,
                    96,
                    96,
                    PixelFormats.Bgr24,
                    null
                );

            unsafe
            {
                byte* pPixmap = (byte*)writeableBitmap.BackBuffer.ToPointer();
                uint pixelSize = 3;
                writeableBitmap.Lock();
                for (int row = 0; row < writeableBitmap.Height; ++row)
                {
                    for (int col = 0; col < writeableBitmap.Width; ++col)
                    {
                        Pixel pixel = pixels[row, col];
                        pPixmap[row * writeableBitmap.BackBufferStride + col * pixelSize] = pixel.B;
                        pPixmap[row * writeableBitmap.BackBufferStride + col * pixelSize + 1] = pixel.G;
                        pPixmap[row * writeableBitmap.BackBufferStride + col * pixelSize + 2] = pixel.R;

                    }
                }
                writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight));
                writeableBitmap.Unlock();
            }
            WPFCanvas.Source = writeableBitmap;
        }
    }
}
