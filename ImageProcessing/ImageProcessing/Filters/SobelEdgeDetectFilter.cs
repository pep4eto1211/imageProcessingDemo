using ImageProcessing.AbstractClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageProcessing.Helperes;
using System.Threading.Tasks;
using ImageProcessing.Enums;

namespace ImageProcessing.Filters
{
    public class SobelEdgeDetectFilter : KernelConvolutionFilter
    {
        private int[,] _kernel;
        private bool _isImageInColor;

        public SobelEdgeDetectFilter(SobelDirection direction, bool isImageInColor)
        {
            this._isImageInColor = isImageInColor;

            switch (direction)
            {
                case SobelDirection.Vertical:
                    this._kernel = new int[3, 3] { { -1, 0, 1},
                                               { -2, 0, 2},
                                               { -1, 0, 1} };
                    break;
                case SobelDirection.Horizontal:
                    this._kernel = new int[3, 3] { { -1, -2, -1},
                                               { 0, 0, 0},
                                               { 1, 2, 1} };
                    break;
                case SobelDirection.Both:
                    this._kernel = new int[3, 3] { { -1, -1, -1},
                                               { -1, 8, -1},
                                               { -1, -1, -1} };
                    break;
            }
        }

        protected override int[,] _Kernel
        {
            get
            {
                return this._kernel;
            }
        }

        protected override int _KernelDimentinSize
        {
            get
            {
                return 3;
            }
        }

        protected override PixelByteData CalculateConvolutedValue(int indexX, int indexY, int endx, int endy, ImageByteData image)
        {
            int newColorValue = 0;

            for (int i = 0; i < _KernelDimentinSize; i++)
            {
                for (int j = 0; j < _KernelDimentinSize; j++)
                {
                    byte intensity = 0;
                    GetImageCoordinatesFromKernelCoordinates(out int currentX, out int currentY, i, j, indexX, indexY);
                    if (((currentX < 0) || (currentY < 0)) || ((currentX > endx - 1) || (currentY > endy - 1)))
                    {
                        newColorValue += 0;
                    }
                    else
                    {
                        PixelByteData pixel = image.GetPixel(currentX, currentY);

                        if (this._isImageInColor)
                        {
                            intensity = (byte)(pixel.R * 0.21 + pixel.G * 0.72 + pixel.B * 0.07);
                        }
                        else
                        {
                            intensity = pixel.R;
                        }

                        newColorValue += intensity * this._Kernel[i, j];
                    }
                }
            }

            int bigValue = newColorValue;
            newColorValue = Math.Abs(newColorValue);
            double newColorDouble = newColorValue / 4;
            newColorValue = (int)Math.Round(newColorDouble);

            PixelByteData newPixel = new PixelByteData((byte)(newColorValue), (byte)(newColorValue), (byte)(newColorValue));
            return newPixel;
        }
    }
}
