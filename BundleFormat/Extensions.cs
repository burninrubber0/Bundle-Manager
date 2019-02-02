using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManagedZLib;
using System.Runtime.InteropServices;
using BundleUtilities;

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
            return ZLib.Compress(self, ZLib.CompressionLevels.BEST_COMPRESSION);
        }
        
        public static byte[] Decompress(this byte[] self, int uncompressedSize)
        {
            return ZLib.Uncompress(self, uncompressedSize);
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

        public static void WriteBND2Archive(this BinaryWriter self, BundleArchive result)
        {
            self.Write(BundleArchive.Magic);
            self.Write(result.Version);
            self.Write((int)result.Platform);

            long rstOffset = self.BaseStream.Position;
            self.Write((int)0);

            self.Write(result.Entries.Count);

            long metadataStartOffset = self.BaseStream.Position;
            self.Write((int)0);

            long dataStartOffset = self.BaseStream.Position;
            self.Write((int)0);

            long extraDataStartOffset = self.BaseStream.Position;
            self.Write((int)0);

            long archiveSizeOffset = self.BaseStream.Position;
            self.Write((int)0);

            self.Write((int)result.Flags);

            self.BaseStream.Position += 8;


            long currentOffset = self.BaseStream.Position;
            self.Seek((int)rstOffset, SeekOrigin.Begin);
            self.Write((int)currentOffset);
            self.Seek((int)currentOffset, SeekOrigin.Begin);
            self.WriteCStr(result.ResourceStringTable);
            currentOffset = self.BaseStream.Position;
            for (int i = 0; i < (16 - (currentOffset % 16)); i++)
                self.Write((byte) 0); // padding


            currentOffset = self.BaseStream.Position;
            self.Seek((int)metadataStartOffset, SeekOrigin.Begin);
            self.Write((int)currentOffset);
            self.Seek((int)currentOffset, SeekOrigin.Begin);

            long[] startOffOffsets = new long[result.Entries.Count];
            long[] extraStartOffOffsets = new long[result.Entries.Count];

            for (int i = 0; i < result.Entries.Count; i++)
            {
                BundleEntry entry = result.Entries[i];

                if (entry.Dirty)
                {
                    byte[] compressedData = entry.Header;
                    byte[] compressedExtraData = entry.Body;
                    if (compressedData != null)
                        entry.UncompressedHeaderSize = compressedData.Length;
                    if (compressedExtraData != null)
                        entry.UncompressedBodySize = compressedExtraData.Length;
                    if (entry.DataCompressed)
                        compressedData = compressedData.Compress();
                    if (entry.ExtraDataCompressed)
                        compressedExtraData = compressedExtraData.Compress();

                    entry.CompressedHeader = compressedData;
                    entry.CompressedBody = compressedExtraData;
                    entry.HeaderSize = compressedData.Length;
                    if (compressedExtraData == null)
                    {
                        entry.BodySize = 0;
                    }
                    else
                    {
                        entry.BodySize = compressedExtraData.Length;
                    }
                }

                self.Write(entry.ID);
                self.Write(entry.References);

                int uncompressedHeaderSize = entry.UncompressedHeaderSize;
                int uncompressedHeaderSizeCache = entry.UncompressedHeaderSizeCache;
                int uncompressedHeaderSizeAndCache = (uncompressedHeaderSizeCache << 28) | uncompressedHeaderSize;

                self.Write(uncompressedHeaderSizeAndCache);

                int uncompressedBodySize = entry.UncompressedBodySize;
                int uncompressedBodySizeCache = entry.UncompressedBodySizeCache;
                long uncompressedBodySizeAndCache = (uncompressedBodySizeCache << 28) | uncompressedBodySize;
                self.Write(uncompressedBodySizeAndCache);
                self.Write(entry.HeaderSize);
                self.Write(entry.BodySize);

                startOffOffsets[i] = self.BaseStream.Position;
                self.Write((int)0);

                extraStartOffOffsets[i] = self.BaseStream.Position;
                self.Write((long)0);
                
                self.Write(entry.DependenciesListOffset);
                self.Write((int)entry.Type);
                self.Write(entry.DependencyCount);
                self.BaseStream.Position += 2;
            }


            currentOffset = self.BaseStream.Position;
            long startData = currentOffset;
            self.Seek((int)dataStartOffset, SeekOrigin.Begin);
            self.Write((int)currentOffset);
            self.Seek((int)currentOffset, SeekOrigin.Begin);

            for (int i = 0; i < result.Entries.Count; i++)
            {
                BundleEntry entry = result.Entries[i];

                currentOffset = self.BaseStream.Position;
                self.Seek((int)startOffOffsets[i], SeekOrigin.Begin);
                self.Write(currentOffset - startData);
                self.Seek((int)currentOffset, SeekOrigin.Begin);

                self.Write(entry.CompressedHeader);
                self.Write(entry.Unknown24);
                int numPadding = 16 - (entry.CompressedHeader.Length + 4) % 16;
                for (int j = 0; j < numPadding; j++)
                    self.Write((byte)0);
            }

            currentOffset = self.BaseStream.Position;
            int padding = 16 - (int)currentOffset % 16;
            for (int j = 0; j < padding; j++)
                self.Write((byte)0);

            for (int j = 0; j < 16 * 4; j++)
                self.Write((byte)0);

            currentOffset = self.BaseStream.Position;
            long startExtraData = currentOffset;
            self.Seek((int)extraDataStartOffset, SeekOrigin.Begin);
            self.Write((int)currentOffset);
            self.Seek((int)currentOffset, SeekOrigin.Begin);


            for (int i = 0; i < result.Entries.Count; i++)
            {
                BundleEntry entry = result.Entries[i];
                if (entry.CompressedBody == null || entry.Body == null)
                    continue;

                currentOffset = self.BaseStream.Position;
                self.Seek((int)extraStartOffOffsets[i], SeekOrigin.Begin);
                self.Write(currentOffset - startExtraData);
                self.Seek((int)currentOffset, SeekOrigin.Begin);

                self.Write(entry.CompressedBody);
                self.Write(entry.Unknown25);

                int numPadding = 16 - (entry.CompressedBody.Length + 4) % 16;
                for (int j = 0; j < numPadding; j++)
                    self.Write((byte)0);

                for (int j = 0; j < 16 * 4; j++)
                    self.Write((byte)0);
            }

            currentOffset = self.BaseStream.Position;
            self.Seek((int)archiveSizeOffset, SeekOrigin.Begin);
            self.Write((int)currentOffset);
            self.Seek((int)currentOffset, SeekOrigin.Begin);
        }
    }
}