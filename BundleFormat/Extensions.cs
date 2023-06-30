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
                if (self[self.Length - 1] == 0
                    && e.Message == "Bad state (incorrect data check)") // Likely a bugged resource
                {
                    uncompressedData = GetDataFromBadAlignedResource(self, uncompressedSize);
                    if (uncompressedData != null)
                        return uncompressedData;
                }
                MessageBox.Show(e.ToString(), e.Source, MessageBoxButtons.OK);
                return null;
            }
            return uncompressedData;
        }

        // Validate resources from BM versions <0.3.0 where alignment corrupted the checksum
        public static byte[] GetDataFromBadAlignedResource(byte[] original, int uncompressedSize)
        {
            // For some reason, the data validates when length is 1 less than it should be.
            // Use this to get the correct uncompressed data.
            byte[] uncompressedData = new byte[uncompressedSize];
            ZlibStream zlibStream = new ZlibStream(new MemoryStream(uncompressedData), CompressionMode.Decompress);
            byte[] trimmed = new ArraySegment<byte>(original, 0, original.Length - 1).ToArray();
            try
            {
                zlibStream.Write(trimmed, 0, trimmed.Length);
            }
            catch (Exception)
            {
                return null;
            }

            byte[] compressed = Compress(uncompressedData);

            // Test first three checksum bytes
            if (original[original.Length - 4] == compressed[compressed.Length - 4]
                && original[original.Length - 3] == compressed[compressed.Length - 3]
                && original[original.Length - 2] == compressed[compressed.Length - 2])
            {
                // Testing of data not necessary, likelihood of 24 bits matching without data matching is negligible
                return uncompressedData;
            }

            return null;
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
