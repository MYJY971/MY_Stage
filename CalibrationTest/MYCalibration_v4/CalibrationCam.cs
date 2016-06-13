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
    class CalibrationCam : ClassicCam
    {
        Calibration4points _calibration4P;
        private List<Vector3> _newPoints3D = new List<Vector3>();
        private float _angleTest = 0.0f;
        #region Constructeurs
        public CalibrationCam (int width, int height)
            :base(width,height)
        {
            _calibration4P = new Calibration4points();
        }

        public CalibrationCam(int width, int height, Vector3 eye, Vector3 target, Vector3 up)
            :base(width,height,eye,target,up)
        {
            /*this._eye = eye;
            this._target = target;
            this._up = up;*/
            _calibration4P = new Calibration4points((int)width, (int)height, Vect3D.ToVect3D(this._eye), Vect3D.ToVect3D(this._target), Vect3D.ToVect3D(this._up));
        }

        #endregion

        #region Calibration

        private List<Vect2D> ToVect2(List<Vector2> lv2)
        {
            List<Vect2D> list2D = new List<Vect2D>();

            foreach (Vector2 vector in lv2)
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

        public override void Calibrate(String calibrationPathFile, List<Vector2> listImagePoints, List<Vector3> listObjectPoints)
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

                this._target0 = this._target;

                SetPerspective(_calibration4P._glProjectionMatrix);

                //SetLookat(_calibration4P._modelView.toMatrix4());

                //Ramène le vecteur target au plan XY pour simplifier la visualisation
                Vector3 axisTarget = this._eye - this._target;
                Vector3 dirAxis = axisTarget;
                dirAxis.Normalize();

                float dist = (- Vector3.Dot(this._eye,Vector3.UnitZ)) / (Vector3.Dot(dirAxis,Vector3.UnitZ)) ;

                this._target = VectMove(this._eye, axisTarget, dist);


                //test
                //this._eye = VectMove(this._eye, Vector3.UnitZ, -this._eye.Z);

                this._eye0 = this._eye;
                this._up0 = this._up;

                UpdateLookAt();
                
                _isCalibrated = true;
            }

        }

        public void SetCalibrationFalse()
        {
            _isCalibrated = false;
            _calibration4P._eyePosition = Vect3D.ToVect3D(this._eye);
            _calibration4P._targetPosition = Vect3D.ToVect3D(this._target);
            _calibration4P._upVector = Vect3D.ToVect3D(this._up);
        }


        #endregion

        #region Control

        public override void Draw()
        {
            base.Draw();
            DrawNewPoints();
        }


        private void DrawNewPoints()
        {
            if(_newPoints3D.Count>0)
            {
                GL.Color3(1.0f, 0.0f,0.0f);
                GL.Begin(BeginMode.Points);
                foreach(Vector3 point in _newPoints3D)
                {
                    GL.Vertex3(point);
                }
                GL.End();
                GL.Color3(1.0f, 1.0f, 1.0f);
            }
        }
        #region Rotation
        public override void RotateEye(float angle)
        {

            RotateAroundAxe3(angle);

            
        }

        private void RotateAroundAxe3(float angle)
        {
            Matrix4 rot = Matrix4.CreateRotationY(angle);
            Vector3 axis = this._eye/* - this._target*/;
            rot = Matrix4.CreateFromAxisAngle(axis, angle);

            this._eye = Vector3.Transform(this._eye, rot);
            this._eye0 = Vector3.Transform(this._eye0, rot);

            this._up = Vector3.Transform(this._up0, rot);
            this._up0 = Vector3.Transform(this._up0, rot);

            UpdateLookAt();
        }

        private void RotateAroundX(float angle)
        {
            Matrix4 rotZ = Matrix4.CreateRotationX(angle);

            this._eye = Vector3.Transform(this._eye0, rotZ) ;
            this._eye0 = Vector3.Transform(this._eye0, rotZ);

            this._up = Vector3.Transform(this._up0, rotZ);
            this._up0 = Vector3.Transform(this._up0, rotZ);

            UpdateLookAt();
        }

        private void RotateAroundY(float angle)
        {
            Matrix4 rotZ = Matrix4.CreateRotationY(angle);

            this._eye = Vector3.Transform(this._eye0, rotZ);
            this._eye0 = Vector3.Transform(this._eye0, rotZ);

            this._up = Vector3.Transform(this._up0, rotZ);
            this._up0 = Vector3.Transform(this._up0, rotZ);

            UpdateLookAt();
        }

        private void RotateAroundCrossTarget(Vector3 axis,float angle)
        {
            Matrix4 rot = Matrix4.CreateFromAxisAngle(axis, angle);

            this._eye = Vector3.Transform(this._eye0, rot);
            this._eye0 = Vector3.Transform(this._eye0, rot);

            this._up = Vector3.Transform(this._up0, rot);
            this._up0 = Vector3.Transform(this._up0, rot);

            UpdateLookAt();
        }

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
            SetDefaultProjection(this._width, this._height);
            SetCalibrationFalse();
            this._useDoubleMatrix = false;
        }


        #endregion

        public override void Correction(Camera surface, out float angleUp, out float angleAxe3)
        {
            if (surface._isCalibrated && _listPoints.Count==4)
            {


                /*
                 Vector3 tar = this._target - this._eye;
                 Vector3 tarS = surface._target - surface._eye;

                 float angle = Vector3.CalculateAngle(tar, tarS);

                Vector3 crossTarget = Vector3.Cross(tar, tarS);

                if (crossTarget.Z < 0)
                    angle = -angle;

                Vector3 up = this._up - this._eye;
                Vector3 upS = surface._up - surface._eye;

                angleUp = Vector3.CalculateAngle(up, upS);

                Vector3 axe3 = this._axe3 - this._eye;
                Vector3 axe3S = surface._axe3 - surface._eye;

                angleAxe3 = Vector3.CalculateAngle(axe3, axe3S);
                

                Vector3 crossAxe3 = Vector3.Cross(axe3, axe3S);

                if (crossAxe3.X < 0)
                    angleAxe3 = -angleAxe3;

                //RotateAroundX(-angle);
                RotateAroundCrossTarget(crossTarget,angle);
                RotateAroundAxe3(angleAxe3);
                 angleAxe3 = 0;
                 angleUp = 0;


                UpdateLookAt();

                tar = this._target - this._eye;
                tarS = surface._target - surface._eye;
                angle = Vector3.CalculateAngle(tar, tarS);

                 up = this._up - this._eye;
                 upS = surface._up - surface._eye;

                angleUp = Vector3.CalculateAngle(up, upS);

                

                Vector3 crossUp = Vector3.Cross(up, upS);

                Vector3 axisUp = VectMove(this._eye, crossUp, 1);

                //RotateAroundCrossTarget(axisUp, -angleUp);

                 axe3 = this._axe3 - this._eye;
                 axe3S = surface._axe3 - surface._eye;

                angleAxe3 = Vector3.CalculateAngle(axe3, axe3S);

                //RotateAroundX(-angleAxe3);

                //RotateAroundAxe3(-0.07854f);
                ////////////////////////////////
                */
                Vector3 camX = VectMove(this._eye, (this._target - this._eye), 1);
                Vector3 camZ = VectMove(this._eye, this._up , 1);
                Vector3 vY = Vector3.Cross(camX, camZ);
                Vector3 camY = VectMove(this._eye, vY , 1);

                Matrix4 OxyzToCam = new Matrix4(new Vector4(camX, 0),
                                            new Vector4(camY, 0),
                                            new Vector4(camZ, 0),
                                            Vector4.UnitW);
                Matrix4 CamToOxyz = OxyzToCam;
                CamToOxyz.Invert();

                Vector3 camSX = VectMove(surface._eye, (surface._target - surface._eye), 1);
                Vector3 camSZ = VectMove(surface._eye, surface._up, 1);
                Vector3 vSY = Vector3.Cross(camSX, camSZ);
                Vector3 camSY = VectMove(surface._eye, vSY, 1);

                Matrix4 OxyzToCamS = new Matrix4(new Vector4(camSX, 0),
                                            new Vector4(camSY, 0),
                                            new Vector4(camSZ, 0),
                                            Vector4.UnitW);

                Matrix4 CamSToOxyz = OxyzToCamS;
                CamSToOxyz.Invert();

                Vector3 p0 = _listPoints.ElementAt(0);

                Matrix4 WorldToLookat = _lookatMatrix;
                Matrix4 LookatToWorld = WorldToLookat;
                LookatToWorld.Invert();

                double[] doubleProj = _projectionMatrixDouble;
                Matrix4 proj = new Matrix4((float)_projectionMatrixDouble[0], (float)_projectionMatrixDouble[1], (float)_projectionMatrixDouble[2], (float)_projectionMatrixDouble[3],
                                           (float)_projectionMatrixDouble[4], (float)_projectionMatrixDouble[5], (float)_projectionMatrixDouble[6], (float)_projectionMatrixDouble[7],
                                           (float)_projectionMatrixDouble[8], (float)_projectionMatrixDouble[9], (float)_projectionMatrixDouble[10], (float)_projectionMatrixDouble[11],
                                           (float)_projectionMatrixDouble[12], (float)_projectionMatrixDouble[13], (float)_projectionMatrixDouble[14], (float)_projectionMatrixDouble[15]);
                Matrix4 invProj = proj; invProj.Invert();

                Matrix4 WorldToLookatS = surface._lookatMatrix;
                //WorldToLookatS.Transpose();
                Matrix4 LookatStoWorld = WorldToLookatS;
                LookatStoWorld.Invert();

                Vector3 p0Cam = Vector3.Transform(p0, this._lookatMatrix);
                Vector3 p0Surface = Vector3.Transform(p0, surface._lookatMatrix);

                //Vector3 res = Vector3.Transform(, LookatToWorld);
                //_newPoints3D.Add(res);
                //_newPoints3D.Add(new Vector3(lp0World.Z, lp0World.Y, lp0World.X));
                Vector3 tmp4 = Vector3.Transform(p0, LookatStoWorld);
                Vector3 tmp5 = Vector3.Transform(p0, LookatToWorld);

                //_newPoints3D.Add(tmp4);
                //_newPoints3D.Add(tmp5);

                foreach(Vector3 point in _listPoints)
                {
                    Vector3 newPoint = point;
                    //newPoint = Vector3.Transform(newPoint, proj);
                    newPoint = Vector3.Transform(newPoint, WorldToLookatS);
                    newPoint = Vector3.Transform(newPoint, LookatToWorld);
                    newPoint = new Vector3(newPoint.X, newPoint.Y, -newPoint.Z);
                    //newPoint = Vector3.Transform(newPoint, invProj);

                    _newPoints3D.Add(newPoint);

                }

                ///////////////////////////////////////////
                ///////////////////////////////////////////

                //Vector3 diffEye = surface._eye - this._eye;
                //Vector3 diffUp = surface._up - this._up;

                //this._eye = VectMove(this._eye, diffEye, diffEye.Length);

                //Vector3 diffUp = surface._up - this._up;
                //this._up = VectMove(this._up, diffUp, diffUp.Length);

                //Vector3 p0 = _listPoints.ElementAt(0);
                //p0 = VectMove(p0, diffEye, diffEye.Length);
                //p0 = VectMove(p0, diffUp, diffUp.Length);

                //_newPoints3D.Add(p0);

                //foreach(Vector3 point in _listPoints)
                //{
                //    Vector3 newPoint = point;
                //    newPoint = VectMove(newPoint, diffEye, diffEye.Length);
                //    newPoint = VectMove(newPoint, diffUp, diffUp.Length);
                //    _newPoints3D.Add(newPoint);
                //}
                UpdateLookAt();

                angleUp = 0;
                angleAxe3 = 0;
            }
            else
            {
                angleAxe3 = 0;
                angleUp = 0;
            }
        }

        //
        private void RotateAroundAxe32(float angle)
        {
            Vector3 axis = this._axe3 - this._eye;
            axis.Normalize();
            Matrix4 rot = Matrix4.CreateFromAxisAngle(axis, angle);

            this._eye = Vector3.Transform(this._eye0, rot) + this._target;
            this._up = Vector3.Transform(this._up0, rot);// + this._target;

            this._eye0 = Vector3.Transform(this._eye0, rot);
            this._up0 = this._up;

            UpdateLookAt();
        }

    }
}
