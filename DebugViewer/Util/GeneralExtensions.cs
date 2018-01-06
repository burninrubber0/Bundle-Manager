using System.IO;
using System.Text;

namespace DebugViewerLib.Util
{
    public static class GeneralExtensions
    {
        public static bool Matches(this string[] self, string[] other)
        {
            if (self.Length != other.Length)
                return false;

            for (int i = 0; i < self.Length; i++)
            {
                if (self[i] != other[i])
                    return false;
            }

            return true;
        }

        public static string[] Rebuild(this string[] self, int size)
        {
            string[] result = new string[size];
            for (int i = 0; i < result.Length; i++)
            {
                if (i >= self.Length)
                {
                    result[i] = "";
                    continue;
                }
                result[i] = self[i];
            }
            return result;
        }

        public static string BuildString(this string[] self)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < self.Length; i++)
            {
                sb.Append(self[i] + "\r\n");
            }

            return sb.ToString();
        }

        public static string AsArrayIndexString(this int[] self)
        {
            StringBuilder sb = new StringBuilder();
            
            for (int i = 0; i < self.Length; i++)
            {
                sb.Append(self[i].ToString());

                if (i < self.Length - 1)
                    sb.Append(", ");
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

        public static bool EOF(this BinaryReader self)
        {
            return self.BaseStream.Position >= self.BaseStream.Length;
        }

        public static string ReadLenString(this BinaryReader self, int size)
        {
            string result = "";

            for (int i = 0; i < size; i++)
            {
                result += (char) self.ReadByte();
            }

            return result;
        }

        public static string ReadLenString(this BinaryReader self)
        {
            int len = self.ReadInt32();

            return self.ReadLenString(len);
        }

        public static string ReadCString(this BinaryReader self)
        {
            string result = "";

            while (!self.EOF())
            {
                char c = (char) self.ReadByte();
                if (c == '\0')
                    break;
                result += c;
            }

            return result;
        }
    }
}
