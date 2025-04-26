using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using System.IO;
using System.Drawing.Drawing2D;

namespace MyNamespace
{
    public partial class Form1 : Form
    {
        // Mat objects to hold the original and processed images
        private Mat? image1;
        private Mat? image2;

        public Form1()
        {
            InitializeComponent();
            
            // Set the application icon using a path relative to the executable
            try
            {
                string iconPath = Path.Combine(Application.StartupPath, "img", "icon.ico");
                if (File.Exists(iconPath))
                {
                    this.Icon = new Icon(iconPath);
                }
                else
                {
                    // Try the relative path in the project directory
                    iconPath = "img\\icon.ico";
                    if (File.Exists(iconPath))
                    {
                        this.Icon = new Icon(iconPath);
                    }
                }
            }
            catch (Exception ex)
            {
                // Icon loading failed but it's not critical
                MessageBox.Show("Note: Could not load icon file - " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set the background color of the form
            this.BackColor = Color.Gray;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                // Set file dialog properties
                openFileDialog1.FileName = "";
                openFileDialog1.Filter = "JPEG (*.jpg)|*.jpg|Bitmap (*.bmp)|*.bmp|All files (*.*)|*.*";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Dispose of the previous image if it exists
                        image1?.Dispose();
                        // Load the selected image
                        image1 = Cv2.ImRead(openFileDialog1.FileName, ImreadModes.Color);

                        // Calculate aspect ratio and resize image to fit PictureBox
                        var aspectRatio = (double)image1.Width / image1.Height;
                        int newWidth = pictureBox1.Width;
                        int newHeight = (int)(newWidth / aspectRatio);

                        if (newHeight > pictureBox1.Height)
                        {
                            newHeight = pictureBox1.Height;
                            newWidth = (int)(newHeight * aspectRatio);
                        }

                        Mat resizedImage = new Mat();
                        Cv2.Resize(image1, resizedImage, new OpenCvSharp.Size(newWidth, newHeight));

                        // Convert Mat to Bitmap and display in PictureBox
                        Bitmap bitmap = MatToBitmap(resizedImage);
                        pictureBox1.Image = bitmap;

                        resizedImage.Dispose();
                    }
                    catch (Exception ex)
                    {
                        // Show error message if loading fails
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }
        }

        private void RedMenuItem_Click(object sender, EventArgs e)
        {
            // Process the red color channel
            ProcessColorChannel(2);
        }

        private void GreenMenuItem_Click(object sender, EventArgs e)
        {
            // Process the green color channel
            ProcessColorChannel(1);
        }

        private void BlueMenuItem_Click(object sender, EventArgs e)
        {
            // Process the blue color channel
            ProcessColorChannel(0);
        }

        private void GrayScaleMenuItem_Click(object sender, EventArgs e)
        {
            if (image1 == null) return;

            image2?.Dispose();
            image2 = image1.Clone();

            // Convert image to grayscale by averaging color channels
            for (int y = 0; y < image2.Rows; y++)
            {
                for (int x = 0; x < image2.Cols; x++)
                {
                    Vec3b color = image2.At<Vec3b>(y, x);
                    byte avg = (byte)((color.Item0 + color.Item1 + color.Item2) / 3);
                    image2.Set(y, x, new Vec3b(avg, avg, avg));
                }
            }

            // Display original and processed images side by side
            Bitmap bitmap = MatToBitmap(image2);
            DisplayImagesSideBySide(MatToBitmap(image1), bitmap);
        }

        private void GetHistogramMenuItem_Click(object sender, EventArgs e)
        {
            if (image1 == null) return;

            // Convert images to Bitmap
            Bitmap originalBitmap = MatToBitmap(image1);
            Bitmap processedBitmap = image2 != null ? MatToBitmap(image2) : null;

            // Create histograms for both images with increased height (300 instead of 200)
            Bitmap originalHistogram = CreateHistogram(originalBitmap, 300);
            Bitmap processedHistogram = processedBitmap != null ? CreateHistogram(processedBitmap, 300) : null;

            // Use a new layout for displaying histograms
            int width = originalBitmap.Width;
            int height = originalBitmap.Height;
            int histHeight = originalHistogram.Height;
            
            // Give the histogram more space in the layout
            int totalWidth = processedBitmap != null ? width * 2 + 30 : width + 20;
            int totalHeight = height + histHeight + 40; // More space between elements
            
            Bitmap finalDisplay = new Bitmap(totalWidth, totalHeight);
            
            using (Graphics g = Graphics.FromImage(finalDisplay))
            {
                // Fill with gradient background
                using (LinearGradientBrush bgBrush = new LinearGradientBrush(
                    new Rectangle(0, 0, totalWidth, totalHeight),
                    Color.FromArgb(40, 40, 40), Color.FromArgb(20, 20, 20), 
                    LinearGradientMode.ForwardDiagonal))
                {
                    g.FillRectangle(bgBrush, 0, 0, totalWidth, totalHeight);
                }
                
                // Add decorative elements
                using (Pen decorPen = new Pen(Color.FromArgb(70, 70, 200), 2))
                {
                    g.DrawRectangle(decorPen, 5, 5, totalWidth - 10, totalHeight - 10);
                }
                
                // Draw original image and histogram
                g.DrawImage(originalBitmap, 10, 10, width, height);
                // Draw histogram larger by scaling it up
                g.DrawImage(originalHistogram, 10, height + 25, width, histHeight);
                
                // Draw processed image and histogram if they exist
                if (processedBitmap != null && processedHistogram != null)
                {
                    g.DrawImage(processedBitmap, width + 20, 10, width, height);
                    g.DrawImage(processedHistogram, width + 20, height + 25, width, histHeight);
                }
                
                // Add labels with drop shadows
                using (Font labelFont = new Font("Segoe UI", 13, FontStyle.Bold)) // Larger font
                {
                    // Shadow effect for text
                    SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 0));
                    SolidBrush textBrush = new SolidBrush(Color.White);
                    
                    // Original image label
                    g.DrawString("Original", labelFont, shadowBrush, 16, 16);
                    g.DrawString("Original", labelFont, textBrush, 15, 15);
                    
                    // Original histogram label with larger background
                    g.FillRectangle(new SolidBrush(Color.FromArgb(150, 0, 0, 0)), 10, height + 25, 120, 30);
                    g.DrawString("Histogram", labelFont, textBrush, 15, height + 28);
                    
                    // Processed image and histogram labels
                    if (processedBitmap != null && processedHistogram != null)
                    {
                        g.DrawString("Processed", labelFont, shadowBrush, width + 26, 16);
                        g.DrawString("Processed", labelFont, textBrush, width + 25, 15);
                        
                        g.FillRectangle(new SolidBrush(Color.FromArgb(150, 0, 0, 0)), width + 20, height + 25, 120, 30);
                        g.DrawString("Histogram", labelFont, textBrush, width + 25, height + 28);
                    }
                }
            }
            
