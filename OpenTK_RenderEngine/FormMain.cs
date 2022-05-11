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

        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _mesh = Mesh.CreateTriangle(0.5f);
            _shader = new StaticShader();
        }
        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Mesh.CleanUp();
            _shader.CleanUp();
        }
        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _shader.Start();
            Render(_mesh);
            _shader.Stop();

            glControl.SwapBuffers();
        }

        private void timer_tick_Tick(object sender, EventArgs e)
        {
            glControl.Update();
        }

        private void glControl_Resize(object sender, EventArgs e)
        {
            glControl.MakeCurrent();
            GL.Viewport(0, 0, glControl.Width, glControl.Height);
            GL.ClearColor(Color.FromArgb(88, 88, 88));
        }

        private void Render(Mesh mesh)
        {
            // bind and enable
            GL.BindVertexArray(mesh.VAOId());
            GL.EnableVertexAttribArray(0);

            // draw triangle
            GL.DrawElements(PrimitiveType.Triangles, mesh.IndexCount(), mesh.IndexType(), 0);

            // disable and unbind
            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);
        }
    }

}
