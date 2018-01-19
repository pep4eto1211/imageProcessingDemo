using ImageProcessing.AbstractClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageProcessing.Helperes;
using System.Threading.Tasks;

namespace ImageProcessing.Filters
{
    public abstract class BlurFilter : KernelConvolutionFilter
    {
        protected override PixelByteData CalculateConvolutedValue(int indexX, int indexY, int endX, int endY, ImageByteData image)
        {
            int red = 0;
            int green = 0;
            int blue = 0;

            int finalDivider = _Divider;

            for (int i = 0; i < _KernelDimentinSize; i++)
            {
                for (int j = 0; j < _KernelDimentinSize; j++)
                {
                    GetImageCoordinatesFromKernelCoordinates(out int currentX, out int currentY, i, j, indexX, indexY);
                    if (((currentX < 0) || (currentY < 0)) || ((currentX > endX - 1) || (currentY > endY - 1)))
                    {
                        //red += 0;
                        //green += 0;
                        //blue += 0;

                        finalDivider -= this._Kernel[i, j];
                    }
                    else
                    {
                        red += image.GetPixel(currentX, currentY).R * this._Kernel[i, j];
                        green += image.GetPixel(currentX, currentY).G * this._Kernel[i, j];
                        blue += image.GetPixel(currentX, currentY).B * this._Kernel[i, j];
                    }
                }
            }

            return new PixelByteData((byte)(red / finalDivider), (byte)(green / finalDivider), (byte)(blue / finalDivider));
        }
    }
}
