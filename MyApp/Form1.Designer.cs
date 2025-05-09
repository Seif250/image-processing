﻿namespace MyNamespace
{
    partial class Form1
    {
        // Components for the form
        private System.ComponentModel.IContainer components = null;

        // UI elements
        private PictureBox pictureBox1;
        private Panel panel1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileMenuItem;
        private ToolStripMenuItem openImageMenuItem;
        private ToolStripMenuItem editMenuItem;
        private ToolStripMenuItem redMenuItem;
        private ToolStripMenuItem greenMenuItem;
        private ToolStripMenuItem blueMenuItem;
        private ToolStripMenuItem grayScaleMenuItem;
        private ToolStripMenuItem getHistogramMenuItem;
        private ToolStripMenuItem histogramEqualizationMenuItem;
        private ToolStripMenuItem filtersMenuItem;
        private ToolStripMenuItem gaussianBlurMenuItem;
        private ToolStripMenuItem sobelEdgeMenuItem;
        private ToolStripMenuItem brightnessAdjustmentMenuItem;
        private ToolStripMenuItem negativeFilterMenuItem;
        private ToolStripMenuItem saturationAdjustmentMenuItem;

        // Dispose method to clean up resources
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Initialize components and layout
        private void InitializeComponent()
        {
            this.panel1 = new Panel();
            this.pictureBox1 = new PictureBox();
            this.menuStrip1 = new MenuStrip();
            this.fileMenuItem = new ToolStripMenuItem();
            this.openImageMenuItem = new ToolStripMenuItem();
            this.editMenuItem = new ToolStripMenuItem();
            this.redMenuItem = new ToolStripMenuItem();
            this.greenMenuItem = new ToolStripMenuItem();
            this.blueMenuItem = new ToolStripMenuItem();
            this.grayScaleMenuItem = new ToolStripMenuItem();
            this.getHistogramMenuItem = new ToolStripMenuItem();
            this.histogramEqualizationMenuItem = new ToolStripMenuItem();
            this.filtersMenuItem = new ToolStripMenuItem();
            this.gaussianBlurMenuItem = new ToolStripMenuItem();
            this.sobelEdgeMenuItem = new ToolStripMenuItem();
            this.brightnessAdjustmentMenuItem = new ToolStripMenuItem();
            this.negativeFilterMenuItem = new ToolStripMenuItem();
            this.saturationAdjustmentMenuItem = new ToolStripMenuItem();

            this.SuspendLayout();
            
            // panel1
            this.panel1.Dock = DockStyle.Fill; // Fill the form
            this.panel1.Padding = new Padding(0, 20, 0, 20); // Padding for the panel
            this.panel1.Controls.Add(this.pictureBox1); // Add PictureBox to panel

            // pictureBox1
            this.pictureBox1.Dock = DockStyle.Fill; // Fill the panel
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom; // Zoom image to fit
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;

            // menuStrip1
            this.menuStrip1.Items.AddRange(new ToolStripItem[] {
                this.fileMenuItem,
                this.editMenuItem,
                this.filtersMenuItem,
                this.getHistogramMenuItem,
                this.histogramEqualizationMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";

            // fileMenuItem
            this.fileMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                this.openImageMenuItem});
            this.fileMenuItem.Name = "fileMenuItem";
            this.fileMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileMenuItem.Text = "File";

            // openImageMenuItem
            this.openImageMenuItem.Name = "openImageMenuItem";
            this.openImageMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openImageMenuItem.Text = "Open Image";
            this.openImageMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);

            // editMenuItem
            this.editMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                this.redMenuItem,
                this.greenMenuItem,
                this.blueMenuItem,
                this.grayScaleMenuItem});
            this.editMenuItem.Name = "editMenuItem";
            this.editMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editMenuItem.Text = "Edit";

            // redMenuItem
            this.redMenuItem.Name = "redMenuItem";
            this.redMenuItem.Size = new System.Drawing.Size(180, 22);
            this.redMenuItem.Text = "Red";
            this.redMenuItem.Click += new System.EventHandler(this.RedMenuItem_Click);

            // greenMenuItem
            this.greenMenuItem.Name = "greenMenuItem";
            this.greenMenuItem.Size = new System.Drawing.Size(180, 22);
            this.greenMenuItem.Text = "Green";
            this.greenMenuItem.Click += new System.EventHandler(this.GreenMenuItem_Click);

            // blueMenuItem
            this.blueMenuItem.Name = "blueMenuItem";
            this.blueMenuItem.Size = new System.Drawing.Size(180, 22);
            this.blueMenuItem.Text = "Blue";
            this.blueMenuItem.Click += new System.EventHandler(this.BlueMenuItem_Click);

            // grayScaleMenuItem
            this.grayScaleMenuItem.Name = "grayScaleMenuItem";
            this.grayScaleMenuItem.Size = new System.Drawing.Size(180, 22);
            this.grayScaleMenuItem.Text = "Gray Scale";
            this.grayScaleMenuItem.Click += new System.EventHandler(this.GrayScaleMenuItem_Click);

            // getHistogramMenuItem
            this.getHistogramMenuItem.Name = "getHistogramMenuItem";
            this.getHistogramMenuItem.Size = new System.Drawing.Size(180, 22);
            this.getHistogramMenuItem.Text = "Get Histogram";
            this.getHistogramMenuItem.Click += new System.EventHandler(this.GetHistogramMenuItem_Click);

            // histogramEqualizationMenuItem
            this.histogramEqualizationMenuItem.Name = "histogramEqualizationMenuItem";
            this.histogramEqualizationMenuItem.Size = new System.Drawing.Size(180, 22);
            this.histogramEqualizationMenuItem.Text = "Histogram Equalization";
            this.histogramEqualizationMenuItem.Click += new System.EventHandler(this.HistogramEqualizationMenuItem_Click);

            // filtersMenuItem
            this.filtersMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                this.gaussianBlurMenuItem,
                this.sobelEdgeMenuItem,
                this.brightnessAdjustmentMenuItem,
                this.negativeFilterMenuItem,
                this.saturationAdjustmentMenuItem});
            this.filtersMenuItem.Name = "filtersMenuItem";
            this.filtersMenuItem.Size = new System.Drawing.Size(50, 20);
            this.filtersMenuItem.Text = "Filters";

            // gaussianBlurMenuItem
            this.gaussianBlurMenuItem.Name = "gaussianBlurMenuItem";
            this.gaussianBlurMenuItem.Size = new System.Drawing.Size(180, 22);
            this.gaussianBlurMenuItem.Text = "Gaussian Blur";
            this.gaussianBlurMenuItem.Click += new System.EventHandler(this.GaussianBlurMenuItem_Click);

            // sobelEdgeMenuItem
            this.sobelEdgeMenuItem.Name = "sobelEdgeMenuItem";
            this.sobelEdgeMenuItem.Size = new System.Drawing.Size(180, 22);
            this.sobelEdgeMenuItem.Text = "Sobel Edge Detection";
            this.sobelEdgeMenuItem.Click += new System.EventHandler(this.SobelEdgeMenuItem_Click);

            // brightnessAdjustmentMenuItem
            this.brightnessAdjustmentMenuItem.Name = "brightnessAdjustmentMenuItem";
            this.brightnessAdjustmentMenuItem.Size = new System.Drawing.Size(180, 22);
            this.brightnessAdjustmentMenuItem.Text = "Brightness Adjustment";
            this.brightnessAdjustmentMenuItem.Click += new System.EventHandler(this.BrightnessAdjustmentMenuItem_Click);

            // negativeFilterMenuItem
            this.negativeFilterMenuItem.Name = "negativeFilterMenuItem";
            this.negativeFilterMenuItem.Size = new System.Drawing.Size(180, 22);
            this.negativeFilterMenuItem.Text = "Negative Filter";
            this.negativeFilterMenuItem.Click += new System.EventHandler(this.NegativeFilterMenuItem_Click);

            // saturationAdjustmentMenuItem
            this.saturationAdjustmentMenuItem.Name = "saturationAdjustmentMenuItem";
            this.saturationAdjustmentMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saturationAdjustmentMenuItem.Text = "Saturation Adjustment";
            this.saturationAdjustmentMenuItem.Click += new System.EventHandler(this.SaturationAdjustmentMenuItem_Click);

            // Form1
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 550);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Text = "برنامج معالجة الصور - Image Processing App";
            this.BackColor = Color.FromArgb(50, 50, 60);
            this.ForeColor = Color.White;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}