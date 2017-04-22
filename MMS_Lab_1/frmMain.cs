using System;
using System.Drawing;
using System.Windows.Forms;
using MMS_Lab_1.Views;
using MMS_Lab_1.Controllers;
using System.Threading;
using System.Diagnostics;

namespace MMS_Lab_1
{
    public partial class frmMain : Form
    {
        private IController _controller;
        private IView _view;
        private String currentImgPath;
        private bool imageOnlyView;
        private bool histogramView;
        private bool allConvolutionsView;

        public frmMain()
        {
            InitializeComponent();
            /*var process = Process.GetCurrentProcess();
            process.ProcessorAffinity = new IntPtr(0x000F);*/
            _controller = new Controller();
            _view = new ImageOnlyView();
            _view.SetParentForm(this);
            _controller.AttachView(_view);
            _controller.SetUnsafeMode(true);
            imageOnlyView = true;
            this.imageOnlyToolStripMenuItem.Checked = true;
            this.allChannelsToolStripMenuItem.Checked = false;
            this.allChannelsHistogramsToolStripMenuItem.Checked = false;
            this.onToolStripMenuItem.Checked = true;
            this.offToolStripMenuItem.Checked = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_controller.GetImageFromModel() != null)
            {
                DialogResult res = MessageBox.Show("Do you want to save the file?", "Save image", MessageBoxButtons.YesNoCancel);
                if (res == DialogResult.Yes)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "Image files(*.bmp; *.jpg; *.png)|*.bmp;*.jpg;*png|All files (*.*)|*.*";
                    sfd.ShowDialog();
                    _controller.Save(sfd.FileName);
                    Application.Exit();
                }
                if (res == DialogResult.No)
                {
                    Application.Exit();
                }
            }
            else
            {
                Application.Exit();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_controller.GetImageFromModel() != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Image files(*.bmp; *.jpg; *.png)|*.bmp;*.jpg;*png|All files (*.*)|*.*";
                sfd.ShowDialog();
                _controller.Save(sfd.FileName);
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files(*.bmp; *.jpg; *.png)|*.bmp;*.jpg;*png|All files (*.*)|*.*";
            DialogResult res = ofd.ShowDialog();

            this.imageOnlyToolStripMenuItem.Checked = false;
            this.allChannelsToolStripMenuItem.Checked = false;
            this.allChannelsHistogramsToolStripMenuItem.Checked = false;
            this.downsampleToolStripMenuItem.Checked = false;


            if (res != DialogResult.Cancel)
            {
                currentImgPath = ofd.FileName;
            }
            Bitmap bmp = _controller.Load(currentImgPath);
            if (!histogramView)
            {
                _controller.SetViewImage(bmp);
            }
            else
            {
                _controller.SetViewHistograms(bmp);
            }
            //Invalidate();
        }

        private void loadCustomImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Compressed Image File(*.cif) | *.cif";
            DialogResult res = ofd.ShowDialog();

            this.imageOnlyToolStripMenuItem.Checked = false;
            this.allChannelsToolStripMenuItem.Checked = false;
            this.allChannelsHistogramsToolStripMenuItem.Checked = false;
            this.downsampleToolStripMenuItem.Checked = false;

            if (res != DialogResult.Cancel)
            {
                currentImgPath = ofd.FileName;
            }
            
