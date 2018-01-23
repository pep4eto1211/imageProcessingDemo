using ImageProcessing.AbstractClasses;
using ImageProcessing.Enums;
using ImageProcessing.Filters;
using ImageProcessing.Helperes;
using ImageProcessing.Windows;
using Microsoft.Win32;
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
            filterQueue.Push(new BinaryFilter(80, false));
            await filterQueue.ApplyEffects();
            processedImage.Source = BitmapToImageSource(filterQueue.Image);

            this.inProgresProgressBar.Visibility = Visibility.Collapsed;
            this.statusTextBlock.Visibility = Visibility.Collapsed;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                string imageFileName = fileDialog.FileName;
                Bitmap bitmap = Bitmap.FromFile(imageFileName) as Bitmap;

                processedImage.Source = BitmapToImageSource(bitmap);

                this._bitmap = bitmap;

                this.filtersMenuItem.IsEnabled = true;
                this.saveFileMenuItem.IsEnabled = true;
            }
        }

        private async void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            await ApplyFilter(new GrayscaleFilter());
        }

        private async void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            await ApplyFilter(new BoxBlurFilter());
        }

        private async void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            await ApplyFilter(new GaussianBlurFilter());
        }

        private async Task ApplyFilter(ImageFilter filter)
        {
            this.inProgresProgressBar.Visibility = Visibility.Visible;
            this.statusTextBlock.Visibility = Visibility.Visible;
            this.statusTextBlock.Text = $"Applying {filter.FilterName} filter";
            
            _bitmap = await filter.Process(_bitmap);
            processedImage.Source = BitmapToImageSource(_bitmap);

            this.inProgresProgressBar.Visibility = Visibility.Collapsed;
            this.statusTextBlock.Visibility = Visibility.Collapsed;
        }

        private async void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            BinaryFilterWindow binaryFilterWindow = new BinaryFilterWindow(this._bitmap);
            if (binaryFilterWindow.ShowDialog() == true)
            {
                byte value = binaryFilterWindow.Value;
                bool isImageColor = binaryFilterWindow.IsImageColor;

                await ApplyFilter(new BinaryFilter(value, isImageColor));
            }
        }

        private async void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            await ApplyFilter(new SobelEdgeDetectFilter(SobelDirection.Both, false));
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JPEG Files|*.jpeg";
            if (saveFileDialog.ShowDialog() == true)
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                String photolocation = saveFileDialog.FileName;

                encoder.Frames.Add(BitmapFrame.Create((BitmapImage)processedImage.Source));

                using (var filestream = new FileStream(photolocation, FileMode.Create))
                {
                    encoder.Save(filestream);
                }
            }
        }
    }
}
