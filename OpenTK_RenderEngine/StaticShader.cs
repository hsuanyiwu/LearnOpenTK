using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_RenderEngine
{
    internal class StaticShader : ShaderProgram
    {
        private static readonly string VERTEX_FILE = "./shaders/shader.vert";
        private static readonly string FRAGMENT_FILE = "./shaders/shader.frag";


        public StaticShader()
        {
            FromFile(VERTEX_FILE, FRAGMENT_FILE);
        }

        protected override void BindAttributes()
        {
            BindAttribute(0, "position");
        }
    }
}
