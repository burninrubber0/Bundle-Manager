using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Ionic.Zlib;

namespace BundleFormat
{
    public static class Extensions
    {
        public static string AsString(this byte[] self)
        {
            if (self == null)
                return "";
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < self.Length; i++)
            {
                sb.Append(self[i].ToString("X2"));
                if ((i + 1) % 16 == 0)
                    sb.Append("\r\n");
                else
                    sb.Append(" ");
            }

            return sb.ToString();
        }

        public static string MakePreview(this byte[] self, int start, int end)
        {
            byte[] subArray = new byte[end - start];
            int index = 0;
            for (int i = start; i < start + end; i++)
            {
                subArray[index++] = self[i];
            }
            return subArray.AsString();
        }

        public static byte[] Compress(this byte[] self)
        {
            byte[] compressedData = new byte[self.Length]; // Size not known yet, use uncompressed size as upper bound
            ZlibStream zlibStream = new ZlibStream(new MemoryStream(self), CompressionMode.Compress, CompressionLevel.BestCompression);
            zlibStream.Read(compressedData, 0, self.Length);
            return new ArraySegment<byte>(compressedData, 0, (int)zlibStream.TotalOut).ToArray(); // Size known, return correctly sized segment
        }
        
        public static byte[] Decompress(this byte[] self, int uncompressedSize)
        {
            byte[] uncompressedData = new byte[uncompressedSize];
            ZlibStream zlibStream = new ZlibStream(new MemoryStream(uncompressedData), CompressionMode.Decompress);
            try
            {
                zlibStream.Write(self, 0, self.Length);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), e.Source, MessageBoxButtons.OK);
                return null;
            }
            return uncompressedData;
        }

        public static bool Matches(this byte[] self, byte[] other)
        {
            if (self == null || other == null)
                return false;
            if (self.Length != other.Length)
                return false;

            for (int i = 0; i < self.Length; i++)
            {
                if (self[i] != other[i])
                    return false;
            }

            return true;
        }

        public static byte Peek(this BinaryReader self)
        {
            byte b = self.ReadByte();
            self.BaseStream.Seek(-1, SeekOrigin.Current);
            return b;
        }

        public static bool VerifyMagic(this BinaryReader self, byte[] magic)
        {
            byte[] readMagic = self.ReadBytes(magic.Length);
            if (readMagic.Matches(magic))
                return true;
            return false;
        }

        public static void Align(this BinaryWriter self, byte alignment)
        {
            if (self.BaseStream.Position % alignment == 0)
                return;
            self.BaseStream.Position = alignment * ((self.BaseStream.Position + (alignment - 1)) / alignment);
            self.BaseStream.Position--;
            self.Write((byte)0);
        }
    }
}
