using System;
using System.Drawing;
using System.Windows.Forms;
using MMS_Lab_1.Views;
using MMS_Lab_1.Controllers;

namespace MMS_Lab_1
{
    public partial class Form1 : Form
    {
        IController _controller;
        IView _view;
        String currentImgPath;
        bool imageOnlyView;

        public Form1()
        {
            InitializeComponent();
            _controller = new Controller();
            _view = new ImageOnlyView();
            _view.SetParentForm(this);
            _controller.AttachView(_view);
            _controller.SetUnsafeMode(true);
            imageOnlyView = true;
            this.imageOnlyToolStripMenuItem.Checked = true;
            this.allChannelsToolStripMenuItem.Checked = false;
            this.onToolStripMenuItem.Checked = true;
            this.offToolStripMenuItem.Checked = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_view.GetImage() != null)
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
            Application.Exit();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Image files(*.bmp; *.jpg; *.png)|*.bmp;*.jpg;*png|All files (*.*)|*.*";
            sfd.ShowDialog();
            _controller.Save(sfd.FileName);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files(*.bmp; *.jpg; *.png)|*.bmp;*.jpg;*png|All files (*.*)|*.*";
            ofd.ShowDialog();
            currentImgPath = ofd.FileName;
            Bitmap bmp = _controller.Load(currentImgPath);
            _controller.SetViewImage(bmp);
            Invalidate();
            Refresh();
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
                Bitmap bmp;
                if ((bmp = _controller.GetImageFromModel()) != null)
                {
                    _controller.SetViewImage(bmp);
                }
                this.imageOnlyToolStripMenuItem.Checked = true;
                this.allChannelsToolStripMenuItem.Checked = false;
                Invalidate();
            }
        }

        private void allChannelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageOnlyView)
            {
                _view.SetVisibility(false);
                _view = new AllChannelsView();
                _view.SetParentForm(this);
                _controller.AttachView(_view);
                imageOnlyView = false;
                Bitmap bmp;
                if ((bmp = _controller.GetImageFromModel()) != null)
                {
                    _controller.SetViewImage(bmp);
                }
                this.imageOnlyToolStripMenuItem.Checked = false;
                this.allChannelsToolStripMenuItem.Checked = true;
                Invalidate();
            }
        }

        private void invertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _controller.SetViewImage(_controller.InvertImage());
        }
    }
}
