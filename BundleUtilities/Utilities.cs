using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

            bool readNull = false;
            for (int i = 0; i < size; i++)
            {
                char c = (char)self.ReadByte();
                if (c == '\0')
                    readNull = true;
                if (!readNull)
                    result += c;
            }

            return result;
        }

        public static string ReadLenString(this BinaryReader self)
        {
            int len = self.ReadInt32();

            return self.ReadLenString(len);
        }

        public static string ReadCStringPtr(this BinaryReader self)
        {
            uint ptr = self.ReadUInt32();
            long pos = self.BaseStream.Position;
            self.BaseStream.Position = ptr;
            string result = self.ReadCStr();
            self.BaseStream.Position = pos;

            return result;
        }

        public static void WriteCStr(this BinaryWriter self, string value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                self.Write((byte)value[i]);
            }
            self.Write((byte) 0);
        }

        /// <summary>
        /// Converts the specified text to an object.
        /// </summary>
        /// <typeparam name="T">The type of the output object</typeparam>
        /// <param name="s">The text representation of the object to convert.</param>
        /// <param name="forceHex">Parse the input string as a hexadecimal number even if the input string is not prefixed with "0x"</param>
        /// <param name="value">An output System.Object that represents the converted text.</param>
        /// <exception cref="NotSupportedException">The string cannot be converted into the appropriate object.</exception>
        public static void Parse<T>(string s, bool forceHex, out T value)
        {
            if (forceHex && !s.StartsWith("0x"))
                s = "0x" + s;
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            value = (T) converter.ConvertFromString(s);
        }
    }
}
