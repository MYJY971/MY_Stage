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
using System.Xml;

using Emgu.CV;
using Emgu.CV.Cvb;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

using Windows.Devices.Sensors;

namespace CameraApplication
{
    public partial class CameraApp : Form
    {
        private static Capture _cameraCapture;
        private String _path ="../../photos/";
        
        //sensor

        private Inclinometer _inclinometer;
        private Compass _compass;
        private double _compass_north;
        private float _inclinometerPitch;
        private float _inclinometerRoll;
        private float _inclinometerYaw;

        public CameraApp()
        {
            InitializeComponent();
            _inclinometer = Inclinometer.GetDefault();
            _compass = Compass.GetDefault();

            //désactive zoom et click droit
            //imageVideo.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            
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

           
            if (_inclinometer != null )
            {
                _inclinometer.ReadingChanged += _sensor_ReadingChanged;

            }

            _compass = Compass.GetDefault();
            if (_compass != null)
            {
                _compass.ReadingChanged += _compass_ReadingChanged; ;
            }

            

            imageVideo.Resize += ImageVideo_Resize;
            Application.Idle += ProcessFrame;
        }

        private void ImageVideo_Resize(object sender, EventArgs e)
        {
            _cameraCapture.SetCaptureProperty(CapProp.FrameWidth, /*1920/**/imageVideo.Size.Width/**/);
            _cameraCapture.SetCaptureProperty(CapProp.FrameHeight, /*1080/**/imageVideo.Size.Height/**/);
        }

        private void _compass_ReadingChanged(Compass sender, CompassReadingChangedEventArgs args)
        {
           _compass_north = args.Reading.HeadingMagneticNorth;
        }

        private void _sensor_ReadingChanged(Inclinometer sender, InclinometerReadingChangedEventArgs args)
        {
            
            _inclinometerPitch = args.Reading.PitchDegrees;
            _inclinometerRoll  = args.Reading.RollDegrees;
            _inclinometerYaw   = args.Reading.YawDegrees;
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            
            Mat frame = _cameraCapture.QueryFrame();
            imageVideo.Image = frame;
            // shootButton.Text = frame.Width+"x"+frame.Height;
        }

        private void shootButton_Click(object sender, EventArgs e)
        {
              

            Mat currentFrame = _cameraCapture.QueryFrame();

            Image<Bgr, Byte> img = new Image<Bgr, Byte>(currentFrame.Bitmap);
            //SaveFileDialog SaveFile = new SaveFileDialog();

            String filename = "photo_" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() +
                               "-" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + "-"
                               + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString() + "-"
                               + DateTime.Now.Millisecond.ToString(); 

            saveJpeg(_path+filename+".jpg", img.ToBitmap(), 500);
            saveXml(_path + filename + ".xml");

            //pour indiquer qu'une photo à été prise
            //imageVideo.Image = new Mat();

            

        }

        private void saveXml(string path)
        {
            XmlTextWriter writer = new XmlTextWriter(path, Encoding.UTF8);

            writer.Formatting = Formatting.Indented;

            writer.WriteStartDocument();

            writer.WriteStartElement("sensor");

            writer.WriteStartElement("compass");
            /**/
            writer.WriteStartElement("north");
            writer.WriteValue(_compass_north);
            writer.WriteEndElement();

            writer.WriteEndElement();

            writer.WriteStartElement("inclinometer");

            writer.WriteStartElement("Pitch");
            writer.WriteValue(_inclinometerPitch);
            writer.WriteEndElement();

            writer.WriteStartElement("Roll");
            writer.WriteValue(_inclinometerRoll);
            writer.WriteEndElement();

            writer.WriteStartElement("Yaw");
            writer.WriteValue(_inclinometerYaw);

            writer.WriteEndDocument();


            writer.Close();
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

        private void shootButton_MouseDown(object sender, MouseEventArgs e)
        {
            shootButton.BackColor = Color.Lime;
        }

        private void shootButton_MouseUp(object sender, MouseEventArgs e)
        {
            shootButton.BackColor = Color.Gainsboro;
        }
    }
}
