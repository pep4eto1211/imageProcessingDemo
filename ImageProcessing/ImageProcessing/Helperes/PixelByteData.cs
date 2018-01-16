using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageProcessing.Helperes
{
    public class PixelByteData
    {
        private byte _r;
        private byte _g;
        private byte _b;

        public PixelByteData(byte r, byte g, byte b)
        {
            this._r = r;
            this._g = g;
            this._b = b;
        }

        public byte R { get => _r; set => _r = value; }
        public byte G { get => _g; set => _g = value; }
        public byte B { get => _b; set => _b = value; }
    }
}
