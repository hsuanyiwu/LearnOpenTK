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
    class Mesh
    {
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

        public static Mesh FromObjFile(string file)
        {
            try
            {
                List<Vector3> vertices = new List<Vector3>();
                List<UInt16> indices = new List<UInt16>();

                List<Vector2> texture_data = new List<Vector2>();
                Vector2[] texture;

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
                            var v = new Vector2(float.Parse(f[1]), float.Parse(f[2]));
                            texture_data.Add(v);
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

                    texture = new Vector2[vertices.Count];
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
                                texture[vertex_i] = texture_data[texture_i];
                                // normal data position
                                int normal_i = int.Parse(v[2]) - 1;
                                normal[vertex_i] += normal_data[normal_i];
                            }
                        }
                    }
                    while ((line = sr.ReadLine()) != null);
                }

                return CreateMesh(vertices.ToArray(), indices.ToArray(), normal, texture);
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
