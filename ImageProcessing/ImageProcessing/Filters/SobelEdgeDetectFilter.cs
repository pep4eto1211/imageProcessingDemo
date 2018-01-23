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
        private bool _isDirectionBoth = false;

        private int[,] _verticalEdgeKernel = new int[3, 3] { { -1, 0, 1},
                                               { -2, 0, 2},
                                               { -1, 0, 1} };

        private int[,] _horizontalEdgeKernel = new int[3, 3] { { -1, -2, -1},
                                               { 0, 0, 0},
                                               { 1, 2, 1} };

        public SobelEdgeDetectFilter(SobelDirection direction, bool isImageInColor)
        {
            this._isImageInColor = isImageInColor;

            switch (direction)
            {
                case SobelDirection.Vertical:
                    this._kernel = _verticalEdgeKernel;
                    break;
                case SobelDirection.Horizontal:
                    this._kernel = _horizontalEdgeKernel;
                    break;
                case SobelDirection.Both:
                    this._isDirectionBoth = true;
                    break;
            }
        }

        public override string FilterName => "Sobel Edge Detection";

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
            int newColorValueVertical = 0;
            int newColorValueHorizontal = 0;

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

                        if (!_isDirectionBoth)
                        {
                            newColorValue += intensity * this._Kernel[i, j]; 
                        }
                        else
                        {
                            newColorValueHorizontal += intensity * this._horizontalEdgeKernel[i, j];
                            newColorValueVertical += intensity * this._verticalEdgeKernel[i, j];
                        }
                    }
                }
            }

            if (!_isDirectionBoth)
            {
                int bigValue = newColorValue;
                newColorValue = Math.Abs(newColorValue);

                if (newColorValue > 255)
                {
                    newColorValue = 255;
                }

                PixelByteData newPixel = new PixelByteData((byte)(newColorValue), (byte)(newColorValue), (byte)(newColorValue));
                return newPixel; 
            }
            else
            {
                int bigValue = (int)Math.Round(Math.Sqrt((newColorValueHorizontal * newColorValueHorizontal) + (newColorValueVertical * newColorValueVertical)));
                if (bigValue > 255)
                {
                    bigValue = 255;
                }

                PixelByteData newPixel = new PixelByteData((byte)(bigValue), (byte)(bigValue), (byte)(bigValue));
                return newPixel;
            }
        }
    }
}
