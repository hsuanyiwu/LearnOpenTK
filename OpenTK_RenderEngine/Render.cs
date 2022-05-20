using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTK_RenderEngine
{
    class Render
    {
        static public void Prepare()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        static public void Draw(Entity entity)
        {
            Mesh mesh = entity.Mesh;

            // bind and enable
            GL.BindVertexArray(mesh.GetVAOId());
            GL.EnableVertexAttribArray(0);

            // draw triangle
            GL.DrawElements(PrimitiveType.Triangles, mesh.IndexCount(), mesh.IndexType(), 0);

            // disable and unbind
            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);
        }
    }
}
