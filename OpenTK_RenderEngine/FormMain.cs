using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTK_RenderEngine
{
    public partial class FormMain : Form
    {
        private Mesh _mesh;
        private StaticShader _shader;
        private Entity _entity;
        private Camera _camera;

        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _shader = new StaticShader();
            _entity = new Entity();
            //_entity.Mesh = Mesh.CreateCubic(0.5f);
            _entity.Mesh = Mesh.FromObjFile(@"res/cube.obj");
            _camera = new Camera();
            _camera.Backward(5.0f);
        }
        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Loader.CleanUp();
            _shader.CleanUp();
        }
        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            Render.Prepare();
            _shader.Start();

            float aspect = glControl.Width * 1.0f / glControl.Height;
            float fov = (float)(60.0f * Math.PI / 180.0f);
            var projection = Matrix4.CreatePerspectiveFieldOfView(fov, aspect, 0.01f, 100.0f);
            _shader.SetProjectionMatrix(projection);
            _shader.SetViewMatrix(_camera.GetViewMatrix());
            _shader.SetModelMatrix(_entity.GetModelMatrix());

            Render.Draw(_entity);

            _shader.Stop();

            glControl.SwapBuffers();
        }

        private void timer_tick_Tick(object sender, EventArgs e)
        {
            glControl.Invalidate();
            _entity.Rotate(0.01f, 0.01f, 0);            
        }

        private void glControl_Resize(object sender, EventArgs e)
        {
            glControl.MakeCurrent();
            GL.Viewport(0, 0, glControl.Width, glControl.Height);
            GL.ClearColor(Color.FromArgb(88, 88, 88));
            GL.Enable(EnableCap.DepthTest);
        }
    }

}
