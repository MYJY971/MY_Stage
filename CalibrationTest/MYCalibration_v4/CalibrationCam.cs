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
            if(_newPoints3D.Count==4)
            {
                GL.Color3(0.0f, 1.0f, 1.0f);
                foreach(Vector3 point in _newPoints3D)
                {
                    GL.Vertex3(point);
                }
                GL.End();
                GL.Color3(1.0f, 1.0f, 1.0f);
            }
        }
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
            SetDefaultProjection(this._width, this._height);
            SetCalibrationFalse();
            this._useDoubleMatrix = false;
        }


        #endregion

        public override void Correction(Camera surface, out float angleUp, out float angleAxe3)
        {
            if (surface._isCalibrated && _listPoints.Count==4)
            {
                Vector3 vectEye = surface._eye - this._eye;
                //this._eye = VectMove(this._eye, vectEye, vectEye.Length);

                Vector3 vectUp =  surface._up - this._up;
                //this._up = VectMove(this._up, vectUp, vectUp.Length);
                

                foreach(Vector3 point in _listPoints)
                {
                    
                    Vector3 newpoint = point;
                    newpoint = VectMove(newpoint, vectEye, vectEye.Length);
                    newpoint = VectMove(newpoint, vectUp, vectUp.Length);
                }

                angleAxe3 = 0;
                angleUp = 0;


                UpdateLookAt();


            }
            else
            {
                angleAxe3 = 0;
                angleUp = 0;
            }
        }

    }
}
