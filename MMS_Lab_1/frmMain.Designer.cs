namespace MMS_Lab_1
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadCustomImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadAudioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filtersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.channelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allChannelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allChannelsHistogramsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downsampleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.meanRemovalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x7ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.edgeDetectHomogenityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timeWarpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.histogramAveragesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unsafeOperationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.offToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorSimilarityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.filtersToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(484, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.loadCustomImageToolStripMenuItem,
            this.loadAudioToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // loadCustomImageToolStripMenuItem
            // 
            this.loadCustomImageToolStripMenuItem.Name = "loadCustomImageToolStripMenuItem";
            this.loadCustomImageToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.loadCustomImageToolStripMenuItem.Text = "Load Custom Image";
            this.loadCustomImageToolStripMenuItem.Click += new System.EventHandler(this.loadCustomImageToolStripMenuItem_Click);
            // 
            // loadAudioToolStripMenuItem
            // 
            this.loadAudioToolStripMenuItem.Name = "loadAudioToolStripMenuItem";
            this.loadAudioToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.loadAudioToolStripMenuItem.Text = "Load Audio";
            this.loadAudioToolStripMenuItem.Click += new System.EventHandler(this.loadAudioToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // filtersToolStripMenuItem
            // 
            this.filtersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.channelsToolStripMenuItem,
            this.colorToolStripMenuItem,
            this.meanRemovalToolStripMenuItem,
            this.invertToolStripMenuItem,
            this.edgeDetectHomogenityToolStripMenuItem,
            this.timeWarpToolStripMenuItem,
            this.histogramAveragesToolStripMenuItem,
            this.colorSimilarityToolStripMenuItem});
            this.filtersToolStripMenuItem.Name = "filtersToolStripMenuItem";
            this.filtersToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.filtersToolStripMenuItem.Text = "Filters";
            // 
            // channelsToolStripMenuItem
            // 
            this.channelsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.imageOnlyToolStripMenuItem,
            this.allChannelsToolStripMenuItem,
            this.allChannelsHistogramsToolStripMenuItem,
            this.downsampleToolStripMenuItem});
            this.channelsToolStripMenuItem.Name = "channelsToolStripMenuItem";
            this.channelsToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.channelsToolStripMenuItem.Text = "Channels";
            // 
            // imageOnlyToolStripMenuItem
            // 
            this.imageOnlyToolStripMenuItem.Name = "imageOnlyToolStripMenuItem";
            this.imageOnlyToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.imageOnlyToolStripMenuItem.Text = "Image only";
            this.imageOnlyToolStripMenuItem.Click += new System.EventHandler(this.imageOnlyToolStripMenuItem_Click);
            // 
            // allChannelsToolStripMenuItem
            // 
            this.allChannelsToolStripMenuItem.Name = "allChannelsToolStripMenuItem";
            this.allChannelsToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.allChannelsToolStripMenuItem.Text = "All Channels (CIE XYZ)";
            this.allChannelsToolStripMenuItem.Click += new System.EventHandler(this.allChannelsToolStripMenuItem_Click);
            // 
            // allChannelsHistogramsToolStripMenuItem
            // 
            this.allChannelsHistogramsToolStripMenuItem.Name = "allChannelsHistogramsToolStripMenuItem";
            this.allChannelsHistogramsToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.allChannelsHistogramsToolStripMenuItem.Text = "All Channels (Histograms)";
            this.allChannelsHistogramsToolStripMenuItem.Click += new System.EventHandler(this.allChannelsHistogramsToolStripMenuItem_Click);
            // 
            // downsampleToolStripMenuItem
            // 
            this.downsampleToolStripMenuItem.Name = "downsampleToolStripMenuItem";
            this.downsampleToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.downsampleToolStripMenuItem.Text = "Downsample";
            this.downsampleToolStripMenuItem.Click += new System.EventHandler(this.downsampleToolStripMenuItem_Click);
            // 
            // colorToolStripMenuItem
            // 
            this.colorToolStripMenuItem.Name = "colorToolStripMenuItem";
            this.colorToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.colorToolStripMenuItem.Text = "Color";
            this.colorToolStripMenuItem.Click += new System.EventHandler(this.colorToolStripMenuItem_Click);
            // 
            // meanRemovalToolStripMenuItem
            // 
            this.meanRemovalToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.x3ToolStripMenuItem,
            this.x5ToolStripMenuItem,
            this.x7ToolStripMenuItem,
            this.allToolStripMenuItem});
            this.meanRemovalToolStripMenuItem.Name = "meanRemovalToolStripMenuItem";
            this.meanRemovalToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.meanRemovalToolStripMenuItem.Text = "Mean Removal";
            // 
            // x3ToolStripMenuItem
            // 
            this.x3ToolStripMenuItem.Name = "x3ToolStripMenuItem";
            this.x3ToolStripMenuItem.Size = new System.Drawing.Size(91, 22);
            this.x3ToolStripMenuItem.Text = "3x3";
            this.x3ToolStripMenuItem.Click += new System.EventHandler(this.x3ToolStripMenuItem_Click);
            // 
            // x5ToolStripMenuItem
            // 
            this.x5ToolStripMenuItem.Name = "x5ToolStripMenuItem";
            this.x5ToolStripMenuItem.Size = new System.Drawing.Size(91, 22);
            this.x5ToolStripMenuItem.Text = "5x5";
            this.x5ToolStripMenuItem.Click += new System.EventHandler(this.x5ToolStripMenuItem_Click);
            // 
            // x7ToolStripMenuItem
            // 
            this.x7ToolStripMenuItem.Name = "x7ToolStripMenuItem";
            this.x7ToolStripMenuItem.Size = new System.Drawing.Size(91, 22);
            this.x7ToolStripMenuItem.Text = "7x7";
            this.x7ToolStripMenuItem.Click += new System.EventHandler(this.x7ToolStripMenuItem_Click);
            // 
            // allToolStripMenuItem
            // 
            this.allToolStripMenuItem.Name = "allToolStripMenuItem";
            this.allToolStripMenuItem.Size = new System.Drawing.Size(91, 22);
            this.allToolStripMenuItem.Text = "All";
            this.allToolStripMenuItem.Click += new System.EventHandler(this.allToolStripMenuItem_Click);
            // 
            // invertToolStripMenuItem
            // 
            this.invertToolStripMenuItem.Name = "invertToolStripMenuItem";
            this.invertToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.invertToolStripMenuItem.Text = "Invert";
            this.invertToolStripMenuItem.Click += new System.EventHandler(this.invertToolStripMenuItem_Click);
            // 
            // edgeDetectHomogenityToolStripMenuItem
            // 
            this.edgeDetectHomogenityToolStripMenuItem.Name = "edgeDetectHomogenityToolStripMenuItem";
            this.edgeDetectHomogenityToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.edgeDetectHomogenityToolStripMenuItem.Text = "Edge Detect Homogenity";
            this.edgeDetectHomogenityToolStripMenuItem.Click += new System.EventHandler(this.edgeDetectHomogenityToolStripMenuItem_Click);
            // 
            // timeWarpToolStripMenuItem
            // 
            this.timeWarpToolStripMenuItem.Name = "timeWarpToolStripMenuItem";
            this.timeWarpToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.timeWarpToolStripMenuItem.Text = "Time Warp";
            this.timeWarpToolStripMenuItem.Click += new System.EventHandler(this.timeWarpToolStripMenuItem_Click);
            // 
            // histogramAveragesToolStripMenuItem
            // 
            this.histogramAveragesToolStripMenuItem.Name = "histogramAveragesToolStripMenuItem";
            this.histogramAveragesToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.histogramAveragesToolStripMenuItem.Text = "Histogram Averages";
            this.histogramAveragesToolStripMenuItem.Click += new System.EventHandler(this.histogramAveragesToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.unsafeOperationToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // unsafeOperationToolStripMenuItem
            // 
            this.unsafeOperationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onToolStripMenuItem,
            this.offToolStripMenuItem});
            this.unsafeOperationToolStripMenuItem.Name = "unsafeOperationToolStripMenuItem";
            this.unsafeOperationToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.unsafeOperationToolStripMenuItem.Text = "Unsafe Operation";
            // 
            // onToolStripMenuItem
            // 
            this.onToolStripMenuItem.Name = "onToolStripMenuItem";
            this.onToolStripMenuItem.Size = new System.Drawing.Size(91, 22);
            this.onToolStripMenuItem.Text = "On";
            this.onToolStripMenuItem.Click += new System.EventHandler(this.onToolStripMenuItem_Click);
            // 
            // offToolStripMenuItem
            // 
            this.offToolStripMenuItem.Name = "offToolStripMenuItem";
            this.offToolStripMenuItem.Size = new System.Drawing.Size(91, 22);
            this.offToolStripMenuItem.Text = "Off";
            this.offToolStripMenuItem.Click += new System.EventHandler(this.offToolStripMenuItem_Click);
            // 
            // colorSimilarityToolStripMenuItem
            // 
            this.colorSimilarityToolStripMenuItem.Name = "colorSimilarityToolStripMenuItem";
            this.colorSimilarityToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.colorSimilarityToolStripMenuItem.Text = "Color Similarity";
            this.colorSimilarityToolStripMenuItem.Click += new System.EventHandler(this.colorSimilarityToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(484, 261);
            this.ControlBox = false;
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "MMS";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filtersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem channelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem meanRemovalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem invertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageOnlyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allChannelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unsafeOperationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem onToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem offToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x5ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x7ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem edgeDetectHomogenityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem timeWarpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allChannelsHistogramsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem histogramAveragesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downsampleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadCustomImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadAudioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorSimilarityToolStripMenuItem;
    }
}

