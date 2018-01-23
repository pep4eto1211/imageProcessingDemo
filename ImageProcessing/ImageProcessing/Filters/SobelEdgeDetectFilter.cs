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
    }
}
