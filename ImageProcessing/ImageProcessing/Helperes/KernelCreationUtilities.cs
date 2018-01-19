using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageProcessing.Helperes
{
    public static class KernelCreationUtilities
    {
        public static byte[,] CalculateGaussianKernel(int length, double weight)
        {
            byte[,] kernel = new byte[length, length];
            double sumTotal = 0;
            
            int kernelRadius = length / 2;
            double distance = 0;
            
            double calculatedEuler = 1.0 / (2.0 * Math.PI * Math.Pow(weight, 2));
            
            for (int filterY = -kernelRadius; filterY <= kernelRadius; filterY++)
            {
                for (int filterX = -kernelRadius; filterX <= kernelRadius; filterX++)
                {
                    distance = ((filterX * filterX) + (filterY * filterY)) / (2 * (weight * weight));
                    kernel[filterY + kernelRadius, filterX + kernelRadius] = (byte)(calculatedEuler * Math.Exp(-distance));
                    sumTotal += kernel[filterY + kernelRadius, filterX + kernelRadius];
                }
            }
            
            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    kernel[y, x] = (byte)(kernel[y, x] * (1.0 / sumTotal));
                }
            }


            return kernel;
        }
    }
}
