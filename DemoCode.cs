//Grayscale filter
protected override ImageByteData ApplyFilterToPixels(ImageByteData image, int x, int y, int endx, int endy)
        {
            ImageByteData newImage = new ImageByteData(image.Data.ToArray(), image.Width, image.Depth);

            Parallel.For(x, endx, (indexX) =>
            {
                Parallel.For(y, endy, (indexY) =>
                {
                    PixelByteData pixel = image.GetPixel(indexX, indexY);
                    byte gray = (byte)(pixel.R * 0.21 + pixel.G * 0.72 + pixel.B * 0.07);
                    pixel.R = gray;
                    pixel.G = gray;
                    pixel.B = gray;

                    newImage.SetPixel(indexX, indexY, pixel);
                });
            });

            return newImage;
        }

//Kernel convolution
protected override ImageByteData ApplyFilterToPixels(ImageByteData image, int x, int y, int endx, int endy)
        {
            ImageByteData newImage = new ImageByteData(image.Data.ToArray(), image.Width, image.Depth);

            Parallel.For(y, endy, (indexY) =>
            {
                Parallel.For(x, endx, (indexX) =>
                {
                    PixelByteData pixel = image.GetPixel(indexX, indexY);
                    pixel = CalculateConvolutedValue(indexX, indexY, endx, endy, image);
                    newImage.SetPixel(indexX, indexY, pixel);
                });
            });

            return newImage;
        }

//Blur filter
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

//Sobel edge detection
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

//Binary filter
protected override ImageByteData ApplyFilterToPixels(ImageByteData image, int x, int y, int endx, int endy)
        {
            ImageByteData newImage = new ImageByteData(image.Data.ToArray(), image.Width, image.Depth);

            Parallel.For(x, endx, (indexX) =>
            {
                Parallel.For(y, endy, (indexY) =>
                {
                    PixelByteData pixel = image.GetPixel(indexX, indexY);
                    byte intensity = 0;

                    if (this._isImageInColor)
                    {
                        intensity = (byte)(pixel.R * 0.21 + pixel.G * 0.72 + pixel.B * 0.07);
                    }
                    else
                    {
                        intensity = pixel.R;
                    }

                    if (intensity < this._threshold)
                    {
                        pixel.R = 0;
                        pixel.G = 0;
                        pixel.B = 0;
                    }
                    else
                    {
                        pixel.R = 255;
                        pixel.G = 255;
                        pixel.B = 255;
                    }

                    newImage.SetPixel(indexX, indexY, pixel);
                });
            });

            return newImage;
        }