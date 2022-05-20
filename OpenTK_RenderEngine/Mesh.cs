using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace OpenTK_RenderEngine
{
    class Loader
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

        public static int CreateBuffer(Vector3[] vertices, UInt16[] indices)
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
    }

    class Mesh
    {
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

            return CreateMesh(vertices, indices);
        }

        public static Mesh CreateCubic(float w)
        {
            float x = w / 2;
            float y = w / 2;
            float z = w / 2;

            var vertices = new Vector3[]
            {
                new Vector3(x, y, z),
                new Vector3(-x, y, z),
                new Vector3(-x, -y, z),
                new Vector3(x, -y, z),

                new Vector3(x, y, -z),
                new Vector3(-x, y, -z),
                new Vector3(-x, -y, -z),
                new Vector3(x, -y, -z),
            };

            var indices = new UInt16[]
            {
                0,1,2,
                2,3,0,
                0,4,5,
                5,1,0,
                4,0,3,
                3,7,4,
                5,1,2,
                2,6,5,
                3,2,6,
                6,7,3,
                5,4,7,
                7,6,5
            };

            return CreateMesh(vertices, indices);
        }

        private static Mesh CreateMesh(Vector3[] vertices, UInt16[] indices)
        {
            int vaoId = Loader.CreateBuffer(vertices, indices);
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

        public int GetVAOId()
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

        public static Mesh FromObjFile(string file)
        {
            try
            {
                List<Vector3> vertices = new List<Vector3>();
                List<UInt16> indices = new List<UInt16>();

                List<Vector3> normal_data = new List<Vector3>();
                Vector3[] normal;

                using (var sr = new StreamReader(file))
                {
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith("v ")) // vertex
                        {
                            var f = line.Split(' ');
                            var v = new Vector3(float.Parse(f[1]), float.Parse(f[2]), float.Parse(f[3]));
                            vertices.Add(v);
                        }
                        else if (line.StartsWith("vt ")) // texture coord of vertex
                        {
                            var f = line.Split(' ');
                        }
                        else if (line.StartsWith("vn ")) // normal of vertex
                        {
                            var f = line.Split(' ');
                            var v = new Vector3(float.Parse(f[1]), float.Parse(f[2]), float.Parse(f[3]));
                            normal_data.Add(v);
                        }
                        else if (line.StartsWith("f ")) // face 
                        {
                            break;
                        }
                    }

                    do
                    {
                        normal = new Vector3[vertices.Count];

                        if (line.StartsWith("f ")) // f p1 p2 p3
                        {
                            var f = line.Split(' '); 
                            for (int i = 0; i < 3; ++i)
                            {
                                var v = f[i + 1].Split('/');
                                // vertex data position -> index
                                int vertex_i = int.Parse(v[0]) - 1;
                                indices.Add((UInt16)vertex_i);
                                // texture data position
                                int texture_i = int.Parse(v[1]) - 1;

                                // normal data position
                                int normal_i = int.Parse(v[2]) - 1;
                                normal[vertex_i] = normal_data[normal_i];
                            }
                        }
                    }
                    while ((line = sr.ReadLine()) != null);
                }

                return CreateMesh(vertices.ToArray(), indices.ToArray());
            }
            catch (Exception e)
            {

            }
            return null;
        }
    }
}
