using ImageProcessing.AbstractClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageProcessing.Helperes;
using System.Threading.Tasks;

namespace ImageProcessing.Filters
{
    public class BinaryFilter : ImageFilter
    {
        private byte _threshold;
        private bool _isImageInColor;

        public BinaryFilter(byte threshold, bool isImageInColor)
        {
            this._threshold = threshold;
            this._isImageInColor = isImageInColor;
        }

        public BinaryFilter()
        {
            this._threshold = 128;
            this._isImageInColor = true;
        }

        protected override ImageByteData ApplyFilterToPixels(ImageByteData image, int x, int y, int endx, int endy)
        {
            ImageByteData newImage = new ImageByteData(image.Data.ToArray(), image.Width, image.Depth);

            Parallel.For(x, endx, (indexX) =>
            {
                Parallel.For(y, endy, (indexY) =>
                {
                    PixelByteData pixel = image.GetPixel(indexX, indexY);
                    byte intensity = 0;

                    if (this._isImageInColor)
                    {
                        intensity = (byte)(pixel.R * 0.21 + pixel.G * 0.72 + pixel.B * 0.07);
                    }
                    else
                    {
                        intensity = pixel.R;
                    }

                    if (intensity < this._threshold)
                    {
                        pixel.R = 0;
                        pixel.G = 0;
                        pixel.B = 0;
                    }
                    else
                    {
                        pixel.R = 255;
                        pixel.G = 255;
                        pixel.B = 255;
                    }

                    newImage.SetPixel(indexX, indexY, pixel);
                });
            });

            return newImage;
        }
    }
}
