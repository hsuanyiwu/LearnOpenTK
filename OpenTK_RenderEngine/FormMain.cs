using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
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
            glControl.MouseWheel += glControl_MouseWheel;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            _shader = new StaticShader();
            _entity = new Entity();
            //_entity.Mesh = Mesh.CreateTriangle(1.0f);
            //_entity.Mesh = Mesh.CreateCubic(0.5f);
            //_entity.Mesh = Mesh.FromObjFile(@"res/cube.obj");
            //_entity.Mesh = Mesh.FromObjFile(@"res/tree.obj");
            //_entity.Texture = Texture.FromFile(@"res/tree.png");
            _entity.Mesh = Mesh.FromObjFile(@"res/lowPolyTree.obj");
            _entity.Texture = Texture.FromFile(@"res/lowPolyTree.png");
            //_entity.Mesh = Mesh.FromObjFile(@"res/dragon.obj");
            //_entity.Mesh = Mesh.FromObjFile(@"res/fern.obj");
            //_entity.Texture = Texture.FromFile(@"res/fern.png");

            _camera = new Camera();
            _camera.Position = new Vector3(0, 5, 5);
            _camera.Pitch = -45;
            //_camera.Yaw = -20;

            _light = new Light();
            _light.Position = new Vector3(20.0f, 20.0f, 20.0f);
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
            //_entity.RotateDeg(0.0f, 1.0f, 0.0f);
        }

        private void glControl_Resize(object sender, EventArgs e)
        {
            glControl.MakeCurrent();
            Render.ResizeWindow(glControl.Width, glControl.Height);
        }

        private void glControl_KeyDown(object sender, KeyEventArgs e)
        {
            float dist = 0.1f;

            switch (e.KeyCode)
            {
                case Keys.W:
                    _camera.Forward(dist);
                    break;
                case Keys.S:
                    _camera.Backward(dist);
                    break;
                case Keys.A:
                    _camera.MoveLeft(dist);
                    break;
                case Keys.D:
                    _camera.MoveRight(dist);
                    break;
                case Keys.E:
                    _camera.MoveUp(dist);
                    break;
                case Keys.Q:
                    _camera.MoveDown(dist);
                    break;
            }
        }

        private bool _mouseDown = false;
        private Point _ptMouse;

        private void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!_mouseDown)
                {
                    _mouseDown = true;
                    _ptMouse = e.Location;
                }
                else
                {
                    float dx = _ptMouse.X - e.X;
                    float dy = _ptMouse.Y - e.Y;
                    _camera.Yaw += dx;
                    _camera.Pitch += dy;
                    _ptMouse = e.Location;
                    Trace.WriteLine($"{_camera.Pitch},{_camera.Yaw}");
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (!_mouseDown)
                {
                    _mouseDown = true;
                    _ptMouse = e.Location;
                }
                else
                {
                    float dx = _ptMouse.X - e.X;
                    float dy = _ptMouse.Y - e.Y;
                    _camera.Arc(dx, dy);
                    _ptMouse = e.Location;
                }
            }
        }

        private void glControl_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
        }

        private void glControl_MouseWheel(object sender, MouseEventArgs e)
        {
            _camera.Forward(e.Delta*0.01f);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _camera.Arc(1, 0);
            Trace.WriteLine($"{_camera.Pitch},{_camera.Yaw}");
        }
    }

}
