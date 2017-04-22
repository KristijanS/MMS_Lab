using System;
using System.Drawing;

namespace MMS_Lab_1.Models
{
    public interface IModel
    {
        Bitmap LoadImage(String path);
        void SaveImage(String path);
        Bitmap GetImage();
        void SetImage(Bitmap bmp);
        Bitmap InvertSafe();
        Bitmap InvertUnsafe();
        Bitmap ColorsSafe(int r, int g, int b);
        Bitmap ColorsUnsafe(int r, int g, int b);
        Bitmap GetChannelX();
        Bitmap GetChannelY();
        Bitmap GetChannelZ();
        Bitmap MeanFilter3x3();
        Bitmap MeanFilter5x5();
        Bitmap MeanFilter7x7();
        Bitmap EdgeDetectHomogenity(byte nThreshold);
        Bitmap GetChannelXHistogram();
        Bitmap GetChannelYHistogram();
        Bitmap GetChannelZHistogram();
        Bitmap TimeWarp(byte factor, bool smoothing);
        Bitmap HistogramAverageFilter(int groupNum);
        Bitmap DownsampleXY();
        Bitmap DownsampleXZ();
        Bitmap DownsampleYZ();
        void SaveCustomImage(String path);
        Bitmap LoadCustomImage(string path);
        void PrepareDownsampledData(int choice);
    }
}
