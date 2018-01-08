using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLib
{
    public static class IOExtensions
    {
        public static Matrix4 ReadMatrix4(this BinaryReader self)
        {
            Matrix4 result = new Matrix4();
            
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[i, j] = self.ReadSingle();
                }
            }
            
            return result;
        }

        public static void Write(this BinaryWriter self, Matrix4 value)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    self.Write(value[i, j]);
                }
            }
        }
    }
}
