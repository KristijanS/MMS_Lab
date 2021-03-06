﻿using MMS_Lab_1.Controllers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MMS_Lab_1.Views
{
    public class AudioView : IView
    {
        private TableLayoutPanel panel;
        private Button playButton;
        private Button stopButton;
        private Button filterButton;
        private Form parent;

        public AudioView()
        {
            panel = new TableLayoutPanel();
            playButton = new Button();
            stopButton = new Button();
            filterButton = new Button();

            playButton.Click += playButton_Click;
            stopButton.Click += stopButton_Click;
            filterButton.Click += filterButton_Click;

            panel.AutoSize = true;

            playButton.Image = new Bitmap(MMS.Properties.Resources.Play);
            stopButton.Image = new Bitmap(MMS.Properties.Resources.Button_Stop);
            filterButton.Text = "Apply Filter";

            playButton.Width = playButton.Image.Width;
            playButton.Height = playButton.Image.Height;

            stopButton.Width = stopButton.Image.Width;
            stopButton.Height = stopButton.Image.Height;

            filterButton.Width = 64;
            filterButton.Height = 64;

            panel.RowCount = 1;
            panel.ColumnCount = 3;
        }

        public void SetVisibility(bool value)
        {
            panel.Visible = value;
            playButton.Visible = value;
            stopButton.Visible = value;
            filterButton.Visible = value;
        }

        public void SetParentForm(Form f)
        {
            parent = f;
            panel.Parent = parent;
            panel.Location = new Point(0, 25);
            playButton.Parent = panel;
            stopButton.Parent = panel;
            filterButton.Parent = panel;
            parent.Controls.Add(panel);
            panel.Controls.Add(playButton, 1, 0);
            panel.Controls.Add(stopButton, 0, 0);
            panel.Controls.Add(filterButton, 2, 0);
            parent.AutoSize = true;
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            ((IAudioController)((frmMain)parent)._controller).Play();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            ((IAudioController)((frmMain)parent)._controller).Stop();
        }

        private void filterButton_Click(object sender, EventArgs e)
        {
            filterButton.Text = "Processing...";

            int channelNo = ((IAudioController)((frmMain)parent)._controller).GetNumberOfChannels();
            frmAudioFilter form = new frmAudioFilter(channelNo);

            DialogResult res = form.ShowDialog();
            ((IAudioController)((frmMain)parent)._controller).ApplyFilter(form.channelValues);
            filterButton.Text = "Apply filter.";
        }
    }
}
