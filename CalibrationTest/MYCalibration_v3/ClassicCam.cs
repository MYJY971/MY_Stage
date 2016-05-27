using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using System.Drawing;

using CalibrationLibrary;

namespace MYCalibration_v3
{
    class ClassicCam : Camera
    {
        /*private Matrix4 _projectionMatrix;
        private Matrix4 _lookatMatrix;
        private double[] _projectionMatrixDouble;
        public Vector3 _eye, _target, _up;
        private Color4 _color;
        private bool _useDoubleMatrix;
        private int _backgroundTextureId;
        private int _width;
        private int _height;*/

        #region Constructeurs
        public ClassicCam(int width, int height)
        {
            this._width = width;
            this._height = height;
            _eye = Vector3.Zero;
            _target = Vector3.Zero;
            _up = new Vector3(0.0f, 0.0f, 1.0f);
            SetDefaultProjection(width, height);
            UpdateLookAt();
            this._color = Color.Silver;
            _useDoubleMatrix = false;
            _isCalibrated = false;

        }

        public ClassicCam(int width, int height, Vector3 eye, Vector3 target, Vector3 up)
        {
            this._eye = eye;
            this._target = target;
            this._up = up;
            this._width = width;
            this._height = height;
            SetDefaultProjection(width, height);
            UpdateLookAt();
            this._color = Color.Silver;
            _useDoubleMatrix = false;
            _isCalibrated = false;

        }
        #endregion

        #region Projection
        public override void SetDefaultProjection(float width, float height)
        {
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, width / height, 0.1f, 100.0f);
        }

        public override void Projection()
        {
            GL.MatrixMode(MatrixMode.Projection);                               // Select The Projection Matrix
            GL.LoadIdentity();                                                  // Reset The Projection Matrix


            if (!_useDoubleMatrix)
            {
                GL.MultMatrix(ref _projectionMatrix);
            }
            else
            {
                GL.MultMatrix(_projectionMatrixDouble);
            }
        }

        public override void SetPerspective(double[] projectionMatrix)
        {
            if (!_useDoubleMatrix)
                _useDoubleMatrix = true;

            _projectionMatrixDouble = projectionMatrix;
        }

        public override void SetPerspective(Matrix4 projectionMatrix)
        {
            if (_useDoubleMatrix)
                _useDoubleMatrix = false;

            this._projectionMatrix = projectionMatrix;
        }
        #endregion

