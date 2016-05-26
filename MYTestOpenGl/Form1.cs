using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tao.OpenGl;
using Tao.FreeGlut;
using GLfloat = System.Single;
using Windows.Devices.Sensors;




namespace MYTestOpenGl
{
    public partial class Form1 : Form
    {
        private float[] _backgroundColor= new float[4] {0.4f, 1.0f, 1.0f, 0.3f };

        private bool _getInverse = false;

        private float _xrot = 0;
        private float _yrot = 0;
        private float _zrot = 0;
        private float _cuberot = 0;

        static double _angleH = 0;
        static double _angleV = 0;
        private static double _angleX = - Math.PI / 2;
        private static double _angleY = Math.PI / 2;
        private static double _angleZ = - Math.PI / 2;
        double radianAngle;

        private float _M11, _M12, _M13, _M21, _M22, _M23, _M31, _M32, _M33;

        private float[,] valMat = new float[3, 3];

        Matrix _matRot = new Matrix(3);
        //Matrix invMat = new Matrix(new float[3, 3] { { -0.937710083185839f, 0.345621601206191f, 0.0360275494416502f }, { -0.108481923561106f, -0.389717322666354f, 0.914550979388236f }, { 0.330018074190422f, 0.85369598891774f, 0.402943999924444f } });
        private Matrix _invMat;
        Vector _target0 = new Vector(0, 0, 0);
        Vector _target = new Vector(0, 0, 0);
        Vector _eye = new Vector(15, 0, 0);
        Vector _up0 = new Vector(0, 0, 1);
        Vector _up = new Vector(0, 0, 1);
        Vector _tmp = new Vector(0, 0, 0);

        RotationMatrix _matRotX = new RotationMatrix(0, _angleX);
        RotationMatrix _matRotY = new RotationMatrix(1, _angleY);
        RotationMatrix _matRotZ = new RotationMatrix(2, _angleZ);

        OrientationSensor _sensor;



        

        bool _initContextGLisOk=false;
        Timer _timer;
        
        public Form1()
        {
            InitializeComponent();

            _timer = new Timer();
            _timer.Interval = 1; //100ms
            _timer.Tick += _timer_Tick;
            _timer.Enabled = true;
            _timer.Start();

            simpleOpenGlControl1.InitializeContexts();
            simpleOpenGlControl1.Paint += SimpleOpenGlControl1_Paint;
            simpleOpenGlControl1.Resize += SimpleOpenGlControl1_Resize;
            simpleOpenGlControl1.KeyDown += SimpleOpenGlControl1_KeyDown;
            
            

            _sensor = OrientationSensor.GetDefault();
            if (_sensor != null)
            {
                _sensor.ReadingChanged += _sensor_ReadingChanged;
                
            }


        }

        private void _sensor_ReadingChanged(OrientationSensor sender, OrientationSensorReadingChangedEventArgs args)
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


            _matRot.SetValues(new float[3, 3] { { _M11, _M12, _M13 }, { _M21, _M22, _M23 }, { _M31, _M32, _M33 } });
            //_matRot = _matRot.ProductMat(_matRotX);
            //_matRot = _matRot.ProductMat(_matRotZ);

            if(_getInverse==false)
            {
                _invMat = _matRot.Inverse();
                _getInverse = true;
            }

            //_matRot = _matRot.ProductMat(_invMat);
            _matRot = _matRot.Rotate(0, (float)_angleX);
            _matRot = _matRot.Rotate(2, (float)_angleZ);
            //_matRot = _matRot.ProductMat(invMat);
            //tmp = target0.Translate(-1, matRot.ProductMat(eye));


            _tmp = _target0.Translate(-1, _matRot.ProductMat(_eye));
            _target = _tmp.Translate(_eye);

            //_target = _matRot.ProductMat(_eye);
            _up = _matRot.ProductMat(_up0);



            //target = matRot.ProductMat(target);

            textBoxM11.Text = string.Format("{0,8:0.00000}", _M11);
            textBoxM12.Text = string.Format("{0,8:0.00000}", _M12);
            textBoxM13.Text = string.Format("{0,8:0.00000}", _M13);
            textBoxM21.Text = string.Format("{0,8:0.00000}", _M21);
            textBoxM22.Text = string.Format("{0,8:0.00000}", _M22);
            textBoxM23.Text = string.Format("{0,8:0.00000}", _M23);
            textBoxM31.Text = string.Format("{0,8:0.00000}", _M31);
            textBoxM32.Text = string.Format("{0,8:0.00000}", _M32);
            textBoxM33.Text = string.Format("{0,8:0.00000}", _M33);
            //ComputeMatrixRotation();
            /*Glu.gluLookAt(15.0, 0.0, 0.0, // position oeil
               0.0, 0.0, 0.0,                // target
               0.0, 1.0, 0.0);              //Up*/

        }

