using System;
using MMS_Lab_1.Views;
using MMS_Lab_1.Models;

namespace MMS_Lab_1.Controllers
{
    public class AudioController : IAudioController
    {
        private IView _view;
        private IAudioModel _model;

        public AudioController()
        {
            _model = new AudioModel();
        }

        public void AttachView(IView view)
        {
            _view = view;
        }

        public void Play()
        {
            _model.Play();
        }

        public void Stop()
        {
            _model.Stop();
        }

        public void ApplyFilter()
        {
            _model.ApplyFilter();
        }

        public void Load(string path)
        {
            _model.Load(path);
        }

        public void Save(string path)
        {
            _model.Save(path);
        }
    }
}
