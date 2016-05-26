using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using TexLib;
using System.Xml.Linq;

using openCV;



namespace MYCalibrationTest
{
    public partial class Form1 : Form
    {
        bool _initContextGLisOk = false;
        Color4 _background = new Color4(0.4f, 1.0f, 1.0f, 0.3f);

        private Vector3 _eye = new Vector3(-10.0f,0.0f,0.0f);
        private Vector3 _target = Vector3.Zero;
        private Vector3 _up = new Vector3(0.0f,0.0f,1.0f);

        private int _backgroundTextureId;
        private bool _useMatrixCalibration=false;

        private float _mouseX, _mouseY;
        private bool _click=false; 

        private double _fx;
        private double _fy;
        private double _k1;
        private double _k2;
        private double _k3;
        private double _p1;
        private double _p2;

        Matrix4 _MYglProjectionMatrix;
        private double[] _glProjectionMatrix;
        Matrix3d _matIntreseque;

        private double[,] _camIntParams  = new double[3, 3];
        private double[] _distorsionCoef = new double[4];

        private int _width;
        private int _height;

        private List<Vector2> _listImagePoints = new List<Vector2>();
        List<Vector3> _listObjectPoints = new List<Vector3>();

        private Matrix4 modelViewTest = new Matrix4();
        Matrix4 _modelView = new Matrix4();

        private bool _isCalibrated = false;

        

        //Bitmap background = new Bitmap("../../photos/photo.jpg");

        public Form1()
        {
            InitializeComponent();

            GetCamParams();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {

            //SetupViewport();

                   
            glControl1.Resize += glControl1_Resize;

            
        }

        private void SetupViewport()
        {
            int w = glControl1.Width;
            int h = glControl1.Height;

            if (h == 0)                                                  // Prevent A Divide By Zero...
                h = 1;                                                   // By Making Height Equal To One

            GL.Viewport(0, 0, w, h);              // Use all of the glControl painting area

            GL.MatrixMode(MatrixMode.Projection);                               // Select The Projection Matrix
            GL.LoadIdentity();                                                  // Reset The Projection Matrix

            GL.Ortho(0, w, 0, h, 0, 1);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //loaded = true;
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            base.OnSizeChanged(e);

            SetupViewport();      


        }

        protected void InitGLContext()
        {

            GL.Enable(EnableCap.Texture2D);                       // Enable Texture Mapping
            GL.ShadeModel(ShadingModel.Smooth);                   // Enable Smooth Shading
            GL.ClearColor(_background)  ;                           // Clear the Color

            // Clear the Color and Depth Buffer
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.ClearDepth(1.0f);										     // Depth Buffer Setup
            GL.Enable(EnableCap.DepthTest);								     // Enables Depth Testing
            GL.DepthFunc(DepthFunction.Lequal);							     // The Type Of Depth Testing To Do
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);  // Really Nice Perspective Calculations

            _initContextGLisOk = true;

        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!_initContextGLisOk)
                InitGLContext();


            int w = glControl1.Width;
            int h = glControl1.Height;

            
            //Background
            GL.MatrixMode(MatrixMode.Projection);                               // Select The Projection Matrix
            GL.LoadIdentity();                                                  // Reset The Projection Matrix

            GL.Ortho(0, w, 0, h, -1, 1);
            


            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.DepthMask(false);
            
            DrawBackground();

            //GL.DrawPixels(_backgroundPicture.Width, _backgroundPicture.Height, PixelFormat.Rgb, PixelType.Bitmap, _backgroundPicture.GetHbitmap());

            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);

            //TEST
            ///*
            if (_click)
                DrawPoint();

            // * /

            //Scene 3D
            ///*
            GL.MatrixMode(MatrixMode.Projection);                               // Select The Projection Matrix
            GL.LoadIdentity();                                                  // Reset The Projection Matrix

            if (_isCalibrated)
                GL.MultMatrix(ref _MYglProjectionMatrix);

