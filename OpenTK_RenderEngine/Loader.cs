using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTK_RenderEngine
{
    class TextureLoader
    {
        public static int FromFile(string path)
        {
            int textureId = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            using (var img = (Bitmap)Image.FromFile(path))
            {
                // flip
                img.RotateFlip(RotateFlipType.RotateNoneFlipY);

                var data = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0,
                    PixelInternalFormat.Rgba, img.Width, img.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                    OpenTK.Graphics.OpenGL.PixelType.UnsignedByte,
                    data.Scan0);

                img.UnlockBits(data);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return textureId;
        }
    }

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
        private static List<int> textureList = new List<int>();

        public static void CleanUp()
        {
            GL.DeleteVertexArrays(vaoList.Count, vaoList.ToArray());
            vaoList.Clear();
            GL.DeleteBuffers(vboList.Count, vboList.ToArray());
            vboList.Clear();
            GL.DeleteTextures(textureList.Count, textureList.ToArray());
            textureList.Clear();
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

        public static int CreateTexture(string filePath)
        {
            int textureId = TextureLoader.FromFile(filePath);
            textureList.Add(textureId);
            return textureId;
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
            GL.VertexAttribPointer(index, 2, VertexAttribPointerType.Float, false, 0, 0);
        }

    }
}
