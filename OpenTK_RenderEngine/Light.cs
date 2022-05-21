using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;

namespace OpenTK_RenderEngine
{
    internal class Light
    {
        private Vector3 _position = Vector3.Zero;
        private Color _color = Color.White;

        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
    }
}
