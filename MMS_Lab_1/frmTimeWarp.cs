using System;
using System.Windows.Forms;

namespace MMS_Lab_1
{
    public partial class frmTimeWarp : Form
    {
        public byte factor { get; set; }
        public bool smoothing { get; set; }

        public frmTimeWarp()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            factor = (byte)this.numericUpDown1.Value;
            smoothing = this.checkBox1.Checked;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
