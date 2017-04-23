using System;
using System.Drawing;
using MMS_Lab_1.Views;

namespace MMS_Lab_1.Controllers
{
    public interface IImageController : IController
    {
        Bitmap Load(String path);
        void Save(String path);
        void SetViewImage(Bitmap bmp);
        Bitmap GetViewImage();
        Bitmap GetViewImageX();
        Bitmap GetViewImageY();
        Bitmap GetViewImageZ();
        void SetViewAllConvolutionsMean(Bitmap bmp);
        void SetUnsafeMode(bool value);
        bool GetUnsafeMode();
        Bitmap GetImageFromModel();
        Bitmap InvertImage();
        Bitmap ColorsFilter(int r, int g, int b);
        Bitmap MeanFilter3x3();
        Bitmap MeanFilter5x5();
        Bitmap MeanFilter7x7();
        Bitmap EdgeDetectHomogenity(byte nThreshold);
        Bitmap TimeWarp(byte factor, bool smoothing);
        void SetViewHistograms(Bitmap bmp);
        Bitmap HistogramAverages(int groupNum);
        void SaveDownsampledImage(String path, int choice);
        void SetViewDownsampledImage(Bitmap bmp);
        Bitmap LoadDownsampledImage(String path);
    }
}
