using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TexLib;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;


namespace TextureTest
{
    public partial class Form1 : Form
    {
        bool _initContextGLisOk = false;
        Color4 _background = new Color4(0.4f, 1.0f, 1.0f, 0.3f);

        private Vector3 _eye = new Vector3(-10.0f, 0.0f, 0.0f);
        private Vector3 _target = Vector3.Zero;
        private Vector3 _up = new Vector3(0.0f, 0.0f, 1.0f);

        private int textureId;
        Bitmap _backgroundPicture = new Bitmap("../../photos/photo.jpg");
        //Bitmap background = new Bitmap("../../photos/photo.jpg");

        public Form1()
        {
            InitializeComponent();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {

            //Image background= Image.FromFile("../../photos/photo.jpg");

            textureId = TexUtil.CreateTextureFromBitmap(_backgroundPicture);

            //SetupViewport();

            //InitGLContext();
            glControl1.Resize += glControl1_Resize;

        }

        private void SetupViewport()
        {

            //texture
            //background
            /*textureId = TexUtil.CreateRGBTexture(2, 2,
            new byte[] {
            255,0,0, // Red
            0,255,0, // Green
            0,0,255, // Blue
            255,255,255 // White
            });*/

            //textureId = TexUtil.CreateTextureFromBitmap(_backgroundPicture);// CreateTextureFromFile("../../photos/photo.jpg");


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
            //GL.ClearColor(_background)  ;                           // Clear the Color

            //Bitmap background = new Bitmap("../../photos/photo.jpg");
            //GL.DrawPixels(background.Width, background.Height, PixelFormat.Rgb, PixelType.Bitmap, background.GetHbitmap());

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

            if (h == 0)                                                  // Prevent A Divide By Zero...
                h = 1;                                                   // By Making Height Equal To One

            //GL.Viewport(0, 0, w, h);              // Use all of the glControl painting area

            GL.MatrixMode(MatrixMode.Projection);                               // Select The Projection Matrix
            GL.LoadIdentity();                                                  // Reset The Projection Matrix

            GL.Ortho(0, w, 0, h, -1, 1);

           

            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadIdentity();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.DepthMask(false);
           // GL.Disable(EnableCap.Lighting);

            //GL.BindTexture(TextureTarget.Texture2D, textureId);

            DrawBackground();

            //GL.DrawPixels(_backgroundPicture.Width, _backgroundPicture.Height, PixelFormat.Rgb, PixelType.Bitmap, _backgroundPicture.GetHbitmap());

            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);


            //Scene 3D
            GL.MatrixMode(MatrixMode.Projection);                               // Select The Projection Matrix
            GL.LoadIdentity();                                                  // Reset The Projection Matrix
            
            Matrix4 matrixPerspective = Matrix4.CreatePerspectiveFieldOfView((float)System.Math.PI / 4, (float)w / (float)h, 0.1f, 100.0f);
            GL.MultMatrix(ref matrixPerspective);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            LookAt();
            
            //Draw the scene
            glDrawScene();
            

            
            glControl1.SwapBuffers();


        }

        private void glDrawScene()
        {

            GL.Rotate(45, Vector3d.UnitZ);
            DrawCube();
            
        }

        private void DrawBackground()
        {
            int w = glControl1.Width;
            int h = glControl1.Height;

            //réinitialise la couleur
            GL.Color3(1.0f, 1.0f, 1.0f);

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
            GL.Vertex3(-1, 1, -1);
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
            //GL.Color3(0.0f, 0.0f, 1.0f);
            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(-1, -1, -1);
            GL.Vertex3(-1, -1, 1);
            GL.Vertex3(-1, 1, 1);
            GL.Vertex3(-1, 1, -1);

            GL.End();

            
            
        }

        private void LookAt()
        {
            Matrix4 lookat = Matrix4.LookAt(_eye, _target, _up);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);
        }

        private void glControl1_MouseClick(object sender, MouseEventArgs e)
        {
            Refresh();
        }
    }
}

