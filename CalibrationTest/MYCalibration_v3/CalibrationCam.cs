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
    class CalibrationCam : ClassicCam
    {
        Calibration4points _calibration4P;

        #region Constructeurs
        public CalibrationCam (int width, int height)
            :base(width,height)
        {
            _calibration4P = new Calibration4points();
        }

        public CalibrationCam(int width, int height, Vector3 eye, Vector3 target, Vector3 up)
            :base(width,height,eye,target,up)
        {
            this._eye = eye;
            this._target = target;
            this._up = up;
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

                UpdateLookAt();

                _isCalibrated = true;
            }

        }


        #endregion
    }
}
