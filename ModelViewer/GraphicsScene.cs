using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelViewer.SceneData;
using OpenTK.Graphics.OpenGL4;

namespace ModelViewer
{
    public class GraphicsScene
    {
        private Shader _shader;

        public delegate void OnFrameRendered();
        public event OnFrameRendered FrameRendered;

        public int Width { get; set; }
        public int Height { get; set; }

        public bool CanRender { get; set; }

        public Scene Scene { get; set; }

        public GraphicsScene(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public void Init()
        {
            if (Scene == null)
                return;
            
            try
            {
                _shader = new Shader("standard");
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex.Message + "\n" + ex.StackTrace);

                CanRender = false;
                return;
            }
            _shader.Compile();

            if (!Scene.InitGraphics())
                return;

            GL.ClearColor(Color.Black);
            GL.ClearDepth(1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            //glShadeModel(GL_SMOOTH);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            CanRender = true;
        }

        public void Update()
        {
            // TODO: Call and Implement
        }

        public void Render()
        {
            GL.Viewport(0, 0, Width, Height);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (CanRender)
            {
                _shader.Bind();

                Scene?.Render();
            }

            GL.Flush();

            FrameRendered?.Invoke();
        }

        public void Cleanup()
        {
            if (CanRender)
            {
                Scene?.Dispose();
                _shader.Dispose();
            }
        }
    }
}
