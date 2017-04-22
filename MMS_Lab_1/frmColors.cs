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
    public partial class frmColors : Form
    {
        public int red { get; set; }
        public int green { get; set; }
        public int blue { get; set; }

        public frmColors()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            red = (int)numericUpDown1.Value;
            green = (int)numericUpDown2.Value;
            blue = (int)numericUpDown3.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
