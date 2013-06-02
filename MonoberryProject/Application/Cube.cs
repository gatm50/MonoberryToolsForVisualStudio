using System;
using BlackberryPlatformServices;
using BlackberryPlatformServices.Screen;
using BlackberryPlatformServices.Screen.Types;
using OpenTKBlackberry.Graphics.ES11;
using OpenTKBlackberry.Platform.Egl;

namespace $safeprojectname$
{
    class Cube
    {
        float[] _rot;
        float[] _rateOfRotationPS;//degrees
        int _viewportWidth, _viewportHeight;

        float[] _cube = {
            -0.5f, 0.5f, 0.5f, // vertex[0]
            0.5f, 0.5f, 0.5f, // vertex[1]
            0.5f, -0.5f, 0.5f, // vertex[2]
            -0.5f, -0.5f, 0.5f, // vertex[3]
            -0.5f, 0.5f, -0.5f, // vertex[4]
            0.5f, 0.5f, -0.5f, // vertex[5]
            0.5f, -0.5f, -0.5f, // vertex[6]
            -0.5f, -0.5f, -0.5f, // vertex[7]
        };

        byte[] _triangles = {
            1, 0, 2, // front
            3, 2, 0,
            6, 4, 5, // back
            4, 6, 7,
            4, 7, 0, // left
            7, 3, 0,
            1, 2, 5, //right
            2, 6, 5,
            0, 1, 5, // top
            0, 5, 4,
            2, 3, 6, // bottom
            3, 7, 6,
        };

        float[] _cubeColors = {
            1.0f, 0.0f, 0.0f, 1.0f,
            0.0f, 1.0f, 0.0f, 1.0f,
            0.0f, 0.0f, 1.0f, 1.0f,
            0.0f, 1.0f, 1.0f, 1.0f,
            1.0f, 0.0f, 0.0f, 1.0f,
            0.0f, 1.0f, 0.0f, 1.0f,
            0.0f, 0.0f, 1.0f, 1.0f,
            0.0f, 1.0f, 1.0f, 1.0f,
        };

        public void Run()
        {
            _rateOfRotationPS = new float[] { 30, 45, 60 };
            _rot = new float[] { 0, 0, 0 };

            BBUtil util;
            using (var nav = new Navigator())
            using (var ctx = new Context(ContextType.SCREEN_APPLICATION_CONTEXT))
            using (var win = new Window(ctx))
            {
                util = new BBUtil(ctx, BBUtil.OpenGLESVersion.ES11);

                Egl.QuerySurface(util.Display, util.Surface, Egl.WIDTH, out _viewportWidth);
                Egl.QuerySurface(util.Display, util.Surface, Egl.HEIGHT, out _viewportHeight);

                this.OnLoad(util);

                nav.OnExit += () =>
                {
                    Console.WriteLine("I am asked to shutdown!?!");
                    PlatformServices.Shutdown(0);
                };
            }
        }

        protected void OnLoad(BBUtil util)
        {
            GL.Enable(All.CullFace);
            GL.ShadeModel(All.Smooth);

            GL.Hint(All.PerspectiveCorrectionHint, All.Nicest);

            // Run the render loop
            PlatformServices.Run(delegate
            {
                for (int j = 0; j < 3; j++)
                    _rot[j] += (float)(_rateOfRotationPS[j] * 0.02);
                this.RenderCube(util);
            });
        }

        void RenderCube(BBUtil util)
        {
            GL.Viewport(0, 0, _viewportWidth, _viewportHeight);

            GL.MatrixMode(All.Projection);
            GL.LoadIdentity();

            if (_viewportWidth > _viewportHeight)
            {
                GL.Ortho(-1.5f, 1.5f, 1.0f, -1.0f, -1.0f, 1.0f);
            }
            else
            {
                GL.Ortho(-1.0f, 1.0f, -1.5f, 1.5f, -1.0f, 1.0f);
            }

            GL.MatrixMode(All.Modelview);
            GL.LoadIdentity();
            GL.Rotate(_rot[0], 1.0f, 0.0f, 0.0f);
            GL.Rotate(_rot[1], 0.0f, 1.0f, 0.0f);
            GL.Rotate(_rot[2], 0.0f, 1.0f, 0.0f);

            GL.ClearColor(0, 0, 0, 1.0f);
            GL.Clear((int)ClearBufferMask.ColorBufferBit);

            GL.VertexPointer(3, All.Float, 0, _cube);
            GL.EnableClientState(All.VertexArray);
            GL.ColorPointer(4, All.Float, 0, _cubeColors);
            GL.EnableClientState(All.ColorArray);
            GL.DrawElements(All.Triangles, 36, All.UnsignedByte, _triangles);

            util.Swap();
        }
    }
}