        private void ComputeMatrixRotation()
        {

        }

        private void SimpleOpenGlControl1_KeyDown(object sender, KeyEventArgs key)
        {
            switch(key.KeyCode)
            {
                case Keys.D:


                    _angleH --;//Math.PI / 2;
                    radianAngle = Math.PI * _angleH / 180.0;
                    _matRotZ.Update(radianAngle);
                    
                    _tmp = _target0.Translate(-1, _matRotZ.ProductMat(_eye));
                    _target = _tmp.Translate(_eye);

                    _up = _matRotZ.ProductMat(_up0);

                    

                    textBoxM11.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(0, 0));
                    textBoxM12.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(0, 1));
                    textBoxM13.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(0, 2));
                    textBoxM21.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(1, 0));
                    textBoxM22.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(1, 1));
                    textBoxM23.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(1, 2));
                    textBoxM31.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(2, 0));
                    textBoxM32.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(2, 1));
                    textBoxM33.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(2, 2));

                    break;

                case Keys.Q:
                    _angleH ++;//Math.PI / 2;
                    radianAngle = Math.PI * _angleH / 180.0;
                    _matRotZ.Update(radianAngle);
                    
                    _tmp = _target0.Translate(-1, _matRotZ.ProductMat(_eye));
                    _target = _tmp.Translate(_eye);

                    _up = _matRotZ.ProductMat(_up0);

                    

                    textBoxM11.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(0, 0));
                    textBoxM12.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(0, 1));
                    textBoxM13.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(0, 2));
                    textBoxM21.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(1, 0));
                    textBoxM22.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(1, 1));
                    textBoxM23.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(1, 2));
                    textBoxM31.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(2, 0));
                    textBoxM32.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(2, 1));
                    textBoxM33.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(2, 2));

                    break;

                case Keys.S:


                    _angleV--;
                    radianAngle = Math.PI * _angleV / 180.0;
                    _matRotY.Update(radianAngle);
                    _tmp = _target0.Translate(-1, _matRotY.ProductMat(_eye));
                    _target = _tmp.Translate(_eye);
                    _up = _matRotY.ProductMat(_up0);

                    textBoxM11.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(0, 0));
                    textBoxM12.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(0, 1));
                    textBoxM13.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(0, 2));
                    textBoxM21.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(1, 0));
                    textBoxM22.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(1, 1));
                    textBoxM23.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(1, 2));
                    textBoxM31.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(2, 0));
                    textBoxM32.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(2, 1));
                    textBoxM33.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(2, 2));

                    break;

                case Keys.Z:
                    _angleV++;
                    radianAngle = Math.PI * _angleV / 180.0;
                    _matRotY.Update(radianAngle);
                    _tmp = _target0.Translate(-1, _matRotY.ProductMat(_eye));
                    _target = _tmp.Translate(_eye);
                    _up = _matRotY.ProductMat(_up0);

                    textBoxM11.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(0, 0));
                    textBoxM12.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(0, 1));
                    textBoxM13.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(0, 2));
                    textBoxM21.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(1, 0));
                    textBoxM22.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(1, 1));
                    textBoxM23.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(1, 2));
                    textBoxM31.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(2, 0));
                    textBoxM32.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(2, 1));
                    textBoxM33.Text = string.Format("{0,8:0.00000}", _matRotZ.GetValue(2, 2));
                    break;





            }
        }


        private void _timer_Tick(object sender, EventArgs e)
        {

            textBoxEye.Text=("("+_eye.GetX()+", " + _eye.GetY() +", "+_eye.GetZ()+")");
            textBoxTarget.Text = ("(" + _target.GetX() + ", " + _target.GetY() + ", " + _target.GetZ() + ")");
            textBoxUp.Text = ("(" + _up.GetX() + ", " + _up.GetY() + ", " + _up.GetZ() + ")");

            _cuberot += 0.5f;   
            //_xrot += 1.0f;
            //vectTarget = matRotAxis.ProductMat(vectTarget0);



            simpleOpenGlControl1.Refresh();
        }


        private void SimpleOpenGlControl1_Resize(object sender, EventArgs e)
        {
            
            base.OnSizeChanged(e);
            Size s = Size;

            if (s.Height == 0)                                                  // Prevent A Divide By Zero...
                s.Height = 1;                                                   // By Making Height Equal To One

            Gl.glViewport(0, 0, s.Width, s.Height);                             // Reset The Current Viewport
            Gl.glMatrixMode(Gl.GL_PROJECTION);                                  // Select The Projection Matrix
            Gl.glLoadIdentity();                                                // Reset The Projection Matrix
            Glu.gluPerspective(45.0, (double)s.Width / (double)s.Height, 0.1, 100);          // Calculate The Aspect Ratio Of The Window
            Gl.glMatrixMode(Gl.GL_MODELVIEW);                                   // Select The Modelview Matrix
            Gl.glLoadIdentity();                                                // Reset The Modelview Matrix
            
        }

        private void SimpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
        {
            glDraw();           
        }


        protected void InitGLContext()
        {
            //Gl.glEnable(Gl.GL_TEXTURE_2D);									// Enable Texture Mapping
            
            Gl.glShadeModel(Gl.GL_SMOOTH);                                  // Enable Smooth Shading
            Gl.glClearColor(_backgroundColor[0], _backgroundColor[1], _backgroundColor[2], _backgroundColor[3]);    // Clear the Color
            // Clear the Color and Depth Buffer
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            Gl.glClearDepth(1.0f);											// Depth Buffer Setup
            Gl.glEnable(Gl.GL_DEPTH_TEST);									// Enables Depth Testing
            Gl.glDepthFunc(Gl.GL_LEQUAL);									// The Type Of Depth Testing To Do
            Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);     // Really Nice Perspective Calculations
            
            _initContextGLisOk = true;
            
        }

        private void glDraw()
        {
            if (!_initContextGLisOk)
                InitGLContext();

          
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);        // Clear The Screen And The Depth Buffer
            Gl.glLoadIdentity();                                                // Reset The View

            //Glu.gluLookAt(10.0, 7.0, -10.0, // position oeil
            //   0.0, 0.0, 0.0,
            //    0.0, 1.0, 0.0);
            //Gl.glTranslatef(0, 0, xrot);
            Look();


           //DrawTrihedral 
            DrawTrihedral();

            //DrawPlane
            DrawPlane();

            Gl.glTranslatef(0.0f, -2.0f, 0.0f);
            //Gl.glRotatef(_cuberot, 0, 0, 1);
            //Gl.glRotatef(yrot, 0, 1, 0);
            ///////////////////////////////////
            DrawCube();
            ///////////////////////////////////
            Gl.glTranslatef(30.0f, 2.0f, 0.0f);
            DrawPyramide();

            Gl.glTranslatef(-30.0f, 0.0f, 0f);

            Gl.glTranslatef(14.0f, 0.0f, 0.0f);
            DrawPortal();

            Gl.glTranslatef(-14.0f, 0.0f, 0.0f);
            DrawTarget();

        }

        private void DrawTrihedral()
        {
            //DrawTrihedral ///////////////////////

            float[] l_couleur = new float[4];
            float l_shin;
            float l_lenghAxis = 1.0f;
            float l_flecheW = 0.1f; float l_flecheH = 0.05f;

            // axe X
            Gl.glColor4f(1.0f, 0.0f, 0.0f, 1.0f);
            l_couleur[0] = 1.0f; l_couleur[1] = 0.0f; l_couleur[2] = 0.0f; l_couleur[3] = 1.0f;
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT, @l_couleur);
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, @l_couleur);
            l_shin = 128;
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SHININESS, ref l_shin);
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_DIFFUSE, @l_couleur);
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_EMISSION, @l_couleur);

            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glVertex3f(0.0f, 0.0f, 0.0f);
            Gl.glVertex3f(l_lenghAxis, 0.0f, 0.0f);
            Gl.glEnd();
            // point axe X
            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glVertex3f(l_lenghAxis - l_flecheW, l_flecheH, 0.0f);
            Gl.glVertex3f(l_lenghAxis, 0.0f, 0.0f);
            Gl.glVertex3f(l_lenghAxis - l_flecheW, -l_flecheH, 0.0f);
            Gl.glEnd();

            // axe Y
            Gl.glColor4f(0.0f, 1.0f, 0.0f, 1.0f);
            l_couleur[0] = 0.0f; l_couleur[1] = 1.0f; l_couleur[2] = 0.0f; l_couleur[3] = 1.0f;
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT, @l_couleur);
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, @l_couleur);
            l_shin = 128;
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SHININESS, ref l_shin);
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_DIFFUSE, @l_couleur);
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_EMISSION, @l_couleur);
            Gl.glColor3f(0.0f, 1.0f, 0.0f);
            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glVertex3f(0.0f, 0.0f, 0.0f);
            Gl.glVertex3f(0.0f, l_lenghAxis, 0.0f);
            Gl.glEnd();
            // pointe axe Y
            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glVertex3f(0.0f - l_flecheH, l_lenghAxis - l_flecheW, 0.0f);
            Gl.glVertex3f(0.0f, l_lenghAxis, 0.0f);
            Gl.glVertex3f(0.0f + l_flecheH, l_lenghAxis - l_flecheW, 0.0f);
            Gl.glEnd();

            // axe Z
            Gl.glColor4f(0.0f, 0.0f, 1.0f, 1.0f);
            l_couleur[0] = 0.0f; l_couleur[1] = 0.0f; l_couleur[2] = 1.0f; l_couleur[3] = 1.0f;
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT, @l_couleur);
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, @l_couleur);
            l_shin = 128;
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SHININESS, ref l_shin);
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_DIFFUSE, @l_couleur);
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_EMISSION, @l_couleur);
            Gl.glColor3f(0.0f, 0.0f, 1.0f);
            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glVertex3f(0.0f, 0.0f, 0.0f);
            Gl.glVertex3f(0.0f, 0.0f, l_lenghAxis);
            Gl.glEnd();
            // pointe axe Z
            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glVertex3f(l_flecheH, 0.0f, l_lenghAxis - l_flecheW);
            Gl.glVertex3f(0.0f, 0.0f, l_lenghAxis);
            Gl.glVertex3f(-l_flecheH, 0.0f, l_lenghAxis - l_flecheW);
            Gl.glEnd();
        }

        private void DrawCube()
        {
            

            Gl.glBegin(Gl.GL_QUADS);
            // Front Face
            Gl.glColor4f(1.0f, 0.0f, 0.0f, 0.0f);
            Gl.glNormal3f(0, 0, 1);
            Gl.glVertex3f(-1, -1, 1);
            Gl.glVertex3f(1, -1, 1);
            Gl.glVertex3f(1, 1, 1);
            Gl.glVertex3f(-1, 1, 1);
            // Back Face
            Gl.glColor3f(0.0f, 1.0f, 0.0f);
            Gl.glVertex3f(-1, -1, -1);
            Gl.glVertex3f(-1, 1, -1);
            Gl.glVertex3f(1, 1, -1);
            Gl.glVertex3f(1, -1, -1);
            // Top Face
            Gl.glColor3f(0.0f, 0.0f, 1.0f);
            Gl.glVertex3f(-1, 1, -1);
            Gl.glVertex3f(-1, 1, 1);
            Gl.glVertex3f(1, 1, 1);
            Gl.glVertex3f(1, 1, -1);
            // Bottom Face
            Gl.glColor3f(1f, 0.4f, 0f);
            Gl.glVertex3f(-1, -1, -1);
            Gl.glVertex3f(1, -1, -1);
            Gl.glVertex3f(1, -1, 1);
            Gl.glVertex3f(-1, -1, 1);
            // Right face
            Gl.glColor3f(1f, 0f, 1f);
            Gl.glVertex3f(1, -1, -1);
            Gl.glVertex3f(1, 1, -1);
            Gl.glVertex3f(1, 1, 1);
            Gl.glVertex3f(1, -1, 1);
            // Left Face
            Gl.glColor3f(1f, 1f, 0f);
            Gl.glVertex3f(-1, -1, -1);
            Gl.glVertex3f(-1, -1, 1);
            Gl.glVertex3f(-1, 1, 1);
            Gl.glVertex3f(-1, 1, -1);
            Gl.glEnd();

            
        }

        private void DrawPlane()
        {
            Gl.glBegin(Gl.GL_QUADS);

            Gl.glColor3f(0.992f, 0.945f, 0.741f);
            Gl.glVertex3f(-10, -10, -1);
            Gl.glVertex3f(-10, 10, -1);
            Gl.glVertex3f(50, 50, -1);
            Gl.glVertex3f(50, -50, -1);

            Gl.glEnd();
        }

        private void DrawPyramide()
        {
            Gl.glRotated(90, 1, 0, 0);
            Gl.glBegin(Gl.GL_TRIANGLES);                                        // Drawing Using Triangles
            Gl.glColor3f(1, 0, 0);                                          // Red
            Gl.glVertex3f(0, 1, 0);                                         // Top Of Triangle (Front)
            Gl.glColor3f(0, 1, 0);                                          // Green
            Gl.glVertex3f(-1, -1, 1);                                       // Left Of Triangle (Front)
            Gl.glColor3f(0, 0, 1);                                          // Blue
            Gl.glVertex3f(1, -1, 1);                                        // Right Of Triangle (Front)
            Gl.glColor3f(1, 0, 0);                                          // Red
            Gl.glVertex3f(0, 1, 0);                                         // Top Of Triangle (Right)
            Gl.glColor3f(0, 0, 1);                                          // Blue
            Gl.glVertex3f(1, -1, 1);                                        // Left Of Triangle (Right)
            Gl.glColor3f(0, 1, 0);                                          // Green
            Gl.glVertex3f(1, -1, -1);                                       // Right Of Triangle (Right)
            Gl.glColor3f(1, 0, 0);                                          // Red
            Gl.glVertex3f(0, 1, 0);                                         // Top Of Triangle (Back)
            Gl.glColor3f(0, 1, 0);                                          // Green
            Gl.glVertex3f(1, -1, -1);                                       // Left Of Triangle (Back)
            Gl.glColor3f(0, 0, 1);                                          // Blue
            Gl.glVertex3f(-1, -1, -1);                                      // Right Of Triangle (Back)
            Gl.glColor3f(1, 0, 0);                                          // Red
            Gl.glVertex3f(0, 1, 0);                                         // Top Of Triangle (Left)
            Gl.glColor3f(0, 0, 1);                                          // Blue
            Gl.glVertex3f(-1, -1, -1);                                      // Left Of Triangle (Left)
            Gl.glColor3f(0, 1, 0);                                          // Green
            Gl.glVertex3f(-1, -1, 1);                                       // Right Of Triangle (Left)

            Gl.glEnd();

            Gl.glRotated(-90, 1, 0, 0);
        }


        private void DrawPortal()
        {
            Gl.glBegin(Gl.GL_QUADS);

            //TOP
            Gl.glColor3f(1.0f, 1.0f, 1.0f);
            Gl.glVertex3f(0.0f, -5.0f, 5.0f);
            Gl.glVertex3f(2.0f, -5.0f, 5.0f);
            Gl.glVertex3f(2.0f, 5.0f, 5.0f);
            Gl.glVertex3f(0.0f, 5.0f, 5.0f);

            //BOTTOM
            Gl.glColor3f(0.2f, 0.4f, 0.0f);
            Gl.glVertex3f(0.0f, -5.0f, -0.99f);
            Gl.glVertex3f(2.0f, -5.0f, -0.99f);
            Gl.glVertex3f(2.0f, 5.0f, -0.99f);
            Gl.glVertex3f(0.0f, 5.0f, -0.99f);

            //RIGHT
            Gl.glColor3f(0.4f, 0.2f, 0.0f);
            Gl.glVertex3f(0.0f, 5.0f, -1.0f);
            Gl.glVertex3f(2.0f, 5.0f, -1.0f);
            Gl.glVertex3f(2.0f, 5.0f, 5.0f);
            Gl.glVertex3f(0.0f, 5.0f, 5.0f);

            //LEFT
            Gl.glColor3f(0.6f, 0.4f, 0.2f);
            Gl.glVertex3f(0.0f, -5.0f, -1.0f);
            Gl.glVertex3f(2.0f, -5.0f, -1.0f);
            Gl.glVertex3f(2.0f, -5.0f, 5.0f);
            Gl.glVertex3f(0.0f, -5.0f, 5.0f);

            Gl.glEnd();
        }

        private void DrawTarget()
        {

            Gl.glDisable(Gl.GL_DEPTH_TEST);

            Gl.glColor3f(0.5f, 0.5f, 0.5f);
            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glVertex3f(_target.GetX(), _target.GetY()  +  0.5f, _target.GetZ());
            Gl.glVertex3f(_target.GetX(), _target.GetY() -  0.5f, _target.GetZ());
            Gl.glEnd();

            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glVertex3f(_target.GetX(), _target.GetY(), _target.GetZ() + 0.5f);
            Gl.glVertex3f(_target.GetX(), _target.GetY(), _target.GetZ() - 0.5f);
            Gl.glEnd();

            Gl.glEnable(Gl.GL_DEPTH_TEST);
        }

        private void Look()
        {



             Glu.gluLookAt(_eye.GetX(), _eye.GetY(), _eye.GetZ(), // position oeil
               _target.GetX(), _target.GetY(), _target.GetZ(),                // target
               _up.GetX(), _up.GetY(), _up.GetZ());              //Up
            //Gl.glRotatef(90, 1, 0, 0);
            //Gl.glRotatef(90, 0, 1, 0);
            //Gl.glRotatef(_xrot, 0, 0, 1);*/

            
            /* Glu.gluLookAt(15.0, 0.0, 0.0, // position oeil
             0, 0, 0,                // target
             0.0, 0.0, 1.0);              //Up*/
        }



    }
}
