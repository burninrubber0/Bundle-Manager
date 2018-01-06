using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nexus;

namespace MathLib
{
    public static class IOExtensions
    {
        public static Matrix3D ReadMatrix3F(this BinaryReader self)
        {
            Matrix3D result = new Matrix3D();
            
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[j, i] = self.ReadSingle();
                }
            }
            
            return result;
        }

        public static void Write(this BinaryWriter self, Matrix3D value)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    self.Write(value[j, i]);
                }
            }
        }
    }
}
