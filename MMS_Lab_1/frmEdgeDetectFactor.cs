using System;
using System.Windows.Forms;

namespace MMS_Lab_1
{
    public partial class frmEdgeDetectFactor : Form
    {
        public byte Threshold { get; set; }

        public frmEdgeDetectFactor()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Threshold = (byte)this.numericUpDown1.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
