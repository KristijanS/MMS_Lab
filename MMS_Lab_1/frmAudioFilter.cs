using System;
using System.Windows.Forms;

namespace MMS_Lab_1
{
    public partial class frmAudioFilter : Form
    {
        public int[] channelValues { get; set; }

        public frmAudioFilter(int channelNum)
        {
            InitializeComponent();
            if (channelNum > 8)
            {
                throw new Exception("Unsuported number of channels.");
            }
            channelValues = new int[channelNum];
            int i = 1;
            foreach (var num in this.Controls)
            {
                if (i <= channelNum && (num is NumericUpDown))
                {
                    ((NumericUpDown)num).Enabled = true;
                    i++;
                }
                else if (num is NumericUpDown)
                {
                    ((NumericUpDown)num).Enabled = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (var num in this.Controls)
            {
                if (num is NumericUpDown)
                {
                    if (((NumericUpDown)num).Enabled)
                    {
                        channelValues[i] = (int)((NumericUpDown)num).Value;
                        i++;
                    }
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
