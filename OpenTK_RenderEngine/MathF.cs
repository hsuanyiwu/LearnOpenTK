using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_RenderEngine
{
    internal class MathF
    {
        public const float RAD_PER_DEG = (float)(Math.PI / 180.0);
        public const float PI = (float)Math.PI;

        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
                return min;
            if(value > max)
                return max;
            return value;
        }
    }
}
