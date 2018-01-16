using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageProcessing.Helperes
{
    public class ImageByteData
    {
        private byte[] _data;
        private int _width;
        private int _depth;

        public ImageByteData(byte[] data, int width, int depth)
        {
            this._data = data;
            this._width = width;
            this._depth = depth;
        }

        public byte[] Data { get => _data; set => _data = value; }
        public int Width { get => _width; set => _width = value; }
        public int Depth { get => _depth; set => _depth = value; }

        public PixelByteData GetPixel(int x, int y)
        {
            var offset = ((y * this._width) + x) * this._depth;

            PixelByteData pixelData = new PixelByteData(_data[offset + 0], _data[offset + 1], _data[offset + 2]);
            return pixelData;
        }

        public void SetPixel(int x, int y, PixelByteData pixel)
        {
            var offset = ((y * this._width) + x) * this._depth;

            _data[offset + 0] = pixel.R;
            _data[offset + 1] = pixel.G;
            _data[offset + 2] = pixel.B;
        }
    }
}
