using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

using Emgu.CV;
using Emgu.CV.Cvb;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.VideoSurveillance;

namespace OpenCVCamera
{
    public partial class Form1 : Form
    {
        private static Capture _cameraCapture;
        private int _count = 0;

        public Form1()
        {
            InitializeComponent();
            
            Run();
        }

        private void Run()
        {
            try
            {
                _cameraCapture = new Capture();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            Application.Idle += ProcessFrame;
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            Mat frame = _cameraCapture.QueryFrame();
            imageBox1.Image = frame;
        }

        private void shootButton_Click(object sender, EventArgs e)
        {
            //OpenFileDialog openfile = new OpenFileDialog();

                Mat m = _cameraCapture.QueryFrame(); 
                
                Image<Bgr, Byte> img = new Image<Bgr, Byte>(m.Bitmap);//(openfile.FileName);
                SaveFileDialog SaveFile = new SaveFileDialog();

                //if(SaveFile.ShowDialog() == DialogResult.OK)
                //{
                    saveJpeg(String.Format("../../image{0}.jpg",++_count)/*SaveFile.FileName*/, img.ToBitmap(), 100);
                //}

        }

        private void saveJpeg(string path, Bitmap img, long quality)
        {
            // Encoder parameter for image quality

            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

            // Jpeg image codec
            ImageCodecInfo jpegCodec = this.getEncoderInfo("image/jpeg");

            if (jpegCodec == null)
                return;

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            img.Save(path, jpegCodec, encoderParams);
        }

        private ImageCodecInfo getEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }
    }
}
