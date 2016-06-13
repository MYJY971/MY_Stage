using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using TexLib;

using System.Xml.Linq;

using CalibrationLibrary;

using openCV;


namespace MYCalibration_v4
{
    public partial class Form1 : Form
    {
        bool _initContextGLisOk = false;
        Color4 _background = new Color4(0.4f, 1.0f, 1.0f, 0.3f);

        private Vector3 _eye = new Vector3(-5.0f, 0.0f, 0.0f);
        private Vector3 _target = Vector3.Zero;
        private Vector3 _up = new Vector3(0.0f, 0.0f, 1.0f);

        private int _backgroundTextureId, _objectTextureId;

        private float _mouseX, _mouseY; 
        

        private List<Vector2> _listImagePoints = new List<Vector2>();
        List<Vector3> _listObjectPoints = new List<Vector3>();

        private String _calibrationPathFile = "../../../../../DATA/calibration.xml";

        private String _currentImagePath = "../../../../../DATA/photos/photo_26-5-2016_9-15-29-432";

        private Camera _calibratedCam,_surfaceCam, _spectatorCam, _currentCam;

        private float _factorSizeWindow;

        private float _angleX, _angleY, _angleZ;

        public Form1()
        {
            InitializeComponent();

        }

        



        private void glControl1_Load(object sender, EventArgs e)
        {
            glControl1.Resize += glControl1_Resize;

            Size imageSize;
            _backgroundTextureId = TexUtil.CreateTextureFromFile(_currentImagePath+".jpg", out imageSize);
            _objectTextureId = TexUtil.CreateTextureFromFile("../../../photos/texture.png");
            glControl1.Size = AdaptSize(imageSize);

            panel2.Size = panel2.Size;
            //glControl1.Width = imageSize.Width / 2;
            //glControl1.Height = imageSize.Height / 2;

            /*int w = glControl1.Width;
            int h = glControl1.Height;

            _calibratedCam = new CalibrationCam(w, h, _eye, _target, _up); //new neoClassicCam(w, h, _eye,_target,_up);
            _calibratedCam.SetColor(Color.BlueViolet);
            _calibratedCam.SetBackgroundTextureId(_backgroundTextureId);

           

            _surfaceCam = new ClassicCam(w, h, _eye, _target, _up);
            _surfaceCam.SetColor(Color.Azure);
            _surfaceCam.SetBackgroundTextureId(_backgroundTextureId);

            _spectatorCam = new SpectatorCam(w, h, _eye, _target, _up, _calibratedCam, _surfaceCam); 
            
            _currentCam = _calibratedCam;*/


            initCameras();

            
            
            SetImagePoints();
            Set3DPoints();
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            base.OnSizeChanged(e);

            SetupViewport();

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

        protected void InitGLContext()
        {

            GL.Enable(EnableCap.Texture2D);                       // Enable Texture Mapping
            GL.ShadeModel(ShadingModel.Smooth);                   // Enable Smooth Shading
            GL.ClearColor(_background);                           // Clear the Color

            // Clear the Color and Depth Buffer
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.ClearDepth(1.0f);										     // Depth Buffer Setup
            GL.Enable(EnableCap.DepthTest);								     // Enables Depth Testing
            GL.DepthFunc(DepthFunction.Lequal);							     // The Type Of Depth Testing To Do
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);  // Really Nice Perspective Calculations


            // Lumiere

            float[] lightPos = { 10.0f, 10.0f, 100.0f, 1.0f };
            GL.Light(LightName.Light0, LightParameter.Position, lightPos);
            GL.Light(LightName.Light0, LightParameter.SpotDirection, new float[] { 0, 0, -1 });

            GL.Light(LightName.Light0, LightParameter.Ambient, new Color4(1.0f, 1.0f, 1.0f, 1.0f));
            GL.Light(LightName.Light0, LightParameter.Diffuse, new Color4(0.6f, 0.6f, 0.6f, 1.0f));
            GL.Light(LightName.Light0, LightParameter.Specular, new Color4(0.6f, 0.6f, 0.6f, 1.0f));

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.ColorMaterial);
            //GL.Enable(EnableCap.CullFace);

