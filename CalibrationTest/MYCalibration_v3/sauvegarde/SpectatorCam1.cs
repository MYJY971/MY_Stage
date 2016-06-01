using System;
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
        

        #region Constructeurs
        public SpectatorCam(int width, int height)
            :base(width,height)
        {
            
        }

        public SpectatorCam(int width, int height, Vector3 eye, Vector3 target, Vector3 up)
            :base(width,height,eye,target,up)
        {
            
        }

        public SpectatorCam(int width, int height, Vector3 eye, Vector3 target, Vector3 up, Camera cam1, Camera cam2)
             : base(width, height, eye, target, up)
        {
            
            this._observedCam1 = cam1;
            this._observedCam2 = cam2;
           
        }
        #endregion

        #region Draw
        public override void Draw()
        {
            //_observedCam1.Projection();

            _observedCam1.Draw();
            //_observedCam2.Projection();
            _observedCam2.Draw();
            //Projection();
            
        }


        /*public override void DrawPlan(Color4 color)
        {
            Color4 colorP1 = color;
            Color4 colorP2 = new Color4(1 - colorP1.R, 1 - colorP1.G, 1 - colorP1.B, 1.0f);

            //_observedCam1.Projection();
            _observedCam1.DrawPlan(colorP1);
            //_observedCam2.Projection();
            _observedCam2.DrawPlan(colorP2);
            //Projection();
            //LookAt();
        }*/
        #endregion

        #region Control


       
        #endregion
   
        #region Spectator
        public override void SetObservedCams(Camera cam1, Camera cam2)
        {
            this._observedCam1 = cam1;
            this._observedCam2 = cam2;
        }

        public override void LookCam(int numCam)
        {
            if (numCam == 0)
                this._target = this._target0;
            else
                this._target = GetObservedCam(numCam)._eye;

            UpdateLookAt();
        }

        public override void ChangePerspective(int numCam)
        {
            Camera cam = GetObservedCam(numCam);

            if (numCam == 0)
            {
                _useDoubleMatrix = false;
                SetDefaultProjection(this._width, this._height);
            }

            else
                if (cam._useDoubleMatrix)
                SetPerspective(cam._projectionMatrixDouble);
            else
                SetPerspective(cam._projectionMatrix);
            
        }

        private Camera GetObservedCam (int num)
        {
            if (num == 1)
                return _observedCam1;
            else if (num == 2)
                return _observedCam2;
            else
                return this;
        }
        #endregion



    }
}
