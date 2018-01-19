using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessing.AbstractClasses;
using ImageProcessing.Helperes;

namespace ImageProcessing.Filters
{
    public class BoxBlurFilter : BlurFilter
    {
        protected override int _KernelDimentinSize
        {
            get
            {
                return 9;
            }
        }
        
        protected override int[,] _Kernel
        {
            get
            {
                return new int[9, 9]   { { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                            { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                            { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                            { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                            { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                            { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                            { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                            { 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                            { 1, 1, 1, 1, 1, 1, 1, 1, 1} };
            }
        }
    }
}
