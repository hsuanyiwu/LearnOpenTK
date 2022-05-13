using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenTK_RenderEngine
{
    internal class Entity
    {
        private Vector3 _position = Vector3.Zero;
        private Vector3 _rotation = Vector3.Zero;

        public Entity()
        {

        }

        public Mesh Mesh
        {
            get;
            set;
        }

        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vector3 Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public Matrix4 GetModelMatrix()
        {
            var mx = Matrix4.CreateRotationX(_rotation.X);
            var my = Matrix4.CreateRotationY(_rotation.Y);
            var mz = Matrix4.CreateRotationZ(_rotation.Z);
            return Matrix4.CreateTranslation(_position) * mx * my * mz;
        }

        public void Move(float dx, float dy, float dz)
        {
            _position.X += dx;
            _position.Y += dy;
            _position.Z += dz;
        }

        public void Rotate(float rotX, float rotY, float rotZ)
        {
            _rotation.X += rotX;
            _rotation.Y += rotY;
            _rotation.Z += rotZ;
        }
    }
}
