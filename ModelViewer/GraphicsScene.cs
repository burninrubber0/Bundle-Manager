using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelViewer.SceneData;
using OpenTK.Graphics.ES20;

namespace ModelViewer
{
    public class GraphicsScene
    {
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
            // TODO: Load Shaders

            if (Scene == null || Scene != null && !Scene.InitGraphics())
                return;

            GL.ClearColor(Color.Black);

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
                Scene?.Render();

            GL.Flush();

            FrameRendered?.Invoke();
        }
    }
}
