using ImageProcessing.AbstractClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessing.Helperes;

namespace ImageProcessing.Filters
{
    public abstract class KernelConvolutionFilter : ImageFilter
    {
        protected abstract int[,] _Kernel
        {
            get;
        }

        protected abstract int _KernelDimentinSize
        {
            get;
        }

        protected int _Divider
        {
            get
            {
                int result = 0;
                for (int i = 0; i < _KernelDimentinSize; i++)
                {
                    for (int j = 0; j < _KernelDimentinSize; j++)
                    {
                        result += _Kernel[i, j];
                    }
                }

                return result;
            }
        }
        
        protected virtual PixelByteData CalculateConvolutedValue(int indexX, int indexY, int endX, int endY, ImageByteData image)
        {
            return null;
        }

        protected void GetImageCoordinatesFromKernelCoordinates(out int imageX, out int imageY, int kernelI, int kernelJ, int originX, int originY)
        {
            imageY = (kernelI - _KernelDimentinSize / 2) + originY;
            imageX = (kernelJ - _KernelDimentinSize / 2) + originX;
        }
    }
}
