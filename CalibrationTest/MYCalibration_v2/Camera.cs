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

namespace MYCalibration_v2
{
    class Camera
    {
        private Matrix4 _projectionMatrix;
        private Matrix4 _lookatMatrix;
        private double[] _projectionMatrixDouble;
        private int _type; //0 projection ortho, 1 projection perspective
        public Vector3 _eye, _target, _up;
        private bool _isPerspective;
        private Color4 _color;
        private bool _useDoubleMatrix;
        private bool _isCalibrated;
        private bool _controlEnabled;
        private int _backgroundTextureId;
        private int _width;
        private int _height;


        private float _angleV,_angleH, _tx, _ty, _tz ;
        private Vector3 _target0, _up0;

        public Calibration4points _calibration4P;

        //public int _width, _height;


        /*Constructeur*/
        public Camera(int type, int width, int height)
        {
            this._type = type;
            this._width = width;
            this._height = height;
            SetDefaultProjection(_type, width, height);
            SetLookat(_eye, _target, _up);

        }

        public Camera(int width, int height)
        {
            this._type = 1;
            _eye = Vector3.Zero;
            _target = Vector3.Zero;
            _up = new Vector3(0.0f, 0.0f, 1.0f);
            //Camera(_type, width, height);
            SetDefaultProjection(_type, width, height);
            SetLookat(_eye, _target, _up);
            this._color = Color.Silver;
            _useDoubleMatrix = false;
            this._width = width;
            this._height = height;

        }

        public Camera(int width, int height, Vector3 eye, Vector3 target, Vector3 up)
        {
            this._type = 1;
            this._eye = eye;
            this._target = target;
            this._up = up;
            this._width = width;
            this._height = height;

            SetDefaultProjection(_type, width, height);
            SetLookat(eye, target, up);
            this._color = Color.Silver;
            _useDoubleMatrix = false;

            _calibration4P = new Calibration4points((int)width, (int)height, Vect3D.ToVect3D(this._eye), Vect3D.ToVect3D(this._target), Vect3D.ToVect3D(this._up));
        }

        /*Methodes*/
        public void Background()
        {
            if (_backgroundTextureId == 0)
            {
                GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
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

                //DrawBackground
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

        public void DrawPlan(Color4 colorPlan)
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

        public void SetBackgroundTextureId(int id)
        {
            this._backgroundTextureId = id;
        }

        public void Calibrate(String calibrationPathFile, List<Vector2> listImagePoints, List<Vector3> listObjectPoints)
        {
            if (!_isCalibrated)
            {
                _calibration4P.SetCalibrationFilePath(calibrationPathFile);
                _calibration4P.SetListObjectPoints(ToVect3(listObjectPoints));
                _calibration4P.SetListImagePoints(ToVect2(listImagePoints));

                _calibration4P.CalibrateManually();

                this._eye = _calibration4P._eyePosition.ToVector3();
                this._target = _calibration4P._targetPosition.ToVector3();
                this._up = _calibration4P._upVector.ToVector3();

                _useDoubleMatrix = true;

                SetPerspective(_calibration4P._glProjectionMatrix);

                //SetLookat(_calibration4P._modelView.toMatrix4());
                SetLookat(this._eye, this._target, this._up);
                _isCalibrated = true;
            }

        }

        private List<Vect2D> ToVect2(List<Vector2> lv2)
        {
            List<Vect2D> list2D = new List<Vect2D>();

            foreach(Vector2  vector in lv2)
            {
                list2D.Add(Vect2D.ToVect2D(vector));
            }

            return list2D;
        }

        private List<Vect3D> ToVect3(List<Vector3> lv3)
        {
            List<Vect3D> list3D = new List<Vect3D>();

            foreach (Vector3 vector in lv3)
            {
                list3D.Add(Vect3D.ToVect3D(vector));
            }

            return list3D;
        }

        public void SetDefaultProjection(int type, float width, float height)
        {

            if (type == 0)
            {
                GL.Ortho(0, width, 0, height, 0, 1);
                _isPerspective = false;

            }
            if (type == 1)
            {
                _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)System.Math.PI / 4, width / height, 0.1f, 100.0f);
                _isPerspective = true;
            }
        }

        public void SetLookat(Matrix4 lookatMatrix)
        {
                _lookatMatrix = lookatMatrix;
        }

        public void UpdateLookAt()
        {
            SetLookat(this._eye, this._target, this._up);
        }

        public void SetLookat(Vector3 eye, Vector3 target, Vector3 up)
        {
            this._eye = eye;
            this._target = target;
            this._up = up;
          _lookatMatrix = Matrix4.LookAt(_eye, _target, _up);

        }

