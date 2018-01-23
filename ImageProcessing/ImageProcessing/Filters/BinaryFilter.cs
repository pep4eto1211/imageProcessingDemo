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

        public override string FilterName => "Binary";

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
    }
}
