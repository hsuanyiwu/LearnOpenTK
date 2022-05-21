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
        private Light _light;
        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _shader = new StaticShader();
            _entity = new Entity();
            //_entity.Mesh = Mesh.CreateTriangle(1.0f);
            //_entity.Mesh = Mesh.CreateCubic(0.5f);
            //_entity.Mesh = Mesh.FromObjFile(@"res/cube.obj");
            _entity.Mesh = Mesh.FromObjFile(@"res/dragon.obj");
            //_entity.Rotate(0, 45, 0);

            _camera = new Camera();
            //_camera.Position = new Vector3(3,3,3);
            _camera.Backward(5.0f);
            _camera.MoveUp(2.0f);

            _light = new Light();
            _light.Position = new Vector3(20.0f,2.0f,20.0f);
            _light.Color = Color.Orange;
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

            // set transform matrix
            _shader.SetProjectionMatrix(Render.GetProjectionMatrix());
            _shader.SetViewMatrix(_camera.GetViewMatrix());
            _shader.SetModelMatrix(_entity.GetModelMatrix());

            // set light
            _shader.SetLight(_light.Position, _light.Color);

            Render.Draw(_entity);

            _shader.Stop();
            glControl.SwapBuffers();
        }

        private void timer_tick_Tick(object sender, EventArgs e)
        {
            glControl.Invalidate();
            _entity.RotateDeg(0.0f, 1.0f, 0.0f);
        }

        private void glControl_Resize(object sender, EventArgs e)
        {
            glControl.MakeCurrent();
            Render.ResizeWindow(glControl.Width, glControl.Height);
        }
    }

}