            pictureBox1.Image = finalDisplay;
        }

        private void HistogramEqualizationMenuItem_Click(object sender, EventArgs e)
        {
            if (image1 == null) return;

            // Clone image1 to image2
            image2?.Dispose();
            image2 = image1.Clone();

            // Convert to grayscale
            Mat grayImage = new Mat();
            Cv2.CvtColor(image2, grayImage, ColorConversionCodes.BGR2GRAY);

            // Calculate histogram
            int[] histogram = new int[256];
            for (int y = 0; y < grayImage.Rows; y++)
            {
                for (int x = 0; x < grayImage.Cols; x++)
                {
                    byte intensity = grayImage.At<byte>(y, x);
                    histogram[intensity]++;
                }
            }

            // Calculate PDF and CDF
            int totalPixels = grayImage.Rows * grayImage.Cols;
            double[] pdf = new double[256];
            double[] cdf = new double[256];
            pdf[0] = (double)histogram[0] / totalPixels;
            cdf[0] = pdf[0];

            for (int i = 1; i < 256; i++)
            {
                pdf[i] = (double)histogram[i] / totalPixels;
                cdf[i] = cdf[i - 1] + pdf[i];
            }

            // Apply histogram equalization
            Mat equalizedImage = new Mat(grayImage.Size(), grayImage.Type());
            for (int y = 0; y < grayImage.Rows; y++)
            {
                for (int x = 0; x < grayImage.Cols; x++)
                {
                    byte intensity = grayImage.At<byte>(y, x);
                    byte newIntensity = (byte)(cdf[intensity] * 255);
                    equalizedImage.Set(y, x, newIntensity);
                }
            }

            // Display original and equalized images with histograms
            Bitmap originalBitmap = MatToBitmap(image1);
            Bitmap equalizedBitmap = MatToBitmap(equalizedImage);
            Bitmap originalHistogram = CreateHistogram(originalBitmap, 300); // Larger histogram height
            Bitmap equalizedHistogram = CreateHistogram(equalizedBitmap, 300); // Larger histogram height

            DisplayImagesAndHistograms(originalBitmap, equalizedBitmap, originalHistogram, equalizedHistogram);
        }

