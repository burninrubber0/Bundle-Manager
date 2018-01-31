using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundleUtilities
{
    public static class Utilities
    {
        public static bool IsValidPath(string path)
        {
            return !string.IsNullOrEmpty(path);
        }

        public static bool FileExists(string path)
        {
            return IsValidPath(path) && File.Exists(path);
        }

        public static byte[] Pad(this byte[] self, int bytes)
        {
            if (self.Length == bytes)
                return self;

            if (self.Length > bytes)
                throw new ArgumentException("Cannot pad to " + bytes);

            byte[] result = new byte[bytes];

            for (int i = 0; i < self.Length; i++)
            {
                result[i] = self[i];
            }

            return result;
        }

        public static bool EOF(this BinaryReader self)
        {
            return self.BaseStream.Position >= self.BaseStream.Length;
        }

        public static string ReadCStr(this BinaryReader self)
        {
            StringBuilder sb = new StringBuilder();

            while (!self.EOF())
            {
                char c = (char)self.ReadByte();
                if (c == '\0')
                    break;
                sb.Append(c);
            }

            return sb.ToString();
        }

        public static byte[] Flip(this byte[] self)
        {
            byte[] result = new byte[self.Length];

            int j = 0;
            for (int i = self.Length - 1; i >= 0; i--)
            {
                result[j] = self[i];
                j++;
            }

            return result;
        }

        public static bool NoData(this byte[] self)
        {
            for (int i = 0; i < self.Length; i++)
            {
                if (self[i] != 0x00)
                    return false;
            }

            return true;
        }

        public static string ReadLenString(this BinaryReader self, int size)
        {
            string result = "";

            for (int i = 0; i < size; i++)
            {
                result += (char)self.ReadByte();
            }

            return result;
        }

        public static string ReadLenString(this BinaryReader self)
        {
            int len = self.ReadInt32();

            return self.ReadLenString(len);
        }
    }
}
