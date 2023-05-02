using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using BundleUtilities;
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
            zlibStream.Write(self, 0, self.Length);
            return uncompressedData;
        }

        /*public static byte[] Decompress(this byte[] self)
        {
            MemoryStream ms = new MemoryStream(self);
            int cmagic1 = ms.ReadByte();
            int cmagic2 = ms.ReadByte();

            if (cmagic1 != 0x78 || cmagic2 != 0xDA)
                return null;

            DeflateStream ds = new DeflateStream(ms, CompressionMode.Decompress);

            MemoryStream ms2 = new MemoryStream();
            ds.CopyTo(ms2);
            ds.Close();

            byte[] result = ms2.ToArray();
            ms2.Close();
            return result;
        }*/

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
            long originalPosition = self.BaseStream.Position;
            self.BaseStream.Position = alignment * ((self.BaseStream.Position + (alignment - 1)) / alignment);
            if (self.BaseStream.Position != originalPosition)
            {
                self.BaseStream.Position--;
                self.Write((byte)0);
            }

            /*long currentOffset = self.BaseStream.Position;
            for (int i = 0; i < (alignment - (currentOffset % alignment)); i++)
                self.Write((byte)0);*/
        }
    }
}
