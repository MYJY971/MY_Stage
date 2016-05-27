﻿using System;
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
    public abstract class Camera
    {
        protected Matrix4 _projectionMatrix;
        protected Matrix4 _lookatMatrix;
        protected double[] _projectionMatrixDouble;
        public Vector3 _eye, _target, _up;
        protected Color4 _color;
        protected bool _useDoubleMatrix;
        protected int _backgroundTextureId;
        protected int _width;
        protected int _height;
        public bool _isCalibrated;

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


        #endregion

        #region Spectator
        public abstract void SetObservedCams(Camera cam1, Camera cam2);
        public abstract void LookCam(Camera cam);
        #endregion

    }
}
