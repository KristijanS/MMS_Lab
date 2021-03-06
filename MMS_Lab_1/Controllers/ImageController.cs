﻿using MMS_Lab_1.Models;
using MMS_Lab_1.Views;
using System.Drawing;
using System;
using System.Threading;

namespace MMS_Lab_1.Controllers
{
    public class ImageController : IImageController, IUndoRedo
    {
        private IImageView _view;
        private IImageModel _model;
        private bool modelUnsafeOperaion;

        public ImageController()
        {
            _model = new ImageModel();
        }

        public void AttachView(IView view)
        {
            _view = (IImageView)view;
        }

        public void Save(string path)
        {
            _model.SaveImage(path);
        }

        public Bitmap Load(string path)
        {
            return _model.LoadImage(path);
        }

        public void SetViewImage(Bitmap bmp)
        {
            _view.SetImage(bmp);
            if (_view is AllChannelsView)
            {
                ((AllChannelsView)_view).SetChannelX(_model.GetChannelX());
                ((AllChannelsView)_view).SetChannelY(_model.GetChannelY());
                ((AllChannelsView)_view).SetChannelZ(_model.GetChannelZ());
            }

        }

        public void SetViewAllConvolutionsMean(Bitmap bmp)
        {
            _view.SetImage(bmp);
            ((AllChannelsView)_view).SetChannelX(_model.MeanFilter3x3());
            ((AllChannelsView)_view).SetChannelY(_model.MeanFilter5x5());
            ((AllChannelsView)_view).SetChannelZ(_model.MeanFilter7x7());
        }

        public void SetUnsafeMode(bool value)
        {
            modelUnsafeOperaion = value;
        }

        public bool GetUnsafeMode()
        {
            return modelUnsafeOperaion;
        }

        public Bitmap GetImageFromModel()
        {
            return _model.GetImage();
        }

        public Bitmap InvertImage()
        {
            if (!modelUnsafeOperaion)
            {
                return _model.InvertSafe();
            }
            return _model.InvertUnsafe();
        }

        public Bitmap ColorsFilter(int r, int g, int b)
        {
            if (!modelUnsafeOperaion)
            {
                return _model.ColorsSafe(r, g, b);
            }
            return _model.ColorsUnsafe(r, g, b);
        }

        public Bitmap MeanFilter3x3()
        {
            return _model.MeanFilter3x3();
        }

        public Bitmap MeanFilter5x5()
        {
            return _model.MeanFilter5x5();
        }

        public Bitmap MeanFilter7x7()
        {
            return _model.MeanFilter7x7();
        }

        public Bitmap EdgeDetectHomogenity(byte nThreshold)
        {
            return _model.EdgeDetectHomogenity(nThreshold);
        }

        public void SetViewHistograms(Bitmap bmp)
        {
            _view.SetImage(bmp);
            ((AllChannelsView)_view).SetChannelX(_model.GetChannelXHistogram());
            ((AllChannelsView)_view).SetChannelY(_model.GetChannelYHistogram());
            ((AllChannelsView)_view).SetChannelZ(_model.GetChannelZHistogram());
        }

        public Bitmap TimeWarp(byte factor, bool smoothing)
        {
            return _model.TimeWarp(factor, smoothing);
        }

        public void Undo()
        {
            ((ImageModel)_model).Undo();
        }

        public void Redo()
        {
            ((ImageModel)_model).Redo();
        }

        public Bitmap HistogramAverages(int grNo)
        {
            return _model.HistogramAverageFilter(grNo);
        }

        public Bitmap GetViewImage()
        {
            return _view.GetImage();
        }

        public Bitmap GetViewImageX()
        {
            if (_view is AllChannelsView)
            {
                return ((AllChannelsView)_view).GetChannelX();
            }
            return null;
        }

        public Bitmap GetViewImageY()
        {
            if (_view is AllChannelsView)
            {
                return ((AllChannelsView)_view).GetChannelY();
            }
            return null;
        }

        public Bitmap GetViewImageZ()
        {
            if (_view is AllChannelsView)
            {
                return ((AllChannelsView)_view).GetChannelZ();
            }
            return null;
        }

        public void SaveDownsampledImage(String path, int choice)
        {
            Thread t = new Thread(() =>
            {
                _model.PrepareDownsampledData(choice);
                _model.SaveCustomImage(path);
            });
            t.Start();
        }

        public Bitmap LoadDownsampledImage(String path)
        {
            return _model.LoadCustomImage(path);
        }

        public void SetViewDownsampledImage(Bitmap bmp)
        {
            _view.SetImage(bmp);
            ((AllChannelsView)_view).SetChannelX(_model.DownsampleXY());
            ((AllChannelsView)_view).SetChannelY(_model.DownsampleXZ());
            ((AllChannelsView)_view).SetChannelZ(_model.DownsampleYZ());
        }

        public Bitmap ColorSimilarityFilter(Color color, Point startLocation, double similarityFactor)
        {
            return _model.ColorSimilarityFilter(color, startLocation, similarityFactor);
        }
    }
}
