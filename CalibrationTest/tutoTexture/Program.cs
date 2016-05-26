using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using TexLib;
using OpenTK.Graphics;
using OpenTK.Input;
using System.Drawing;

namespace tutoTexture
{
    public class Demo : GameWindow
    {
        private int textureId;

        public Demo()
          : base(800, 600, GraphicsMode.Default, "OpenTK TexLib Demo")
        {
        }

        public override void OnLoad(EventArgs e)
        {
            //
            // Initialize alpha-blended texturing
            //
            TexUtil.InitTexturing();
            //
            // Creating a texture
            //
            textureId = TexUtil.CreateRGBTexture(2, 2,
              new byte[] {
          255,0,0, // Red
          0,255,0, // Green
          0,0,255, // Blue
          255,255,255 // White
              });
            OnResize(null);
            GL.ClearColor(Color.Blue);
        }

        public override void OnUpdateFrame(UpdateFrameEventArgs e)
        {
            if (Keyboard[Key.Escape])
                Exit();
        }

        protected override void OnResize(OpenTK.Platform.ResizeEventArgs e)
        {
            GL.Viewport(new System.Drawing.Size(Width, Height));
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, Width, 0, Height, -1, 1);
        }

        public override void OnRenderFrame(RenderFrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0, 0); GL.Vertex2(10, 10);
            GL.TexCoord2(1, 0); GL.Vertex2(100, 10);
            GL.TexCoord2(1, 1); GL.Vertex2(100, 100);
            GL.TexCoord2(0, 1); GL.Vertex2(10, 100);
            GL.End();
            SwapBuffers();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (Demo demo = new Demo())
            {
                demo.Run(30.0, 0.0);
            }
        }
    }
}