        public void LookAt()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref _lookatMatrix);

        }

        public void SetPerspective(Matrix4 projectionMatrix)
        {
            if (!_isPerspective)
                _isPerspective = true;

            if (_useDoubleMatrix)
                _useDoubleMatrix = false;

            this._projectionMatrix = projectionMatrix; 

            

        }

        public void SetPerspective(double[] projectionMatrix)
        {
            if (!_isPerspective)
                _isPerspective = true;

            if (!_useDoubleMatrix)
                _useDoubleMatrix = true;

            _projectionMatrixDouble = projectionMatrix;

        }

        public void Projection()
        {
            _isPerspective = true;
            GL.MatrixMode(MatrixMode.Projection);                               // Select The Projection Matrix
            GL.LoadIdentity();                                                  // Reset The Projection Matrix

            if (this._type == 0)
            {
                //GL.Ortho(0, width, 0, height, 0, 1);
            }

            else
            {
                if (!_useDoubleMatrix)
                {
                    GL.MultMatrix(ref _projectionMatrix);
                }
                else
                {
                    GL.MultMatrix(_projectionMatrixDouble);
                }
            }

        }

        public void SetColor ( Color4 color)
            {
            this._color = color;
            }

        public void Draw()
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

        private Vector3 VectMove(Vector3 p_point, Vector3 p_dir, double p_dist)
        {
            Vector3 l_dir = p_dir;
            l_dir.Normalize();
            l_dir = Vector3.Multiply(l_dir, (float)p_dist);

            return Vector3.Add(p_point, l_dir);//Sum(p_point, l_dir);
        }


        public void SetControl(bool b)
        {
            _controlEnabled = b;
            if (_controlEnabled)
            {
                _target0 = this._target;
                _up0 = this._up;
                _angleV = 0;
                _angleH = 0;
                _tx = 0;
                _ty = 0;
                _tz = 0;
            }
            
        }

        #region EventKey

        #region Rotation

        public void KeyUP ()
        {
            if(_controlEnabled)
            {
                _angleV--;
                float radianAngle = (float)Math.PI * _angleV / 180;
                float radianAngleH = (float)Math.PI * _angleH / 180;

                Matrix4 rotY = Matrix4.CreateRotationY(radianAngle);
                Matrix4 rotZ = Matrix4.CreateRotationZ(radianAngleH);

                Vector3 tmp1 = Vector3.Transform(this._eye, rotY);
                tmp1 = Vector3.Transform(tmp1, rotZ);

                Vector3 tmp2 = _target0 - tmp1;
                this._target = tmp2 + this._eye;
                this._up = Vector3.Transform(_up0, rotY);

                //this._target = Vector3.Transform(this._target, rotX);
                UpdateLookAt();
            }
        }

        public void KeyDOWN()
        {
            if (_controlEnabled)
            {
               
                _angleV ++;
                float radianAngleV = (float)Math.PI * _angleV / 180;
                float radianAngleH = (float)Math.PI * _angleH / 180;

                Matrix4 rotY = Matrix4.CreateRotationY(radianAngleV);
                Matrix4 rotZ = Matrix4.CreateRotationZ(radianAngleH);
                Vector3 tmp1 = Vector3.Transform(this._eye, rotY);
                        tmp1 = Vector3.Transform(tmp1, rotZ);
                Vector3 tmp2 = _target0 - tmp1;
                this._target = tmp2 + this._eye;
                this._up = Vector3.Transform(_up0, rotY);

                //this._target = Vector3.Transform(this._target, rotX);
                UpdateLookAt();
            }
        }

        public void KeyRIGHT()
        {
            if (_controlEnabled)
            {

                _angleH--;
                float radianAngleV = (float)Math.PI * _angleV / 180;
                float radianAngleH = (float)Math.PI * _angleH / 180;


                Matrix4 rotY = Matrix4.CreateRotationY(radianAngleV);
                Matrix4 rotZ = Matrix4.CreateRotationZ(radianAngleH);

                Vector3 tmp1 = Vector3.Transform(this._eye, rotZ);
                tmp1 = Vector3.Transform(tmp1, rotY);
                Vector3 tmp2 = _target0 - tmp1;
                this._target = tmp2 + this._eye;
                this._up = Vector3.Transform(_up0, rotZ);

                UpdateLookAt();
            }
        }

        public void KeyLEFT()
        {
            if (_controlEnabled)
            {

                _angleH++;
                float radianAngleV = (float)Math.PI * _angleV / 180;
                float radianAngleH = (float)Math.PI * _angleH / 180;


                Matrix4 rotY = Matrix4.CreateRotationY(radianAngleV);
                Matrix4 rotZ = Matrix4.CreateRotationZ(radianAngleH);

                Vector3 tmp1 = Vector3.Transform(this._eye, rotZ);
                tmp1 = Vector3.Transform(tmp1, rotY);
                Vector3 tmp2 = _target0 - tmp1;
                this._target = tmp2 + this._eye;
                this._up = Vector3.Transform(_up0, rotZ);

                UpdateLookAt();
            }
        }

        #endregion

        #region Translation

        //anvancer
        public void KeyZ()
        {
            if(_controlEnabled)
            {
                _tx = 0;
                _tx++;
                this._eye = this._eye + new Vector3(1.0f, 0.0f, 0.0f);
                UpdateLookAt();
            }
            
        }

        //Reculer
        public void KeyS()
        {
            if (_controlEnabled)
            {
                _tx = 0;
                _tx--;
                this._eye = this._eye + new Vector3(-1.0f, 0.0f, 0.0f);
                UpdateLookAt();
            }
           
        }

        //Gauche
        public void KeyQ()
        {
            if (_controlEnabled)
            {
                _ty++;
                this._eye = this._eye + new Vector3(0.0f, 1.0f, 0.0f);
                UpdateLookAt();
            }
           
        }

        //Droite
        public void KeyD()
        {
            if (_controlEnabled)
            {
                _ty--;
                this._eye = this._eye + new Vector3(0.0f, -1.0f, 0.0f);
                UpdateLookAt();
            }
            
        }

        //Monter
        public void KeySpace()
        {
            if (_controlEnabled)
            {
                _tz++;
                this._eye = this._eye + new Vector3(0.0f, 0.0f, 1.0f);
                UpdateLookAt();
            }
            
        }

        //Descendre
        public void KeyShift()
        {
            if (_controlEnabled)
            {
                _tz--;
                this._eye = this._eye + new Vector3(0.0f, 0.0f, 1.0f);
                UpdateLookAt();
            }
           
        }
        #endregion

        #endregion

    }
}
