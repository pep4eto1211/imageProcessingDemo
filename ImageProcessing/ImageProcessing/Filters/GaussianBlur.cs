using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageProcessing.Filters
{
    public class GaussianBlurFilter : BlurFilter
    {
        protected override int _KernelDimentinSize
        {
            get
            {
                return 3;
            }
        }

        protected override int[,] _Kernel
        {
            get
            {
                return new int[3, 3]   { { 1, 2, 1},
                                          { 2, 4, 2},
                                          { 1, 2, 1} };
            }
        }
    }
}
