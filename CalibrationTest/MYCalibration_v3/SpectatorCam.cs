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
    class SpectatorCam : Camera
    {
        Camera _cam1, _cam2;

        public SpectatorCam(int width, int height, Vector3 eye, Vector3 target, Vector3 up, Camera c1, Camera c2 )
            : base(width,height,eye,target,up)
        {
            this._cam1 = c1;
            this._cam2 = c2;
            
        }

        public SpectatorCam(Camera c1, Camera c2)
            :base(c1.GetWidth(), c1.GetHeight(), c1._eye, c1._target, c1._up)
        {
            this._cam1 = c1;
            this._cam2 = c2;
        }
        
         
    }
}
