using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessing.AbstractClasses;
using ImageProcessing.Helperes;

namespace ImageProcessing.Filters
{
    public class BoxBlurFilter : ImageFilter
    {
        private const int _KernelDimentinSize = 9;

        /*
         * 0 0 | 0 1 | 0 2
         * 1 0 | 1 1 | 1 2
         * 2 0 | 2 1 | 2 2
         * 
         * ---------------
         * 
         * -1 -1 | -1 0 | -1 1
         * 0 -1  | 0 0  | 0 1
         * 1 -1  | 1 0  | 1 1
        */

        private byte[,] _kernel = new byte[9, 9]   { { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                                     { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                                     { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                                     { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                                     { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                                     { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                                     { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                                     { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                                     { 1, 1, 1, 1, 1, 1, 1, 1, 1} };

        private int _Divider
        {
            get
            {
                int result = 0;
                for (int i = 0; i < _KernelDimentinSize; i++)
                {
                    for (int j = 0; j < _KernelDimentinSize; j++)
                    {
                        result += _kernel[i, j];
                    }
                }

                return result;
            }
        }

        protected override void ApplyFilterToPixels(ImageByteData image, int x, int y, int endx, int endy)
        {
            Parallel.For(y, endy, (indexY) =>
            {
                Parallel.For(x, endx, (indexX) =>
                {
                    PixelByteData pixel = image.GetPixel(indexX, indexY);
                    pixel = CalculateConvolutedValue(indexX, indexY, endx, endy, image);
                    image.SetPixel(indexX, indexY, pixel);
                });
            });
        }

        private PixelByteData CalculateConvolutedValue(int indexX, int indexY, int endX, int endY, ImageByteData image)
        {
            int red = 0;
            int green = 0;
            int blue = 0;

            for (int i = 0; i < _KernelDimentinSize; i++)
            {
                for (int j = 0; j < _KernelDimentinSize; j++)
                {
                    int currentY = (i - _KernelDimentinSize / 2) + indexY;
                    int currentX = (j - _KernelDimentinSize / 2) + indexX;
                    if (((currentX < 0) || (currentY < 0)) || ((currentX > endX - 1) || (currentY > endY - 1)))
                    {
                        red += 0;
                        green += 0;
                        blue += 0;
                    }
                    else
                    {
                        red += image.GetPixel(currentX, currentY).R * this._kernel[i, j];
                        green += image.GetPixel(currentX, currentY).G * this._kernel[i, j];
                        blue += image.GetPixel(currentX, currentY).B * this._kernel[i, j];
                    }
                }
            }
            
            return new PixelByteData((byte)(red / _Divider), (byte)(green / _Divider), (byte)(blue / _Divider));
        }
    }
}
