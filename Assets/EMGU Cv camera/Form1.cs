using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EMGU_Cv_camera
{
    public partial class Form1 : Form
    {
        VideoCapture capture;
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (capture == null)
            {
                capture = new Emgu.CV.VideoCapture(0);
            }
            capture.ImageGrabbed += Capture_ImageGrabbed;
            capture.Start();
        }

        private void Capture_ImageGrabbed(object sender, EventArgs e)
        {
            try
            {
                Mat mat = new Mat();
                capture.Retrieve(mat);
                camOutput.Image = mat.ToImage<Bgr, byte>().ToBitmap();
                FilteredView.Image = DetectSkin(mat.ToImage<Bgr, byte>()).ToBitmap();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Image<Gray, Byte> DetectSkin(Image<Bgr, Byte> img)
        {
            Image<Ycc, Byte> currentYCrCbFrame = img.Convert<Ycc, Byte>();
            Image<Gray, Byte> skin = new Image<Gray, Byte>(img.Width, img.Height);
            int y;
            int cr;
            int cb;
            int l;
            int x1;
            int y1;
            int value;

            int rows = img.Rows;
            int cols = img.Cols;
            Byte[,,] YCrCbData = currentYCrCbFrame.Data;
            Byte[,,] skinData = skin.Data;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    y = YCrCbData[i, j, 0];
                    cr = YCrCbData[i, j, 1];
                    cb = YCrCbData[i, j, 2];

                    cb -= 109;
                    cr -= 152;
                    x1 = (819 * cr - 614 * cb) / 32 + 51;
                    y1 = (819 * cr + 614 * cb) / 32 + 77;
                    x1 = x1 * 41 / 1024;
                    y1 = y1 * 73 / 1024;
                    value = x1 * x1 + y1 * y1;
                    if (y < 100)
                        skinData[i, j, 0] = (value < 700) ? (Byte)255 : (Byte)0;
                    else
                        skinData[i, j, 0] = (value < 850) ? (Byte)255 : (Byte)0;
                }
            }
            Mat rect_6 = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(6, 6), new Point(3, 3));
            CvInvoke.Erode(skin, skin, rect_6, new Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Reflect, default(MCvScalar));
            CvInvoke.Dilate(skin, skin, rect_6, new Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Reflect, default(MCvScalar));
            return skin;
        }
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (capture != null)
            {
                capture.Stop();
            }
        }
        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (capture != null)
            {
                capture.Pause();
            }
        }
    }
}
