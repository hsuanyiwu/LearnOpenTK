﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenTK_RenderEngine
{
    internal class Camera
    {
        private Vector3 _position = Vector3.UnitZ;
        private Vector3 _front = -Vector3.UnitZ;
        private Vector3 _up = Vector3.UnitY;

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(_position, _position + _front, _up);
        }
    }
}
