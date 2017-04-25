using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MMS_Lab_1
{
    public partial class frmSimilarityStartLocation : Form
    {
        public Point startLocation { get; set; }

        public frmSimilarityStartLocation(Bitmap bmp)
        {
            InitializeComponent();

            panel1.AutoScroll = true;

            Rectangle screen = Screen.PrimaryScreen.Bounds;
            Size size = new Size(screen.Width, screen.Height);

            pictureBox1.Image = bmp;
            pictureBox1.Parent = panel1;

            this.MaximumSize = size;
        }

        private void pictureBox1_MouseClick_1(object sender, MouseEventArgs e)
        {
            startLocation = e.Location;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
