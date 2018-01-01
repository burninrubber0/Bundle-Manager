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

        public static BND2Archive ReadBND2Archive(this BinaryReader self, bool console = false)
        {
            BND2Archive result = new BND2Archive();

            if (!self.VerifyMagic(BND2Archive.MAGIC))
                return null;

            result.Console = console;

            result.Version = self.ReadInt32();
            result.Unknown2 = self.ReadInt32();
            result.Unknown3 = self.ReadInt32();
            result.FileCount = self.ReadInt32();
            result.Unknown4 = self.ReadInt32();
            result.DataStart = self.ReadInt32();
            result.ExtraDataStart = self.ReadInt32();
            result.ArchiveSize = self.ReadInt32();
            result.Unknown6 = self.ReadInt32();
            result.Unknown7 = self.ReadInt32();
            result.Unknown8 = self.ReadInt32();

            if (console)
            {
                result.Version = Util.ReverseBytes(result.Version);
                result.Unknown2 = Util.ReverseBytes(result.Unknown2);
                result.Unknown3 = Util.ReverseBytes(result.Unknown3);
                result.FileCount = Util.ReverseBytes(result.FileCount);
                result.Unknown4 = Util.ReverseBytes(result.Unknown4);
                result.DataStart = Util.ReverseBytes(result.DataStart);
                result.ExtraDataStart = Util.ReverseBytes(result.ExtraDataStart);
                result.ArchiveSize = Util.ReverseBytes(result.ArchiveSize);
                result.Unknown6 = Util.ReverseBytes(result.Unknown6);
                result.Unknown7 = Util.ReverseBytes(result.Unknown7);
                result.Unknown8 = Util.ReverseBytes(result.Unknown8);
            }

            //long dataOffset = result.DataStart;

            for (int i = 0; i < result.FileCount; i++)
            {
                BND2Entry entry = new BND2Entry();

                entry.Index = i;

                entry.Console = console;

                entry.ID = self.ReadInt32();
                entry.Checksum = self.ReadInt32();
                entry.Unknown11 = self.ReadInt32();
                entry.Unknown12 = self.ReadInt32();
                entry.Unknown13 = self.ReadInt32();
                entry.Unknown14 = self.ReadInt32();
                entry.Unknown15 = self.ReadInt32();
                entry.FileSize = self.ReadInt32();
                entry.ExtraSize = self.ReadInt64();
                entry.StartOff = self.ReadInt32();
                entry.ExtraStartOff = self.ReadInt64();
                entry.Unknown21 = self.ReadInt32();
                int FileDef = self.ReadInt32();
                entry.Unknown23 = self.ReadInt32();

                if (console)
                {
                    entry.ID = Util.ReverseBytes(entry.ID);
                    entry.Checksum = Util.ReverseBytes(entry.Checksum);
                    entry.Unknown11 = Util.ReverseBytes(entry.Unknown11);
                    entry.Unknown12 = Util.ReverseBytes(entry.Unknown12);
                    entry.Unknown13 = Util.ReverseBytes(entry.Unknown13);
                    entry.Unknown14 = Util.ReverseBytes(entry.Unknown14);
                    entry.Unknown15 = Util.ReverseBytes(entry.Unknown15);
                    entry.FileSize = Util.ReverseBytes(entry.FileSize);
                    entry.ExtraSize = Util.ReverseBytes(entry.ExtraSize);
                    entry.StartOff = Util.ReverseBytes(entry.StartOff);
                    entry.ExtraStartOff = Util.ReverseBytes(entry.ExtraStartOff);
                    entry.Unknown21 = Util.ReverseBytes(entry.Unknown21);
                    FileDef = Util.ReverseBytes(FileDef);
                    entry.Unknown23 = Util.ReverseBytes(entry.Unknown23);
                }

                entry.Type = (EntryType) FileDef;

                // Data
                long offset = self.BaseStream.Position;
                self.BaseStream.Seek(result.DataStart + entry.StartOff, SeekOrigin.Begin);

                byte[] data = self.ReadBytes(entry.FileSize);

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


                entry.CData = data;

                /*entry.Unknown24 = self.ReadInt32();
                List<byte> unk = new List<byte>();
                byte x;
                while ((x = self.ReadByte()) == 0x0)
                    unk.Add(x);
                entry.Unknown25 = unk.ToArray();*/

                //entry.Unknown24 = self.ReadInt32();
                //entry.Unknown25 = self.ReadInt32();

                entry.Data = data.Decompress();
                if (entry.Data == null)
                {
                    entry.DataCompressed = false;
                    entry.Data = data;
                }
                else
                {
                    entry.DataCompressed = true;
                }
                self.BaseStream.Seek(offset, SeekOrigin.Begin);

                // Extra Data
                if (entry.ExtraSize > 0 && result.ExtraDataStart != 0)
                {
                    self.BaseStream.Seek(result.ExtraDataStart + entry.ExtraStartOff, SeekOrigin.Begin);
                    //try
                    //{
                    byte[] extra = self.ReadBytes((int)entry.ExtraSize);
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

                    entry.CExtraData = extra;

                    //entry.Unknown26 = self.ReadBytes(32);
                    /*List<byte> unk2 = new List<byte>();
                    byte x2;
                    while ((x2 = self.ReadByte()) == 0x0)
                        unk2.Add(x2);
                    entry.Unknown27 = unk2.ToArray();*/

                    entry.ExtraData = extra.Decompress();
                    if (entry.ExtraData == null)
                    {
                        entry.ExtraDataCompressed = false;
                        entry.ExtraData = extra;
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
                    entry.ExtraData = null;
                }

                entry.Dirty = false;

                result.Entries.Add(entry);
            }

            return result;
        }

        public static void WriteBND2Archive(this BinaryWriter self, BND2Archive result)
        {
            self.Write(BND2Archive.MAGIC);
            self.Write(result.Version);
            self.Write(result.Unknown2);
            self.Write(result.Unknown3);
            self.Write(result.Entries.Count);
            self.Write(result.Unknown4);

            long dataStartOffset = self.BaseStream.Position;
            self.Write((int)0);
            //self.Write((int)result.Entries.Count * 64 + 48);

            long extraDataStartOffset = self.BaseStream.Position;
            self.Write((int)0);

            long archiveSizeOffset = self.BaseStream.Position;
            self.Write((int)0);

            self.Write(result.Unknown6);
            self.Write(result.Unknown7);
            self.Write(result.Unknown8);

            long[] startOffOffsets = new long[result.Entries.Count];
            long[] extraStartOffOffsets = new long[result.Entries.Count];

            for (int i = 0; i < result.Entries.Count; i++)
            {
                BND2Entry entry = result.Entries[i];

                if (entry.Dirty)
                {
                    byte[] compressedData = entry.Data;
                    byte[] compressedExtraData = entry.ExtraData;
                    if (entry.DataCompressed)
                        compressedData = compressedData.Compress();
                    if (entry.ExtraDataCompressed)
                        compressedExtraData = compressedExtraData.Compress();

                    entry.CData = compressedData;
                    entry.CExtraData = compressedExtraData;
                    entry.FileSize = compressedData.Length;// + 2;
                    //if (entry.DataCompressed)
                    //    entry.FileSize += 2;
                    if (compressedExtraData == null)
                    {
                        entry.ExtraSize = 0;
                    }
                    else
                    {
                        entry.ExtraSize = compressedExtraData.Length;// + 32;
                        //if (entry.ExtraDataCompressed)
                        //    entry.ExtraSize += 2;
                    }
                }

                self.Write(entry.ID);
                self.Write(entry.Checksum);
                self.Write(entry.Unknown11);
                self.Write(entry.Unknown12);
                self.Write(entry.Unknown13);
                self.Write(entry.Unknown14);
                self.Write(entry.Unknown15);
                self.Write(entry.FileSize);
                self.Write(entry.ExtraSize);

                startOffOffsets[i] = self.BaseStream.Position;
                self.Write((int)0);

                extraStartOffOffsets[i] = self.BaseStream.Position;
                self.Write((long)0);
                
                self.Write(entry.Unknown21);
                self.Write((int)entry.Type);
                self.Write(entry.Unknown23);

                //64;

            }


            long currentOffset = self.BaseStream.Position;
            long startData = currentOffset;
            self.Seek((int)dataStartOffset, SeekOrigin.Begin);
            self.Write((int)currentOffset);
            self.Seek((int)currentOffset, SeekOrigin.Begin);

            for (int i = 0; i < result.Entries.Count; i++)
            {
                BND2Entry entry = result.Entries[i];

                /*if (i == 0)
                {
                    entry.StartOff = 0;
                }
                else
                {
                    BND2Entry prevEntry = result.Entries[i - 1];
                    entry.StartOff = prevEntry.StartOff + prevEntry.FileSize + 1;
                }*/

                currentOffset = self.BaseStream.Position;
                self.Seek((int)startOffOffsets[i], SeekOrigin.Begin);
                self.Write(currentOffset - startData);
                self.Seek((int)currentOffset, SeekOrigin.Begin);

                self.Write(entry.CData);
                self.Write(entry.Unknown24);
                int numPadding = 16 - (entry.CData.Length + 4) % 16;
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
                //int numPadding = entry.Data.Length % 16;
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
                BND2Entry entry = result.Entries[i];
                if (entry.CExtraData == null || entry.ExtraData == null)
                    continue;

                currentOffset = self.BaseStream.Position;
                self.Seek((int)extraStartOffOffsets[i], SeekOrigin.Begin);
                self.Write(currentOffset - startExtraData);
                self.Seek((int)currentOffset, SeekOrigin.Begin);

                self.Write(entry.CExtraData);
                self.Write(entry.Unknown25);
                //self.Write(entry.Unknown26);
                //self.Write(entry.Unknown27);
                //self.Write((byte)0);
                int numPadding = 16 - (entry.CExtraData.Length + 4) % 16;
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