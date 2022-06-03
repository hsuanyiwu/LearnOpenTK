using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTK_RenderEngine
{
    class Render
    {
        static private Matrix4 _matProjection = Matrix4.Identity;

        static public void Prepare()
        {
            GL.ClearColor(Color.FromArgb(88, 88, 88));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
//            GL.PolygonMode(PolygonMode.)
        }

        static public void Draw(Entity entity)
        {
            Mesh mesh = entity.Mesh;
            Texture texture = entity.Texture;

            // bind and enable
            GL.BindVertexArray(mesh.GetVAOId());
            GL.EnableVertexAttribArray((int)VAO_INDEX.VERTEX);
            GL.EnableVertexAttribArray((int)VAO_INDEX.NORMAL);

            if(texture != null)
            {
                GL.EnableVertexAttribArray((int)VAO_INDEX.TEXTURE);
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, texture.TextureId);
            }

            // draw triangle
            GL.DrawElements(PrimitiveType.Triangles, mesh.IndexCount(), mesh.IndexType(), 0);


            if (texture != null)
            {
                GL.DisableVertexAttribArray((int)VAO_INDEX.TEXTURE);
            }

            // disable and unbind
            GL.DisableVertexAttribArray((int)VAO_INDEX.VERTEX);
            GL.DisableVertexAttribArray((int)VAO_INDEX.NORMAL);
            GL.BindVertexArray(0);
        }

        static public void ResizeWindow(int width, int height)
        {
            float aspect = width * 1.0f / height;
            float fov = (float)(60.0f * Math.PI / 180.0f);
            _matProjection = Matrix4.CreatePerspectiveFieldOfView(fov, aspect, 0.01f, 100.0f);

            GL.Viewport(0, 0, width, height);
        }

        static public Matrix4 GetProjectionMatrix()
        {
            return _matProjection;
        }
    }
}
