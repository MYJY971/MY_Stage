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


namespace MYCalibration_v4
{
    class SurfaceCam : ClassicCam
    {
        //private float _angle;
        private Vector3 _target0, _eye0, _targetZ;

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
            this._eye0 = eye; 
            
        }

        
        #endregion


        public override void RotateTarget(Matrix4 rotation)
        {
            base.RotateTarget(rotation);

            Vector3 tmp2 = this._target0 - this._target;
            this._target = VectMove(this._target, tmp2, (double)tmp2.Length);
            this._eye = VectMove(this._eye, tmp2, (double)tmp2.Length);
            this._eye0 = _eye;

            UpdateLookAt();
        }

        public override void UpdateLookAt()
        {
            base.UpdateLookAt();
            //this._targetZ = VectMove(this._target, Vector3.UnitZ, this._eye.Z - this._target.Z);
        }

        public override void RotateFromFile(string path)
        {
            Matrix4 rotation = GetSurfaceRot(path);
            RotateTarget(rotation);
            //RotateUp(rotation);
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
            Camera c1 = calibratedCam;

            //change perspective
            ChangePerspective(calibratedCam._projectionMatrixDouble);

            //change eye
            //Vector3 eyeCenter = calibratedCam._eye - this._target0;
            //Vector3 axeTarget = this._target - this._eye;
            //this._eye = VectMove(this._eye, axeTarget, (float)axeTarget.Length - (float)eyeCenter.Length);
            //fait correspondre les deux "targets"

            Vector3 tmp = c1._target - this._target;
            this._target = VectMove(this._target, tmp, (double)tmp.Length);
            //Adapte la position de l'oeil
            this._eye = VectMove(this._eye, tmp, (double)tmp.Length);

            //met le "eye" à la même hauteur que le eye de C1

            Vector3 targetAxiss = this._target - this._eye;
            Vector3 targetC1 = c1._target - c1._eye;
            this._eye = VectMove(this._eye, targetAxiss, targetAxiss.Length - targetC1.Length);
            this._eye0 = VectMove(this._eye0, targetAxiss, targetAxiss.Length - targetC1.Length); 
            
            //axe de symétrie entre les deux caméra
            Vector3 axis = VectMove(this._target, Vector3.UnitZ, this._eye.Z - this._target.Z);
            /*
            Vector3 vadjC1 = axis - this._target;
            Vector3 vhypC1 = c1._eye - c1._target;
            float angleC1 = Vector3.CalculateAngle(vadjC1, vhypC1);

            float adj = (float)Math.Cos(angleC1)*vhypC1.Length;

            Vector3 vectHyp = this._eye - this._target;
            
            float angle2 = Vector3.CalculateAngle(vectHyp, vadjC1);
            float hyp = adj / (float)(Math.Cos(angle2));

            Vector3 targetAxis = this._target - this._eye;

            this._eye = VectMove(this._eye, targetAxis, targetAxis.Length - hyp);
            this._eye0 = VectMove(this._eye0, targetAxis, targetAxis.Length - hyp);
            */
            Vector3 vecEyeAxis = this._eye - axis;
            Vector3 vecEyeC1Axis = c1._eye - axis;

            float angle3 = Vector3.CalculateAngle(vecEyeC1Axis, vecEyeAxis);
            float magicNumber = (float)Math.PI * 177.0438f / 180;
            RotateEye(angle3);

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

        public override void RotateEye(float angle)
        {
            Vector3 axis = VectMove(this._target, Vector3.UnitZ, this._eye.Z - this._target.Z);
            axis.Normalize();
            Matrix4 rotZ = /*Matrix4.CreateFromAxisAngle(axis, angle);//= */Matrix4.CreateRotationZ(angle);

            this._eye = Vector3.Transform(this._eye0, rotZ)+this._target;

            UpdateLookAt();
        }

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

        public override void Draw()
        {
            base.Draw();
            Vector3 axis = VectMove(this._target, Vector3.UnitZ, this._eye.Z-this._target.Z);

            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Begin(BeginMode.Lines);
            GL.Vertex3(axis);
            GL.Vertex3(this._target);
            GL.End();
            GL.Color4(this._color);
        }
    }

    

}