            Bitmap bmp = _controller.LoadDownsampledImage(currentImgPath);
            if (!histogramView)
            {
                _controller.SetViewImage(bmp);
            }
            else
            {
                _controller.SetViewHistograms(bmp);
            }
        }

        private void onToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _controller.SetUnsafeMode(true);
            this.onToolStripMenuItem.Checked = true;
            this.offToolStripMenuItem.Checked = false;
        }

        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _controller.SetUnsafeMode(false);
            this.onToolStripMenuItem.Checked = false;
            this.offToolStripMenuItem.Checked = true;
        }

        private void imageOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!imageOnlyView)
            {
                _view.SetVisibility(false);
                _view = new ImageOnlyView();
                _view.SetParentForm(this);
                _controller.AttachView(_view);
                imageOnlyView = true;
                histogramView = false;
                allConvolutionsView = false;
                Bitmap bmp;
                if ((bmp = _controller.GetImageFromModel()) != null)
                {

                    _controller.SetViewImage(bmp);

                }
                this.imageOnlyToolStripMenuItem.Checked = true;
                this.allChannelsToolStripMenuItem.Checked = false;
                this.allChannelsHistogramsToolStripMenuItem.Checked = false;
                this.downsampleToolStripMenuItem.Checked = false;
            }
        }

        private void allChannelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageOnlyView || histogramView)
            {
                _view.SetVisibility(false);
                _view = new AllChannelsView();
                _view.SetParentForm(this);
                _controller.AttachView(_view);
                imageOnlyView = false;
                histogramView = false;
                allConvolutionsView = false;
                Bitmap bmp;
                if ((bmp = _controller.GetImageFromModel()) != null)
                {
                    _controller.SetViewImage(bmp);

                }
                this.imageOnlyToolStripMenuItem.Checked = false;
                this.allChannelsToolStripMenuItem.Checked = true;
                this.allChannelsHistogramsToolStripMenuItem.Checked = false;
                this.downsampleToolStripMenuItem.Checked = false;
            }
        }

        private void invertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_controller.GetImageFromModel() != null)
            {
                Thread t = new Thread(() =>
                {
                    if (!histogramView)
                    {
                        _controller.SetViewImage(_controller.InvertImage());
                    }
                    else
                    {
                        _controller.SetViewHistograms(_controller.InvertImage());
                    }
                });
                t.Start();
            }
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_controller.GetImageFromModel() != null)
            {
                int r = 0, g = 0, b = 0;
                frmColors form = new frmColors();
                DialogResult result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    r = form.red;
                    g = form.green;
                    b = form.blue;
                }
                Thread t = new Thread(() =>
                {
                    if (!histogramView)
                    {
                        _controller.SetViewImage(_controller.ColorsFilter(r, g, b));
                    }
                    else
                    {
                        _controller.SetViewHistograms(_controller.ColorsFilter(r, g, b));
                    }

                });
                t.Start();
            }
        }

        private void x3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_controller.GetImageFromModel() != null)
            {
                Thread t = new Thread(() =>
                {
                    if (!histogramView)
                    {
                        _controller.SetViewImage(_controller.MeanFilter3x3());
                    }
                    else
                    {
                        _controller.SetViewHistograms(_controller.MeanFilter3x3());
                    }

                });
                t.Start();
            }
        }

        private void x5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_controller.GetImageFromModel() != null)
            {
                Thread t = new Thread(() =>
                {
                    if (!histogramView)
                    {
                        _controller.SetViewImage(_controller.MeanFilter5x5());
                    }
                    else
                    {
                        _controller.SetViewHistograms(_controller.MeanFilter5x5());
                    }
                });
                t.Start();
            }
        }

        private void x7ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_controller.GetImageFromModel() != null)
            {
                Thread t = new Thread(() =>
                {
                    if (!histogramView)
                    {
                        _controller.SetViewImage(_controller.MeanFilter7x7());
                    }
                    else
                    {
                        _controller.SetViewHistograms(_controller.MeanFilter7x7());
                    }

                });
                t.Start();
            }
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageOnlyView || histogramView)
            {
                _view.SetVisibility(false);
                _view = new AllChannelsView();
                _view.SetParentForm(this);
                _controller.AttachView(_view);
                imageOnlyView = false;
                histogramView = false;
                allConvolutionsView = true;

                Bitmap bmp;
                if ((bmp = _controller.GetImageFromModel()) != null)
                {

                    _controller.SetViewAllConvolutionsMean(bmp);

                }
                this.imageOnlyToolStripMenuItem.Checked = false;
                //this.allChannelsToolStripMenuItem.Checked = true;
                this.allChannelsHistogramsToolStripMenuItem.Checked = false;
                this.downsampleToolStripMenuItem.Checked = false;
            }
        }

        private void allChannelsHistogramsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageOnlyView || !histogramView)
            {
                _view.SetVisibility(false);
                _view = new AllChannelsView();
                _view.SetParentForm(this);
                _controller.AttachView(_view);
                imageOnlyView = false;
                histogramView = true;
                allConvolutionsView = false;
                Bitmap bmp;
                if ((bmp = _controller.GetImageFromModel()) != null)
                {

                    _controller.SetViewHistograms(bmp);

                }
                this.imageOnlyToolStripMenuItem.Checked = false;
                this.allChannelsHistogramsToolStripMenuItem.Checked = true;
                this.allChannelsToolStripMenuItem.Checked = false;
                this.downsampleToolStripMenuItem.Checked = false;
            }
        }

        private void edgeDetectHomogenityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_controller.GetImageFromModel() != null)
            {
                byte nThreshold = 0;
                frmEdgeDetectFactor form = new frmEdgeDetectFactor();
                DialogResult result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    nThreshold = form.Threshold;
                }
                Thread t = new Thread(() =>
                {
                    if (!histogramView)
                    {
                        _controller.SetViewImage(_controller.EdgeDetectHomogenity(nThreshold));
                    }
                    else
                    {
                        _controller.SetViewHistograms(_controller.EdgeDetectHomogenity(nThreshold));
                    }

                });
                t.Start();
            }
        }

        private void timeWarpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_controller.GetImageFromModel() != null)
            {
                byte factor = 0;
                bool smoothing = true;
                frmTimeWarp form = new frmTimeWarp();
                DialogResult result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    factor = form.factor;
                    smoothing = form.smoothing;
                }
                Thread t = new Thread(() =>
                {
                    if (!histogramView)
                    {
                        _controller.SetViewImage(_controller.TimeWarp(factor, smoothing));
                    }
                    else
                    {
                        _controller.SetViewHistograms(_controller.TimeWarp(factor, smoothing));
                    }

                });
                t.Start();
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_controller.GetImageFromModel() != null)
            {
                Thread t = new Thread(() =>
                {
                    if (allConvolutionsView)
                    {
                        ((Controller)_controller).Undo();
                        ((Controller)_controller).Undo();
                        ((Controller)_controller).Undo();
                        ((Controller)_controller).Undo();
                        _controller.SetViewAllConvolutionsMean(_controller.GetImageFromModel());
                    }
                    else
                    {
                        ((Controller)_controller).Undo();
                        if (!histogramView)
                        {
                            _controller.SetViewImage(_controller.GetImageFromModel());
                        }
                        else
                        {
                            _controller.SetViewHistograms(_controller.GetImageFromModel());
                        }
                    }
                });
                t.Start();
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_controller.GetImageFromModel() != null)
            {
                Thread t = new Thread(() =>
                {
                    if (allConvolutionsView)
                    {
                        ((Controller)_controller).Redo();
                        ((Controller)_controller).Redo();
                        ((Controller)_controller).Redo();
                        ((Controller)_controller).Redo();
                        _controller.SetViewAllConvolutionsMean(_controller.GetImageFromModel());
                    }
                    else
                    {
                        ((Controller)_controller).Redo();
                        if (!histogramView)
                        {
                            _controller.SetViewImage(_controller.GetImageFromModel());
                        }
                        else
                        {
                            _controller.SetViewHistograms(_controller.GetImageFromModel());
                        }
                    }

                });
                t.Start();
            }
        }

        private void histogramAveragesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_controller.GetImageFromModel() != null)
            {
                int grNo = 1;
                frmHistogramAverages form = new frmHistogramAverages();
                DialogResult res = form.ShowDialog();
                if (res == DialogResult.OK)
                {
                    grNo = form.groupNumber;
                }
                Thread t = new Thread(() =>
                {
                    if (!histogramView)
                    {
                        _controller.SetViewImage(_controller.HistogramAverages(grNo));
                    }
                    else
                    {
                        _controller.SetViewHistograms(_controller.HistogramAverages(grNo));
                    }
                });
                t.Start();
            }
        }

        private void downsampleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_controller.GetImageFromModel() != null)
            {
                if (imageOnlyView || histogramView)
                {
                    _view.SetVisibility(false);
                    _view = new AllChannelsView();
                    _view.SetParentForm(this);
                    _controller.AttachView(_view);
                    imageOnlyView = false;
                    histogramView = false;
                    allConvolutionsView = false;
                    _controller.SetViewDownsampledImage(_controller.GetImageFromModel());
                    this.imageOnlyToolStripMenuItem.Checked = false;
                    this.allChannelsHistogramsToolStripMenuItem.Checked = false;
                    this.allChannelsToolStripMenuItem.Checked = false;
                    this.downsampleToolStripMenuItem.Checked = true;
                }
                frmChooseDownsampledImage form = new frmChooseDownsampledImage();
                this.SetDesktopLocation(0, 0);
                DialogResult res = form.ShowDialog();

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Compressed Image File(*.cif)|*.cif";
                sfd.ShowDialog();

                if (res == DialogResult.OK)
                {
                    _controller.SaveDownsampledImage(sfd.FileName, (int)form.Choice);
                }
            }
        }


    }
}
