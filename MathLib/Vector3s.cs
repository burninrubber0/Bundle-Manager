using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace MathLib
{
    public struct Vector3S
    {
        public short X;
        public short Y;
        public short Z;

        public Vector3S(short x, short y, short z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3S(Vector3 vec3)
        {
            X = (short)Math.Round(vec3.X);
            Y = (short)Math.Round(vec3.Y);
            Z = (short)Math.Round(vec3.Z);
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ", " + Z + ")";
        }
    }
}
