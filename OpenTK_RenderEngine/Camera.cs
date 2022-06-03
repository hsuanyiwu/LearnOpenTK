using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using OpenTK;

namespace OpenTK_RenderEngine
{
    internal class Camera
    {
        // position
        private Vector3 _position = Vector3.UnitZ;
        private Vector3 _front = -Vector3.UnitZ;
        private Vector3 _up = Vector3.UnitY;
        private Vector3 _right = Vector3.UnitX;
        // attitude
        private float _pitch = 0.0f;
        private float _yaw = 0.0f;

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(_position, _position + _front, _up);
        }

        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vector3 Up
        {
            get { return _up; }
        }

        public Vector3 Right
        {
            get { return _right; }
        }

        public void Forward(float dist)
        {
            _position += _front * dist;
        }

        public void Backward(float dist)
        {
            _position -= _front * dist;
        }

        public void MoveUp(float dist)
        {
            _position += _up * dist;
        }

        public void MoveDown(float dist)
        {
            _position -= _up * dist;
        }

        public void MoveRight(float dist)
        {
            _position += _right * dist;
        }

        public void MoveLeft(float dist)
        {
            _position -= _right * dist;
        }

        public float Pitch
        {
            get { return _pitch; }
            set
            {
                if (value == _pitch)
                    return;
                _pitch = MathF.Clamp(value, -89, 89);
                UpdateAttitude();
            }
        }

        public float Yaw
        {
            get { return _yaw; }
            set
            {
                if (value == _yaw)
                    return;
                _yaw = value;
                UpdateAttitude();
            }
        }

        public void Arc(float dx, float dy)
        {
            this.Pitch -= dy;
            this.Yaw -= dx;

            double pitch = _pitch * Math.PI / 180;
            double yaw = _yaw * Math.PI / 180;
            double len = Position.Length;

            double y = len * Math.Sin(-pitch);
            double x = len * Math.Cos(-pitch) * Math.Sin(yaw);
            double z = len * Math.Cos(-pitch) * Math.Cos(yaw);
            Position = new Vector3((float)x, (float)y, (float)z);
            Trace.WriteLine($"{x} {y} {z}");
        }

        private void UpdateAttitude()
        {
            double pitch = _pitch * Math.PI / 180;
            double yaw = _yaw * Math.PI / 180;

            _front.X = -(float)(Math.Cos(pitch) * Math.Sin(yaw));
            _front.Y = (float)Math.Sin(pitch);
            _front.Z = -(float)(Math.Cos(pitch) * Math.Cos(yaw));
            _front = Vector3.Normalize(_front);

            _right = Vector3.Cross(_front, Vector3.UnitY);
            _right = Vector3.Normalize(_right);

            _up = Vector3.Cross(_right, _front);
        }

    }
}