            else
            {
                Matrix4 matrixPerspective = Matrix4.CreatePerspectiveFieldOfView((float)System.Math.PI / 4, (float)w / (float)h, 0.1f, 100.0f);
                GL.MultMatrix(ref matrixPerspective);
            }

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            LookAt();

            //Draw the scene
            glDrawScene();


            //*/
            glControl1.SwapBuffers();
        }

        private void DrawBackground()
        {
            int w = glControl1.Width;
            int h = glControl1.Height;

            //réinitialise la couleur
            GL.Color3(1.0f, 1.0f, 1.0f);

            GL.BindTexture(TextureTarget.Texture2D, _backgroundTextureId);

            GL.Begin(PrimitiveType.Quads);


            GL.TexCoord2(1, 1);
            GL.Vertex2(0, 0);
            GL.TexCoord2(0, 1);
            GL.Vertex2(w, 0);
            GL.TexCoord2(0, 0);
            GL.Vertex2(w, h);
            GL.TexCoord2(1, 0);
            GL.Vertex2(0, h);

            GL.End();
        }

        private void glDrawScene()
        {
            
            GL.Rotate(45, Vector3d.UnitZ);
            DrawCube();

            DrawPoint();
        }

        private void DrawCube()
        {
            GL.Begin(PrimitiveType.Quads);
            // Front Face
            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(-1, -1, 1);
            GL.Vertex3(1, -1, 1);
            GL.Vertex3(1, 1, 1);
            GL.Vertex3(-1, 1, 1);

            // Back Face
            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(-1, -1, -1);
            GL.Vertex3(- 1, 1, -1);
            GL.Vertex3(1, 1, -1);
            GL.Vertex3(1, -1, -1);

            // Top Face
            GL.Color3(1.0f, 1.0f, 0.0f);
            GL.Vertex3(-1, 1, -1);
            GL.Vertex3(-1, 1, 1);
            GL.Vertex3(1, 1, 1);
            GL.Vertex3(1, 1, -1);

            // Bottom Face
            GL.Color3(1f, 0.4f, 0f);
            GL.Vertex3(-1, -1, -1);
            GL.Vertex3(1, -1, -1);
            GL.Vertex3(1, -1, 1);
            GL.Vertex3(-1, -1, 1);

            // Right face
            GL.Color3(1f, 0f, 1f);
            GL.Vertex3(1, -1, -1);
            GL.Vertex3(1, 1, -1);
            GL.Vertex3(1, 1, 1);
            GL.Vertex3(1, -1, 1);

            // Left Face
            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(-1, -1, -1);
            GL.Vertex3(-1, -1, 1);
            GL.Vertex3(-1, 1, 1);
            GL.Vertex3(-1, 1, -1);

            GL.End();

        }

        private void LookAt()
        {
            Matrix4 lookat;
            if (_isCalibrated)
                lookat = _modelView;
            //Matrix4 modelView = _modelView;
            else
                lookat = Matrix4.LookAt(_eye, _target, _up);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Image Files (*.tif; *.dcm; *.jpg; *.jpeg; *.bmp)|*.tif; *.dcm; *.jpg; *.jpeg; *.bmp";
            
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                _backgroundTextureId = TexUtil.CreateTextureFromFile(openFile.FileName);
                //_useMatrixCalibration = true;
                _listImagePoints = new List<Vector2>();
                Refresh();
            }

            //textBox1.Text = ""+_p2;
            
           
        }
        
        private void GetCamParams()
        {
            XDocument calibration = XDocument.Load("../../calibration.xml");


            //String valfx = calibration.Element("calibration").Element("fx").Value;
            _fx = Convert.ToDouble(calibration.Element("calibration").Element("fx").Value.Replace(".", ","));
            _fy = Convert.ToDouble(calibration.Element("calibration").Element("fy").Value.Replace(".", ","));

            _k1 = Convert.ToDouble(calibration.Element("calibration").Element("k1").Value.Replace(".", ","));
            _k2 = Convert.ToDouble(calibration.Element("calibration").Element("k2").Value.Replace(".", ","));
            _k3 = Convert.ToDouble(calibration.Element("calibration").Element("k3").Value.Replace(".", ","));

            _p1 = Convert.ToDouble(calibration.Element("calibration").Element("p1").Value.Replace(".", ","));
            _p2 = Convert.ToDouble(calibration.Element("calibration").Element("p2").Value.Replace(".", ","));




            ///////
            _camIntParams = new double[3, 3] { { _fx,      0,      this.Width/2.0},
                                               {   0,    _fy,     this.Height/2.0},
                                               {   0,      0,                   1} };

            _distorsionCoef = new double[] { _k1, _k2, _p1, _p2 };





            //textBox1.Text = "" + _p2;

            //ComputeProjectionMatrix();
            ComputeMatIntreseque();


        }

        private void ComputeProjectionMatrix()
        {
            float w = glControl1.Width;
            float h = glControl1.Height;
            float fx = (float)_fx;
            float fy = (float)_fy;

            float zfar = 1000;
            float zNear = 1;

            float cx = w/2;
            float cy = h/2;

            /*_glProjectionMatrix = new double[16] {2*(fx/w), 0,  0,  0,
                                                        0, -2* (fy/h), 0, 0,
                                                        1-2 * (cx/w), 1-2*(cy/h), -(zfar+zNear)/(zfar-zNear), -1,
                                                        0, 0, -2* (zfar*zNear)/(zfar-zNear), 0};*/

            _MYglProjectionMatrix = new Matrix4(2*(fx/w),0,0,0,
                                             0, -2*(fy/h),0,0,
                                             1-2 * (cx/w), 1-2*(cy/h), -(zfar + zNear) / (zfar - zNear), 1,
                                             0, 0, -2 * (zfar * zNear) / (zfar - zNear), 0 );

            //_useMatrixCalibration = true;
        }

        private void ComputeMatIntreseque()
        {
            int w = glControl1.Width;
            int h = glControl1.Height;

            _matIntreseque = new Matrix3d(_fx,   0, w / 2,
                                          0,   _fy, h / 2,
                                          0,     0,     1);
        }

        private void DrawPoint()
        {
            GL.PointSize(5f);

            foreach(Vector2 point in _listImagePoints)
            {
                GL.Begin(PrimitiveType.Points);
                GL.Color3(1.0f, 0.0f, 0.0f);
                //GL.Vertex3(-1, 1, -1);
                GL.Vertex2(point.X, point.Y);
                GL.End();               
            }

            
            GL.Color3(1.0f, 1.0f, 1.0f);

            
        }

        private void glControl1_MouseClick(object sender, MouseEventArgs e)
        {
            _click = true;
            _mouseX = e.X;
            _mouseY =glControl1.Height - e.Y;
            //textBox1.Text = "(" + e.X + "," + e.Y + ")";
            Vector2 v = new Vector2(_mouseX, _mouseY);

            if (_listImagePoints.Count < 4)
            {
                _listImagePoints.Add(new Vector2(_mouseX, _mouseY));
            }


            
            Refresh();
        }


        //compute Pose
        private void ComputePose(List<Vector3> objectPoints, List<Vector2> imagePoints, double[,] camMatrix, 
                                 bool useDistCoeffs, double[] distorsionCoef, out double[] transVect, out double[] rvec,
                                 out double[,] rotMat, out double reprojectionErrorPx, bool updateTAndR, out CvMat tvec_copy,
                                 out CvMat cvrvec_copy)
        {


            int point_count = 4;

            rvec = new double[3];
            transVect = new double[3];
            rotMat = new double[3, 3];

            CvMat objPoints = cvlib.CvCreateMat(3, point_count, cvlib.CV_32FC1);
            CvMat imgPoints = cvlib.CvCreateMat(2, point_count, cvlib.CV_32FC1);
            CvMat K = cvlib.CvCreateMat(3, 3, cvlib.CV_32FC1);
            CvMat cvrvec = cvlib.CvCreateMat(3, 1, cvlib.CV_32FC1);
            CvMat tvec = cvlib.CvCreateMat(3, 1, cvlib.CV_32FC1);
            CvMat distCoeff = cvlib.CvCreateMat(1, 4, cvlib.CV_32FC1);
            CvMat R = cvlib.CvCreateMat(3, 3, cvlib.CV_32FC1);

            // Initialisation des données
            // Points image
            for (int i = 0; i < point_count; i++)
            {
                cvlib.CvSetReal2D(ref imgPoints, 0, i, imagePoints[i].X);
                cvlib.CvSetReal2D(ref imgPoints, 1, i, imagePoints[i].Y);
            }
            // Points objet
            for (int i = 0; i < point_count; i++)
            {
                cvlib.CvSetReal2D(ref objPoints, 0, i, objectPoints[i].X);
                cvlib.CvSetReal2D(ref objPoints, 1, i, objectPoints[i].Y);
                cvlib.CvSetReal2D(ref objPoints, 2, i, objectPoints[i].Z);
            }
            // Matrice de camera interne
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    cvlib.CvSetReal2D(ref K, i, j, camMatrix[i, j]);
                }
            }

            if (useDistCoeffs)
            {
                cvlib.CvSetReal1D(ref distCoeff, 0, distorsionCoef[0]);
                cvlib.CvSetReal1D(ref distCoeff, 1, distorsionCoef[1]);
                cvlib.CvSetReal1D(ref distCoeff, 2, distorsionCoef[2]);
                cvlib.CvSetReal1D(ref distCoeff, 3, distorsionCoef[3]);
                //cvlib.CvSetReal1D(ref distCoeff, 4, distorsionCoef[4]);
            }
            else // On force les coeffs de distortion a zéro pour éviter les problèmes de pointeurs
            {
                cvlib.CvSetReal1D(ref distCoeff, 0, 0);
                cvlib.CvSetReal1D(ref distCoeff, 1, 0);
                cvlib.CvSetReal1D(ref distCoeff, 2, 0);
                cvlib.CvSetReal1D(ref distCoeff, 3, 0);
                //cvlib.CvSetReal1D(ref distCoeff, 4, 0);
            }

            cvlib.CvFindExtrinsicCameraParams2(ref objPoints, ref imgPoints, ref K, ref distCoeff, ref cvrvec, ref tvec);

            // Mise à jour des vecteur tvec et rvec
            /*if (updateTAndR)
            {
                tvec = tvec;
                this.cvrvec = cvrvec;
            }*/


            // Copie du vecteur translation et de rotation de opencv->transVect
            for (int j = 0; j < 3; j++)
            {
                transVect[j] = cvlib.CvGetReal1D(ref tvec, j);
                rvec[j] = cvlib.CvGetReal1D(ref cvrvec, j);

            }

            CvMat jcb = cvlib.CvCreateMat(3, 9, cvlib.CV_32FC1);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    cvlib.CvSetReal2D(ref jcb, i, j, 0);
                }
            }

            // Manip StackOverFlow reference opencv opengl


            cvlib.CvSetReal1D(ref cvrvec, 0, -cvlib.CvGetReal1D(ref cvrvec, 0));
            cvlib.CvSetReal1D(ref cvrvec, 1, -cvlib.CvGetReal1D(ref cvrvec, 1));

            cvlib.CvRodrigues2(ref cvrvec, ref R, ref jcb);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    rotMat[i, j] = cvlib.CvGetReal2D(ref R, i, j);
                }
            }




            ////////////////////////////////////////////
            ///Calcul des erreurs de  reprojection de l'objet de calibration
            ///////////////////////////////////////////

            cvlib.CvSetReal1D(ref cvrvec, 0, -cvlib.CvGetReal1D(ref cvrvec, 0)); // car on a inversé avant
            cvlib.CvSetReal1D(ref cvrvec, 1, -cvlib.CvGetReal1D(ref cvrvec, 1)); // idem
            CvMat imgPointsReprojected = cvlib.CvCreateMat(2, point_count, cvlib.CV_32FC1);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    cvlib.CvSetReal2D(ref jcb, i, j, 0);
                }
            }

            cvlib.CvRodrigues2(ref cvrvec, ref R, ref jcb);

            // Mise à jour de la matrice de rotation
            /*if (updateTAndR)
            {
                this.R = R;
            }*/

            // Calcul de la représentation en quaternions
            double[,] r = new double[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    r[i, j] = cvlib.CvGetReal2D(ref R, i, j);
                }
            }

            // http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/
            if (updateTAndR)
            {
                double w = System.Math.Sqrt(1.0 + r[0, 0] + r[1, 1] + r[2, 2]) / 2.0;
                double w4 = 4.0 * w;
                double xq = (r[2, 1] - r[1, 2]) / w4;
                double yq = (r[0, 2] - r[2, 0]) / w4;
                double zq = (r[1, 0] - r[0, 1]) / w4;

                //quaternion = new double[4] { w, xq, yq, zq };
            }


            cvlib.CvProjectPoints2(ref objPoints, ref cvrvec, ref tvec, ref K, ref distCoeff, ref imgPointsReprojected);

            reprojectionErrorPx = 0.0;

            for (int i = 0; i < point_count; i++)
            {
                double x = cvlib.CvGetReal2D(ref imgPoints, 0, i);
                double y = cvlib.CvGetReal2D(ref imgPoints, 1, i);

                double xRep = cvlib.CvGetReal2D(ref imgPointsReprojected, 0, i);
                double yRep = cvlib.CvGetReal2D(ref imgPointsReprojected, 1, i);

                double squareErr = (xRep - x) * (xRep - x) + (yRep - y) * (yRep - y);
                reprojectionErrorPx += System.Math.Sqrt(squareErr);
            }

            ///////////////////////////////////////////
            ///////////////////////////////////////////

            tvec_copy = cvlib.CvCreateMat(3, 1, cvlib.CV_32FC1);
            cvrvec_copy = cvlib.CvCreateMat(3, 1, cvlib.CV_32FC1);

            for (int i = 0; i < 3; i++)
            {
                cvlib.CvSetReal1D(ref tvec_copy, i, cvlib.CvGetReal1D(ref tvec, i));
                cvlib.CvSetReal1D(ref cvrvec_copy, i, cvlib.CvGetReal1D(ref cvrvec, i));
            }

            ///////////////////////////////////////////
            ///////////////////////////////////////////

            tvec_copy = cvlib.CvCreateMat(3, 1, cvlib.CV_32FC1);
            cvrvec_copy = cvlib.CvCreateMat(3, 1, cvlib.CV_32FC1);

            for (int i = 0; i < 3; i++)
            {
                cvlib.CvSetReal1D(ref tvec_copy, i, cvlib.CvGetReal1D(ref tvec, i));
                cvlib.CvSetReal1D(ref cvrvec_copy, i, cvlib.CvGetReal1D(ref cvrvec, i));
            }

           
        }

        //calibration
        private void Calibrate()
        {
            double[,] rotMat = new double[3, 3]; // Matrice de rotation adaptée à OpenGl 
            double[] rvec = new double[3];        // Vecteur de rotation
            double[] transVect = new double[3];   // Vecteur de translation

            double w = glControl1.Width;
            double h = glControl1.Height;
            double fx = _camIntParams[0, 0];
            double fy = _camIntParams[1, 1];

            double zfar = 1000;
            double zNear = 1;

            double cx = _camIntParams[0, 2];
            double cy = _camIntParams[1, 2];


            _glProjectionMatrix = new double[16] {2*(fx/w), 0,  0,  0,
                                                        0, -2* (fy/h), 0, 0,
                                                        1-2 * (cx/w), 1-2*(cy/h), -(zfar+zNear)/(zfar-zNear), -1,
                                                        0, 0, -2* (zfar*zNear)/(zfar-zNear), 0};

            double reprojectionError = 0.0;
            CvMat tvec_copy, cvrvec_copy;
            ComputePose(_listObjectPoints, _listImagePoints, _camIntParams, true, _distorsionCoef, out transVect, out rvec, out rotMat, out reprojectionError, true, out tvec_copy, out cvrvec_copy);
            //ReprojectionError = reprojectionError;

            modelViewTest = new Matrix4((float)rotMat[0, 0], (float)rotMat[1, 0], (float)rotMat[2, 0], 0,
                                        (float)rotMat[0, 1], (float)rotMat[1, 1], (float)rotMat[2, 1], 0,
                                        (float)rotMat[0, 2], (float)rotMat[1, 2], (float)rotMat[2, 2], 0,
                                        (float)transVect[0], (float)transVect[1], (float)-transVect[2], 1);
            modelViewTest.Invert();

            Vector4 eyePos = Vector4.Transform(new Vector4(0, 0, 0, 1), modelViewTest);
            eyePos /= eyePos.W;

            #region transformations des paramètres extrinsèques en eyePosition, upVector, targetPosition

            Vector3 realTransVect = new Vector3((float)transVect[0], (float)transVect[1], -(float)transVect[2]);

            _eye = new Vector3(eyePos.X, eyePos.Y, eyePos.Z);

            Matrix4 rotatMatrixTransPosed = new Matrix4((float)rotMat[0, 0], (float)rotMat[1, 0], (float)rotMat[2, 0], 0,
                                                       (float)rotMat[0, 1], (float)rotMat[1, 1], (float)rotMat[2, 1], 0,
                                                       (float)rotMat[0, 2], (float)rotMat[1, 2], (float)rotMat[2, 2], 0,
                                                       0, 0, 0, 1);
            rotatMatrixTransPosed.Transpose();

            _up = Vector3.Transform(new Vector3(0, 1, 0), rotatMatrixTransPosed);

            Vector3 ZVectorTransf = Vector3.Transform(new Vector3(0, 0, -1), rotatMatrixTransPosed); //on multiplie par -1 car la caméra OpenGL regarde en direction de -Z;

            //Vect3D target = Vect3D.Move(new Vect3D(_eyePosition.X, _eyePosition.Y, _eyePosition.Z), new Vect3D(ZVectorTransf.X, ZVectorTransf.Y, ZVectorTransf.Z), 20);


            _target = vectMove(new Vector3(_eye.X, _eye.Y, _eye.Z), new Vector3(ZVectorTransf.X, ZVectorTransf.Y, ZVectorTransf.Z), 20);

            Matrix4 modelView = Matrix4.LookAt(_eye, _target, _up);
            _modelView = modelView;

            //modelView.Transpose();

            #endregion

        }

        private void button2_Click(object sender, EventArgs e)
        {
           if(_listImagePoints.Count==4)
            {
                Calibrate();
                _isCalibrated = true;
            }
            
            

        }

        Vector3 vectMove(Vector3 p_point, Vector3 p_dir, double p_dist)
        {
            Vector3 l_dir = p_dir;
            l_dir.Normalize();
            
            

            
            return Vector3.Add(p_point, l_dir);//Sum(p_point, l_dir);
        }

        private void Set3DPoints()
        {
            _listObjectPoints.Add(new Vector3(0, 0, 0));
            _listObjectPoints.Add(new Vector3(0, 14.8f, 0));
            _listObjectPoints.Add(new Vector3(21f, 14.8f, 0));
            _listObjectPoints.Add(new Vector3(0, 14.8f, 0));
        }
    }
}