        private Bitmap CreateHistogram(Bitmap bitmap, int height)
        {
            // Initialize histograms for each color channel
            int[] histogramR = new int[256];
            int[] histogramG = new int[256];
            int[] histogramB = new int[256];

            // Calculate histograms
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color color = bitmap.GetPixel(x, y);
                    histogramR[color.R]++;
                    histogramG[color.G]++;
                    histogramB[color.B]++;
                }
            }

            // Find maximum histogram value for scaling
            int max = Math.Max(Math.Max(histogramR.Max(), histogramG.Max()), histogramB.Max());

            // Create histogram image with wider bars for visibility
            int width = 512; // Doubled width to make each bar 2 pixels wide
            Bitmap histImage = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(histImage))
            {
                g.Clear(Color.FromArgb(30, 30, 30)); // Darker background
                
                // Add grid lines for better readability
                using (Pen gridPen = new Pen(Color.FromArgb(50, 255, 255, 255)))
                {
                    // Horizontal grid lines
                    for (int y = 0; y < height; y += height / 10)
                    {
                        g.DrawLine(gridPen, 0, y, width, y);
                    }
                    // Vertical grid lines
                    for (int x = 0; x < width; x += 64) // 64 pixels = 32 original values
                    {
                        g.DrawLine(gridPen, x, 0, x, height);
                    }
                }

                // Draw histogram lines for each color channel using thicker pens and fill
                using (Pen redPen = new Pen(Color.Red, 2))
                using (Pen greenPen = new Pen(Color.Green, 2))
                using (Pen bluePen = new Pen(Color.Blue, 2))
                {
                    for (int i = 0; i < 256; i++)
                    {
                        int x = i * 2; // Scale x coordinate by 2
                        
                        int rHeight = (int)(histogramR[i] * height / max);
                        int gHeight = (int)(histogramG[i] * height / max);
                        int bHeight = (int)(histogramB[i] * height / max);

                        // Draw thicker lines for better visibility
                        g.DrawLine(redPen, x, height, x, height - rHeight);
                        g.DrawLine(redPen, x+1, height, x+1, height - rHeight);
                        
                        g.DrawLine(greenPen, x, height, x, height - gHeight);
                        g.DrawLine(greenPen, x+1, height, x+1, height - gHeight);
                        
                        g.DrawLine(bluePen, x, height, x, height - bHeight);
                        g.DrawLine(bluePen, x+1, height, x+1, height - bHeight);
                    }
                }
                
                // Add labels for ranges
                using (Font labelFont = new Font("Arial", 8))
                {
                    g.DrawString("0", labelFont, Brushes.White, 2, height - 15);
                    g.DrawString("64", labelFont, Brushes.White, 128, height - 15);
                    g.DrawString("128", labelFont, Brushes.White, 256, height - 15);
                    g.DrawString("192", labelFont, Brushes.White, 384, height - 15);
                    g.DrawString("255", labelFont, Brushes.White, 500, height - 15);
                }
            }

            return histImage;
        }

        private void DisplayHistograms(Bitmap originalHistogram, Bitmap processedHistogram)
        {
            // Combine histograms side by side
            Bitmap combined = new Bitmap(originalHistogram.Width * 2, originalHistogram.Height);

            using (Graphics g = Graphics.FromImage(combined))
            {
                g.DrawImage(originalHistogram, 0, 0, originalHistogram.Width, originalHistogram.Height);

                if (processedHistogram != null)
                {
                    g.DrawImage(processedHistogram, originalHistogram.Width, 0, processedHistogram.Width, processedHistogram.Height);
                }
            }

            pictureBox1.Image = combined;
        }

        private void DisplayImagesAndHistograms(Bitmap original, Bitmap equalized, Bitmap originalHist, Bitmap equalizedHist)
        {
            // Use a grid layout (2x2) with larger histogram area
            int maxWidth = Math.Max(original.Width, equalized.Width);
            int maxHeight = Math.Max(original.Height, equalized.Height);
            int histHeight = originalHist.Height;
            
            // Create the final image with a 2x2 grid layout with more space
            Bitmap finalDisplay = new Bitmap(maxWidth * 2 + 20, maxHeight + histHeight + 30);
            
            using (Graphics g = Graphics.FromImage(finalDisplay))
            {
                // Fill background with a gradient
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    new Rectangle(0, 0, finalDisplay.Width, finalDisplay.Height),
                    Color.DarkSlateGray, Color.Black, 45f))
                {
                    g.FillRectangle(brush, 0, 0, finalDisplay.Width, finalDisplay.Height);
                }
                
                // Draw grid lines
                using (Pen gridPen = new Pen(Color.FromArgb(100, 255, 255, 255), 2)) // Thicker line
                {
                    g.DrawLine(gridPen, maxWidth + 10, 0, maxWidth + 10, finalDisplay.Height);
                    g.DrawLine(gridPen, 0, maxHeight + 15, finalDisplay.Width, maxHeight + 15);
                }
                
                // Draw each image and histogram in its grid cell
                // Top-left: Original image
                g.DrawImage(original, 0, 0, maxWidth, maxHeight);
                
                // Top-right: Processed image
                g.DrawImage(equalized, maxWidth + 20, 0, maxWidth, maxHeight);
                
                // Bottom-left: Original histogram (larger)
                g.DrawImage(originalHist, 0, maxHeight + 20, maxWidth, histHeight);
                
                // Bottom-right: Processed histogram (larger)
                g.DrawImage(equalizedHist, maxWidth + 20, maxHeight + 20, maxWidth, histHeight);
                
                // Add labels
                using (Font titleFont = new Font("Arial", 13, FontStyle.Bold)) // Larger font
                {
                    // Create semi-transparent backgrounds for text
                    SolidBrush textBgBrush = new SolidBrush(Color.FromArgb(150, 0, 0, 0));
                    SolidBrush textBrush = new SolidBrush(Color.White);
                    
                    // Draw labels with backgrounds
                    // Original image
                    g.FillRectangle(textBgBrush, 10, 10, 150, 30);
                    g.DrawString("Original Image", titleFont, textBrush, 15, 13);
                    
                    // Processed image
                    g.FillRectangle(textBgBrush, maxWidth + 30, 10, 150, 30);
                    g.DrawString("Processed Image", titleFont, textBrush, maxWidth + 35, 13);
                    
                    // Original histogram (larger background)
                    g.FillRectangle(textBgBrush, 10, maxHeight + 25, 170, 30);
                    g.DrawString("Original Histogram", titleFont, textBrush, 15, maxHeight + 28);
                    
                    // Processed histogram
                    g.FillRectangle(textBgBrush, maxWidth + 30, maxHeight + 25, 170, 30);
                    g.DrawString("Processed Histogram", titleFont, textBrush, maxWidth + 35, maxHeight + 28);
                }
            }
            
            pictureBox1.Image = finalDisplay;
        }

        private void ProcessColorChannel(int channelIndex)
        {
            if (image1 == null) return;

            image2?.Dispose();
            image2 = image1.Clone();

            // Split image into color channels
            Mat[] channels = Cv2.Split(image2);

            // Zero out all channels except the selected one
            for (int i = 0; i < channels.Length; i++)
            {
                if (i != channelIndex)
                {
                    channels[i].SetTo(0);
                }
            }

            // Merge channels back into a single image
            Cv2.Merge(channels, image2);

            // Display original and processed images side by side
            Bitmap bitmap = MatToBitmap(image2);
            DisplayImagesSideBySide(MatToBitmap(image1), bitmap);
        }

        private void DisplayImagesSideBySide(Bitmap original, Bitmap processed)
        {
            // Show images vertically (one on top of the other) instead of side by side
            Bitmap combined = new Bitmap(Math.Max(original.Width, processed.Width), 
                                        original.Height + processed.Height);

            using (Graphics g = Graphics.FromImage(combined))
            {
                g.Clear(Color.DarkGray); // Background color between images
                
                // Draw original image at the top
                int x1 = (combined.Width - original.Width) / 2; // Center horizontally
                g.DrawImage(original, x1, 0, original.Width, original.Height);
                
                // Draw a separator line
                g.DrawLine(new Pen(Color.White, 3), 0, original.Height, combined.Width, original.Height);
                
                // Draw processed image below
                int x2 = (combined.Width - processed.Width) / 2; // Center horizontally
                g.DrawImage(processed, x2, original.Height, processed.Width, processed.Height);
                
                // Add labels
                using (Font font = new Font("Arial", 12, FontStyle.Bold))
                {
                    // Draw semi-transparent background for text
                    g.FillRectangle(new SolidBrush(Color.FromArgb(100, 0, 0, 0)), 10, 10, 100, 25);
                    g.FillRectangle(new SolidBrush(Color.FromArgb(100, 0, 0, 0)), 10, original.Height + 10, 100, 25);
                    
                    // Draw text
                    g.DrawString("Original", font, Brushes.White, 15, 12);
                    g.DrawString("Processed", font, Brushes.White, 15, original.Height + 12);
                }
            }

            pictureBox1.Image = combined;
        }

        private Bitmap MatToBitmap(Mat mat)
        {
            // Convert Mat to Bitmap
            using (var ms = mat.ToMemoryStream())
            {
                return new Bitmap(ms);
            }
        }

        private Mat BitmapToMat(Bitmap bitmap)
        {
            // Convert Bitmap to Mat
            using (var ms = new System.IO.MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                return Mat.FromStream(ms, ImreadModes.Color);
            }
        }

        // Filter Methods
        private void GaussianBlurMenuItem_Click(object sender, EventArgs e)
        {
            if (image1 == null) return;

            image2?.Dispose();
            image2 = new Mat();
            // Apply Gaussian Blur with a larger kernel size
            Cv2.GaussianBlur(image1, image2, new OpenCvSharp.Size(31, 31), 0);

            // Display original and blurred images side by side
            DisplayImagesSideBySide(MatToBitmap(image1), MatToBitmap(image2));
        }

        private void SobelEdgeMenuItem_Click(object sender, EventArgs e)
        {
            if (image1 == null) return;

            image2?.Dispose();
            image2 = new Mat();
            // Convert to grayscale and apply Sobel edge detection
            Cv2.CvtColor(image1, image2, ColorConversionCodes.BGR2GRAY);
            Cv2.Sobel(image2, image2, MatType.CV_8U, 1, 1);

            // Display original and edge-detected images side by side
            DisplayImagesSideBySide(MatToBitmap(image1), MatToBitmap(image2));
        }

        private void BrightnessAdjustmentMenuItem_Click(object sender, EventArgs e)
        {
            if (image1 == null) return;

            image2?.Dispose();
            image2 = image1.Clone();
            // Increase brightness by 50
            image2.ConvertTo(image2, -1, 1, 50);

            // Display original and brightness-adjusted images side by side
            DisplayImagesSideBySide(MatToBitmap(image1), MatToBitmap(image2));
        }

        private void NegativeFilterMenuItem_Click(object sender, EventArgs e)
        {
            if (image1 == null) return;

            image2?.Dispose();
            image2 = new Mat();
            // Apply negative filter
            Cv2.BitwiseNot(image1, image2);

            // Display original and negative images side by side
            DisplayImagesSideBySide(MatToBitmap(image1), MatToBitmap(image2));
        }

        // Add saturation adjustment method
        private void SaturationAdjustmentMenuItem_Click(object sender, EventArgs e)
        {
            if (image1 == null) return;

            image2?.Dispose();
            image2 = image1.Clone();
            
            // Convert to HSV for adjusting saturation
            Mat hsvImage = new Mat();
            Cv2.CvtColor(image2, hsvImage, ColorConversionCodes.BGR2HSV);
            
            // Split the HSV channels
            Mat[] hsvChannels = Cv2.Split(hsvImage);
            
            // Increase saturation by a factor (1.5 means 50% increase)
            Cv2.Multiply(hsvChannels[1], new Scalar(1.5), hsvChannels[1]);
            
            // Merge channels back
            Cv2.Merge(hsvChannels, hsvImage);
            
            // Convert back to BGR
            Cv2.CvtColor(hsvImage, image2, ColorConversionCodes.HSV2BGR);
            
            // Dispose of temporary resources
            hsvImage.Dispose();
            foreach (var channel in hsvChannels)
            {
                channel.Dispose();
            }
            
            // Display original and saturation-adjusted images side by side
            DisplayImagesSideBySide(MatToBitmap(image1), MatToBitmap(image2));
        }
    }
}