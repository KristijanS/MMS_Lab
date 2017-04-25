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
            pictureBox1.Image = bmp;
            this.Size = pictureBox1.Image.Size;
        }

        private void pictureBox1_MouseClick_1(object sender, MouseEventArgs e)
        {
            startLocation = e.Location;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
