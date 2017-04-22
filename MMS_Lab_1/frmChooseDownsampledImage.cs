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
    public partial class frmChooseDownsampledImage : Form
    {
        public uint Choice { get; set; }

        public frmChooseDownsampledImage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                Choice = 1;//izabrano X:Y:Z 1:1:4
            }
            if (radioButton2.Checked)
            {
                Choice = 2;//izabrano X:Y:Z 1:4:1
            }
            if (radioButton3.Checked)
            {
                Choice = 3;//izabrano X:Y:Z 4:1:1
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
