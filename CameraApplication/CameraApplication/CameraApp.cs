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
        private OrientationSensor _orientationSensor;
        private double _compass_north;
        private float _inclinometerPitch;
        private float _inclinometerRoll;
        private float _inclinometerYaw;
        private float _M11, _M12, _M13,
                      _M21, _M22, _M23,
                      _M31, _M32, _M33;

        public CameraApp()
        {
            InitializeComponent();
            _inclinometer = Inclinometer.GetDefault();
            _compass = Compass.GetDefault();
            _orientationSensor = OrientationSensor.GetDefault();

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
                _inclinometer.ReadingChanged += _inclinometer_ReadingChanged;

            }

            //_compass = Compass.GetDefault();
            if (_compass != null)
            {
                _compass.ReadingChanged += _compass_ReadingChanged; ;
            }

            if(_orientationSensor !=null)
            {
                _orientationSensor.ReadingChanged += _orientationSensor_ReadingChanged;
            }
            

            imageVideo.Resize += ImageVideo_Resize;
            Application.Idle += ProcessFrame;
        }

        private void ImageVideo_Resize(object sender, EventArgs e)
        {
            _cameraCapture.SetCaptureProperty(CapProp.FrameWidth, /*1920/**/imageVideo.Size.Width/**/);
            _cameraCapture.SetCaptureProperty(CapProp.FrameHeight, /*1080/**/imageVideo.Size.Height/**/);
        }

        #region Récupération des données des capteurs

        private void _compass_ReadingChanged(Compass sender, CompassReadingChangedEventArgs args)
        {
           _compass_north = args.Reading.HeadingMagneticNorth;
        }

        private void _inclinometer_ReadingChanged(Inclinometer sender, InclinometerReadingChangedEventArgs args)
        {
            
            _inclinometerPitch = args.Reading.PitchDegrees;
            _inclinometerRoll  = args.Reading.RollDegrees;
            _inclinometerYaw   = args.Reading.YawDegrees;
        }

        private void _orientationSensor_ReadingChanged(OrientationSensor sender, OrientationSensorReadingChangedEventArgs args)
        {
            _M11 = args.Reading.RotationMatrix.M11;
            _M12 = args.Reading.RotationMatrix.M12;
            _M13 = args.Reading.RotationMatrix.M13;

            _M21 = args.Reading.RotationMatrix.M21;
            _M22 = args.Reading.RotationMatrix.M22;
            _M23 = args.Reading.RotationMatrix.M23;

            _M31 = args.Reading.RotationMatrix.M31;
            _M32 = args.Reading.RotationMatrix.M32;
            _M33 = args.Reading.RotationMatrix.M33;
        }
        #endregion

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

                    //Boussole
                    writer.WriteStartElement("compass");
            /**/
                        writer.WriteStartElement("north");
                        writer.WriteValue(_compass_north);
                        writer.WriteEndElement();

                    writer.WriteEndElement();
                    
                    //Inclinomètre (retourne l'angle de rotation autour des axes X,Y,Z)
                    writer.WriteStartElement("inclinometer");
                        
                        //angle de rotation autour de l'axe x
                        writer.WriteStartElement("Pitch");
                        writer.WriteValue(_inclinometerPitch);
                        writer.WriteEndElement();

                        //angle de rotation autour de l'axe y
                        writer.WriteStartElement("Roll");
                        writer.WriteValue(_inclinometerRoll);
                        writer.WriteEndElement();
            
                        //angle de rotation autour de l'axe z
                        writer.WriteStartElement("Yaw");
                        writer.WriteValue(_inclinometerYaw);
                        writer.WriteEndElement();

                    writer.WriteEndElement();

                    //Capteur d'orientation (retourne la matrice de rotation)
                    writer.WriteStartElement("orientation");

                        writer.WriteStartElement("M11");
                        writer.WriteValue(_M11);
                        writer.WriteEndElement();

                        writer.WriteStartElement("M12");
                        writer.WriteValue(_M12);
                        writer.WriteEndElement();

                        writer.WriteStartElement("M13");
                        writer.WriteValue(_M13);
                        writer.WriteEndElement();

                        writer.WriteStartElement("M21");
                        writer.WriteValue(_M21);
                        writer.WriteEndElement();

                        writer.WriteStartElement("M22");
                        writer.WriteValue(_M22);
                        writer.WriteEndElement();

                        writer.WriteStartElement("M23");
                        writer.WriteValue(_M23);
                        writer.WriteEndElement();

                        writer.WriteStartElement("M31");
                        writer.WriteValue(_M31);
                        writer.WriteEndElement();

                        writer.WriteStartElement("M32");
                        writer.WriteValue(_M32);
                        writer.WriteEndElement();

                        writer.WriteStartElement("M33");
                        writer.WriteValue(_M33);
                        

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
