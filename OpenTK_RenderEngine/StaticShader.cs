using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenTK_RenderEngine
{
    internal class StaticShader : ShaderProgram
    {
        private static readonly string VERTEX_FILE = "./shaders/shader.vert";
        private static readonly string FRAGMENT_FILE = "./shaders/shader.frag";


        private int _locMatModel;
        private int _locMatView;
        private int _locMatProj;


        public StaticShader()
        {
            FromFile(VERTEX_FILE, FRAGMENT_FILE);
        }

        protected override void OnProgramAttached()
        {
            BindAttribute(0, "position");            
        }
        protected override void OnProgramLinked()
        {
            _locMatModel = GetUniformLocation("matModel");
            _locMatView = GetUniformLocation("matView");
            _locMatProj = GetUniformLocation("matProj");
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
    }
}
