using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Diagnostics;

namespace OpenTK_RenderEngine
{
    enum VAO_INDEX
    {
        VERTEX = 0,
        NORMAL = 1,
        TEXTURE = 2,
    }

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

        public static int CreateBuffer(Vector3[] vertices, UInt16[] indices, Vector3[] normal = null, Vector2[] texture = null)
        {
            // vertex attribute object
            int vaoId = GL.GenVertexArray();
            vaoList.Add(vaoId);
            GL.BindVertexArray(vaoId);

            // store vbo data
            StoreBufferData((int)VAO_INDEX.VERTEX, vertices);
            if (normal != null)
                StoreBufferData((int)VAO_INDEX.NORMAL, normal);
            if (texture != null)
                StoreBufferData((int)VAO_INDEX.TEXTURE, texture);
            StoreIndexData(indices);

            // unbind buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);

            return vaoId;
        }

        private static void StoreIndexData(ushort[] indices)
        {
            int vboId = GL.GenBuffer();
            vboList.Add(vboId);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, vboId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(UInt16), indices, BufferUsageHint.StaticDraw);
        }

        private static void StoreBufferData(int index, Vector3[] data)
        {
            int vboId = GL.GenBuffer();
            vboList.Add(vboId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboId);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * Vector3.SizeInBytes, data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(index, 3, VertexAttribPointerType.Float, false, 0, 0);
        }

        private static void StoreBufferData(int index, Vector2[] data)
        {
            int vboId = GL.GenBuffer();
            vboList.Add(vboId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboId);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * Vector2.SizeInBytes, data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(index, 3, VertexAttribPointerType.Float, false, 0, 0);
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
                new Vector3(0, y, z),
                new Vector3(-x, -y, z),
                new Vector3(x, -y, z),
            };

            var normal = new Vector3[]
            {
                Vector3.UnitZ,
                Vector3.UnitZ,
                Vector3.UnitZ
            };

            var indices = new UInt16[]
            {
                0,1,2,
            };

            return CreateMesh(vertices, indices, normal);
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

            var normal = new Vector3[]
            {
                new Vector3(0,0,1),
                new Vector3(0,0,1),
                new Vector3(0,0,1),
                new Vector3(0,0,1),

                new Vector3(0,0,-1),
                new Vector3(0,0,-1),
                new Vector3(0,0,-1),
                new Vector3(0,0,-1),
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

            return CreateMesh(vertices, indices, normal);
        }

        private static Mesh CreateMesh(Vector3[] vertices, UInt16[] indices, Vector3[] normal = null, Vector2[] texture = null)
        {
            int vaoId = Loader.CreateBuffer(vertices, indices, normal, texture);
            return new Mesh(vaoId, indices.Length);
        }


        private int _vaoId;
        private int _icount;

        private Mesh(int vaoId, int icount)
        {
            _vaoId = vaoId;
            _icount = icount;
        }

        public int GetVAOId()
        {
            return _vaoId;
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

                    normal = new Vector3[vertices.Count];
                    do
                    {
                        if (line.StartsWith("f ")) // f p1 p2 p3
                        {
                            var f = line.Split(' ');
                            for (int i = 0; i < 3; ++i)
                            {
                                var v = f[1 + i].Split('/');
                                // vertex data position -> index
                                int vertex_i = int.Parse(v[0]) - 1;
                                indices.Add((UInt16)vertex_i);
                                // texture data position
                                int texture_i = int.Parse(v[1]) - 1;

                                // normal data position
                                int normal_i = int.Parse(v[2]) - 1;
                                normal[vertex_i] += normal_data[normal_i];
                            }
                        }
                    }
                    while ((line = sr.ReadLine()) != null);
                }

                return CreateMesh(vertices.ToArray(), indices.ToArray(), normal);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                throw e;
            }
            return null;
        }
    }
}
