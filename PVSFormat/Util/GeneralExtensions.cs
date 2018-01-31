using System.IO;
using System.Text;

namespace PVSFormat.Util
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

        public static bool EOF(this BinaryReader self)
        {
            return self.BaseStream.Position >= self.BaseStream.Length;
        }
    }
}
