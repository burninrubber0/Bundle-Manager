using System;
using System.Buffers.Binary;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Reflection;

namespace BundleUtilities
{
    public class LogWriter
    {
        private string m_exePath = string.Empty;

        public LogWriter(string logMessage)
        {
            LogWrite(logMessage);
        }

        public void LogWrite(string logMessage)
        {
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "log.txt"))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.WriteLine("{0} {1} : {2}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString(), logMessage);
            }
            catch (Exception ex)
            {
            }
        }
    }

    public static class Utilities
    {
        public static ulong CalcLookup8(string text)
        {
            byte[] message = Encoding.ASCII.GetBytes(text);
            var hashValue = Lookup8.Hash(message, (ulong)message.Length, 0xABCDEF0011223344);

            return hashValue;
        }

        public static void WriteEncryptedString(this BinaryWriter self, EncryptedString id, bool xbox = false)
        {
            ulong value = id.Encrypted;
            if (xbox)
                value = BinaryPrimitives.ReverseEndianness(value);
            self.Write(value);
        }

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

        public static void WriteCStr(this BinaryWriter self, string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            for (int i = 0; i < bytes.Length; i++)
            {
                self.Write(bytes[i]);
            }
            self.Write((byte)0);
        }
        
        public static void WriteUniquePadding(this BinaryWriter self, int numberOfPadding)
        {
            for (int i = 0; i < numberOfPadding; i++)
            {
                self.Write((byte)0);
            }

        }

        public static void WriteStringPadding(this BinaryWriter self)
        {
            long currentLength = self.BaseStream.Length;
            if (currentLength % 8 != 0)
            {
                for (int i = 0; i < (8 - currentLength % 8); i++)
                {
                    self.Write((byte)0);
                }
            };
        }

        // Add padding: Has to be divisible by 16, else add padding
        public static void WritePadding(this BinaryWriter self)
        {
            long currentLength = self.BaseStream.Length;
            if (currentLength % 16 != 0)
            {
                for (int i = 0; i < (16 - currentLength % 16); i++)
                {
                    self.Write((byte)0);
                }
            };
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
            value = (T)converter.ConvertFromString(s);
        }
    }
}
