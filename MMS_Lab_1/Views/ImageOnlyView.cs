using System;
using System.Drawing;
using System.Windows.Forms;

namespace MMS_Lab_1.Views
{
    public class ImageOnlyView : IView
    {
        PictureBox image;
        Panel imagePanel;
        Form parent;

        public ImageOnlyView()
        {
            imagePanel = new Panel();
            image = new PictureBox();
        }

        public void SetParentForm(Form f)
        {
            parent = f;
            imagePanel.Parent = parent;
        }

        public void SetImage(Bitmap bmp)
        {
            image.Dock = DockStyle.None;
            image.SizeMode = PictureBoxSizeMode.AutoSize;
            image.Anchor = (AnchorStyles.Top | AnchorStyles.Left);

            Rectangle screen = Screen.PrimaryScreen.Bounds;
            Size size = new Size(screen.Width, screen.Height);

            
            image.Parent = imagePanel;
            
            imagePanel.Dock = DockStyle.Fill;
            imagePanel.AutoSize = true;
            imagePanel.Anchor = (AnchorStyles.Top | AnchorStyles.Left);

            if (imagePanel.InvokeRequired)
            {
                imagePanel.Invoke(new Action(() => imagePanel.AutoScroll = true));
            }
            else
            {
                imagePanel.AutoScroll = true;
            }

            if(image.InvokeRequired)
            {
                image.Invoke(new Action(() => image.Image = bmp));
            }
            else
            {
                image.Image = bmp;
            }
            if (imagePanel.InvokeRequired)
            {
                imagePanel.Invoke(new Action(() => imagePanel.Controls.Add(image) ));
            }
            else
            {
                imagePanel.Controls.Add(image);
            }

            parent.AutoSize = true;
            
            parent.MaximumSize = size;
            
            if (parent.InvokeRequired)
            {
                parent.Invoke(new Action(() => parent.Controls.Add(imagePanel)));
                parent.Invoke(new Action(() => parent.AutoScroll = true));
                parent.Invoke(new Action(() => parent.FormBorderStyle = FormBorderStyle.FixedSingle));
            }
            else
            {
                parent.AutoScroll = true;
                parent.FormBorderStyle = FormBorderStyle.FixedSingle;
                parent.Controls.Add(imagePanel);
            }
        }

        public Bitmap GetImage()
        {
            return (Bitmap)image.Image;
        }

        public void SetVisibility(bool value)
        {
            imagePanel.Visible = value;
            image.Visible = value;
        }
    }
}
