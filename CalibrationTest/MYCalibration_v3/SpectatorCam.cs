﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using System.Drawing;


namespace MYCalibration_v3
{
    class SpectatorCam : ClassicCam
    {
        Camera _observedCam1, _observedCam2;
        Vector3 _eye0, _target0, _up0;
        float _angleV, _angleH;

        #region Constructeurs
        public SpectatorCam(int width, int height)
            :base(width,height)
        {
            this._eye0 = this._eye;
            this._target0 = this._target;
            this._up0 = this._up;
            this._angleH = 0;
            this._angleV = 0;
        }

        public SpectatorCam(int width, int height, Vector3 eye, Vector3 target, Vector3 up)
            :base(width,height,eye,target,up)
        {
            this._eye0 = eye;
            this._target0 = target;
            this._up0 = up;
            this._angleH = 0;
            this._angleV = 0;
        }

        public SpectatorCam(int width, int height, Vector3 eye, Vector3 target, Vector3 up, Camera cam1, Camera cam2)
             : base(width, height, eye, target, up)
        {
            this._eye0 = eye;
            this._target0 = target;
            this._up0 = up;
            this._observedCam1 = cam1;
            this._observedCam2 = cam2;
            this._angleH = 0;
            this._angleV = 0;
        }
        #endregion

        #region Draw
        public override void DrawPlan(Color4 color)
        {
            Color4 colorP1 = color;
            Color4 colorP2 = new Color4(1 - colorP1.R, 1 - colorP1.G, 1 - colorP1.B, 1.0f);

            _observedCam1.Projection();
            _observedCam1.LookAt();
            _observedCam1.DrawPlan(colorP1);
            _observedCam2.Projection();
            _observedCam2.LookAt();
            _observedCam2.DrawPlan(colorP2);
            Projection();
            LookAt();
        }
        #endregion

        #region Control
        #region Rotation
        public override void KeyDOWN()
        {
            _angleV++;
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

        public override void KeyLEFT()
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

        public override void KeyRIGHT()
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

        public override void KeyUP()
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

            UpdateLookAt();
        }
        #endregion

        #region translation
        //avancer
        public override void KeyZ()
        {
            Vector3 translation = new Vector3(1.0f, 0.0f, 0.0f);
            this._eye = this._eye + translation;
            this._target = this._target + translation;

            UpdateLookAt();
        }

        //Reculer
        public override void KeyS()
        {
            Vector3 translation = new Vector3(-1.0f, 0.0f, 0.0f);
            this._eye = this._eye + translation;
            this._target = this._target + translation;

            UpdateLookAt();
        }

        //Gauche
        public override void KeyQ()
        {
            Vector3 translation = new Vector3(0.0f, 1.0f, 0.0f);
            this._eye = this._eye + translation;
            this._target = this._target + translation;

            UpdateLookAt();
        }

        //Droite
        public override void KeyD()
        {
            Vector3 translation = new Vector3(0.0f, -1.0f, 0.0f);
            this._eye = this._eye + translation;
            this._target = this._target + translation;

            UpdateLookAt();
        }
        
        //Monter
        public override void KeySpace()
        {
            Vector3 translation = new Vector3(0.0f, 0.0f, 1.0f);
            this._eye = this._eye + translation;
            this._target = this._target + translation;

            UpdateLookAt();
        }

        //Descendre
        public override void KeyX()
        {
            Vector3 translation = new Vector3(0.0f, 0.0f, -1.0f);
            this._eye = this._eye + translation;
            this._target = this._target + translation;

            UpdateLookAt();
        }
        
        #endregion

        public override void ReinitializePosition()
        {
            this._eye = _eye0;
            this._target = _target0;
            this._up = _up0;
            this._angleH = 0;
            this._angleV = 0;
            UpdateLookAt();
        }
        #endregion

        #region Spectator
        public override void SetObservedCams(Camera cam1, Camera cam2)
        {
            this._observedCam1 = cam1;
            this._observedCam2 = cam2;
        }

        public override void LookCam(Camera cam)
        {
            this._target = cam._target;
            UpdateLookAt();
        }
        #endregion



    }
}
