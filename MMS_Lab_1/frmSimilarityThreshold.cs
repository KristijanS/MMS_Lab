using System;
using System.Windows.Forms;

namespace MMS_Lab_1
{
    public partial class frmSimilarityThreshold : Form
    {
        public double similarityThreshold { get; set; }

        public frmSimilarityThreshold()
        {
            InitializeComponent();
            similarityThreshold = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            similarityThreshold = (double)numericUpDown1.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
