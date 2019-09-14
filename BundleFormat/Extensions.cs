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

		public static void Align(this BinaryWriter self, byte alignment)
		{
			self.BaseStream.Position = alignment * ((self.BaseStream.Position + (alignment - 1)) / alignment);
			self.BaseStream.Position--;
			self.Write((byte)0);

			/*long currentOffset = self.BaseStream.Position;
			for (int i = 0; i < (alignment - (currentOffset % alignment)); i++)
				self.Write((byte)0);*/
		}

        public static void WriteBND2Archive(this BinaryWriter self, BundleArchive result)
        {
            self.Write(BundleArchive.Magic);
            self.Write(result.Version);
            self.Write((int)result.Platform);

            long rstOffset = self.BaseStream.Position;
            self.Write((int)0);

            self.Write(result.Entries.Count);

            long idBlockOffsetPos = self.BaseStream.Position;
            self.Write((int)0);

			long[] fileBlockOffsetsPos = new long[3];

			for (int i = 0; i < fileBlockOffsetsPos.Length; i++)
			{
				fileBlockOffsetsPos[i] = self.BaseStream.Position;
				self.BaseStream.Position += 4;
			}

            self.Write((int)result.Flags);

			self.Align(16);

			long currentOffset = self.BaseStream.Position;
			self.BaseStream.Position = rstOffset;
			self.Write((uint)currentOffset);
			self.BaseStream.Position = currentOffset;
			
			if (result.Flags.HasFlag(Flags.HasResourceStringTable))
			{
				self.WriteCStr(result.ResourceStringTable);

				self.Align(16);
			}

			// ID Block
            currentOffset = self.BaseStream.Position;
            self.Seek((int)idBlockOffsetPos, SeekOrigin.Begin);
            self.Write((int)currentOffset);
            self.Seek((int)currentOffset, SeekOrigin.Begin);

			List<long[]> entryDataPointerPos = new List<long[]>();

			List<byte[][]> compressedBlocks = new List<byte[][]>();

			for (int i = 0; i < result.Entries.Count; i++)
            {
                BundleEntry entry = result.Entries[i];

				compressedBlocks.Add(new byte[3][]);

                self.Write(entry.ID);
                self.Write(entry.References);

				for (int j = 0; j < entry.EntryBlocks.Length; j++)
				{
					EntryBlock entryBlock = entry.EntryBlocks[j];
					uint uncompressedSize = 0;
					if (entryBlock.Data != null)
						uncompressedSize = (uint)entryBlock.Data.Length;
					//self.Write((entryBlock.UncompressedAlignment << 28) | uncompressedSize);
					self.Write((uint)(uncompressedSize | (BitScan.BitScanReverse(entryBlock.UncompressedAlignment) << 28)));
				}

				for (int j = 0; j < entry.EntryBlocks.Length; j++)
				{
					EntryBlock entryBlock = entry.EntryBlocks[j];
					if (entryBlock.Data == null)
						self.Write((uint)0);
					else
					{
						compressedBlocks[i][j] = entryBlock.Data.Compress();
						self.Write(compressedBlocks[i][j].Length);
					}
				}

				entryDataPointerPos.Add(new long[3]);
				for (int j = 0; j < entryDataPointerPos[i].Length; j++)
				{
					entryDataPointerPos[i][j] = self.BaseStream.Position;
					self.BaseStream.Position += 4;
				}
                
                self.Write(entry.DependenciesListOffset);
                self.Write((int)entry.Type);
                self.Write(entry.DependencyCount);

                self.BaseStream.Position += 2; // Padding
            }

			// Data Block
			for (int i = 0; i < 3; i++)
			{
				long blockStart = self.BaseStream.Position;
				self.BaseStream.Position = fileBlockOffsetsPos[i];
				self.Write((uint)blockStart);
				self.BaseStream.Position = blockStart;

				for (int j = 0; j < result.Entries.Count; j++)
				{
					BundleEntry entry = result.Entries[j];
					EntryBlock entryBlock = entry.EntryBlocks[i];
					bool compressed = result.Flags.HasFlag(Flags.Compressed);
					uint size = compressed ? (compressedBlocks[j][i] == null ? 0 : (uint)compressedBlocks[j][i].Length) : (uint)entryBlock.Data.Length;

					if (size > 0)
					{
						long pos = self.BaseStream.Position;
						self.BaseStream.Position = entryDataPointerPos[j][i];
						self.Write((uint)(pos - blockStart));
						self.BaseStream.Position = pos;

						if (compressed)
							self.Write(compressedBlocks[j][i]);
						else
							self.Write(entryBlock.Data);

						self.Align((i != 0 && j != result.Entries.Count - 1) ? (byte)0x80 : (byte)16);
					}
				}

				if (i != 2)
					self.Align(0x80);
			}
        }
    }
}