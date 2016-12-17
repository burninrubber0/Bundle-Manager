using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardExtension
{
    public static class IOExtensions
    {
        public static bool EOF(this Stream self)
        {
            return self.Position >= self.Length;
        }

        public static byte[] ReadToEnd(this BinaryReader self)
        {
            List<byte> result = new List<byte>();
            while (!self.BaseStream.EOF())
            {
                result.Add(self.ReadByte());
            }
            return result.ToArray();
        }
    }
}
