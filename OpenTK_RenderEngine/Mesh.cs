using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTK_RenderEngine
{
    class Mesh
    {
        private static List<int> vaoList = new List<int>();
        private static List<int> vboList = new List<int>();

        public static void CleanUp()
        {
            foreach (var id in vaoList)
                GL.DeleteVertexArray(id);
            foreach (var id in vboList)
                GL.DeleteBuffer(id);
        }

        private static int CreateBuffer(Vector3[] vertices, UInt16[] indices)
        {
            // vertex attribute object
            int vaoId = GL.GenVertexArray();
            vaoList.Add(vaoId);
            GL.BindVertexArray(vaoId);

            // vertex buffer object
            int vertexId = GL.GenBuffer();
            vboList.Add(vertexId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexId);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * Vector3.SizeInBytes, vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            // index buffer object
            int indexId = GL.GenBuffer();
            vboList.Add(indexId);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(UInt16), indices, BufferUsageHint.StaticDraw);

            // unbind buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            return vaoId;
        }

        public static Mesh CreateTriangle(float w)
        {
            float x = w / 2;
            float y = w / 2;
            float z = 0;

            var vertices = new Vector3[]
            {
                //new Vector3(x, y, z),
                new Vector3(-x, y, z),
                new Vector3(-x, -y, z),
                new Vector3(x, -y, z),
            };

            var indices = new UInt16[]
            {
                0,1,2,
                //2,3,0,
            };

            int vaoId = CreateBuffer(vertices, indices);
            return new Mesh(vaoId, vertices.Length, indices.Length);
        }

        private int _vaoId;
        private int _vcount;
        private int _icount;

        private Mesh(int vaoId, int vcount, int icount)
        {
            _vaoId = vaoId;
            _vcount = vcount;
            _icount = icount;
        }

        public int VAOId()
        {
            return _vaoId;
        }

        public int VertexCount()
        {
            return _vcount;
        }

        public int IndexCount()
        {
            return _icount;
        }

        public DrawElementsType IndexType()
        {
            return DrawElementsType.UnsignedShort;
        }
    }
}
