using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTK_RenderEngine
{
    internal abstract class ShaderProgram
    {
        private int _programId;
        private int _vertShaderId;
        private int _fragShaderId;

        public ShaderProgram()
        {

        }

        protected void FromFile(string vertFile, string fragFile)
        {
            var vertText = File.ReadAllText(vertFile);
            var fragText = File.ReadAllText(fragFile);
            FromText(vertText, fragText);
        }

        protected void FromText(string vertText, string fragText)
        {
            // load shader
            _vertShaderId = LoadShader(vertText, ShaderType.VertexShader);
            _fragShaderId = LoadShader(fragText, ShaderType.FragmentShader);

            // create
            _programId = GL.CreateProgram();
            GL.AttachShader(_programId, _vertShaderId);
            GL.AttachShader(_programId, _fragShaderId);

            // set uniform by sub class
            OnProgramAttached();

            // link
            GL.LinkProgram(_programId);
            int result;
            GL.GetProgram(_programId, GetProgramParameterName.LinkStatus, out result);
            if (result != (int)All.True)
                throw new Exception($"link program error: {_programId}");

            // get uniform by sub class
            OnProgramLinked();
        }

        private int LoadShader(string txt, ShaderType type)
        {
            // create
            int shaderId = GL.CreateShader(type);
            GL.ShaderSource(shaderId, txt);

            // compile
            GL.CompileShader(shaderId);
            int result;
            GL.GetShader(shaderId, ShaderParameter.CompileStatus, out result);
            if (result != (int)All.True)
                throw new Exception($"compiling shader error: {GL.GetShaderInfoLog(shaderId)}");

            return shaderId;
        }

        public void CleanUp()
        {
            Stop();
            // detach and delete shader
            GL.DetachShader(_programId, _vertShaderId);
            GL.DetachShader(_programId, _fragShaderId);
            GL.DeleteShader(_vertShaderId);
            GL.DeleteShader(_fragShaderId);
            // delete program
            GL.DeleteProgram(_programId);
        }

        protected abstract void OnProgramAttached();
        protected abstract void OnProgramLinked();

        protected void BindAttribute(int attribute, string name)
        {
            GL.BindAttribLocation(_programId, attribute, name);
        }

        public void Start()
        {
            GL.UseProgram(_programId);
        }

        public void Stop()
        {
            GL.UseProgram(0);
        }

        protected int GetUniformLocation(string name)
        {
            return GL.GetUniformLocation(_programId, name);
        }

        protected void SetFloat(int location, float v)
        {
            GL.Uniform1(location, v);
        }

        protected void SetInt(int location, int v)
        {
            GL.Uniform1(location++, v);
        }
                
        protected void SetVector(int location, Vector3 v)
        {
            GL.Uniform3(location, v);
        }

        protected void SetMatrix(int location, Matrix4 m)
        {
            GL.UniformMatrix4(location, false, ref m);
        }
    }
}
