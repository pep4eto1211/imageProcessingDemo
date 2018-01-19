using ImageProcessing.AbstractClasses;
using ImageProcessing.Enums;
using ImageProcessing.Filters;
using ImageProcessing.Helperes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
using System.Windows.Threading;

namespace ImageProcessing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bitmap _bitmap;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                string imageFileName = fileDialog.FileName;
                Bitmap bitmap = Bitmap.FromFile(imageFileName) as Bitmap;

                processedImage.Source = BitmapToImageSource(bitmap);

                this._bitmap = bitmap;

                this.applyEffectButton.IsEnabled = true;
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.inProgresProgressBar.Visibility = Visibility.Visible;
            this.statusTextBlock.Visibility = Visibility.Visible;

            //ImageFilter filter = new SobelEdgeDetectFilter(SobelDirection.Horizontal, true);
            //Bitmap resultBitmap = await filter.Process(_bitmap);
            //processedImage.Source = BitmapToImageSource(resultBitmap);

            FilterQueue filterQueue = new FilterQueue(_bitmap);
            filterQueue.Push(new GrayscaleFilter());
            filterQueue.Push(new GaussianBlurFilter());
            filterQueue.Push(new SobelEdgeDetectFilter(SobelDirection.Horizontal, false));
            filterQueue.Push(new BinaryFilter(10, false));
            await filterQueue.ApplyEffects();
            processedImage.Source = BitmapToImageSource(filterQueue.Image);

            this.inProgresProgressBar.Visibility = Visibility.Collapsed;
            this.statusTextBlock.Visibility = Visibility.Collapsed;
        }
    }
}
