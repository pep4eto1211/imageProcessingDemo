using ImageProcessing.AbstractClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing.Helperes
{
    public class FilterQueue
    {
        private Queue<ImageFilter> _filters;
        private Bitmap _image;

        public FilterQueue(Bitmap image)
        {
            this._image = image;
            this._filters = new Queue<ImageFilter>();
        }

        public void Push(ImageFilter filter)
        {
            this._filters.Enqueue(filter);
        }

        public async Task ApplyEffects()
        {
            while (this._filters.Count > 0)
            {
                ImageFilter filter = this._filters.Dequeue();
                this._image = await filter.Process(this._image);
            }
        }

        public Bitmap Image
        {
            get => _image; set => _image = value;
        }
    }
}
