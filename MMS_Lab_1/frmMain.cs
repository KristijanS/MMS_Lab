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
        public IController _controller;
        public IView _view;
        private String currentImgPath;
        private bool imageOnlyView;
        private bool histogramView;
        private bool allConvolutionsView;
        private bool audioView;

        private delegate void DisableControls();
        private delegate void EnableControls();

        private DisableControls disableDelegate;
        private EnableControls enableDelegate;

        public frmMain()
        {
            InitializeComponent();
            disableDelegate = new DisableControls(DisableImageControls);
            enableDelegate = new EnableControls(EnableImageControls);
            _controller = new ImageController();
            _view = new ImageOnlyView();
            _view.SetParentForm(this);
            ((ImageController)_controller).AttachView((IImageView)_view);
            ((ImageController)_controller).SetUnsafeMode(true);
            imageOnlyView = true;
            this.imageOnlyToolStripMenuItem.Checked = true;
            this.allChannelsToolStripMenuItem.Checked = false;
            this.allChannelsHistogramsToolStripMenuItem.Checked = false;
            this.onToolStripMenuItem.Checked = true;
            this.offToolStripMenuItem.Checked = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageOnlyView || histogramView || allConvolutionsView)
            {
                if (((ImageController)_controller).GetImageFromModel() != null)
                {
                    DialogResult res = MessageBox.Show("Do you want to save the file?", "Save image", MessageBoxButtons.YesNoCancel);
                    if (res == DialogResult.Yes)
                    {
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "Image files(*.bmp; *.jpg; *.png)|*.bmp;*.jpg;*png|All files (*.*)|*.*";
                        sfd.ShowDialog();
                        ((ImageController)_controller).Save(sfd.FileName);
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
            else
            {
                Application.Exit();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((ImageController)_controller).GetImageFromModel() != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Image files(*.bmp; *.jpg; *.png)|*.bmp;*.jpg;*png|All files (*.*)|*.*";
                sfd.ShowDialog();
                ((ImageController)_controller).Save(sfd.FileName);
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (audioView)
            {
                if(_controller != null && _controller is AudioController)
                {
                    ((AudioController)_controller).Stop();
                }
                _controller = new ImageController();
                _view.SetVisibility(false);
                _view = new ImageOnlyView();
                _view.SetParentForm(this);
                _view.SetVisibility(true);
                ((ImageController)_controller).AttachView((IImageView)_view);
                ((ImageController)_controller).SetUnsafeMode(true);
                imageOnlyView = true;
                audioView = false;
                Invoke(enableDelegate);
                this.imageOnlyToolStripMenuItem.Checked = true;
                this.allChannelsToolStripMenuItem.Checked = false;
                this.allChannelsHistogramsToolStripMenuItem.Checked = false;
                this.onToolStripMenuItem.Checked = true;
                this.offToolStripMenuItem.Checked = false;
            }
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
            else
            {
                return;
            }
            Bitmap bmp = ((ImageController)_controller).Load(currentImgPath);
            if (!histogramView)
            {
                ((ImageController)_controller).SetViewImage(bmp);
            }
            else
            {
                ((ImageController)_controller).SetViewHistograms(bmp);
            }
            //Invalidate();
        }

        private void loadCustomImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_controller != null && _controller is AudioController)
            {
                ((AudioController)_controller).Stop();
            }
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

            Bitmap bmp = ((ImageController)_controller).LoadDownsampledImage(currentImgPath);
            if (!histogramView)
            {
                ((ImageController)_controller).SetViewImage(bmp);
            }
            else
            {
                ((ImageController)_controller).SetViewHistograms(bmp);
            }
        }

        private void onToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((ImageController)_controller).SetUnsafeMode(true);
            this.onToolStripMenuItem.Checked = true;
            this.offToolStripMenuItem.Checked = false;
        }

        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((ImageController)_controller).SetUnsafeMode(false);
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
                ((ImageController)_controller).AttachView((IImageView)_view);
                imageOnlyView = true;
                histogramView = false;
                allConvolutionsView = false;
                Bitmap bmp;
                if ((bmp = ((ImageController)_controller).GetImageFromModel()) != null)
                {

                    ((ImageController)_controller).SetViewImage(bmp);

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
                ((ImageController)_controller).AttachView((IImageView)_view);
                imageOnlyView = false;
                histogramView = false;
                allConvolutionsView = false;
                Bitmap bmp;
                if ((bmp = ((ImageController)_controller).GetImageFromModel()) != null)
                {
                    ((ImageController)_controller).SetViewImage(bmp);

                }
                this.imageOnlyToolStripMenuItem.Checked = false;
                this.allChannelsToolStripMenuItem.Checked = true;
                this.allChannelsHistogramsToolStripMenuItem.Checked = false;
                this.downsampleToolStripMenuItem.Checked = false;
            }
        }

        private void invertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((ImageController)_controller).GetImageFromModel() != null)
            {
                Thread t = new Thread(() =>
                {
                    Invoke(disableDelegate);
                    if (!histogramView)
                    {
                        ((ImageController)_controller).SetViewImage(((ImageController)_controller).InvertImage());
                    }
                    else
                    {
                        ((ImageController)_controller).SetViewHistograms(((ImageController)_controller).InvertImage());
                    }
                    Invoke(enableDelegate);
                });
                t.Start();
            }
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((ImageController)_controller).GetImageFromModel() != null)
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
                    Invoke(disableDelegate);
                    if (!histogramView)
                    {
                        ((ImageController)_controller).SetViewImage(((ImageController)_controller).ColorsFilter(r, g, b));
                    }
                    else
                    {
                        ((ImageController)_controller).SetViewHistograms(((ImageController)_controller).ColorsFilter(r, g, b));
                    }
                    Invoke(enableDelegate);
                });
                t.Start();
            }
        }

        private void x3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((ImageController)_controller).GetImageFromModel() != null)
            {
                Thread t = new Thread(() =>
                {
                    Invoke(disableDelegate);
                    if (!histogramView)
                    {
                        ((ImageController)_controller).SetViewImage(((ImageController)_controller).MeanFilter3x3());
                    }
                    else
                    {
                        ((ImageController)_controller).SetViewHistograms(((ImageController)_controller).MeanFilter3x3());
                    }
                    Invoke(enableDelegate);
                });
                t.Start();
            }
        }

        private void x5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((ImageController)_controller).GetImageFromModel() != null)
            {
                Thread t = new Thread(() =>
                {
                    Invoke(disableDelegate);
                    if (!histogramView)
                    {
                        ((ImageController)_controller).SetViewImage(((ImageController)_controller).MeanFilter5x5());
                    }
                    else
                    {
                        ((ImageController)_controller).SetViewHistograms(((ImageController)_controller).MeanFilter5x5());
                    }
                    Invoke(enableDelegate);
                });
                t.Start();
            }
        }

        private void x7ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((ImageController)_controller).GetImageFromModel() != null)
            {
                Thread t = new Thread(() =>
                {
                    Invoke(disableDelegate);
                    if (!histogramView)
                    {
                        ((ImageController)_controller).SetViewImage(((ImageController)_controller).MeanFilter7x7());
                    }
                    else
                    {
                        ((ImageController)_controller).SetViewHistograms(((ImageController)_controller).MeanFilter7x7());
                    }
                    Invoke(enableDelegate);
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
                ((ImageController)_controller).AttachView((IImageView)_view);
                imageOnlyView = false;
                histogramView = false;
                allConvolutionsView = true;

                Bitmap bmp;
                if ((bmp = ((ImageController)_controller).GetImageFromModel()) != null)
                {
                    Invoke(disableDelegate);
                    ((ImageController)_controller).SetViewAllConvolutionsMean(bmp);
                    Invoke(enableDelegate);
                }
                this.imageOnlyToolStripMenuItem.Checked = false;
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
                ((ImageController)_controller).AttachView((IImageView)_view);
                imageOnlyView = false;
                histogramView = true;
                allConvolutionsView = false;
                Bitmap bmp;
                if ((bmp = ((ImageController)_controller).GetImageFromModel()) != null)
                {
                    Invoke(disableDelegate);
                    ((ImageController)_controller).SetViewHistograms(bmp);
                    Invoke(enableDelegate);
                }
                this.imageOnlyToolStripMenuItem.Checked = false;
                this.allChannelsHistogramsToolStripMenuItem.Checked = true;
                this.allChannelsToolStripMenuItem.Checked = false;
                this.downsampleToolStripMenuItem.Checked = false;
            }
        }

        private void edgeDetectHomogenityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((ImageController)_controller).GetImageFromModel() != null)
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
                    Invoke(disableDelegate);
                    if (!histogramView)
                    {
                        ((ImageController)_controller).SetViewImage(((ImageController)_controller).EdgeDetectHomogenity(nThreshold));
                    }
                    else
                    {
                        ((ImageController)_controller).SetViewHistograms(((ImageController)_controller).EdgeDetectHomogenity(nThreshold));
                    }
                    Invoke(enableDelegate);
                });
                t.Start();
            }
        }

        private void timeWarpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((ImageController)_controller).GetImageFromModel() != null)
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
                    Invoke(disableDelegate);
                    if (!histogramView)
                    {
                        ((ImageController)_controller).SetViewImage(((ImageController)_controller).TimeWarp(factor, smoothing));
                    }
                    else
                    {
                        ((ImageController)_controller).SetViewHistograms(((ImageController)_controller).TimeWarp(factor, smoothing));
                    }
                    Invoke(enableDelegate);
                });
                t.Start();
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((ImageController)_controller).GetImageFromModel() != null)
            {
                Thread t = new Thread(() =>
                {
                    if (allConvolutionsView)
                    {
                        ((ImageController)((ImageController)_controller)).Undo();
                        ((ImageController)((ImageController)_controller)).Undo();
                        ((ImageController)((ImageController)_controller)).Undo();
                        ((ImageController)((ImageController)_controller)).Undo();
                        ((ImageController)_controller).SetViewAllConvolutionsMean(((ImageController)_controller).GetImageFromModel());
                    }
                    else
                    {
                        ((ImageController)((ImageController)_controller)).Undo();
                        if (!histogramView)
                        {
                            ((ImageController)_controller).SetViewImage(((ImageController)_controller).GetImageFromModel());
                        }
                        else
                        {
                            ((ImageController)_controller).SetViewHistograms(((ImageController)_controller).GetImageFromModel());
                        }
                    }
                });
                t.Start();
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((ImageController)_controller).GetImageFromModel() != null)
            {
                Thread t = new Thread(() =>
                {
                    if (allConvolutionsView)
                    {
                        ((ImageController)((ImageController)_controller)).Redo();
                        ((ImageController)((ImageController)_controller)).Redo();
                        ((ImageController)((ImageController)_controller)).Redo();
                        ((ImageController)((ImageController)_controller)).Redo();
                        ((ImageController)_controller).SetViewAllConvolutionsMean(((ImageController)_controller).GetImageFromModel());
                    }
                    else
                    {
                        ((ImageController)((ImageController)_controller)).Redo();
                        if (!histogramView)
                        {
                            ((ImageController)_controller).SetViewImage(((ImageController)_controller).GetImageFromModel());
                        }
                        else
                        {
                            ((ImageController)_controller).SetViewHistograms(((ImageController)_controller).GetImageFromModel());
                        }
                    }

                });
                t.Start();
            }
        }

        private void histogramAveragesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((ImageController)_controller).GetImageFromModel() != null)
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
                    Invoke(disableDelegate);
                    if (!histogramView)
                    {
                        ((ImageController)_controller).SetViewImage(((ImageController)_controller).HistogramAverages(grNo));
                    }
                    else
                    {
                        ((ImageController)_controller).SetViewHistograms(((ImageController)_controller).HistogramAverages(grNo));
                    }
                    Invoke(enableDelegate);
                });
                t.Start();
            }
        }

        private void downsampleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((ImageController)_controller).GetImageFromModel() != null)
            {
                if (imageOnlyView || histogramView)
                {
                    _view.SetVisibility(false);
                    _view = new AllChannelsView();
                    _view.SetParentForm(this);
                    ((ImageController)_controller).AttachView((IImageView)_view);
                    imageOnlyView = false;
                    histogramView = false;
                    allConvolutionsView = false;
                    Invoke(disableDelegate);
                    ((ImageController)_controller).SetViewDownsampledImage(((ImageController)_controller).GetImageFromModel());
                    Invoke(enableDelegate);
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
                    ((ImageController)_controller).SaveDownsampledImage(sfd.FileName, (int)form.Choice);
                }
            }
        }

        private void loadAudioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _controller = new AudioController();
            _view.SetVisibility(false);
            _view = new AudioView();
            imageOnlyView = histogramView = allConvolutionsView = false;
            audioView = true;
            Invoke(disableDelegate);
            _view.SetParentForm(this);
            _view.SetVisibility(true);
            ((AudioController)_controller).AttachView(_view);
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "WAV Files(*.wav)|*.wav";

            DialogResult res = ofd.ShowDialog();

            if (res == DialogResult.OK)
            {
                ((AudioController)_controller).Load(ofd.FileName);
            }
        }

        private void DisableImageControls()
        {
            this.x7ToolStripMenuItem.Enabled = false;
            this.x5ToolStripMenuItem.Enabled = false;
            this.x3ToolStripMenuItem.Enabled = false;
            this.unsafeOperationToolStripMenuItem.Enabled = false;
            this.allChannelsHistogramsToolStripMenuItem.Enabled = false;
            this.allChannelsToolStripMenuItem.Enabled = false;
            this.allToolStripMenuItem.Enabled = false;
            this.channelsToolStripMenuItem.Enabled = false;
            this.colorToolStripMenuItem.Enabled = false;
            this.downsampleToolStripMenuItem.Enabled = false;
            this.edgeDetectHomogenityToolStripMenuItem.Enabled = false;
            this.editToolStripMenuItem.Enabled = false;
            this.filtersToolStripMenuItem.Enabled = false;
            this.histogramAveragesToolStripMenuItem.Enabled = false;
            this.imageOnlyToolStripMenuItem.Enabled = false;
            this.invertToolStripMenuItem.Enabled = false;
            this.meanRemovalToolStripMenuItem.Enabled = false;
            this.offToolStripMenuItem.Enabled = false;
            this.onToolStripMenuItem.Enabled = false;
            this.optionsToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Enabled = false;
            this.timeWarpToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Enabled = false;
            this.colorSimilarityToolStripMenuItem.Enabled = false;
        }

        private void EnableImageControls()
        {
            this.x7ToolStripMenuItem.Enabled = true;
            this.x5ToolStripMenuItem.Enabled = true;
            this.x3ToolStripMenuItem.Enabled = true;
            this.unsafeOperationToolStripMenuItem.Enabled = true;
            this.allChannelsHistogramsToolStripMenuItem.Enabled = true;
            this.allChannelsToolStripMenuItem.Enabled = true;
            this.allToolStripMenuItem.Enabled = true;
            this.channelsToolStripMenuItem.Enabled = true;
            this.colorToolStripMenuItem.Enabled = true;
            this.downsampleToolStripMenuItem.Enabled = true;
            this.edgeDetectHomogenityToolStripMenuItem.Enabled = true;
            this.editToolStripMenuItem.Enabled = true;
            this.filtersToolStripMenuItem.Enabled = true;
            this.histogramAveragesToolStripMenuItem.Enabled = true;
            this.imageOnlyToolStripMenuItem.Enabled = true;
            this.invertToolStripMenuItem.Enabled = true;
            this.meanRemovalToolStripMenuItem.Enabled = true;
            this.offToolStripMenuItem.Enabled = true;
            this.onToolStripMenuItem.Enabled = true;
            this.optionsToolStripMenuItem.Enabled = true;
            this.redoToolStripMenuItem.Enabled = true;
            this.saveToolStripMenuItem.Enabled = true;
            this.timeWarpToolStripMenuItem.Enabled = true;
            this.undoToolStripMenuItem.Enabled = true;
            this.colorSimilarityToolStripMenuItem.Enabled = true;
        }

        private void colorSimilarityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((ImageController)_controller).GetImageFromModel() != null)
            {
                frmSimilarityThreshold form = new frmSimilarityThreshold();
                DialogResult res = form.ShowDialog();

                double similarityThreshold = 0.0;
                if (res == DialogResult.OK)
                {
                    similarityThreshold = form.similarityThreshold;
                    ColorDialog cd = new ColorDialog();
                    cd.AllowFullOpen = true;
                    cd.ShowHelp = false;
                    cd.FullOpen = true;

                    Color resultingColor;
                    if (cd.ShowDialog() == DialogResult.OK)
                    {
                        resultingColor = cd.Color;
                        Point start;
                        frmSimilarityStartLocation formStart = new frmSimilarityStartLocation(((ImageController)_controller).GetImageFromModel());
                        DialogResult resStart = formStart.ShowDialog();
                        if (resStart == DialogResult.OK)
                        {
                            start = formStart.startLocation;
                            Thread t = new Thread(() =>
                            {
                                Invoke(disableDelegate);
                                if (!histogramView)
                                {
                                    ((ImageController)_controller).SetViewImage(((ImageController)_controller).ColorSimilarityFilter(resultingColor, start, similarityThreshold));
                                }
                                else
                                {
                                    ((ImageController)_controller).SetViewHistograms(((ImageController)_controller).ColorSimilarityFilter(resultingColor, start, similarityThreshold));
                                }
                                Invoke(enableDelegate);
                            });
                            t.Start();
                        }
                    }
                }
            }
        }
    }
}
