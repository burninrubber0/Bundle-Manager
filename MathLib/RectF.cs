using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLib
{
    public class RectF
    {
        public float X;
        public float Width;
        public float Y;
        public float Height;

        public override string ToString()
        {
            return "{X: " + X + ", Y: " + Y + ", Width: " + Width + ", Height: " + Height + "}";
        }
    }
}
