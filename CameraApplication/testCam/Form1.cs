using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace testCam
{
    
    public partial class Form1 : Form
    {
        private Capture _capture = null;
        private bool _captureInProgress;

        public Form1()
        {
            InitializeComponent();
            CvInvoke.UseOpenCL = false;

            try
            {
                _capture = new Capture();
                _capture.ImageGrabbed += ProcessFrame;
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }

        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            Mat frame = new Mat();
            _capture.Retrieve(frame, 0);
            captureImageBox.Image = frame;
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (_capture != null)
            {
                if (_captureInProgress)
                {  //stop the capture
                    startButton.Text = "Start Capture";
                    _capture.Pause();
                }
                else
                {
                    //start the capture
                    startButton.Text = "Stop";
                    _capture.Start();
                }

                _captureInProgress = !_captureInProgress;
            }
        }
    }
}
