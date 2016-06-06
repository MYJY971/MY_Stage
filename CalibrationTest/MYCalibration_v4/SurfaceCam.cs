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
using System.Xml.Linq;

using Windows.Devices.Sensors;

namespace MYCalibration_v4
{
    class SurfaceCam : ClassicCam
    {
        private float _angle;
        private Vector3 _target0;
        private OrientationSensor _orientationSensor;

        #region Constructeurs
        public SurfaceCam(int width, int height)
            :base(width,height)
        {
            this._target0 = Vector3.Zero;
           
        }

        public SurfaceCam(int width, int height, Vector3 eye, Vector3 target, Vector3 up)
            :base(width,height,eye,target,up)
        {
            this._target0 = target;
        }
        #endregion

        public override void RotateTarget(Matrix4 rotation)
        {
            base.RotateTarget(rotation);

            Vector3 tmp2 = this._target0 - this._target;
            this._target = VectMove(this._target, tmp2, (double)tmp2.Length);
            this._eye = VectMove(this._eye, tmp2, (double)tmp2.Length);
        }

        public override void RotateFromFile(string path)
        {
            Matrix4 rotation = GetSurfaceRot(path);
            RotateTarget(rotation);
        }


        private Matrix4 GetSurfaceRot(string path)
        {
            Matrix4 res = Matrix4.Identity;
            try
            {
                XDocument matriceSensor = XDocument.Load(path);

                float M11, M12, M13,
                      M21, M22, M23,
                      M31, M32, M33;

                M11 = (float)Convert.ToDouble(matriceSensor.Element("sensor").Element("orientation").Element("M11").Value.Replace(".", ","));
                M12 = (float)Convert.ToDouble(matriceSensor.Element("sensor").Element("orientation").Element("M12").Value.Replace(".", ","));
                M13 = (float)Convert.ToDouble(matriceSensor.Element("sensor").Element("orientation").Element("M13").Value.Replace(".", ","));

                M21 = (float)Convert.ToDouble(matriceSensor.Element("sensor").Element("orientation").Element("M21").Value.Replace(".", ","));
                M22 = (float)Convert.ToDouble(matriceSensor.Element("sensor").Element("orientation").Element("M22").Value.Replace(".", ","));
                M23 = (float)Convert.ToDouble(matriceSensor.Element("sensor").Element("orientation").Element("M23").Value.Replace(".", ","));

                M31 = (float)Convert.ToDouble(matriceSensor.Element("sensor").Element("orientation").Element("M31").Value.Replace(".", ","));
                M32 = (float)Convert.ToDouble(matriceSensor.Element("sensor").Element("orientation").Element("M32").Value.Replace(".", ","));
                M33 = (float)Convert.ToDouble(matriceSensor.Element("sensor").Element("orientation").Element("M33").Value.Replace(".", ","));

                res = new Matrix4(M11, M12, M13, 0.0f,
                                  M21, M22, M23, 0.0f,
                                  M31, M32, M33, 0.0f,
                                  0.0f, 0.0f, 0.0f, 1.0f);

                //transformation pour adapter le repere de la surface à celui d'openG
                Matrix4 rotX = MYRotationX(-(float)Math.PI / 2);
                Matrix4 rotZ = MYRotationZ(-(float)Math.PI / 2);

                res = Matrix4.Mult(res, rotX);
                res = Matrix4.Mult(res, rotZ);
                //res = Matrix4.Mult(res, rotY);


            }
            catch (Exception e)
            {

                Console.Error.WriteLine("" + e);
            }

            return res;
        }

        #region Calibration
        public override void Calibrate(Camera calibratedCam)
        {
            //change perspective
            ChangePerspective(calibratedCam._projectionMatrixDouble);

            //change eye
            Vector3 eyeCenter = calibratedCam._eye - this._target0;
            Vector3 axeTarget = this._target - this._eye;
            //this._eye = VectMove(this._eye, axeTarget, (float)axeTarget.Length - (float)eyeCenter.Length);

            UpdateLookAt();
        }
        #endregion

        #region Control
        public override void KeyZ()
        {
            Vector3 axis = this._target - this._eye;
            this._eye = VectMove(this._eye, axis, 0.05);
            UpdateLookAt();
        }

        public override void KeyS()
        {
            Vector3 axis = this._eye - this._target;
            //Vector3 axis = this._target - this._eye;
            this._eye = VectMove(this._eye, axis, 0.05);
            UpdateLookAt();
        }
        #endregion

        #region matrice Rotation
        //rotation X
        private Matrix4 MYRotationX(float angle)
        {
            Matrix4 result;

            float cos = (float)System.Math.Cos(angle);
            float sin = (float)System.Math.Sin(angle);

            result.Row0 = Vector4.UnitX;
            result.Row1 = new Vector4(0.0f, cos, -sin, 0.0f);
            result.Row2 = new Vector4(0.0f, sin, cos, 0.0f);
            result.Row3 = Vector4.UnitW;

            return result;
        }
        //rotation Z
        private Matrix4 MYRotationZ(float angle)
        {
            Matrix4 result;

            float cos = (float)System.Math.Cos(angle);
            float sin = (float)System.Math.Sin(angle);

            result.Row0 = new Vector4(cos, -sin, 0.0f, 0.0f);
            result.Row1 = new Vector4(sin, cos, 0.0f, 0.0f);
            result.Row2 = Vector4.UnitZ;
            result.Row3 = Vector4.UnitW;

            return result;
        }
        #endregion
    }



}
