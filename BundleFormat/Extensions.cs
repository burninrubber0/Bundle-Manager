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
                //if ((i % 16 == 0 && i != 0 && i != 16) || i == 15)
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
            /*byte[] outputArray = new byte[self.Length*2];
            GCHandle output = GCHandle.Alloc(outputArray, GCHandleType.Pinned);
            GCHandle input = GCHandle.Alloc(self, GCHandleType.Pinned);

            uint outLen = ZLib.Compress2(input.AddrOfPinnedObject(), output.AddrOfPinnedObject(), (uint)self.Length, ZLibCompressionLevels.BEST_COMPRESSION);

            byte[] result = new byte[outLen];
            for (uint i = 0; i < result.Length; i++)
            {
                //if (i >= outputArray.Length)
                //    break;
                result[i] = outputArray[i];
            }

            input.Free();
            output.Free();

            return result;*/

            /*MemoryStream msin = new MemoryStream(self);

            // Magic
            Stream outcompress = File.Open("temp.bin", FileMode.Create);
            DeflateStream ds = new DeflateStream(outcompress, CompressionMode.Compress);

            msin.CopyTo(ds);
            
            ds.Flush();
            msin.Close();
            
            ds.Close();
            outcompress.Close();

            MemoryStream msout = new MemoryStream();
            msout.WriteByte(0x78);
            msout.WriteByte(0xDA);
            Stream incompress = File.Open("temp.bin", FileMode.Open);
            incompress.CopyTo(msout);
            incompress.Close();

            msout.Flush();
            byte[] ret = msout.ToArray();
            msout.Close();

            File.Delete("temp.bin");

            return ret;*/
        }
        
        public static byte[] Decompress(this byte[] self)
        {
            //return ZLib.Uncompress(self, ?);
            
            MemoryStream ms = new MemoryStream(self);
            int cmagic1 = ms.ReadByte();
            int cmagic2 = ms.ReadByte();

            if (cmagic1 != 0x78 || cmagic2 != 0xDA)
                return null;//self;

            DeflateStream ds = new DeflateStream(ms, CompressionMode.Decompress);

            MemoryStream ms2 = new MemoryStream();
            ds.CopyTo(ms2);
            ds.Close();

            byte[] result = ms2.ToArray();
            ms2.Close();
            return result;
        }

        // old thing

        /*byte[] buffer = new byte[1024];
        int numRead;
        while ((numRead = msin.Read(buffer, 0, buffer.Length)) != 0)
        {
            ds.Write(buffer, 0, numRead);
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

        public static BundleArchive ReadBND2Archive(this BinaryReader self, bool console = false)
        {
            BundleArchive result = new BundleArchive();

            if (!self.VerifyMagic(BundleArchive.MAGIC))
                return null;

            result.Console = console;

            result.Version = self.ReadInt32();
            result.Unknown2 = self.ReadInt32();
            result.Unknown3 = self.ReadInt32();
            result.FileCount = self.ReadInt32();
            result.MetadataStart = self.ReadInt32();
            result.HeadStart = self.ReadInt32();
            result.BodyStart = self.ReadInt32();
            result.ArchiveSize = self.ReadInt32();
            int CompressionType = self.ReadInt32();
            result.Unknown7 = self.ReadInt32();
            result.Unknown8 = self.ReadInt32();

            if (console)
            {
                result.Version = Util.ReverseBytes(result.Version);
                result.Unknown2 = Util.ReverseBytes(result.Unknown2);
                result.Unknown3 = Util.ReverseBytes(result.Unknown3);
                result.FileCount = Util.ReverseBytes(result.FileCount);
                result.MetadataStart = Util.ReverseBytes(result.MetadataStart);
                result.HeadStart = Util.ReverseBytes(result.HeadStart);
                result.BodyStart = Util.ReverseBytes(result.BodyStart);
                result.ArchiveSize = Util.ReverseBytes(result.ArchiveSize);
                CompressionType = Util.ReverseBytes(CompressionType);
                result.Unknown7 = Util.ReverseBytes(result.Unknown7);
                result.Unknown8 = Util.ReverseBytes(result.Unknown8);
            }

            result.CompressionType = (CompressionType) CompressionType;

            //long dataOffset = result.HeadStart;

            for (int i = 0; i < result.FileCount; i++)
            {
                BundleEntry entry = new BundleEntry(result);

                entry.Index = i;

                entry.Console = console;

                entry.ID = self.ReadUInt64();
                entry.References = self.ReadInt32();
                entry.Unknown12 = self.ReadInt32();
                int uncompressedHeaderSize = self.ReadInt32();
                int uncompressedBodySize = self.ReadInt32();
                entry.Unknown15 = self.ReadInt32();
                entry.HeaderSize = self.ReadInt32();
                entry.BodySize = self.ReadInt64();
                entry.HeadOffset = self.ReadInt32();
                entry.BodyOffset = self.ReadInt64();
                entry.DependenciesListOffset = self.ReadInt32();
                int fileType = self.ReadInt32();
                entry.DependencyCount = self.ReadInt16();
                entry.Unknown = self.ReadInt16();

                if (console)
                {
                    entry.ID = Util.ReverseBytes(entry.ID);
                    entry.References = Util.ReverseBytes(entry.References);
                    entry.Unknown12 = Util.ReverseBytes(entry.Unknown12);
                    uncompressedHeaderSize = Util.ReverseBytes(uncompressedHeaderSize);
                    uncompressedBodySize = Util.ReverseBytes(uncompressedBodySize);
                    entry.Unknown15 = Util.ReverseBytes(entry.Unknown15);
                    entry.HeaderSize = Util.ReverseBytes(entry.HeaderSize);
                    entry.BodySize = Util.ReverseBytes(entry.BodySize);
                    entry.HeadOffset = Util.ReverseBytes(entry.HeadOffset);
                    entry.BodyOffset = Util.ReverseBytes(entry.BodyOffset);
                    entry.DependenciesListOffset = Util.ReverseBytes(entry.DependenciesListOffset);
                    fileType = Util.ReverseBytes(fileType);
                    entry.DependencyCount = Util.ReverseBytes(entry.DependencyCount);
                    entry.Unknown = Util.ReverseBytes(entry.Unknown);
                }

                entry.UncompressedHeaderSize = uncompressedHeaderSize & 0x0FFFFFFF;
                entry.UncompressedHeaderSizeCache = uncompressedHeaderSize >> 16;
                entry.UncompressedBodySize = uncompressedBodySize & 0x0FFFFFFF;
                entry.UncompressedBodySizeCache = uncompressedBodySize >> 16;

                entry.Type = (EntryType) fileType;

                // Header
                long offset = self.BaseStream.Position;
                self.BaseStream.Seek(result.HeadStart + entry.HeadOffset, SeekOrigin.Begin);

                byte[] data = self.ReadBytes(entry.HeaderSize);

                try
                {
                    //if (i < result.FileCount - 1)
                    //{
                    entry.Unknown24 = self.ReadInt32();

                    if (console)
                    {
                        entry.Unknown24 = (int)Util.ReverseBytes((uint)entry.Unknown24);
                    }
                    //}
                }
                catch (EndOfStreamException) { }


                entry.CompressedHeader = data;

                /*entry.Unknown24 = self.ReadInt32();
                List<byte> unk = new List<byte>();
                byte x;
                while ((x = self.ReadByte()) == 0x0)
                    unk.Add(x);
                entry.Unknown25 = unk.ToArray();*/

                //entry.Unknown24 = self.ReadInt32();
                //entry.Unknown25 = self.ReadInt32();

                entry.Header = data.Decompress();
                if (entry.Header == null)
                {
                    entry.DataCompressed = false;
                    entry.Header = data;
                }
                else
                {
                    entry.DataCompressed = true;
                }
                self.BaseStream.Seek(offset, SeekOrigin.Begin);

                // Extra Header
                if (entry.BodySize > 0 && result.BodyStart != 0)
                {
                    self.BaseStream.Seek(result.BodyStart + entry.BodyOffset, SeekOrigin.Begin);
                    //try
                    //{
                    byte[] extra = self.ReadBytes((int)entry.BodySize);
                    try
                    {
                        //if (i < result.FileCount - 1)
                        //{
                        entry.Unknown25 = self.ReadInt32();

                        if (console)
                        {
                            entry.Unknown25 = (int)Util.ReverseBytes((uint)entry.Unknown25);
                        }
                        //}
                    }
                    catch (EndOfStreamException) { }

                    entry.CompressedBody = extra;

                    //entry.Unknown26 = self.ReadBytes(32);
                    /*List<byte> unk2 = new List<byte>();
                    byte x2;
                    while ((x2 = self.ReadByte()) == 0x0)
                        unk2.Add(x2);
                    entry.Unknown27 = unk2.ToArray();*/

                    entry.Body = extra.Decompress();
                    if (entry.Body == null)
                    {
                        entry.ExtraDataCompressed = false;
                        entry.Body = extra;
                    }
                    else
                    {
                        entry.ExtraDataCompressed = true;
                    }
                    //}
                    //catch (EndOfStreamException) { }
                    self.BaseStream.Seek(offset, SeekOrigin.Begin);
                }
                else
                {
                    entry.Body = null;
                }

                entry.Dirty = false;

                result.Entries.Add(entry);
            }

            return result;
        }

        public static void WriteBND2Archive(this BinaryWriter self, BundleArchive result)
        {
            self.Write(BundleArchive.MAGIC);
            self.Write(result.Version);
            self.Write(result.Unknown2);
            self.Write(result.Unknown3);
            self.Write(result.Entries.Count);
            self.Write(result.MetadataStart);

            long dataStartOffset = self.BaseStream.Position;
            self.Write((int)0);
            //self.Write((int)result.Entries.Count * 64 + 48);

            long extraDataStartOffset = self.BaseStream.Position;
            self.Write((int)0);

            long archiveSizeOffset = self.BaseStream.Position;
            self.Write((int)0);

            self.Write((int)result.CompressionType);
            self.Write(result.Unknown7);
            self.Write(result.Unknown8);

            long[] startOffOffsets = new long[result.Entries.Count];
            long[] extraStartOffOffsets = new long[result.Entries.Count];

            for (int i = 0; i < result.Entries.Count; i++)
            {
                BundleEntry entry = result.Entries[i];

                if (entry.Dirty)
                {
                    byte[] compressedData = entry.Header;
                    byte[] compressedExtraData = entry.Body;
                    if (entry.DataCompressed)
                        compressedData = compressedData.Compress();
                    if (entry.ExtraDataCompressed)
                        compressedExtraData = compressedExtraData.Compress();

                    entry.CompressedHeader = compressedData;
                    entry.CompressedBody = compressedExtraData;
                    entry.HeaderSize = compressedData.Length;// + 2;
                    //if (entry.DataCompressed)
                    //    entry.HeaderSize += 2;
                    if (compressedExtraData == null)
                    {
                        entry.BodySize = 0;
                    }
                    else
                    {
                        entry.BodySize = compressedExtraData.Length;// + 32;
                        //if (entry.ExtraDataCompressed)
                        //    entry.BodySize += 2;
                    }
                }

                self.Write(entry.ID);
                self.Write(entry.References);
                self.Write(entry.Unknown12);

                int uncompressedHeaderSize = entry.UncompressedHeaderSize;
                int uncompressedHeaderSizeCache = entry.UncompressedHeaderSizeCache;
                int uncompressedHeaderSizeAndCache = (uncompressedHeaderSizeCache << 16) | uncompressedHeaderSize;

                self.Write(uncompressedHeaderSizeAndCache);

                int uncompressedBodySize = entry.UncompressedBodySize;
                int uncompressedBodySizeCache = entry.UncompressedBodySizeCache;
                int uncompressedBodySizeAndCache = (uncompressedBodySizeCache << 16) | uncompressedBodySize;
                self.Write(uncompressedBodySizeAndCache);
                self.Write(entry.Unknown15);
                self.Write(entry.HeaderSize);
                self.Write(entry.BodySize);

                startOffOffsets[i] = self.BaseStream.Position;
                self.Write((int)0);

                extraStartOffOffsets[i] = self.BaseStream.Position;
                self.Write((long)0);
                
                self.Write(entry.DependenciesListOffset);
                self.Write((int)entry.Type);
                self.Write(entry.DependencyCount);
                self.Write(entry.Unknown);

                //64;

            }


            long currentOffset = self.BaseStream.Position;
            long startData = currentOffset;
            self.Seek((int)dataStartOffset, SeekOrigin.Begin);
            self.Write((int)currentOffset);
            self.Seek((int)currentOffset, SeekOrigin.Begin);

            for (int i = 0; i < result.Entries.Count; i++)
            {
                BundleEntry entry = result.Entries[i];

                /*if (i == 0)
                {
                    entry.HeadOffset = 0;
                }
                else
                {
                    BundleEntry prevEntry = result.Entries[i - 1];
                    entry.HeadOffset = prevEntry.HeadOffset + prevEntry.HeaderSize + 1;
                }*/

                currentOffset = self.BaseStream.Position;
                self.Seek((int)startOffOffsets[i], SeekOrigin.Begin);
                self.Write(currentOffset - startData);
                self.Seek((int)currentOffset, SeekOrigin.Begin);

                self.Write(entry.CompressedHeader);
                self.Write(entry.Unknown24);
                int numPadding = 16 - (entry.CompressedHeader.Length + 4) % 16;
                for (int j = 0; j < numPadding; j++)
                    self.Write((byte)0);
                //self.Write(entry.Unknown24);
                //self.Write(entry.Unknown25);
                //self.Write((byte)0);
                //self.Write((byte)0);
                //self.Write((byte)0);
                //self.Write(entry.Unknown24);
                //self.Write(entry.Unknown25);
                //self.Write((byte)0);
                //int numPadding = entry.Header.Length % 16;
                //for (int j = 0; j < numPadding; j++)
                //    self.Write((byte)0);
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
                //self.Write(entry.Unknown26);
                //self.Write(entry.Unknown27);
                //self.Write((byte)0);
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