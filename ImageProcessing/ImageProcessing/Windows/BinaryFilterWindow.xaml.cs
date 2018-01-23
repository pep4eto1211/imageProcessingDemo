using ImageProcessing.AbstractClasses;
using ImageProcessing.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Shapes;

namespace ImageProcessing.Windows
{
    /// <summary>
    /// Interaction logic for BinaryFilterWindow.xaml
    /// </summary>
    public partial class BinaryFilterWindow : Window
    {
        private Bitmap _bitmap;
        private bool _isImageColor = true;
        private byte _value;

        public bool IsImageColor { get => _isImageColor; set => _isImageColor = value; }
        public byte Value { get => _value; set => _value = value; }

        public BinaryFilterWindow(Bitmap bitmap)
        {
            InitializeComponent();
            this._bitmap = bitmap;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await UpdateInterface();
        }

        private async void binaryValueSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.valueBox != null)
            {
                await UpdateInterface();
            }
        }

        private async Task UpdateInterface()
        {
            this.valueBox.Text = binaryValueSlider.Value.ToString();
            Value = (byte)binaryValueSlider.Value;
            ImageFilter filter = new BinaryFilter(Value, this.IsImageColor);
            Bitmap result = await filter.Process(this._bitmap);
            this.previewImage.Source = BitmapToImageSource(result);
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
            this.DialogResult = true;
            this.Close();
        }
    }
}
