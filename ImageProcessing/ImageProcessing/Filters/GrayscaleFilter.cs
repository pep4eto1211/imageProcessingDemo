using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ImageProcessing.AbstractClasses;
using ImageProcessing.Helperes;

namespace ImageProcessing.Filters
{
    public class GrayscaleFilter : ImageFilter
    {
        #region Helper methods
        protected override ImageByteData ApplyFilterToPixels(ImageByteData image, int x, int y, int endx, int endy)
        {
            ImageByteData newImage = new ImageByteData(image.Data.ToArray(), image.Width, image.Depth);

            Parallel.For(x, endx, (indexX) =>
            {
                Parallel.For(y, endy, (indexY) =>
                {
                    PixelByteData pixel = image.GetPixel(indexX, indexY);
                    byte gray = (byte)(pixel.R * 0.21 + pixel.G * 0.72 + pixel.B * 0.07);
                    pixel.R = gray;
                    pixel.G = gray;
                    pixel.B = gray;

                    image.SetPixel(indexX, indexY, pixel);
                });
            });

            return newImage;
        }
        #endregion
    }
}