        #region LookAt
        public override void LookAt()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref _lookatMatrix);
        }

        public override void SetLookat(Vector3 eye, Vector3 target, Vector3 up)
        {
            this._eye = eye;
            this._target = target;
            this._up = up;
            _lookatMatrix = Matrix4.LookAt(_eye, _target, _up);
        }

        public override void UpdateLookAt()
        {
            SetLookat(this._eye, this._target, this._up);
        }
        #endregion

        #region Background
        public override void SetBackgroundTextureId(int id)
        {
            this._backgroundTextureId = id;
        }

        public override void Background()
        {
            if (_backgroundTextureId == 0)
            {
                GL.ClearColor(Color4.DarkGray);
            }
            else
            {
                int w = this._width;
                int h = this._height;


                //Set Projection Ortho
                GL.MatrixMode(MatrixMode.Projection);                               // Select The Projection Matrix
                GL.LoadIdentity();                                                  // Reset The Projection Matrix

                GL.Ortho(0, w, 0, h, -1, 1);

                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();

                GL.Disable(EnableCap.DepthTest);
                GL.Enable(EnableCap.Texture2D);
                GL.DepthMask(false);

                //réinitialise la couleur
                GL.Color3(1.0f, 1.0f, 1.0f);

                GL.BindTexture(TextureTarget.Texture2D, _backgroundTextureId);

                GL.Begin(BeginMode.Quads);


                GL.TexCoord2(0, 1);
                GL.Vertex2(0, 0);
                GL.TexCoord2(1, 1);
                GL.Vertex2(w, 0);
                GL.TexCoord2(1, 0);
                GL.Vertex2(w, h);
                GL.TexCoord2(0, 0);
                GL.Vertex2(0, h);

                GL.End();


                GL.Disable(EnableCap.Texture2D);
                GL.Enable(EnableCap.DepthTest);
                GL.DepthMask(true);
            }
        }

        #endregion

        #region DrawMethods
        private Vector3 VectMove(Vector3 p_point, Vector3 p_dir, double p_dist)
        {
            Vector3 l_dir = p_dir;
            l_dir.Normalize();
            l_dir = Vector3.Multiply(l_dir, (float)p_dist);

            return Vector3.Add(p_point, l_dir);
        }

        public override void SetColor(Color4 color)
        {
            this._color = color;
        }

        public override void Draw()
        {
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, new Color4(0.3f, 0.3f, 0.3f, 1.0f));
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, 128f);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Emission, new Color4(0.0f, 0.0f, 0.0f, 1.0f));


            GL.Color4(_color);


            Vector3 target = VectMove(this._eye, this._target, 1);
            Vector3 up = VectMove(this._eye, this._up, 1);


            Matrix4 rotationMat1 = Matrix4.CreateFromAxisAngle(this._target, (float)Math.PI / 2);
            Matrix4 rotationMat2 = Matrix4.CreateFromAxisAngle(this._target, (float)Math.PI / 4);

            Vector3 p0 = Vector3.Transform(this._up, rotationMat2);

            Vector3 p1 = Vector3.Transform(p0, rotationMat1);

            Vector3 p2 = Vector3.Transform(p1, rotationMat1);

            Vector3 p3 = Vector3.Transform(p2, rotationMat1);


            p0 = VectMove(this._eye, p0, 1);
            p1 = VectMove(this._eye, p1, 1);
            p2 = VectMove(this._eye, p2, 1);
            p3 = VectMove(this._eye, p3, 1);

            Vector3 p4 = VectMove(this._eye, -this._target, 1);

            GL.Begin(BeginMode.Lines);

            //Target Vector
            GL.Color3(1.0f, 1.0f, 0.0f);
            GL.Vertex3(this._eye);
            GL.Vertex3(target);

            //Up Vector
            GL.Color3(0.0f, 1.0f, 1.0f);
            GL.Vertex3(this._eye);
            GL.Vertex3(up);

            GL.Color4(_color);

            GL.End();


            GL.Begin(BeginMode.Quads);
            //Face
            GL.Normal3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(p0);
            GL.Vertex3(p1);
            GL.Vertex3(p2);
            GL.Vertex3(p3);

            GL.End();

            GL.Begin(BeginMode.Triangles);

            //Left Face
            GL.Normal3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(p0);
            GL.Vertex3(p1);
            GL.Vertex3(p4);

            //Bottom Face
            GL.Normal3(0.0f, 0.0f, -1.0f);
            GL.Vertex3(p1);
            GL.Vertex3(p2);
            GL.Vertex3(p4);

            //Right Face
            GL.Normal3(0.0f, -1.0f, 0.0f);
            GL.Vertex3(p2);
            GL.Vertex3(p3);
            GL.Vertex3(p4);

            //Up Face
            GL.Normal3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(p3);
            GL.Vertex3(p0);
            GL.Vertex3(p4);

            GL.End();

            GL.Color3(1.0f, 1.0f, 1.0f);
        }

        public override void DrawPlan(Color4 colorPlan)
        {
            GL.Color4(colorPlan);
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


            GL.End();

            GL.Color3(1.0f, 1.0f, 1.0f);
        }
        #endregion

        #region Calibration
        public override void Calibrate(string calibrationPathFile, List<Vector2> listImagePoints, List<Vector3> listObjectPoints)
        {
            return ;
        }
        #endregion

        #region Control

        #region Rotation
        public override void KeyDOWN()
        {
            return;
        }
        public override void KeyLEFT()
        {
            return;
        }
        public override void KeyRIGHT()
        {
            return;
        }
        public override void KeyUP()
        {
            return;
        }
        #endregion

        #region translation
        public override void KeyD()
        {
            return;
        }
        public override void KeyQ()
        {
            return;
        }
        public override void KeyS()
        {
            return;
        }
        public override void KeySpace()
        {
            return;
        }
        public override void KeyX()
        {
            return;
        }
        public override void KeyZ()
        {
            return;
        }
        #endregion

        public override void ReinitializePosition()
        {
            return;
        }


        #endregion

        #region Spectator
        public override void SetObservedCams(Camera cam1, Camera cam2)
        {
            return;
        }

        public override void LookCam(Camera cam)
        {
            
        }

        #endregion




























    }
}
