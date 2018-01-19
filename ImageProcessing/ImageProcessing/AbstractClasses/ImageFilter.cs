using ImageProcessing.Helperes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.AbstractClasses
{
    public abstract class ImageFilter
    {
        public async Task<Bitmap> Process(Bitmap image)
        {
            Bitmap processedImage = new Bitmap(image);

            var rect = new Rectangle(0, 0, processedImage.Width, processedImage.Height);
            var data = processedImage.LockBits(rect, ImageLockMode.ReadWrite, processedImage.PixelFormat);
            var depth = Bitmap.GetPixelFormatSize(data.PixelFormat) / 8; //bytes per pixel

            var buffer = new byte[data.Width * data.Height * depth];

            //copy pixels to buffer
            Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);

            await Task.Run(() =>
            {
                ImageByteData oldImageData = new ImageByteData(buffer, data.Width, depth);

                ImageByteData processedImageData = ApplyFilterToPixels(oldImageData, 0, 0, data.Width, data.Height);

                //Copy the buffer back to image
                Marshal.Copy(processedImageData.Data, 0, data.Scan0, buffer.Length);
            });
            
            processedImage.UnlockBits(data);

            return processedImage;
        }

        protected abstract ImageByteData ApplyFilterToPixels(ImageByteData image, int x, int y, int endx, int endy);
    }
}
