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

namespace MYCalibration_v4
{
    public abstract class Camera
    {
        public Matrix4 _projectionMatrix;
        public Matrix4 _lookatMatrix;
        public double[] _projectionMatrixDouble;
        public Vector3 _eye, _target, _up, _axe3;
        protected Color4 _color;
        public bool _useDoubleMatrix;
        protected int _backgroundTextureId;
        protected int _width;
        protected int _height;
        public bool _isCalibrated;
        public List<Vector3> _listPlanPoints = new List<Vector3>();
        public List<Vector3> _listPoints = new List<Vector3>();

        #region Projection

        public abstract void SetDefaultProjection(float width, float height);
        public abstract void Projection();
        public abstract void SetPerspective(Matrix4 projectionMatrix);
        public abstract void SetPerspective(double[] projectionMatrix);

        #endregion

        #region LookAt
        public abstract void LookAt();
        public abstract void SetLookat(Vector3 eye, Vector3 target, Vector3 up);
        public abstract void UpdateLookAt();
        #endregion

        #region Background
        public abstract void SetBackgroundTextureId(int id);
        public abstract void Background();
        #endregion

        #region Draw Methods
        public abstract void SetColor(Color4 color);
        public abstract void Draw();
        public abstract void DrawPlan(Color4 colorPlan);
        #endregion

        #region Calibration
        public abstract void Calibrate(String calibrationPathFile, List<Vector2> listImagePoints, List<Vector3> listObjectPoints);
        public abstract void Calibrate(Camera calibratedCam);
        #endregion

        #region Control

        #region Rotation
        public abstract void KeyUP();
        public abstract void KeyDOWN();
        public abstract void KeyRIGHT();
        public abstract void KeyLEFT();


        #endregion

        #region Translation
        public abstract void KeyZ();
        public abstract void KeyS();
        public abstract void KeyQ();
        public abstract void KeyD();
        public abstract void KeySpace();
        public abstract void KeyX();

        #endregion

        public abstract void ReinitializePosition();

        public abstract void RotateUp(Matrix4 matRotation);

        public abstract void RotateTarget(Matrix4 rotation);

        public abstract void RotateEye(float angle);

        public abstract void RotateFromFile(string path);

        //public abstract void RotatePosition(Matrix4 matRotation);

        #endregion

        public abstract void Correction(Camera surface, out float angle1, out float tr2 );
       
        #region Setters
        public abstract void SetTarget(Vector3 target);
        public abstract void SetEye(Vector3 eye);
        public abstract void SetUp(Vector3 up);

        public abstract void setPoints(List<Vector3> list);
        #endregion

        #region Spectator
        public abstract void SetObservedCams(Camera cam1, Camera cam2);
        public abstract void LookCam(int numCam);
        public abstract void ChangePerspective(int numCam);
        public abstract void ChangePerspective(double[] mat);

        #endregion

        
    }
}
