using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace MMS_Lab_1.Models
{
    public class ImageOnlyModel : IModel
    {
        Bitmap image;

        public ImageOnlyModel()
        {

        }

        public Bitmap GetImage()
        {
            return image;
        }

        public Bitmap InvertSafe()
        {
            if (image != null)
            {
                for (int i = 0; i < image.Width; i++)
                {
                    for (int j = 0; j < image.Height; j++)
                    {
                        Color pixel = image.GetPixel(i, j);

                        image.SetPixel(i, j, Color.FromArgb(pixel.A, 255 - pixel.R, 255 - pixel.G, 255 - pixel.B));
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
                BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);
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

        public Bitmap LoadImage(string path)
        {
            try
            {
                image = (Bitmap)Image.FromFile(path);
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

            }
        }
    }
}