            _initContextGLisOk = true;

        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!_initContextGLisOk)
                InitGLContext();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            
            ///Background 2D
            _currentCam.Background();

            if (_listImagePoints.Count>0 && _currentCam==_calibratedCam && _calibratedCam._isCalibrated==false)
                DrawPoint();

            ///Scene 3D
     
            _currentCam.Projection();

            _currentCam.LookAt();

            GL.Scale(1, 1, -1);
            //Draw the scene
            DrawScene();

            glControl1.SwapBuffers();
        }

        #region Draw Methodes

        #region Background
        private void DrawBackground()
        {
            int w = glControl1.Width;
            int h = glControl1.Height;

            //réinitialise la couleur
            GL.Color3(1.0f, 1.0f, 1.0f);

            GL.BindTexture(TextureTarget.Texture2D, _backgroundTextureId);

            GL.Begin(BeginMode.Quads);


            GL.TexCoord2(0, 1);GL.Vertex2(0, 0);
            GL.TexCoord2(1, 1);GL.Vertex2(w, 0);
            GL.TexCoord2(1, 0);GL.Vertex2(w, h);
            GL.TexCoord2(0, 0);GL.Vertex2(0, h);

            GL.End();
        }
        #endregion

        #region Scene
        private void DrawScene()
        {

            /*GL.Rotate(_angle, Vector3d.UnitZ);
            GL.Translate(0.0f, 3.0f, 0.0f);
            DrawCube();
            GL.Translate(0.0f, -3.0f, 0.0f); 
            GL.Rotate(-_angle, Vector3d.UnitZ);*/

            //Matrix4 rot = Matrix4.CreateRotationZ((float)Math.PI / 2);

            //GL.Rotate(-90, Vector3d.UnitZ);
            DrawTrihedral();
           // GL.Rotate(90, Vector3d.UnitZ);

            DrawObject3D();


            //_surfaceCam.Draw();
            //_surfaceCam.DrawPlan(Color4.Red);
            //_calibratedCam.Draw();
            //_calibratedCam.DrawPlan(Color4.White);
            _currentCam.Draw();
            _currentCam.DrawPlan(new Color4(1.0f,1.0f,1.0f,1.0f));

            //DrawCamera();
        }
        #endregion

        #region Objects
        private void DrawObject3D()
        {

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, new Color4(0.3f, 0.3f, 0.3f, 1.0f));
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, 12.8f);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Emission, new Color4(0.0f, 0.0f, 0.0f, 1.0f));

            GL.Color3(1.0f, 1.0f, 1.0f);
            GL.Enable(EnableCap.Texture2D);

            GL.BindTexture(TextureTarget.Texture2D, _objectTextureId);
            GL.Begin(BeginMode.Quads);

            //Bottom face
            GL.Normal3(0.0f, 0.0f, -1.0f);
            GL.TexCoord2(1, 1); GL.Vertex3(0.042f, -0.0296f, 0.0f);
            GL.TexCoord2(0, 1); GL.Vertex3(0.168f, -0.0296f, 0.0f);
            GL.TexCoord2(0, 0); GL.Vertex3(0.168f, -0.1184f, 0.0f);
            GL.TexCoord2(1, 0); GL.Vertex3(0.042f, -0.1184f, 0.0f);
            //Left face
            GL.Normal3(0.0f, 1.0f, 0.0f);
            GL.TexCoord2(0, 0); GL.Vertex3(0.042f, -0.0296f, 0.0f);
            GL.TexCoord2(1, 0); GL.Vertex3(0.042f, -0.0296f, 0.05f);
            GL.TexCoord2(1, 1); GL.Vertex3(0.042f, -0.1184f, 0.05f);
            GL.TexCoord2(0, 1); GL.Vertex3(0.042f, -0.1184f, 0.0f);
            //Back face
            GL.Normal3(1.0f, 0.0f, 0.0f);
            GL.TexCoord2(0, 1); GL.Vertex3(0.042f, -0.0296f, 0.0f);
            GL.TexCoord2(1, 1); GL.Vertex3(0.042f, -0.0296f, 0.05f);
            GL.TexCoord2(1, 0); GL.Vertex3(0.168f, -0.0296f, 0.05f);
            GL.TexCoord2(0, 0); GL.Vertex3(0.168f, -0.0296f, 0.0f);
            //Right face
            GL.Normal3(0.0f, -1.0f, 0.0f);
            GL.TexCoord2(1, 0); GL.Vertex3(0.168f, -0.0296f, 0.0f);
            GL.TexCoord2(1, 1); GL.Vertex3(0.168f, -0.0296f, 0.05f);
            GL.TexCoord2(0, 1); GL.Vertex3(0.168f, -0.1184f, 0.05f);
            GL.TexCoord2(0, 0); GL.Vertex3(0.168f, -0.1184f, 0.0f);
            //Front face
            GL.Normal3(-1.0f, 0.0f, 0.0f);
            GL.TexCoord2(0, 0); GL.Vertex3(0.042f, -0.1184f, 0.0f);
            GL.TexCoord2(1, 0); GL.Vertex3(0.042f, -0.1184f, 0.05f);
            GL.TexCoord2(1, 1); GL.Vertex3(0.168f, -0.1184f, 0.05f);
            GL.TexCoord2(0, 1); GL.Vertex3(0.168f, -0.1184f, 0.0f);
            //Top face
            GL.Normal3(0.0f, 0.0f, 1.0f);
            GL.TexCoord2(0, 1); GL.Vertex3(0.042f, -0.0296f, 0.05f);
            GL.TexCoord2(0, 0); GL.Vertex3(0.168f, -0.0296f, 0.05f);
            GL.TexCoord2(1, 0); GL.Vertex3(0.168f, -0.1184f, 0.05f);
            GL.TexCoord2(1, 1); GL.Vertex3(0.042f, -0.1184f, 0.05f);

            GL.End();

            GL.Disable(EnableCap.Texture2D);

            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Begin(BeginMode.Points);
            foreach(Vector3 point in _listObjectPoints)
            {
                GL.Vertex3(point);
            }
            GL.End();

            GL.Color3(1.0f, 1.0f, 1.0f);
        }

        private void DrawPlan()
        {
            GL.Begin(BeginMode.Lines);

            float x = 0.5f;
            float y = 0.5f;

            //horizontale
            GL.Vertex3(-x, y, 0.0f);
            GL.Vertex3(x, y, 0.0f);

            for (int i = 0; i < 10; ++i)
            {
                y = y - 0.1f;
                GL.Vertex3(-x, y, 0.0f);
                GL.Vertex3(x, y, 0.0f);
            }

            y = 0.5f;

            //Verticale
            GL.Vertex3(x, y, 0.0f);
            GL.Vertex3(x, -y, 0.0f);

            for (int i = 0; i < 10; ++i)
            {
                x = x - 0.1f;
                GL.Vertex3(x, y, 0.0f);
                GL.Vertex3(x, -y, 0.0f);
            }

            //GL.Vertex3(1.0f, 1.0f, 0.0f);



            GL.End();
        }

        private void DrawCube()
        {
            float[] l_couleur = new float[4];
            
            // axe X
            //GL.Color4(1.0f, 0.0f, 0.0f, 0.5f);
            //l_couleur[0] = 1.0f; l_couleur[1] = 0.0f; l_couleur[2] = 0.0f; l_couleur[3] = 1.0f;
            ///*
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, new Color4(0.3f, 0.3f, 0.3f, 1.0f));
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, 128f);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Emission, new Color4(0.0f, 0.0f, 0.0f, 1.0f)/*@l_couleur*/);

            GL.Begin(BeginMode.Quads);
            // Front Face
            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.Normal3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(-0.5f, -0.5f, 0.5f);
            GL.Vertex3(0.5f, -0.5f, 0.5f);
            GL.Vertex3(0.5f, 0.5f, 0.5f);
            GL.Vertex3(-0.5f, 0.5f, 0.5f);

            // Back Face
            //GL.Color3(0.0f, 1.0f, 0.0f);
            GL.Normal3(0.0f, 0.0f, -1.0f);
            GL.Vertex3(-0.5, -0.5, -0.5);
            GL.Vertex3(-0.5, 0.5, -0.5);
            GL.Vertex3(0.5, 0.5, -0.5);
            GL.Vertex3(0.5, -0.5, -0.5);

            // Top Face
            //GL.Color3(1.0f, 1.0f, 0.0f);
            GL.Normal3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(-0.5, 0.5, -0.5);
            GL.Vertex3(-0.5, 0.5, 0.5);
            GL.Vertex3(0.5, 0.5, 0.5);
            GL.Vertex3(0.5, 0.5, -0.5);

            // Bottom Face
            //GL.Color3(1f, 0.4f, 0f);
            GL.Normal3(0.0f, -1.0f, 0.0f);
            GL.Vertex3(-0.5, -0.5, -0.5);
            GL.Vertex3(0.5, -0.5, -0.5);
            GL.Vertex3(0.5, -0.5, 0.5);
            GL.Vertex3(-0.5, -0.5, 0.5);

            // Right face
            //GL.Color3(1f, 0f, 1f);
            GL.Normal3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(0.5, -0.5, -0.5);
            GL.Vertex3(0.5, 0.5, -0.5);
            GL.Vertex3(0.5, 0.5, 0.5);
            GL.Vertex3(0.5, -0.5, 0.5);

            // Left Face
            //GL.Color3(0.0f, 0.0f, 1.0f);
            GL.Normal3(-1.0f, 0.0f, 0.0f);
            GL.Vertex3(-0.5, -0.5, -0.5);
            GL.Vertex3(-0.5, -0.5, 0.5);
            GL.Vertex3(-0.5, 0.5, 0.5);
            GL.Vertex3(-0.5, 0.5, -0.5);

            GL.Color3(1.0f, 1.0f, 1.0f);

            GL.End();

        }

        private void DrawPoint()
        {
            GL.PointSize(5f);

            foreach (Vector2 point in _listImagePoints)
            {
                GL.Begin(BeginMode.Points);
                GL.Color3(1.0f, 0.0f, 0.0f);
                //GL.Vertex3(-1, 1, -1);
                GL.Vertex2(point.X, glControl1.Height - point.Y);
                GL.End();
                GL.Color3(1.0f, 1.0f, 1.0f);
            }

        }

        private void DrawTrihedral()
        {
            //DrawTrihedral ///////////////////////

            float[] l_couleur = new float[4];
            float l_shin;
            float l_lenghAxis = _currentCam._eye.Length/10;//0.1f;
            float l_flecheW = 0.01f; float l_flecheH = 0.005f;

            // axe X
            GL.Color4(1.0f, 0.0f, 0.0f, 1.0f);
            l_couleur[0] = 1.0f; l_couleur[1] = 0.0f; l_couleur[2] = 0.0f; l_couleur[3] = 1.0f;
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Ambient, @l_couleur);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, @l_couleur);
            l_shin = 128;
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, l_shin);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, @l_couleur);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Emission, @l_couleur);

            GL.Begin(BeginMode.LineStrip);
            GL.Vertex3(0.0f, 0.0f, 0.0001f);
            GL.Vertex3(l_lenghAxis, 0.0f, 0.0f);
            GL.End();

            // point axe X
            GL.Begin(BeginMode.LineStrip);
            GL.Vertex3(l_lenghAxis - l_flecheW, l_flecheH, 0.0f);
            GL.Vertex3(l_lenghAxis, 0.0f, 0.0f);
            GL.Vertex3(l_lenghAxis - l_flecheW, -l_flecheH, 0.0f);
            GL.End();

            // axe Y
            GL.Vertex4(0.0f, 1.0f, 0.0f, 1.0f);
            l_couleur[0] = 0.0f; l_couleur[1] = 1.0f; l_couleur[2] = 0.0f; l_couleur[3] = 1.0f;
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Ambient, @l_couleur);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, @l_couleur);
            l_shin = 128;
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, l_shin);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, @l_couleur);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Emission, @l_couleur);
            GL.Color3(0.0f, 1.0f, 0.0f);

            GL.Begin(BeginMode.LineStrip);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, l_lenghAxis, 0.0001f);
            GL.End();
            // pointe axe Y
            GL.Begin(BeginMode.LineStrip);
            GL.Vertex3(0.0f - l_flecheH, l_lenghAxis - l_flecheW, 0.0f);
            GL.Vertex3(0.0f, l_lenghAxis, 0.0f);
            GL.Vertex3(0.0f + l_flecheH, l_lenghAxis - l_flecheW, 0.0f);
            GL.End();

            // axe Z
            GL.Color4(0.0f, 0.0f, 1.0f, 1.0f);
            l_couleur[0] = 0.0f; l_couleur[1] = 0.0f; l_couleur[2] = 1.0f; l_couleur[3] = 1.0f;
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Ambient, @l_couleur);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, @l_couleur);
            l_shin = 128;
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, l_shin);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, @l_couleur);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Emission, @l_couleur);
            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.Begin(BeginMode.LineStrip);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, l_lenghAxis);
            GL.End();
            // pointe axe Z
            GL.Begin(BeginMode.LineStrip);
            GL.Vertex3(l_flecheH, 0.0f, l_lenghAxis - l_flecheW);
            GL.Vertex3(0.0f, 0.0f, l_lenghAxis);
            GL.Vertex3(-l_flecheH, 0.0f, l_lenghAxis - l_flecheW);
            GL.End();
        }
        #endregion

        #endregion





        private void Calibrate()
        {
            _calibratedCam.Calibrate(_calibrationPathFile, _listImagePoints, _listObjectPoints);
            //_surfaceCam.SetTarget(_calibratedCam._target);
            //_surfaceCam.SetEye(_calibratedCam._eye);
            //_surfaceCam.SetUp(_calibratedCam._up);
            //_surfaceCam.ChangePerspective(_calibratedCam._projectionMatrixDouble);
            _surfaceCam.Calibrate(_calibratedCam);
            

        }

        private void Set3DPoints()
        {
            _listObjectPoints.Add(new Vector3(0f, 0f, 0f));
            _listObjectPoints.Add(new Vector3(0.21f, 0f, 0f)); //2.20f,0f,0f));
            _listObjectPoints.Add(new Vector3(0.21f, -0.148f, 0f ));//2.20f,-1.45f,0f));
            _listObjectPoints.Add(new Vector3(0f, -0.148f, 0f )); //0f,-1.45f,0f));
        }

        private void SetImagePoints()
        {
            _listImagePoints = new List<Vector2>();

            //string p= "../../../photos/";
            if (_currentImagePath == "C:\\Stage\\Yanis\\CSharp_projects\\MY_Stage\\CalibrationTest\\MYCalibration_v4\\photos\\photo_2-6-2016_15-53-24-270")
            {
                _listImagePoints.Add(new Vector2(476f, 430)); //392.296f,  221.876f
                _listImagePoints.Add(new Vector2(489, 279)); //634.203f,  189.495f
                _listImagePoints.Add(new Vector2(670, 293)); //681.638f,  287.72f
                _listImagePoints.Add(new Vector2(695, 439)); //412.448f,  327.44f
            }
            else
            {
                _listImagePoints.Add(new Vector2(380f, 146f)); //392.296f,  221.876f
                _listImagePoints.Add(new Vector2(667f, 148f)); //634.203f,  189.495f
                _listImagePoints.Add(new Vector2(661f, 273f)); //681.638f,  287.72f
                _listImagePoints.Add(new Vector2(334f, 269f)); //412.448f,  327.44f
            }
            
        }


        #region Event Listeners
        #region glControl 
        private void glControl1_MouseClick(object sender, MouseEventArgs e)
        {
            //_click = true;
            _mouseX = e.X;
            _mouseY = e.Y;
            //textBox1.Text = "(" + e.X + "," + e.Y + ")";
            Vector2 v = new Vector2(_mouseX, _mouseY);

            if (_currentCam == _calibratedCam)
            {
                if (_listImagePoints.Count < 4)
                {
                    _listImagePoints.Add(new Vector2(_mouseX, _mouseY));
                }
            }


            if (_listImagePoints.Count == 4)
            {
                textBox1.Text = "" + _listImagePoints.ElementAt(0);
                textBox2.Text = "" + _listImagePoints.ElementAt(1);
                textBox3.Text = "" + _listImagePoints.ElementAt(2);
                textBox4.Text = "" + _listImagePoints.ElementAt(3);
            }

            //Matrix4 rotationMat = Matrix4.CreateRotationZ(_angle);
            //_eye = Vector3.Transform(_eye, rotationMat);

            //
            /*textBoxXangle.Text = "" + _calibratedCam._eye;
            textBoxYangle.Text = "" + _calibratedCam._target;
            textBoxZangle.Text = "" + _calibratedCam._up;

            textBox5.Text = "" + _surfaceCam._eye;
            textBox6.Text = "" + _surfaceCam._target;
            textBox7.Text = "" + _surfaceCam._up;*/
            //

            Refresh();
        }
        #endregion

        #region Button 
        private void button1_Click(object sender, EventArgs e)
        {


            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Image Files (*.tif; *.dcm; *.jpg; *.jpeg; *.bmp)|*.tif; *.dcm; *.jpg; *.jpeg; *.bmp";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                Size imageSize;
                _backgroundTextureId = TexUtil.CreateTextureFromFile(openFile.FileName, out imageSize);
                glControl1.Size = AdaptSize(imageSize);
                panel2.Size = panel2.Size;
                /*_calibratedCam.SetBackgroundTextureId(_backgroundTextureId);
                _surfaceCam.SetBackgroundTextureId(_backgroundTextureId);
                //glControl1.Width = imageSize.Width / 2;
                //glControl1.Height = imageSize.Height / 2;*/
                _listImagePoints = new List<Vector2>();
                /*_calibratedCam._isCalibrated = false;
                _calibratedCam.SetLookat(_eye, _target, _up);
                _surfaceCam.SetLookat(_eye, _target, _up);*/

                _currentImagePath = openFile.FileName.Substring(0,openFile.FileName.Length-4);

                initCameras();

                

                Refresh();
            }

            //textBox1.Text = ""+_p2;
            //textBox1.Text = "" + _listImagePoints.Count;

        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            if (_listImagePoints.Count == 4)
            {

                Calibrate();
                
                Refresh();

            }

        }

        private void buttonReinit_Click(object sender, EventArgs e)
        {
            _listImagePoints = new List<Vector2>();
            //_calibratedCam._isCalibrated = false;
            /*_calibratedCam.SetLookat(_eye, _target, _up);
            _calibratedCam.ReinitializePosition();
            _surfaceCam.SetLookat(_eye, _target, _up);
            _currentCam = _calibratedCam;*/
            initCameras();
            Refresh();
        }

        private void buttonDefaultPoints_Click(object sender, EventArgs e)
        {
            SetImagePoints();
        }

        private void buttonCorrection_Click(object sender, EventArgs e)
        {
            float yVal, zVal;
            _calibratedCam.Correction(_surfaceCam,out yVal,out zVal);
            textBoxYangle.Text =""+ yVal;
            textBoxZangle.Text = "" + zVal;
            Refresh();
        }

        private void buttonCamCalib_Click(object sender, EventArgs e)
        {
            _currentCam = _calibratedCam;
            Refresh();
        }

        private void buttonSurfaceCam_Click(object sender, EventArgs e)
        {
            _currentCam = _surfaceCam;
            Refresh();
        }

        private void buttonSpectatorCam_Click(object sender, EventArgs e)
        {
            _currentCam = _spectatorCam;
            //_spectatorCam._target = _calibratedCam._eye;
            Refresh();
        }


        #endregion

        #region Keys
        private void glControl1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs key)
        {
            //base.OnPreviewKeyDown(key);
            //if (_currentCam == _spectatorCam)
            //{
            float radianAngleX;
            float radianAngleY;
            float radianAngleZ;
            Matrix4 rotX; 
            Matrix4 rotY;
            Matrix4 rotZ;
            Matrix4 rotation;
            switch (key.KeyCode)
                {
                    case Keys.Up:
                        _spectatorCam.KeyUP();
                        break;
                    case Keys.Down:
                        _spectatorCam.KeyDOWN();
                        break;
                    case Keys.Left:
                        _spectatorCam.KeyLEFT();
                        break;
                    case Keys.Right:
                        _spectatorCam.KeyRIGHT();
                        break;

                    case Keys.Z:
                        _spectatorCam.KeyZ();
                        break;
                    case Keys.Q:
                        _surfaceCam.KeyS();
                        break;
                    case Keys.S:
                        _spectatorCam.KeyS();
                        break;
                    case Keys.D:
                        _surfaceCam.KeyZ();
                        break;
                    case Keys.Space:
                        _spectatorCam.KeySpace();
                        break;
                    case Keys.X:
                    _angleZ = -0.01f;//(float)Math.PI * 180 / 180;
                    _surfaceCam.RotateEye(_angleZ);
                    break;
                    case Keys.I:
                        _spectatorCam.ReinitializePosition();
                        break;
                    case Keys.C:
                    _angleZ =0.01f;//(float)Math.PI * 180 / 180;
                    _surfaceCam.RotateEye(_angleZ);
                    
                        break;
                    case Keys.V:
                    _angleY+= 0.005f;
                    radianAngleX = (float)Math.PI * _angleX / 180;
                    radianAngleY = (float)Math.PI * _angleY / 180;
                    radianAngleZ = (float)Math.PI * _angleZ / 180;

                    rotX = Matrix4.CreateRotationX(radianAngleX);
                    rotY = Matrix4.CreateRotationY(radianAngleY);
                    rotZ = Matrix4.CreateRotationZ(radianAngleZ);

                    rotation = Matrix4.Mult(rotX, rotY);
                    rotation = Matrix4.Mult(rotation, rotZ);

                    _surfaceCam.RotateUp(rotation);
                    textBoxYangle.Text = "" +_angleY+"("+ radianAngleY+")";
                    break;
                    case Keys.B:
                    _angleZ += 0.005f;
                    radianAngleX = (float)Math.PI * _angleX / 180;
                    radianAngleY = (float)Math.PI * _angleY / 180;
                    radianAngleZ = (float)Math.PI * _angleZ / 180;

                    rotX = Matrix4.CreateRotationX(radianAngleX);
                    rotY = Matrix4.CreateRotationY(radianAngleY);
                    rotZ = Matrix4.CreateRotationZ(radianAngleZ);

                    rotation = Matrix4.Mult(rotX, rotY);
                    rotation = Matrix4.Mult(rotation, rotZ);

                    
                    _surfaceCam.RotateUp(rotation);
                    
                    break;
                case Keys.N:
                    

                    break;
                case Keys.NumPad1:
                        _currentCam.LookCam(0);
                        break;
                    case Keys.NumPad2:
                        _currentCam.LookCam(1);
                        break;
                    case Keys.NumPad3:
                        _currentCam.LookCam(2);
                        break;
                    case Keys.NumPad4:
                        _currentCam.ChangePerspective(0);
                        break;
                    case Keys.NumPad5:
                        _currentCam.ChangePerspective(1);
                        break;
                    case Keys.NumPad6:
                        _currentCam.ChangePerspective(2);
                        break;



                }
                Refresh();
            //}
        }

               


        #endregion

        #endregion

        public void initCameras()
        {
            int w = glControl1.Width;
            int h = glControl1.Height;

            _calibratedCam = new CalibrationCam(w, h, _eye, _target, _up); 
            _calibratedCam.SetColor(new Color4(75, 0, 130, 0));
            _calibratedCam.SetBackgroundTextureId(_backgroundTextureId);
            _calibratedCam.setPoints(_listObjectPoints);


            _surfaceCam = new SurfaceCam(w, h, _eye, _target, _up);
            _surfaceCam.SetColor(new Color4(244, 102, 27, 0));
            _surfaceCam.SetBackgroundTextureId(_backgroundTextureId);

            //_surfaceCam.RotateUp(GetSurfaceRot(_currentImagePath + ".xml"));
            //_surfaceCam.RotateTarget(GetSurfaceRot(_currentImagePath + ".xml"));
            _surfaceCam.RotateFromFile(_currentImagePath + ".xml");


            _spectatorCam = new SpectatorCam(w, h, _eye, _target, _up, _calibratedCam, _surfaceCam);

            _currentCam = _calibratedCam;

            _angleX = 0.0f;
            _angleY = 0.0f;
            _angleZ = 0.0f;
            
        }

        private Matrix4 GetSurfaceRot(string path)
        {
            Matrix4 res = Matrix4.Identity;
            try
            {
                XDocument matriceSensor = XDocument.Load(path);

                float M11, M12, M13,
                      M21, M22, M23,
                      M31, M32, M33;

                M11 = (float)Convert.ToDouble(matriceSensor.Element("sensor").Element("orientation").Element("M11").Value.Replace(".", ","));
                M12 = (float)Convert.ToDouble(matriceSensor.Element("sensor").Element("orientation").Element("M12").Value.Replace(".", ","));
                M13 = (float)Convert.ToDouble(matriceSensor.Element("sensor").Element("orientation").Element("M13").Value.Replace(".", ","));

                M21 = (float)Convert.ToDouble(matriceSensor.Element("sensor").Element("orientation").Element("M21").Value.Replace(".", ","));
                M22 = (float)Convert.ToDouble(matriceSensor.Element("sensor").Element("orientation").Element("M22").Value.Replace(".", ","));
                M23 = (float)Convert.ToDouble(matriceSensor.Element("sensor").Element("orientation").Element("M23").Value.Replace(".", ","));

                M31 = (float)Convert.ToDouble(matriceSensor.Element("sensor").Element("orientation").Element("M31").Value.Replace(".", ","));
                M32 = (float)Convert.ToDouble(matriceSensor.Element("sensor").Element("orientation").Element("M32").Value.Replace(".", ","));
                M33 = (float)Convert.ToDouble(matriceSensor.Element("sensor").Element("orientation").Element("M33").Value.Replace(".", ","));

                res = new Matrix4(M11, M12, M13, 0.0f,
                                  M21, M22, M23, 0.0f,
                                  M31, M32, M33, 0.0f,
                                  0.0f, 0.0f, 0.0f, 1.0f);

                //transformation pour adapter le repere de la surface à celui d'openGL
                //float magicNumber = 3.773402f;
                Matrix4 rotX = Matrix4.CreateRotationX(/*-90*/- (float)Math.PI / 2/**/);
                Matrix4 rotZ = Matrix4.CreateRotationZ(/*-90*/-(float)Math.PI / 2/**/);
                //Matrix4 rotY = Matrix4.CreateRotationY(/**/(float)Math.PI / 2 + magicNumber);

               res = Matrix4.Mult(res, rotX);
               res = Matrix4.Mult(res, rotZ);
               //res = Matrix4.Mult(res, rotY);


            }
            catch (Exception e)
            {

                Console.Error.WriteLine("" + e);
            }

            return res;
        }

        private Size AdaptSize(Size s)
        {

            /*if ((s.Width > s.Height) && (s.Width > panel2.Size.Width))
            {
                _factorSizeWindow =(float)panel2.Size.Width / (float)s.Width;
            }
            else if ((s.Width < s.Height) && (s.Height > panel2.Size.Height))
            {
                _factorSizeWindow = (float)panel2.Size.Height / (float)s.Height;
            }
            else
                _factorSizeWindow = 1.0f;

            return new Size((int)(s.Width * _factorSizeWindow), (int)(s.Height * _factorSizeWindow));*/
            float diffW, diffH;

            diffW = s.Width - panel2.Size.Width;
            diffH = s.Height - panel2.Size.Height;

            if(diffW>0 || diffH>0)
            {
                if(s.Width <= s.Height)
                {
                    _factorSizeWindow = (float)panel2.Size.Width / (float)s.Width;
                }
                else
                {
                    _factorSizeWindow = (float)panel2.Size.Height / (float)s.Height;
                }
            }
            else
            {
                _factorSizeWindow = 1.0f;
            }

            return new Size((int)(s.Width * _factorSizeWindow), (int)(s.Height * _factorSizeWindow));


        }

        /*private Size AdaptSize(Size s)
        {
            return FactoriseSize(FactoriseSize(s));
        }*/

        
    }
}
