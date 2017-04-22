using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MMS_Lab_1.Models
{
    public class Model : IModel, IUndoRedo
    {
        Bitmap image;
        Bitmap channelX;
        Bitmap channelY;
        Bitmap channelZ;
        SortedDictionary<int, int> histogramX;
        SortedDictionary<int, int> histogramY;
        SortedDictionary<int, int> histogramZ;

        Object objLock;

        Config config;

        CustomStack<Bitmap> undoStack;
        CustomStack<Bitmap> redoStack;

        byte[] customImageXData, customImageYData, customImageZData;
        byte[] customImageData;

        byte downsamplingMethod = 0;

        public Model()
        {
            histogramX = new SortedDictionary<int, int>();
            histogramY = new SortedDictionary<int, int>();
            histogramZ = new SortedDictionary<int, int>();
            config = new Config();
            config.LoadConfig("Config.ini");
            undoStack = new CustomStack<Bitmap>();
            redoStack = new CustomStack<Bitmap>();
            undoStack.Capacity = redoStack.Capacity = config.StackCapacity;
            objLock = new Object();
        }

        public Bitmap GetImage()
        {
            return image;
        }

        public Bitmap InvertSafe()
        {

            if (image != null)
            {
                lock (objLock)
                {
                    undoStack.Push(new Bitmap(image));
                    redoStack.Clear();
                    channelX = channelY = channelZ = null;
                }
                for (int i = 0; i < image.Width; i++)
                {
                    for (int j = 0; j < image.Height; j++)
                    {
                        lock (objLock)
                        {
                            Color pixel = image.GetPixel(i, j);

                            image.SetPixel(i, j, Color.FromArgb(pixel.A, 255 - pixel.R, 255 - pixel.G, 255 - pixel.B));
                        }
                    }
                }

                return image;
            }
            return null;
        }

        public Bitmap InvertUnsafe()
        {
            if (image != null)
            {
                lock (objLock)
                {
                    undoStack.Push(new Bitmap(image));
                    redoStack.Clear();
                    channelX = channelY = channelZ = null;
                }
                BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                int stride = data.Stride;
                IntPtr Scan0 = data.Scan0;

                unsafe
                {
                    byte* p = (byte*)(void*)Scan0;
                    int nOffset = stride - image.Width * 3;
                    int nWidth = image.Width * 3;

                    for (int i = 0; i < image.Height; ++i)
                    {
                        for (int j = 0; j < nWidth; ++j)
                        {
                            p[0] = (byte)(255 - p[0]);
                            ++p;
                        }
                        p += nOffset;
                    }
                }
                image.UnlockBits(data);
                return image;
            }
            return null;
        }

        public Bitmap ColorsSafe(int r, int g, int b)
        {
            if (image != null)
            {
                lock (objLock)
                {
                    undoStack.Push(new Bitmap(image));
                    redoStack.Clear();
                    channelX = channelY = channelZ = null;
                }
                for (int i = 0; i < image.Width; i++)
                {
                    for (int j = 0; j < image.Height; j++)
                    {
                        lock (objLock)
                        {
                            Color pixel = image.GetPixel(i, j);

                            int newR = r + pixel.R;
                            if (newR < 0)
                            {
                                newR = 0;
                            }
                            if (newR > 255)
                            {
                                newR = 255;
                            }
                            int newG = g + pixel.G;
                            if (newG < 0)
                            {
                                newG = 0;
                            }
                            if (newG > 255)
                            {
                                newG = 255;
                            }
                            int newB = b + pixel.B;
                            if (newB < 0)
                            {
                                newB = 0;
                            }
                            if (newB > 255)
                            {
                                newB = 255;
                            }
                            image.SetPixel(i, j, Color.FromArgb(newR, newG, newB));
                        }
                    }
                }
                return image;
            }
            return null;
        }

        public Bitmap ColorsUnsafe(int r, int g, int b)
        {
            if (image != null)
            {
                lock (objLock)
                {
                    undoStack.Push(new Bitmap(image));
                    redoStack.Clear();
                    channelX = channelY = channelZ = null;
                }
                BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                int stride = data.Stride;
                IntPtr Scan0 = data.Scan0;

                unsafe
                {
                    byte* p = (byte*)(void*)Scan0;
                    int nOffset = stride - image.Width * 3;
                    int nWidth = image.Width;
                    int nPixel;

                    for (int i = 0; i < image.Height; ++i)
                    {
                        for (int j = 0; j < nWidth; ++j)
                        {
                            nPixel = p[2] + r;
                            nPixel = Math.Max(nPixel, 0);
                            p[2] = (byte)Math.Min(255, nPixel);

                            nPixel = p[1] + g;
                            nPixel = Math.Max(nPixel, 0);
                            p[1] = (byte)Math.Min(255, nPixel);

                            nPixel = p[2] + b;
                            nPixel = Math.Max(nPixel, 0);
                            p[2] = (byte)Math.Min(255, nPixel);

                            p += 3;
                        }
                        p += nOffset;
                    }
                }
                image.UnlockBits(data);
                return image;
            }
            return null;
        }

        public Bitmap LoadImage(string path)
        {
            try
            {
                lock (objLock)
                {
                    undoStack.Clear();
                    redoStack.Clear();
                    image = (Bitmap)Image.FromFile(path);
                }
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
                image = image.Clone(rect, PixelFormat.Format24bppRgb);
                channelX = null;
                channelY = null;
                channelZ = null;
                histogramX.Clear();
                histogramY.Clear();
                histogramZ.Clear();
                return image;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void SaveImage(string path)
        {
            try
            {
                image.Save(path);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }
        }

        public Bitmap GetChannelX()
        {
            if (image != null)
            {
                if (channelX == null)
                {
                    channelX = new Bitmap(image);
                    for (int i = 0; i < image.Width; i++)
                    {
                        for (int j = 0; j < image.Height; j++)
                        {
                            lock (objLock)
                            {
                                Color pixel = image.GetPixel(i, j);
                                double x = 0, y = 0, z = 0;
                                RGBToXYZ(pixel, out x, out y, out z);
                                if (!histogramX.ContainsKey((int)x))
                                {
                                    histogramX[(int)x] = 0;//TODO: Probaj da iskoristis array za Histogram.ToHistogram
                                }
                                else
                                {
                                    histogramX[(int)x] += 1;
                                }
                                y = 0;
                                z = 0;
                                Color resultingPixel = XYZToRGB(x, y, z);
                                channelX.SetPixel(i, j, resultingPixel);
                            }
                        }
                    }
                    for (int i = 0; i < 96; i++)
                    {
                        //Popunjavanje vrednosti koje nedostaju u histogramu
                        if (!histogramX.ContainsKey(i))
                        {
                            histogramX[i] = 0;
                        }
                    }
                    return channelX;
                }
                return channelX;
            }
            throw new Exception("Image is not set.");
        }

        public Bitmap GetChannelY()
        {
            if (image != null)
            {
                if (channelY == null)
                {
                    channelY = new Bitmap(image);
                    for (int i = 0; i < image.Width; i++)
                    {
                        for (int j = 0; j < image.Height; j++)
                        {
                            lock (objLock)
                            {
                                Color pixel = image.GetPixel(i, j);
                                double x = 0, y = 0, z = 0;
                                RGBToXYZ(pixel, out x, out y, out z);
                                if (!histogramY.ContainsKey((int)y))
                                {
                                    histogramY[(int)y] = 0;
                                }
                                else
                                {
                                    histogramY[(int)y] += 1;
                                }
                                x = 0;
                                z = 0;
                                Color resultingPixel = XYZToRGB(x, y, z);
                                channelY.SetPixel(i, j, resultingPixel);
                            }
                        }
                    }
                    for (int i = 0; i < 101; i++)
                    {
                        if (!histogramY.ContainsKey(i))
                        {
                            histogramY[i] = 0;
                        }
                    }
                    return channelY;
                }
                return channelY;
            }
            throw new Exception("Image is not set.");
        }

        public Bitmap GetChannelZ()
        {
            if (image != null)
            {
                if (channelZ == null)
                {
                    channelZ = new Bitmap(image);
                    for (int i = 0; i < image.Width; i++)
                    {
                        for (int j = 0; j < image.Height; j++)
                        {
                            lock (objLock)
                            {
                                Color pixel = image.GetPixel(i, j);
                                double x = 0, y = 0, z = 0;
                                RGBToXYZ(pixel, out x, out y, out z);
                                if (!histogramZ.ContainsKey((int)z))
                                {
                                    histogramZ[(int)z] = 0;
                                }
                                else
                                {
                                    histogramZ[(int)z] += 1;
                                }
                                x = 0;
                                y = 0;
                                Color resultingPixel = XYZToRGB(x, y, z);
                                channelZ.SetPixel(i, j, resultingPixel);
                            }
                        }
                    }
                    for (int i = 0; i < 109; i++)
                    {
                        if (!histogramZ.ContainsKey(i))
                        {
                            histogramZ[i] = 0;
                        }
                    }
                    return channelZ;
                }
                return channelZ;
            }
            throw new Exception("Image is not set.");
        }

        private void RGBToXYZ(Color rgb, out double x, out double y, out double z)
        {
            double linearR = (rgb.R / 255.0);
            double linearG = (rgb.G / 255.0);
            double linearB = (rgb.B / 255.0);

            if (linearR > 0.04045) linearR = Math.Pow((linearR + 0.055) / 1.055, 2.4);
            else linearR = linearR / 12.92;
            if (linearG > 0.04045) linearG = Math.Pow((linearG + 0.055) / 1.055, 2.4);
            else linearG = linearG / 12.92;
            if (linearB > 0.04045) linearB = Math.Pow((linearB + 0.055) / 1.055, 2.4);
            else linearB = linearB / 12.92;

            linearR *= 100;
            linearG *= 100;
            linearB *= 100;

            x = linearR * 0.4124 + linearG * 0.3576 + linearB * 0.1805;
            y = linearR * 0.2126 + linearG * 0.7152 + linearB * 0.0722;
            z = linearR * 0.0193 + linearG * 0.1192 + linearB * 0.9505;
        }

        private Color XYZToRGB(double x, double y, double z)
        {
            x /= 100;
            y /= 100;
            z /= 100;

            double linearR = x * 3.2406 + y * -1.5372 + z * -0.4986;
            double linearG = x * -0.9689 + y * 1.8758 + z * 0.0415;
            double linearB = x * 0.0557 + y * -0.2040 + z * 1.0570;

            if (linearR > 0.0031308) linearR = 1.055 * Math.Pow(linearR, (1 / 2.4)) - 0.055;
            else linearR = 12.92 * linearR;
            if (linearG > 0.0031308) linearG = 1.055 * Math.Pow(linearG, (1 / 2.4)) - 0.055;
            else linearG = 12.92 * linearG;
            if (linearB > 0.0031308) linearB = 1.055 * Math.Pow(linearB, (1 / 2.4)) - 0.055;
            else linearB = 12.92 * linearB;

            int r = (byte)(linearR * 255);
            int g = (byte)(linearG * 255);
            int b = (byte)(linearB * 255);

            return Color.FromArgb(r, g, b);
        }

        private Bitmap expandImage(Bitmap img, int borderSize)
        {
            Bitmap expandedImage = new Bitmap(img, new Size(img.Width + 2 * borderSize, img.Height + 2 * borderSize));
            Graphics g = Graphics.FromImage(expandedImage);
            g.Clear(Color.FromArgb(127, 127, 127));
            int x = (expandedImage.Width - img.Width) / 2;
            int y = (expandedImage.Height - img.Height) / 2;
            g.DrawImage(img, new Rectangle(x, y, img.Width, img.Height));
            g.Dispose();
            return expandedImage;
        }

        private Bitmap shrinkImage(Bitmap img, int borderSize)
        {
            Bitmap shrunkImage;

            shrunkImage = img.Clone(new Rectangle(borderSize, borderSize, img.Width - 2 * borderSize, img.Height - 2 * borderSize), PixelFormat.Format24bppRgb);

            return shrunkImage;
        }

        public Bitmap MeanFilter3x3()
        {

            if (image != null)
            {
                lock (objLock)
                {
                    undoStack.Push(new Bitmap(image));
                    redoStack.Clear();
                    channelX = channelY = channelZ = null;
                }
                int[] convolutionMatrix = { -1, -1, -1,
                                            -1, 2, -1,
                                            -1, -1, -1 };
                ConvolutionMatrix matrix = new ConvolutionMatrix(convolutionMatrix);
                int mid = matrix.matrix3x3.Length / 2;

                Bitmap bmp = new Bitmap(image);

                bmp = expandImage(bmp, 1);
                Color[] p = new Color[matrix.matrix3x3.Length];
                for (int i = 1; i < bmp.Width - 1; i++)
                {
                    for (int j = 1; j < bmp.Height - 1; j++)
                    {
                        lock (objLock)
                        {
                            p[0] = bmp.GetPixel(i - 1, j - 1);
                            p[1] = bmp.GetPixel(i - 1, j);
                            p[2] = bmp.GetPixel(i - 1, j + 1);

                            p[3] = bmp.GetPixel(i, j - 1);
                            p[4] = bmp.GetPixel(i, j);
                            p[5] = bmp.GetPixel(i, j + 1);

                            p[6] = bmp.GetPixel(i + 1, j - 1);
                            p[7] = bmp.GetPixel(i + 1, j);
                            p[8] = bmp.GetPixel(i + 1, j + 1);

                            byte[] pixelsR = new byte[matrix.matrix3x3.Length];
                            byte[] pixelsG = new byte[matrix.matrix3x3.Length];
                            byte[] pixelsB = new byte[matrix.matrix3x3.Length];

                            for (int k = 0; k < p.Length; k++)
                            {
                                pixelsR[k] = p[k].R;
                                pixelsG[k] = p[k].G;
                                pixelsB[k] = p[k].B;
                            }

                            pixelsR = ApplyConvolution(pixelsR, matrix.matrix3x3, 0);
                            pixelsG = ApplyConvolution(pixelsG, matrix.matrix3x3, 0);
                            pixelsB = ApplyConvolution(pixelsB, matrix.matrix3x3, 0);

                            bmp.SetPixel(i, j, Color.FromArgb(pixelsR[mid], pixelsG[mid], pixelsB[mid]));
                        }

                    }
                }

                bmp = shrinkImage(bmp, 1);
                image = bmp;
                return bmp;
            }
            throw new Exception("Image is not set.");
        }

        public Bitmap MeanFilter5x5()
        {
            if (image != null)
            {
                lock (objLock)
                {
                    undoStack.Push(new Bitmap(image));
                    redoStack.Clear();
                    channelX = channelY = channelZ = null;
                }
                int[] convolutionMatrix = { -1, -1, -1,
                                            -1, 2, -1,
                                            -1, -1, -1 };
                ConvolutionMatrix matrix = new ConvolutionMatrix(convolutionMatrix);
                int mid = matrix.matrix5x5.Length / 2;

                Bitmap bmp = new Bitmap(image);

                bmp = expandImage(bmp, 2);
                Color[] p = new Color[matrix.matrix5x5.Length];
                for (int i = 2; i < bmp.Width - 2; i++)
                {
                    for (int j = 2; j < bmp.Height - 2; j++)
                    {
                        lock (objLock)
                        {
                            p[0] = bmp.GetPixel(i - 2, j - 2);
                            p[1] = bmp.GetPixel(i - 2, j - 1);
                            p[2] = bmp.GetPixel(i - 2, j);
                            p[3] = bmp.GetPixel(i - 2, j + 1);
                            p[4] = bmp.GetPixel(i - 2, j + 2);

                            p[5] = bmp.GetPixel(i - 1, j - 2);
                            p[6] = bmp.GetPixel(i - 1, j - 1);
                            p[7] = bmp.GetPixel(i - 1, j);
                            p[8] = bmp.GetPixel(i - 1, j + 1);
                            p[9] = bmp.GetPixel(i - 1, j + 2);

                            p[10] = bmp.GetPixel(i, j - 2);
                            p[11] = bmp.GetPixel(i, j - 1);
                            p[12] = bmp.GetPixel(i, j);
                            p[13] = bmp.GetPixel(i, j + 1);
                            p[14] = bmp.GetPixel(i, j + 2);

                            p[15] = bmp.GetPixel(i + 1, j - 2);
                            p[16] = bmp.GetPixel(i + 1, j - 1);
                            p[17] = bmp.GetPixel(i + 1, j);
                            p[18] = bmp.GetPixel(i + 1, j + 1);
                            p[19] = bmp.GetPixel(i + 1, j + 2);

                            p[20] = bmp.GetPixel(i + 2, j - 2);
                            p[21] = bmp.GetPixel(i + 2, j - 1);
                            p[22] = bmp.GetPixel(i + 2, j);
                            p[23] = bmp.GetPixel(i + 2, j + 1);
                            p[24] = bmp.GetPixel(i + 2, j + 2);

                            byte[] pixelsR = new byte[matrix.matrix5x5.Length];
                            byte[] pixelsG = new byte[matrix.matrix5x5.Length];
                            byte[] pixelsB = new byte[matrix.matrix5x5.Length];

                            for (int k = 0; k < p.Length; k++)
                            {
                                pixelsR[k] = p[k].R;
                                pixelsG[k] = p[k].G;
                                pixelsB[k] = p[k].B;
                            }

                            pixelsR = ApplyConvolution(pixelsR, matrix.matrix5x5, 0);
                            pixelsG = ApplyConvolution(pixelsG, matrix.matrix5x5, 0);
                            pixelsB = ApplyConvolution(pixelsB, matrix.matrix5x5, 0);

                            bmp.SetPixel(i, j, Color.FromArgb(pixelsR[mid], pixelsG[mid], pixelsB[mid]));
                        }

                    }
                }

                bmp = shrinkImage(bmp, 2);
                image = bmp;
                return bmp;
            }
            throw new Exception("Image is not set.");
        }

        public Bitmap MeanFilter7x7()
        {
            if (image != null)
            {
                lock (objLock)
                {
                    undoStack.Push(new Bitmap(image));
                    redoStack.Clear();
                    channelX = channelY = channelZ = null;
                }
                int[] convolutionMatrix = { -1, -1, -1,
                                            -1, 2, -1,
                                            -1, -1, -1 };
                ConvolutionMatrix matrix = new ConvolutionMatrix(convolutionMatrix);
                int mid = matrix.matrix7x7.Length / 2;

                Bitmap bmp = new Bitmap(image);
                bmp = expandImage(bmp, 3);
                Color[] p = new Color[matrix.matrix7x7.Length];
                for (int i = 3; i < bmp.Width - 3; i++)
                {
                    for (int j = 3; j < bmp.Height - 3; j++)
                    {
                        lock (objLock)
                        {
                            p[0] = bmp.GetPixel(i - 3, j - 3);
                            p[1] = bmp.GetPixel(i - 3, j - 2);
                            p[2] = bmp.GetPixel(i - 3, j - 1);
                            p[3] = bmp.GetPixel(i - 3, j);
                            p[4] = bmp.GetPixel(i - 3, j + 1);
                            p[5] = bmp.GetPixel(i - 3, j + 2);
                            p[6] = bmp.GetPixel(i - 3, j + 3);

                            p[7] = bmp.GetPixel(i - 2, j - 3);
                            p[8] = bmp.GetPixel(i - 2, j - 2);
                            p[9] = bmp.GetPixel(i - 2, j - 1);
                            p[10] = bmp.GetPixel(i - 2, j);
                            p[11] = bmp.GetPixel(i - 2, j + 1);
                            p[12] = bmp.GetPixel(i - 2, j + 2);
                            p[13] = bmp.GetPixel(i - 2, j + 3);

                            p[14] = bmp.GetPixel(i - 1, j - 3);
                            p[15] = bmp.GetPixel(i - 1, j - 2);
                            p[16] = bmp.GetPixel(i - 1, j - 1);
                            p[17] = bmp.GetPixel(i - 1, j);
                            p[18] = bmp.GetPixel(i - 1, j + 1);
                            p[19] = bmp.GetPixel(i - 1, j + 2);
                            p[20] = bmp.GetPixel(i - 1, j + 3);

                            p[21] = bmp.GetPixel(i, j - 3);
                            p[22] = bmp.GetPixel(i, j - 2);
                            p[23] = bmp.GetPixel(i, j - 1);
                            p[24] = bmp.GetPixel(i, j);
                            p[25] = bmp.GetPixel(i, j + 1);
                            p[26] = bmp.GetPixel(i, j + 2);
                            p[27] = bmp.GetPixel(i, j + 3);

                            p[28] = bmp.GetPixel(i + 1, j - 3);
                            p[29] = bmp.GetPixel(i + 1, j - 2);
                            p[30] = bmp.GetPixel(i + 1, j - 1);
                            p[31] = bmp.GetPixel(i + 1, j);
                            p[32] = bmp.GetPixel(i + 1, j + 1);
                            p[33] = bmp.GetPixel(i + 1, j + 2);
                            p[34] = bmp.GetPixel(i + 1, j + 3);

                            p[35] = bmp.GetPixel(i + 2, j - 3);
                            p[36] = bmp.GetPixel(i + 2, j - 2);
                            p[37] = bmp.GetPixel(i + 2, j - 1);
                            p[38] = bmp.GetPixel(i + 2, j);
                            p[39] = bmp.GetPixel(i + 2, j + 1);
                            p[40] = bmp.GetPixel(i + 2, j + 2);
                            p[41] = bmp.GetPixel(i + 2, j + 3);

                            p[42] = bmp.GetPixel(i + 3, j - 3);
                            p[43] = bmp.GetPixel(i + 3, j - 2);
                            p[44] = bmp.GetPixel(i + 3, j - 1);
                            p[45] = bmp.GetPixel(i + 3, j);
                            p[46] = bmp.GetPixel(i + 3, j + 1);
                            p[47] = bmp.GetPixel(i + 3, j + 2);
                            p[48] = bmp.GetPixel(i + 3, j + 3);

                            byte[] pixelsR = new byte[matrix.matrix7x7.Length];
                            byte[] pixelsG = new byte[matrix.matrix7x7.Length];
                            byte[] pixelsB = new byte[matrix.matrix7x7.Length];

                            for (int k = 0; k < p.Length; k++)
                            {
                                pixelsR[k] = p[k].R;
                                pixelsG[k] = p[k].G;
                                pixelsB[k] = p[k].B;
                            }

                            pixelsR = ApplyConvolution(pixelsR, matrix.matrix7x7, 0);
                            pixelsG = ApplyConvolution(pixelsG, matrix.matrix7x7, 0);
                            pixelsB = ApplyConvolution(pixelsB, matrix.matrix7x7, 0);

                            bmp.SetPixel(i, j, Color.FromArgb(pixelsR[mid], pixelsG[mid], pixelsB[mid]));
                        }

                    }
                }

                bmp = shrinkImage(bmp, 3);
                image = bmp;
                return bmp;
            }
            throw new Exception("Image is not set.");
        }

        private byte[] ApplyConvolution(byte[] pixels, int[] convolutionMatrix, byte offset, int fact = 0)
        {
            int sum = 0;
            int factor = 0;

            if (pixels.Length != convolutionMatrix.Length)
            {
                throw new Exception("The pixel matrix has to be the same size as the convolution matrix.");
            }

            for (int i = 0; i < pixels.Length; i++)
            {
                factor += convolutionMatrix[i];
                sum += pixels[i] * convolutionMatrix[i];
            }

            if (fact != 0)
            {
                factor = fact;
            }
            byte k = (byte)((sum / factor) + offset);
            pixels[pixels.Length / 2] = k;

            return pixels;
        }

        public Bitmap EdgeDetectHomogenity(byte nThreshold)
        {
            if (image != null)
            {
                lock (objLock)
                {
                    undoStack.Push(new Bitmap(image));
                    redoStack.Clear();
                    channelX = channelY = channelZ = null;
                }
                Bitmap imageCopy = (Bitmap)image.Clone();

                BitmapData imageData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                BitmapData copyData = imageCopy.LockBits(new Rectangle(0, 0, imageCopy.Width, imageCopy.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                int stride = imageData.Stride;
                IntPtr Scan0 = imageData.Scan0;
                IntPtr Scan0_copy = copyData.Scan0;

                unsafe
                {
                    byte* p = (byte*)(void*)Scan0;
                    byte* p2 = (byte*)(void*)Scan0_copy;

                    int nOffset = stride - image.Width * 3;
                    int nWidth = image.Width * 3;
                    int nPixel = 0, nPixelMax = 0;

                    p += stride;
                    p2 += stride;

                    for (int y = 1; y < image.Height - 1; ++y)
                    {
                        p += 3;
                        p2 += 3;
                        for (int x = 3; x < nWidth - 3; ++x)
                        {
                            nPixelMax = Math.Abs(p2[0] - (p2 + stride - 3)[0]);

                            nPixel = Math.Abs(p2[0] - (p2 + stride)[0]);
                            if (nPixel > nPixelMax)
                            {
                                nPixelMax = nPixel;
                            }

                            nPixel = Math.Abs(p2[0] - (p2 + stride + 3)[0]);
                            if (nPixel > nPixelMax)
                            {
                                nPixelMax = nPixel;
                            }

                            nPixel = Math.Abs(p2[0] - (p2 - stride)[0]);
                            if (nPixel > nPixelMax)
                            {
                                nPixelMax = nPixel;
                            }

                            nPixel = Math.Abs(p2[0] - (p2 + stride)[0]);
                            if (nPixel > nPixelMax)
                            {
                                nPixelMax = nPixel;
                            }

                            nPixel = Math.Abs(p2[0] - (p2 - stride - 3)[0]);
                            if (nPixel > nPixelMax)
                            {
                                nPixelMax = nPixel;
                            }

                            nPixel = Math.Abs(p2[0] - (p2 - stride)[0]);
                            if (nPixel > nPixelMax)
                            {
                                nPixelMax = nPixel;
                            }

                            nPixel = Math.Abs(p2[0] - (p2 - stride + 3)[0]);
                            if (nPixel > nPixelMax)
                            {
                                nPixelMax = nPixel;
                            }

                            if (nPixelMax < (int)nThreshold)
                            {
                                nPixelMax = 0;
                            }

                            p[0] = (byte)nPixelMax;

                            ++p;
                            ++p2;
                        }

                        p += 3 + nOffset;
                        p2 += 3 + nOffset;
                    }
                }

                image.UnlockBits(imageData);
                imageCopy.UnlockBits(copyData);

                return image;
            }
            throw new Exception("Image is not set.");
        }

        public Bitmap GetChannelXHistogram()
        {
            if (image != null)
            {

                if (channelX == null)
                {
                    histogramX.Clear();
                    GetChannelX();
                }

                return Resize(Histogram.ToBitmap(histogramX), channelX.Width, channelX.Height, true);
            }
            throw new Exception("Image is not set.");
        }

        public Bitmap GetChannelYHistogram()
        {
            if (image != null)
            {

                if (channelY == null)
                {
                    histogramY.Clear();
                    GetChannelY();
                }

                return Resize(Histogram.ToBitmap(histogramY), channelY.Width, channelY.Height, true);
            }
            throw new Exception("Image is not set.");
        }

        public Bitmap GetChannelZHistogram()
        {
            if (image != null)
            {

                if (channelZ == null)
                {
                    histogramZ.Clear();
                    GetChannelZ();
                }

                return Resize(Histogram.ToBitmap(histogramZ), channelZ.Width, channelZ.Height, true);
            }
            throw new Exception("Image is not set.");
        }

        public static Bitmap Resize(Bitmap b, int nWidth, int nHeight, bool bBilinear)
        {
            Bitmap bTemp = (Bitmap)b.Clone();
            b = new Bitmap(nWidth, nHeight, bTemp.PixelFormat);

            double nXFactor = (double)bTemp.Width / (double)nWidth;
            double nYFactor = (double)bTemp.Height / (double)nHeight;

            if (bBilinear)
            {
                double fraction_x, fraction_y, one_minus_x, one_minus_y;
                int ceil_x, ceil_y, floor_x, floor_y;
                Color c1 = new Color();
                Color c2 = new Color();
                Color c3 = new Color();
                Color c4 = new Color();
                byte red, green, blue;

                byte b1, b2;

                for (int x = 0; x < b.Width; ++x)
                    for (int y = 0; y < b.Height; ++y)
                    {
                        // Setup

                        floor_x = (int)Math.Floor(x * nXFactor);
                        floor_y = (int)Math.Floor(y * nYFactor);
                        ceil_x = floor_x + 1;
                        if (ceil_x >= bTemp.Width) ceil_x = floor_x;
                        ceil_y = floor_y + 1;
                        if (ceil_y >= bTemp.Height) ceil_y = floor_y;
                        fraction_x = x * nXFactor - floor_x;
                        fraction_y = y * nYFactor - floor_y;
                        one_minus_x = 1.0 - fraction_x;
                        one_minus_y = 1.0 - fraction_y;

                        c1 = bTemp.GetPixel(floor_x, floor_y);
                        c2 = bTemp.GetPixel(ceil_x, floor_y);
                        c3 = bTemp.GetPixel(floor_x, ceil_y);
                        c4 = bTemp.GetPixel(ceil_x, ceil_y);

                        // Blue
                        b1 = (byte)(one_minus_x * c1.B + fraction_x * c2.B);

                        b2 = (byte)(one_minus_x * c3.B + fraction_x * c4.B);

                        blue = (byte)(one_minus_y * (double)(b1) + fraction_y * (double)(b2));

                        // Green
                        b1 = (byte)(one_minus_x * c1.G + fraction_x * c2.G);

                        b2 = (byte)(one_minus_x * c3.G + fraction_x * c4.G);

                        green = (byte)(one_minus_y * (double)(b1) + fraction_y * (double)(b2));

                        // Red
                        b1 = (byte)(one_minus_x * c1.R + fraction_x * c2.R);

                        b2 = (byte)(one_minus_x * c3.R + fraction_x * c4.R);

                        red = (byte)(one_minus_y * (double)(b1) + fraction_y * (double)(b2));

                        b.SetPixel(x, y, System.Drawing.Color.FromArgb(255, red, green, blue));
                    }
            }
            else
            {
                for (int x = 0; x < b.Width; ++x)
                    for (int y = 0; y < b.Height; ++y)
                        b.SetPixel(x, y, bTemp.GetPixel((int)(Math.Floor(x * nXFactor)), (int)(Math.Floor(y * nYFactor))));
            }

            return b;
        }

        public Bitmap TimeWarp(byte factor, bool smoothing)
        {
            if (image != null)
            {
                undoStack.Push(new Bitmap(image));
                redoStack.Clear();
                channelX = channelY = channelZ = null;

                int nWidth = image.Width;
                int nHeight = image.Height;

                FloatPoint[,] fp = new FloatPoint[nWidth, nHeight];
                Point[,] pt = new Point[nWidth, nHeight];

                Point mid = new Point();
                mid.X = nWidth / 2;
                mid.Y = nHeight / 2;

                double theta, radius;
                double newX, newY;

                for (int x = 0; x < nWidth; ++x)
                    for (int y = 0; y < nHeight; ++y)
                    {
                        int trueX = x - mid.X;
                        int trueY = y - mid.Y;
                        theta = Math.Atan2((trueY), (trueX));

                        radius = Math.Sqrt(trueX * trueX + trueY * trueY);

                        double newRadius = Math.Sqrt(radius) * factor;

                        newX = mid.X + (newRadius * Math.Cos(theta));
                        if (newX > 0 && newX < nWidth)
                        {
                            fp[x, y].X = newX;
                            pt[x, y].X = (int)newX;
                        }
                        else
                        {
                            fp[x, y].X = 0.0;
                            pt[x, y].X = 0;
                        }

                        newY = mid.Y + (newRadius * Math.Sin(theta));
                        if (newY > 0 && newY < nHeight)
                        {
                            fp[x, y].Y = newY;
                            pt[x, y].Y = (int)newY;
                        }
                        else
                        {
                            fp[x, y].Y = 0.0;
                            pt[x, y].Y = 0;
                        }
                    }

                if (!smoothing)
                    OffsetFilterAbs(image, pt);
                else
                    OffsetFilterAntiAlias(image, fp);

                return image;
            }
            throw new Exception("Image is not set.");
        }

        public bool OffsetFilterAbs(Bitmap b, Point[,] offset)
        {
            Bitmap bSrc = (Bitmap)b.Clone();


            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int scanline = bmData.Stride;

            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr SrcScan0 = bmSrc.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* pSrc = (byte*)(void*)SrcScan0;

                int nOffset = bmData.Stride - b.Width * 3;
                int nWidth = b.Width;
                int nHeight = b.Height;

                int xOffset, yOffset;

                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        xOffset = offset[x, y].X;
                        yOffset = offset[x, y].Y;

                        if (yOffset >= 0 && yOffset < nHeight && xOffset >= 0 && xOffset < nWidth)
                        {
                            p[0] = pSrc[(yOffset * scanline) + (xOffset * 3)];
                            p[1] = pSrc[(yOffset * scanline) + (xOffset * 3) + 1];
                            p[2] = pSrc[(yOffset * scanline) + (xOffset * 3) + 2];
                        }

                        p += 3;
                    }
                    p += nOffset;
                }
            }

            b.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);

            return true;
        }

        public bool OffsetFilterAntiAlias(Bitmap b, FloatPoint[,] fp)
        {
            Bitmap bSrc = (Bitmap)b.Clone();

            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int scanline = bmData.Stride;

            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr SrcScan0 = bmSrc.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* pSrc = (byte*)(void*)SrcScan0;

                int nOffset = bmData.Stride - b.Width * 3;
                int nWidth = b.Width;
                int nHeight = b.Height;

                double xOffset, yOffset;

                double fraction_x, fraction_y, one_minus_x, one_minus_y;
                int ceil_x, ceil_y, floor_x, floor_y;
                Byte p1, p2;

                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        xOffset = fp[x, y].X;
                        yOffset = fp[x, y].Y;

                        // Setup

                        floor_x = (int)Math.Floor(xOffset);
                        floor_y = (int)Math.Floor(yOffset);
                        ceil_x = floor_x + 1;
                        ceil_y = floor_y + 1;
                        fraction_x = xOffset - floor_x;
                        fraction_y = yOffset - floor_y;
                        one_minus_x = 1.0 - fraction_x;
                        one_minus_y = 1.0 - fraction_y;

                        if (floor_y >= 0 && ceil_y < nHeight && floor_x >= 0 && ceil_x < nWidth)
                        {
                            // Blue

                            p1 = (Byte)(one_minus_x * (double)(pSrc[floor_y * scanline + floor_x * 3]) +
                                fraction_x * (double)(pSrc[floor_y * scanline + ceil_x * 3]));

                            p2 = (Byte)(one_minus_x * (double)(pSrc[ceil_y * scanline + floor_x * 3]) +
                                fraction_x * (double)(pSrc[ceil_y * scanline + 3 * ceil_x]));

                            p[x * 3 + y * scanline] = (Byte)(one_minus_y * (double)(p1) + fraction_y * (double)(p2));

                            // Green

                            p1 = (Byte)(one_minus_x * (double)(pSrc[floor_y * scanline + floor_x * 3 + 1]) +
                                fraction_x * (double)(pSrc[floor_y * scanline + ceil_x * 3 + 1]));

                            p2 = (Byte)(one_minus_x * (double)(pSrc[ceil_y * scanline + floor_x * 3 + 1]) +
                                fraction_x * (double)(pSrc[ceil_y * scanline + 3 * ceil_x + 1]));

                            p[x * 3 + y * scanline + 1] = (Byte)(one_minus_y * (double)(p1) + fraction_y * (double)(p2));

                            // Red

                            p1 = (Byte)(one_minus_x * (double)(pSrc[floor_y * scanline + floor_x * 3 + 2]) +
                                fraction_x * (double)(pSrc[floor_y * scanline + ceil_x * 3 + 2]));

                            p2 = (Byte)(one_minus_x * (double)(pSrc[ceil_y * scanline + floor_x * 3 + 2]) +
                                fraction_x * (double)(pSrc[ceil_y * scanline + 3 * ceil_x + 2]));

                            p[x * 3 + y * scanline + 2] = (Byte)(one_minus_y * (double)(p1) + fraction_y * (double)(p2));
                        }
                    }
                }
            }

            b.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);

            return true;
        }

        public void Undo()
        {
            if (undoStack.Count > 0)
            {
                redoStack.Push(new Bitmap(image));

                channelX = channelY = channelZ = null;
                histogramX.Clear();
                histogramY.Clear();
                histogramZ.Clear();

                image = undoStack.Pop();
            }
        }

        public void Redo()
        {
            if (redoStack.Count > 0)
            {
                undoStack.Push(new Bitmap(image));

                channelX = channelY = channelZ = null;
                histogramX.Clear();
                histogramY.Clear();
                histogramZ.Clear();


                image = redoStack.Pop();
            }
        }

        public Bitmap HistogramAverageFilter(int groupNum)
        {
            if (image != null)
            {
                lock (objLock)
                {
                    undoStack.Push(new Bitmap(image));
                    redoStack.Clear();
                }

                if (channelX == null)
                {
                    GetChannelX();
                }
                if (channelY == null)
                {
                    GetChannelY();
                }
                if (channelZ == null)
                {
                    GetChannelZ();
                }

                //priprema za rad sa histogramom
                int[] minX = new int[(95 / groupNum) + 1];
                int[] maxX = new int[(95 / groupNum) + 1];
                int[] avgX = new int[(95 / groupNum) + 1];

                int[] minY = new int[(100 / groupNum) + 1];
                int[] maxY = new int[(100 / groupNum) + 1];
                int[] avgY = new int[(100 / groupNum) + 1];

                int[] minZ = new int[(108 / groupNum) + 1];
                int[] maxZ = new int[(108 / groupNum) + 1];
                int[] avgZ = new int[(108 / groupNum) + 1];

                int elementsInGroupX = 95 / groupNum;
                int elementsInGroupY = 100 / groupNum;
                int elementsInGroupZ = 108 / groupNum;

                int remainingX = 95 - groupNum * elementsInGroupX;
                int remainingY = 100 - groupNum * elementsInGroupY;
                int remainingZ = 108 - groupNum * elementsInGroupZ;

                for (int i = 0; i < 95 / groupNum; i++)
                {
                    minX[i] = i * groupNum;
                    maxX[i] = (i + 1) * groupNum;
                }
                //vrati rezultat
                minX[minX.Length - 1] = maxX[maxX.Length - 2];
                maxX[maxX.Length - 1] = 95;

                for (int i = 0; i < minX.Length; i++)
                {
                    avgX[i] = (maxX[i] + minX[i]) / 2;
                }

                for (int i = 0; i < 100 / groupNum; i++)
                {
                    minY[i] = i * groupNum;
                    maxY[i] = (i + 1) * groupNum;
                }
                minY[minY.Length - 1] = maxY[maxY.Length - 2];
                maxY[maxY.Length - 1] = 100;

                for (int i = 0; i < minY.Length; i++)
                {
                    avgY[i] = (maxY[i] + minY[i]) / 2;
                }

                for (int i = 0; i < 108 / groupNum; i++)
                {
                    minZ[i] = i * groupNum;
                    maxZ[i] = (i + 1) * groupNum;
                }
                minZ[minZ.Length - 1] = maxZ[maxZ.Length - 2];
                maxZ[maxZ.Length - 1] = 108;

                for (int i = 0; i < minZ.Length; i++)
                {
                    avgZ[i] = (maxZ[i] + minZ[i]) / 2;
                }


                //obrada slike
                for (int i = 0; i < image.Width; i++)
                {
                    for (int j = 0; j < image.Height; j++)
                    {
                        lock (objLock)
                        {
                            Color pixel = image.GetPixel(i, j);
                            double x = 0, y = 0, z = 0;
                            RGBToXYZ(pixel, out x, out y, out z);
                            //postavi prosecno x,y,z iz odgovarajuce zone
                            for (int k = 0; k < minX.Length; k++)
                            {
                                if (x > minX[k] && x <= maxX[k])
                                {
                                    x = avgX[k];
                                    break;
                                }
                            }

                            for (int k = 0; k < minY.Length; k++)
                            {
                                if (y > minY[k] && y <= maxY[k])
                                {
                                    y = avgY[k];
                                    break;
                                }
                            }

                            for (int k = 0; k < minZ.Length; k++)
                            {
                                if (z > minZ[k] && z <= maxZ[k])
                                {
                                    z = avgZ[k];
                                    break;
                                }
                            }

                            //vrati nazad u rgb
                            Color resultingPixel = XYZToRGB(x, y, z);
                            image.SetPixel(i, j, resultingPixel);
                        }
                    }
                }
                lock (objLock)
                {
                    channelX = channelY = channelZ = null;
                    histogramX.Clear();
                    histogramY.Clear();
                    histogramZ.Clear();
                }
                return image;
            }

            throw new Exception("Image is not set.");
        }

        public void SetImage(Bitmap bmp)
        {
            image = bmp;
            channelX = channelY = channelZ = null;
            histogramX.Clear();
            histogramY.Clear();
            histogramZ.Clear();
        }

        public Bitmap DownsampleXY()
        {
            if (image != null)
            {
                if (image.Width % 4 != 0)
                {
                    image = new Bitmap(image, new Size(image.Width + image.Width % 4, image.Height));
                }
                else if (image.Height % 4 != 0)
                {
                    image = new Bitmap(image, new Size(image.Width, image.Height + image.Height % 4));
                }
                else if (image.Width % 4 != 0 && image.Height % 4 != 0)
                {
                    image = new Bitmap(image, new Size(image.Width + image.Width % 4, image.Height + image.Height % 4));
                }

                Bitmap bmp = new Bitmap(image);




                for (int i = 0; i < bmp.Width - 1; i += 2)
                {
                    for (int j = 0; j < bmp.Height - 1; j += 2)
                    {
                        Color[] p = new Color[4];
                        p[0] = bmp.GetPixel(i, j);
                        p[1] = bmp.GetPixel(i + 1, j);
                        p[2] = bmp.GetPixel(i, j + 1);
                        p[3] = bmp.GetPixel(i + 1, j + 1);

                        double x0, y0, z0, x1, y1, z1, x2, y2, z2, x3, y3, z3;

                        RGBToXYZ(p[0], out x0, out y0, out z0);
                        RGBToXYZ(p[1], out x1, out y1, out z1);
                        RGBToXYZ(p[2], out x2, out y2, out z2);
                        RGBToXYZ(p[3], out x3, out y3, out z3);

                        double avgX, avgY;
                        avgX = (x0 + x1 + x2 + x3) / 4;
                        avgY = (y0 + y1 + y2 + y3) / 4;

                        Color[] res = new Color[4];
                        res[0] = XYZToRGB(avgX, avgY, z0);
                        res[1] = XYZToRGB(avgX, avgY, z1);
                        res[2] = XYZToRGB(avgX, avgY, z2);
                        res[3] = XYZToRGB(avgX, avgY, z3);



                        bmp.SetPixel(i, j, res[0]);
                        bmp.SetPixel(i + 1, j, res[1]);
                        bmp.SetPixel(i, j + 1, res[2]);
                        bmp.SetPixel(i + 1, j + 1, res[3]);
                    }
                }



                return bmp;
            }
            throw new Exception("Image is not set.");
        }

        public Bitmap DownsampleXZ()
        {
            if (image != null)
            {
                if (image.Width % 4 != 0)
                {
                    image = new Bitmap(image, new Size(image.Width + image.Width % 4, image.Height));
                }
                else if (image.Height % 4 != 0)
                {
                    image = new Bitmap(image, new Size(image.Width, image.Height + image.Height % 4));
                }
                else if (image.Width % 4 != 0 && image.Height % 4 != 0)
                {
                    image = new Bitmap(image, new Size(image.Width + image.Width % 4, image.Height + image.Height % 4));
                }

                Bitmap bmp = new Bitmap(image);


                for (int i = 0; i < bmp.Width - 1; i += 2)
                {
                    for (int j = 0; j < bmp.Height - 1; j += 2)
                    {
                        Color[] p = new Color[4];
                        p[0] = bmp.GetPixel(i, j);
                        p[1] = bmp.GetPixel(i + 1, j);
                        p[2] = bmp.GetPixel(i, j + 1);
                        p[3] = bmp.GetPixel(i + 1, j + 1);

                        double x0, y0, z0, x1, y1, z1, x2, y2, z2, x3, y3, z3;

                        RGBToXYZ(p[0], out x0, out y0, out z0);
                        RGBToXYZ(p[1], out x1, out y1, out z1);
                        RGBToXYZ(p[2], out x2, out y2, out z2);
                        RGBToXYZ(p[3], out x3, out y3, out z3);

                        double avgX, avgZ;
                        avgX = (x0 + x1 + x2 + x3) / 4;
                        avgZ = (z0 + z1 + z2 + z3) / 4;

                        Color[] res = new Color[4];
                        res[0] = XYZToRGB(avgX, y0, avgZ);
                        res[1] = XYZToRGB(avgX, y1, avgZ);
                        res[2] = XYZToRGB(avgX, y2, avgZ);
                        res[3] = XYZToRGB(avgX, y3, avgZ);


                        bmp.SetPixel(i, j, res[0]);
                        bmp.SetPixel(i + 1, j, res[1]);
                        bmp.SetPixel(i, j + 1, res[2]);
                        bmp.SetPixel(i + 1, j + 1, res[3]);
                    }
                }

                return bmp;
            }
            throw new Exception("Image is not set.");
        }

        public Bitmap DownsampleYZ()
        {
            if (image != null)
            {
                if (image.Width % 4 != 0)
                {
                    image = new Bitmap(image, new Size(image.Width + image.Width % 4, image.Height));
                }
                else if (image.Height % 4 != 0)
                {
                    image = new Bitmap(image, new Size(image.Width, image.Height + image.Height % 4));
                }
                else if (image.Width % 4 != 0 && image.Height % 4 != 0)
                {
                    image = new Bitmap(image, new Size(image.Width + image.Width % 4, image.Height + image.Height % 4));
                }

                Bitmap bmp = new Bitmap(image);

                for (int i = 0; i < bmp.Width - 1; i += 2)
                {
                    for (int j = 0; j < bmp.Height - 1; j += 2)
                    {
                        Color[] p = new Color[4];
                        p[0] = bmp.GetPixel(i, j);
                        p[1] = bmp.GetPixel(i + 1, j);
                        p[2] = bmp.GetPixel(i, j + 1);
                        p[3] = bmp.GetPixel(i + 1, j + 1);

                        double x0, y0, z0, x1, y1, z1, x2, y2, z2, x3, y3, z3;

                        RGBToXYZ(p[0], out x0, out y0, out z0);
                        RGBToXYZ(p[1], out x1, out y1, out z1);
                        RGBToXYZ(p[2], out x2, out y2, out z2);
                        RGBToXYZ(p[3], out x3, out y3, out z3);

                        double avgY, avgZ;
                        avgZ = (z0 + z1 + z2 + z3) / 4;
                        avgY = (y0 + y1 + y2 + y3) / 4;

                        Color[] res = new Color[4];
                        res[0] = XYZToRGB(x0, avgY, avgZ);
                        res[1] = XYZToRGB(x1, avgY, avgZ);
                        res[2] = XYZToRGB(x2, avgY, avgZ);
                        res[3] = XYZToRGB(x3, avgY, avgZ);

                        bmp.SetPixel(i, j, res[0]);
                        bmp.SetPixel(i + 1, j, res[1]);
                        bmp.SetPixel(i, j + 1, res[2]);
                        bmp.SetPixel(i + 1, j + 1, res[3]);
                    }
                }

                return bmp;
            }
            throw new Exception("Image is not set.");
        }

        public void SaveCustomImage(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using (BinaryWriter bw = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
                {

                    bw.Write(image.Width);
                    bw.Write(image.Height);
                    bw.Write(downsamplingMethod);
                    bw.Write(customImageData.Length);
                    bw.Write(customImageXData.Length);
                    bw.Write(customImageYData.Length);
                    bw.Write(customImageZData.Length);

                    uint originalSize = (uint)customImageData.Length;
                    byte[] compressedData = new byte[originalSize];

                    int compressedSize = ShannonFano.Compress(customImageData, compressedData, originalSize);

                    bw.Write(compressedSize);

                    //potrebno je da se trimuje niz kompresovanih bajtova na pravu velicinu pre snimanja
                    byte[] trimmedData = new byte[compressedSize];

                    for (int i = 0; i < compressedSize; i++)
                    {
                        trimmedData[i] = compressedData[i];
                    }

                    bw.Write(trimmedData);
                    bw.Close();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }
        }

        public Bitmap LoadCustomImage(string path)
        {
            try
            {
                using (BinaryReader br = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    lock (objLock)
                    {
                        undoStack.Clear();
                        redoStack.Clear();
                        channelX = channelY = channelZ = null;
                        histogramX.Clear();
                        histogramY.Clear();
                        histogramZ.Clear();
                    }

                    image = new Bitmap(br.ReadInt32(), br.ReadInt32(), PixelFormat.Format24bppRgb);
                    downsamplingMethod = br.ReadByte();
                    int customImageDataLength = br.ReadInt32();
                    int customImageDataLengthX = br.ReadInt32();
                    int customImageDataLengthY = br.ReadInt32();
                    int customImageDataLengthZ = br.ReadInt32();
                    int compressedDataSize = br.ReadInt32();

                    byte[] decompressedData = new byte[customImageDataLength];
                    byte[] compressedData = new byte[compressedDataSize];
                    for (int i = 0; i < compressedData.Length; i++)
                    {
                        compressedData[i] = br.ReadByte();
                    }

                    ShannonFano.Decompress(compressedData, decompressedData, (uint)compressedDataSize, (uint)customImageDataLength);

                    ReconstructImage(decompressedData, customImageDataLengthX, customImageDataLengthY, customImageDataLengthZ);

                    return image;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                return null;
            }
        }

        private void ReconstructImage(byte[] imageData, int lengthX, int lengthY, int lengthZ)
        {
            int brojac = 0;

            for (int i = 0; i < image.Width - 1; i += 2)
            {
                for (int j = 0; j < image.Height - 1; j += 2)
                {
                    Color[] p = new Color[4];

                    p[0] = Color.FromArgb(imageData[brojac], imageData[brojac + lengthX], imageData[brojac + lengthX + lengthY]);
                    brojac++;

                    image.SetPixel(i, j, p[0]);


                    p[1] = Color.FromArgb(imageData[brojac], imageData[brojac + lengthX], imageData[brojac + lengthX + lengthY]);
                    brojac++;

                    image.SetPixel(i + 1, j, p[1]);

                    p[2] = Color.FromArgb(imageData[brojac], imageData[brojac + lengthX], imageData[brojac + lengthX + lengthY]);
                    brojac++;

                    image.SetPixel(i, j + 1, p[2]);

                    p[3] = Color.FromArgb(imageData[brojac], imageData[brojac + lengthX], imageData[brojac + lengthX + lengthY]);
                    brojac++;

                    image.SetPixel(i + 1, j + 1, p[3]);
                }

            }
        }

        public void PrepareDownsampledData(int choice)
        {
            if (choice == 3)
            {
                PrepareDownsampledDataYZ();
            }
            if (choice == 2)
            {
                PrepareDownsampledDataXZ();
            }
            if (choice == 1)
            {
                PrepareDownsampledDataXY();
            }
        }

        private void PrepareDownsampledDataXY()
        {
            Bitmap bmp = new Bitmap(image);
            downsamplingMethod = 1;
            List<byte> rChannel = new List<byte>();
            List<byte> gChannel = new List<byte>();
            List<byte> bChannel = new List<byte>();

            List<byte> data = new List<byte>();
            for (int i = 0; i < bmp.Width - 1; i += 2)
            {
                for (int j = 0; j < bmp.Height - 1; j += 2)
                {
                    Color[] p = new Color[4];
                    p[0] = bmp.GetPixel(i, j);
                    p[1] = bmp.GetPixel(i + 1, j);
                    p[2] = bmp.GetPixel(i, j + 1);
                    p[3] = bmp.GetPixel(i + 1, j + 1);

                    double x0, y0, z0, x1, y1, z1, x2, y2, z2, x3, y3, z3;

                    RGBToXYZ(p[0], out x0, out y0, out z0);
                    RGBToXYZ(p[1], out x1, out y1, out z1);
                    RGBToXYZ(p[2], out x2, out y2, out z2);
                    RGBToXYZ(p[3], out x3, out y3, out z3);

                    double avgX, avgY;
                    avgX = (x0 + x1 + x2 + x3) / 4;
                    avgY = (y0 + y1 + y2 + y3) / 4;

                    Color[] res = new Color[4];

                    res[0] = XYZToRGB(avgX, avgY, z0);
                    res[1] = XYZToRGB(avgX, avgY, z1);
                    res[2] = XYZToRGB(avgX, avgY, z2);
                    res[3] = XYZToRGB(avgX, avgY, z3);

                    rChannel.Add(res[0].R);
                    rChannel.Add(res[1].R);
                    rChannel.Add(res[2].R);
                    rChannel.Add(res[3].R);

                    gChannel.Add(res[0].G);
                    gChannel.Add(res[1].G);
                    gChannel.Add(res[2].G);
                    gChannel.Add(res[3].G);

                    bChannel.Add(res[0].B);
                    bChannel.Add(res[1].B);
                    bChannel.Add(res[2].B);
                    bChannel.Add(res[3].B);
                }
            }
            customImageXData = rChannel.ToArray();
            customImageYData = gChannel.ToArray();
            customImageZData = bChannel.ToArray();

            data.AddRange(customImageXData);
            data.AddRange(customImageYData);
            data.AddRange(customImageZData);

            customImageData = data.ToArray();
        }

        private void PrepareDownsampledDataXZ()
        {
            Bitmap bmp = new Bitmap(image);
            downsamplingMethod = 2;
            List<byte> rChannel = new List<byte>();
            List<byte> gChannel = new List<byte>();
            List<byte> bChannel = new List<byte>();

            List<byte> data = new List<byte>();
            for (int i = 0; i < bmp.Width - 1; i += 2)
            {
                for (int j = 0; j < bmp.Height - 1; j += 2)
                {
                    Color[] p = new Color[4];
                    p[0] = bmp.GetPixel(i, j);
                    p[1] = bmp.GetPixel(i + 1, j);
                    p[2] = bmp.GetPixel(i, j + 1);
                    p[3] = bmp.GetPixel(i + 1, j + 1);

                    double x0, y0, z0, x1, y1, z1, x2, y2, z2, x3, y3, z3;

                    RGBToXYZ(p[0], out x0, out y0, out z0);
                    RGBToXYZ(p[1], out x1, out y1, out z1);
                    RGBToXYZ(p[2], out x2, out y2, out z2);
                    RGBToXYZ(p[3], out x3, out y3, out z3);

                    double avgX, avgZ;
                    avgX = (x0 + x1 + x2 + x3) / 4;
                    avgZ = (z0 + z1 + z2 + z3) / 4;

                    Color[] res = new Color[4];

                    res[0] = XYZToRGB(avgX, y0, avgZ);
                    res[1] = XYZToRGB(avgX, y1, avgZ);
                    res[2] = XYZToRGB(avgX, y2, avgZ);
                    res[3] = XYZToRGB(avgX, y3, avgZ);

                    rChannel.Add(res[0].R);
                    rChannel.Add(res[1].R);
                    rChannel.Add(res[2].R);
                    rChannel.Add(res[3].R);

                    gChannel.Add(res[0].G);
                    gChannel.Add(res[1].G);
                    gChannel.Add(res[2].G);
                    gChannel.Add(res[3].G);

                    bChannel.Add(res[0].B);
                    bChannel.Add(res[1].B);
                    bChannel.Add(res[2].B);
                    bChannel.Add(res[3].B);
                }
            }
            customImageXData = rChannel.ToArray();
            customImageYData = gChannel.ToArray();
            customImageZData = bChannel.ToArray();

            data.AddRange(customImageXData);
            data.AddRange(customImageYData);
            data.AddRange(customImageZData);

            customImageData = data.ToArray();
        }

        private void PrepareDownsampledDataYZ()
        {
            Bitmap bmp = new Bitmap(image);
            downsamplingMethod = 3;
            List<byte> rChannel = new List<byte>();
            List<byte> gChannel = new List<byte>();
            List<byte> bChannel = new List<byte>();

            List<byte> data = new List<byte>();
            for (int i = 0; i < bmp.Width - 1; i += 2)
            {
                for (int j = 0; j < bmp.Height - 1; j += 2)
                {
                    Color[] p = new Color[4];
                    p[0] = bmp.GetPixel(i, j);
                    p[1] = bmp.GetPixel(i + 1, j);
                    p[2] = bmp.GetPixel(i, j + 1);
                    p[3] = bmp.GetPixel(i + 1, j + 1);

                    double x0, y0, z0, x1, y1, z1, x2, y2, z2, x3, y3, z3;

                    RGBToXYZ(p[0], out x0, out y0, out z0);
                    RGBToXYZ(p[1], out x1, out y1, out z1);
                    RGBToXYZ(p[2], out x2, out y2, out z2);
                    RGBToXYZ(p[3], out x3, out y3, out z3);

                    double avgY, avgZ;
                    avgY = (y0 + y1 + y2 + y3) / 4;
                    avgZ = (z0 + z1 + z2 + z3) / 4;

                    Color[] res = new Color[4];

                    res[0] = XYZToRGB(x0, avgY, avgZ);
                    res[1] = XYZToRGB(x1, avgY, avgZ);
                    res[2] = XYZToRGB(x2, avgY, avgZ);
                    res[3] = XYZToRGB(x3, avgY, avgZ);

                    rChannel.Add(res[0].R);
                    rChannel.Add(res[1].R);
                    rChannel.Add(res[2].R);
                    rChannel.Add(res[3].R);

                    gChannel.Add(res[0].G);
                    gChannel.Add(res[1].G);
                    gChannel.Add(res[2].G);
                    gChannel.Add(res[3].G);

                    bChannel.Add(res[0].B);
                    bChannel.Add(res[1].B);
                    bChannel.Add(res[2].B);
                    bChannel.Add(res[3].B);
                }
            }
            customImageXData = rChannel.ToArray();
            customImageYData = gChannel.ToArray();
            customImageZData = bChannel.ToArray();

            data.AddRange(customImageXData);
            data.AddRange(customImageYData);
            data.AddRange(customImageZData);

            customImageData = data.ToArray();
        }
    }



    public struct FloatPoint
    {
        public double X;
        public double Y;
    }
}
