using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;


namespace OpenTK_RenderEngine
{
    internal class StaticShader : ShaderProgram
    {
        private static readonly string VERTEX_FILE = "./shaders/shader.vert";
        private static readonly string FRAGMENT_FILE = "./shaders/shader.frag";


        private int _locMatModel;
        private int _locMatView;
        private int _locMatProj;
        private int _locLightPos;
        private int _locLightColor;

        public StaticShader()
        {
            FromFile(VERTEX_FILE, FRAGMENT_FILE);
        }

        protected override void OnProgramAttached()
        {
            BindAttribute((int)VAO_INDEX.VERTEX, "aPosition");
            BindAttribute((int)VAO_INDEX.NORMAL, "aNormal");
            BindAttribute((int)VAO_INDEX.TEXTURE, "aTexCoord");
        }
        protected override void OnProgramLinked()
        {
            _locMatModel = GetUniformLocation("matModel");
            _locMatView = GetUniformLocation("matView");
            _locMatProj = GetUniformLocation("matProj");

            _locLightPos = GetUniformLocation("lightPos");
            _locLightColor = GetUniformLocation("lightColor");
        }

        public void SetModelMatrix(Matrix4 m)
        {
            SetMatrix(_locMatModel, m);
        }

        public void SetViewMatrix(Matrix4 m)
        {
            SetMatrix(_locMatView, m);
        }

        public void SetProjectionMatrix(Matrix4 m)
        {
            SetMatrix(_locMatProj, m);
        }

        public void SetLight(Vector3 lighPos, Color color)
        {
            SetVector(_locLightPos, lighPos);
            float r = 1.0f / 255.0f;
            var clr = new Vector3(color.R * r, color.G * r, color.B * r);
            SetVector(_locLightColor, clr);
        }
    }
}
