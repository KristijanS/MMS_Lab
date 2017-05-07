using System;
using System.Drawing;
using System.Windows.Forms;

namespace MMS_Lab_1.Views
{
    public class AllChannelsView : IImageView, IChannelView
    {
        private TableLayoutPanel channelsPanel;
        private PictureBox image;
        private PictureBox channel_R;
        private PictureBox channel_G;
        private PictureBox channel_B;
        private Form parent;

        public AllChannelsView()
        {
            channelsPanel = new TableLayoutPanel();
            channelsPanel.Dock = DockStyle.Fill;
            channelsPanel.AutoSize = true;

            image = new PictureBox();
            channel_R = new PictureBox();
            channel_G = new PictureBox();
            channel_B = new PictureBox();

            image.Image = channel_R.Image = channel_G.Image = channel_B.Image = new Bitmap(MMS.Properties.Resources.PlaceholderImg);

            channelsPanel.Controls.Add(image, 0, 0);
            channelsPanel.Controls.Add(channel_R, 0, 1);
            channelsPanel.Controls.Add(channel_G, 1, 0);
            channelsPanel.Controls.Add(channel_B, 1, 1);
        }

        public void SetVisibility(bool value)
        {
            channelsPanel.Visible = value;
            image.Visible = value;
            channel_R.Visible = value;
            channel_G.Visible = value;
            channel_B.Visible = value;
        }

        public Bitmap GetImage()
        {
            return (Bitmap)image.Image;
        }

        public void SetImage(Bitmap bmp)
        {
            image.Dock = DockStyle.Fill;
            image.SizeMode = PictureBoxSizeMode.AutoSize;
            image.Parent = channelsPanel;

            Rectangle rect = Screen.PrimaryScreen.Bounds;

            Rectangle newSize = new Rectangle();

            if (rect.Width < bmp.Width * 2)
            {
                newSize.Width = rect.Width / 2;
            }
            else
            {
                newSize.Width = bmp.Width;
            }
            if (rect.Height < bmp.Height * 2)
            {
                newSize.Height = rect.Height / 2;
            }
            else
            {
                newSize.Height = bmp.Height;
            }
            Size size = new Size(newSize.Width, newSize.Height);

            if (image.InvokeRequired)
            {
                image.Invoke(new Action(() => image.Image = new Bitmap(bmp, size)));
            }
            else
            {
                image.Image = new Bitmap(bmp, size);
            }
            if (channelsPanel.InvokeRequired)
            {
                channelsPanel.Invoke(new Action(() => { channelsPanel.AutoSize = true; channelsPanel.Dock = DockStyle.Fill; }));
            }
            else
            {
                channelsPanel.AutoSize = true;
                channelsPanel.Dock = DockStyle.Fill;
            }
            if (parent.InvokeRequired)
            {
                parent.Invoke(new Action(() => { parent.AutoSize = true; parent.FormBorderStyle = FormBorderStyle.FixedSingle; }));
            }
            else
            {
                parent.AutoSize = true;
                parent.FormBorderStyle = FormBorderStyle.FixedSingle;
            }
        }

        public void SetParentForm(Form f)
        {
            parent = f;
            channelsPanel.Parent = parent;
            parent.Controls.Add(channelsPanel);
        }

        public void SetChannelX(Bitmap bmp)
        {
            channel_R.Dock = DockStyle.Fill;
            channel_R.SizeMode = PictureBoxSizeMode.AutoSize;
            channel_R.Parent = channelsPanel;

            Rectangle rect = Screen.PrimaryScreen.Bounds;

            Rectangle newSize = new Rectangle();

            if (rect.Width < bmp.Width * 2)
            {
                newSize.Width = rect.Width / 2;
            }
            else
            {
                newSize.Width = bmp.Width;
            }
            if (rect.Height < bmp.Height * 2)
            {
                newSize.Height = rect.Height / 2;
            }
            else
            {
                newSize.Height = bmp.Height;
            }
            Size size = new Size(newSize.Width, newSize.Height);

            if (channel_R.InvokeRequired)
            {
                channel_R.Invoke(new Action(() => channel_R.Image = new Bitmap(bmp, size)));
            }
            else
            {
                channel_R.Image = new Bitmap(bmp, size);
            }
        }

        public void SetChannelY(Bitmap bmp)
        {
            channel_G.Dock = DockStyle.Fill;
            channel_G.SizeMode = PictureBoxSizeMode.AutoSize;
            channel_G.Parent = channelsPanel;

            Rectangle rect = Screen.PrimaryScreen.Bounds;

            Rectangle newSize = new Rectangle();

            if (rect.Width < bmp.Width * 2)
            {
                newSize.Width = rect.Width / 2;
            }
            else
            {
                newSize.Width = bmp.Width;
            }
            if (rect.Height < bmp.Height * 2)
            {
                newSize.Height = rect.Height / 2;
            }
            else
            {
                newSize.Height = bmp.Height;
            }
            Size size = new Size(newSize.Width, newSize.Height);


            if (channel_G.InvokeRequired)
            {
                channel_G.Invoke(new Action(() => channel_G.Image = new Bitmap(bmp, size)));
            }
            else
            {
                channel_G.Image = new Bitmap(bmp, size);
            }
        }

        public void SetChannelZ(Bitmap bmp)
        {
            channel_B.Dock = DockStyle.Fill;
            channel_B.SizeMode = PictureBoxSizeMode.AutoSize;
            channel_B.Parent = channelsPanel;

            Rectangle rect = Screen.PrimaryScreen.Bounds;

            Rectangle newSize = new Rectangle();

            if (rect.Width < bmp.Width * 2)
            {
                newSize.Width = rect.Width / 2;
            }
            else
            {
                newSize.Width = bmp.Width;
            }
            if (rect.Height < bmp.Height * 2)
            {
                newSize.Height = rect.Height / 2;
            }
            else
            {
                newSize.Height = bmp.Height;
            }
            Size size = new Size(newSize.Width, newSize.Height);

            if (channel_B.InvokeRequired)
            {
                channel_B.Invoke(new Action(() => channel_B.Image = new Bitmap(bmp, size)));
            }
            else
            {
                channel_B.Image = new Bitmap(bmp, size);
            }
        }

        public Bitmap GetChannelX()
        {
            return (Bitmap)channel_R.Image;
        }

        public Bitmap GetChannelY()
        {
            return (Bitmap)channel_G.Image;
        }

        public Bitmap GetChannelZ()
        {
            return (Bitmap)channel_B.Image;
        }
    }
}
