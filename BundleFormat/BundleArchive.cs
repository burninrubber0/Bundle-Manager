using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using BundleUtilities;

namespace BundleFormat
{
    [Flags]
    public enum Flags
    {
        Compressed = 1,
        UnusedFlag1 = 2, // Always set
        UnusedFlag2 = 4, // Always set
        HasResourceStringTable = 8
    }

    public enum BundlePlatform
    {
        PC = 1,
        X360 = 2,// << 24, // Big Endian
        PS3 = 3// << 24 // Big Endian
    }

    public class BundleArchive
    {
        public static readonly byte[] Magic = new byte[] { 0x62, 0x6E, 0x64, 0x32 };

        public string Path;

        public int Version;
        public BundlePlatform Platform;
        public int RSTOffset; // Normally 30
        public int EntryCount;
        public int IDBlockOffset;
        //public int HeadStart;
        //public int BodyStart;
        //public int ArchiveSize;
        public Flags Flags;
        
        public bool Console => Platform == BundlePlatform.X360 || Platform == BundlePlatform.PS3;

        public string ResourceStringTable;
        public List<BundleEntry> Entries;

        private bool _dirty;
        public bool Dirty
        {
            get
            {
                if (_dirty)
                    return true;
                foreach (BundleEntry entry in Entries)
                {
                    if (entry.Dirty)
                        return true;
                }
                return false;
            }
            set
            {
                _dirty = value;
                if (!_dirty)
                {
                    foreach(BundleEntry entry in Entries)
                    {
                        entry.Dirty = false;
                    }
                }
            }
        }

        public BundleArchive()
        {
            this.Entries = new List<BundleEntry>();
        }

        public BundleEntry GetEntryByID(ulong id)
        {
            for (int i = 0; i < Entries.Count; i++)
            {
                BundleEntry entry = Entries[i];
                if (entry.ID == id)
                    return entry;
            }
            return null;
        }

        public static BundleArchive Read(string path)
        {
            using (Stream s = File.OpenRead(path))
            {
                try
                {
                    BinaryReader2 br = new BinaryReader2(s);

                    BundleArchive result = Read(br);
                    if (result == null)
                    {
                        br.Close();

                        return null;
                    }

                    result.Path = path;

                    br.Close();

                    return result;
                }
                catch (IOException ex)
                {
                    Debug.WriteLine(ex.Message + "\n" + ex.StackTrace);
                    return null;
                } catch (ReadFailedError ex)
				{
					Debug.WriteLine(ex.Message + "\n" + ex.StackTrace);
					return null;
				}
            }
        }

        public static BundleArchive Read(BinaryReader2 br)
        {
            BundleArchive result = new BundleArchive();

            if (!br.VerifyMagic(BundleArchive.Magic))
                return null;

            br.BaseStream.Position += 4;

            int platform = br.ReadInt32();
            if (platform != 1)
                platform = Util.ReverseBytes(platform);
            result.Platform = (BundlePlatform) platform;
            br.BigEndian = result.Console;

            br.BaseStream.Position -= 8;
            result.Version = br.ReadInt32();
			if (result.Version != 2)
			{
				throw new ReadFailedError("Unsupported Bundle Version: " + result.Version);
			}
            br.BaseStream.Position += 4;

            result.RSTOffset = br.ReadInt32();
            result.EntryCount = br.ReadInt32();
            result.IDBlockOffset = br.ReadInt32();
			uint[] fileBlockOffsets = new uint[3];
            fileBlockOffsets[0] = br.ReadUInt32();
			fileBlockOffsets[1] = br.ReadUInt32();
			fileBlockOffsets[2] = br.ReadUInt32();
            result.Flags = (Flags)br.ReadInt32();

			// 8 Bytes Padding

            br.BaseStream.Position = result.IDBlockOffset;

            for (int i = 0; i < result.EntryCount; i++)
            {
                BundleEntry entry = new BundleEntry(result);

                entry.Index = i;

                entry.Platform = result.Platform;

                entry.ID = br.ReadUInt64();
                entry.References = br.ReadUInt64();

				entry.EntryBlocks = new EntryBlock[3];
				for (int j = 0; j < entry.EntryBlocks.Length; j++)
				{
					entry.EntryBlocks[j] = new EntryBlock();
				}

				uint[] blockUncompressedSizes = new uint[3];

				for (int j = 0; j < entry.EntryBlocks.Length; j++)
				{
					uint uncompressedSize = br.ReadUInt32();
					blockUncompressedSizes[j] = uncompressedSize & ~(0xFU << 28);
					entry.EntryBlocks[j].UncompressedAlignment = (uint)(1 << ((int)uncompressedSize >> 28));
				}

				uint[] blockCompressedSizes = new uint[3];

				for (int j = 0; j < entry.EntryBlocks.Length; j++)
				{
					blockCompressedSizes[j] = br.ReadUInt32();
				}

				for (int j = 0; j < entry.EntryBlocks.Length; j++)
				{
					uint blockOffset = br.ReadUInt32();
					long lastAddr = br.BaseStream.Position;
					br.BaseStream.Position = blockOffset + fileBlockOffsets[j];

					EntryBlock block = entry.EntryBlocks[j];

					bool compressed = result.Flags.HasFlag(Flags.Compressed);

					uint readSize = compressed ? blockCompressedSizes[j] : blockUncompressedSizes[j];
					if (readSize == 0)
					{
						block.Data = null;
						br.BaseStream.Position = lastAddr;
						continue;
					}

					block.Data = br.ReadBytes((int)readSize);
					if (compressed)
						block.Data = block.Data.Decompress((int)blockUncompressedSizes[j]);
					br.BaseStream.Position = lastAddr;
				}

				entry.DependenciesListOffset = br.ReadInt32();
				entry.Type = (EntryType)br.ReadInt32();
				entry.DependencyCount = br.ReadInt16();

                br.BaseStream.Position += 2; // Padding

                entry.Dirty = false;

                result.Entries.Add(entry);
            }

			if (result.Flags.HasFlag(Flags.HasResourceStringTable))
			{
				br.BaseStream.Position = result.RSTOffset;

				// TODO: Store only the debug info and not the full string
				result.ResourceStringTable = br.ReadCStr();

				XmlDocument doc = new XmlDocument();
				doc.LoadXml(result.ResourceStringTable);

				XmlElement root = doc["ResourceStringTable"];
				XmlNodeList resources = root.GetElementsByTagName("Resource");

				foreach (XmlElement ele in resources)
				{
					if (!ulong.TryParse(ele.Attributes["id"].Value, NumberStyles.AllowHexSpecifier, CultureInfo.CurrentCulture, out ulong resourceID))
						continue;

					foreach (var entry in result.Entries)
					{
						if (entry.ID != resourceID)
							continue;

						entry.DebugInfo.Name = ele.Attributes["name"].Value;
						entry.DebugInfo.TypeName = ele.Attributes["type"].Value;
						break;
					}
				}
			}

			return result;
        }

		public void Write(string path)
		{
			//if (Console)
			//    ConvertToPC();
			Stream s = File.Open(path, FileMode.Create);
			BinaryWriter bw = new BinaryWriter(s);

			Write(bw);

			bw.Flush();
			bw.Close();
		}

		public void Write(BinaryWriter bw)
		{
			bw.Write(BundleArchive.Magic);
			bw.Write(this.Version);
			bw.Write((int)this.Platform);

			long rstOffset = bw.BaseStream.Position;
			bw.Write((int)0);

			bw.Write(this.Entries.Count);

			long idBlockOffsetPos = bw.BaseStream.Position;
			bw.Write((int)0);

			long[] fileBlockOffsetsPos = new long[3];

			for (int i = 0; i < fileBlockOffsetsPos.Length; i++)
			{
				fileBlockOffsetsPos[i] = bw.BaseStream.Position;
				bw.BaseStream.Position += 4;
			}

			bw.Write((int)this.Flags);

			bw.Align(16);

			long currentOffset = bw.BaseStream.Position;
			bw.BaseStream.Position = rstOffset;
			bw.Write((uint)currentOffset);
			bw.BaseStream.Position = currentOffset;

			if (this.Flags.HasFlag(Flags.HasResourceStringTable))
			{
				// TODO: Rebuild RST from DebugInfo
				bw.WriteCStr(this.ResourceStringTable);

				bw.Align(16);
			}

			// ID Block
			currentOffset = bw.BaseStream.Position;
			bw.Seek((int)idBlockOffsetPos, SeekOrigin.Begin);
			bw.Write((int)currentOffset);
			bw.Seek((int)currentOffset, SeekOrigin.Begin);

			List<long[]> entryDataPointerPos = new List<long[]>();

			List<byte[][]> compressedBlocks = new List<byte[][]>();

			for (int i = 0; i < this.Entries.Count; i++)
			{
				BundleEntry entry = this.Entries[i];

				compressedBlocks.Add(new byte[3][]);

				bw.Write(entry.ID);
				bw.Write(entry.References);

				for (int j = 0; j < entry.EntryBlocks.Length; j++)
				{
					EntryBlock entryBlock = entry.EntryBlocks[j];
					uint uncompressedSize = 0;
					if (entryBlock.Data != null)
						uncompressedSize = (uint)entryBlock.Data.Length;
					//self.Write((entryBlock.UncompressedAlignment << 28) | uncompressedSize);
					bw.Write((uint)(uncompressedSize | (BitScan.BitScanReverse(entryBlock.UncompressedAlignment) << 28)));
				}

				for (int j = 0; j < entry.EntryBlocks.Length; j++)
				{
					EntryBlock entryBlock = entry.EntryBlocks[j];
					if (entryBlock.Data == null)
						bw.Write((uint)0);
					else
					{
						compressedBlocks[i][j] = entryBlock.Data.Compress();
						bw.Write(compressedBlocks[i][j].Length);
					}
				}

				entryDataPointerPos.Add(new long[3]);
				for (int j = 0; j < entryDataPointerPos[i].Length; j++)
				{
					entryDataPointerPos[i][j] = bw.BaseStream.Position;
					bw.BaseStream.Position += 4;
				}

				bw.Write(entry.DependenciesListOffset);
				bw.Write((int)entry.Type);
				bw.Write(entry.DependencyCount);

				bw.BaseStream.Position += 2; // Padding
			}

			// Data Block
			for (int i = 0; i < 3; i++)
			{
				long blockStart = bw.BaseStream.Position;
				bw.BaseStream.Position = fileBlockOffsetsPos[i];
				bw.Write((uint)blockStart);
				bw.BaseStream.Position = blockStart;

				for (int j = 0; j < this.Entries.Count; j++)
				{
					BundleEntry entry = this.Entries[j];
					EntryBlock entryBlock = entry.EntryBlocks[i];
					bool compressed = this.Flags.HasFlag(Flags.Compressed);
					uint size = compressed ? (compressedBlocks[j][i] == null ? 0 : (uint)compressedBlocks[j][i].Length) : (uint)entryBlock.Data.Length;

					if (size > 0)
					{
						long pos = bw.BaseStream.Position;
						bw.BaseStream.Position = entryDataPointerPos[j][i];
						bw.Write((uint)(pos - blockStart));
						bw.BaseStream.Position = pos;

						if (compressed)
							bw.Write(compressedBlocks[j][i]);
						else
							bw.Write(entryBlock.Data);

						bw.Align((i != 0 && j != this.Entries.Count - 1) ? (byte)0x80 : (byte)16);
					}
				}

				if (i != 2)
					bw.Align(0x80);
			}
		}

		public static bool IsBundle(string path)
        {
            bool result;

            try
            {
                Stream s = File.Open(path, FileMode.Open, FileAccess.Read);
                BinaryReader2 br = new BinaryReader2(s);

                result = br.VerifyMagic(Magic);

                br.Close();
                s.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "\n\n" + ex.StackTrace);
                result = false;
            }

            return result;
        }

        public static List<uint> GetEntryIDs(string path, bool console = false)
        {
            List<uint> result = new List<uint>();

            Stream s = File.Open(path, FileMode.Open, FileAccess.Read);
            BinaryReader2 br = new BinaryReader2(s);

            if (!br.VerifyMagic(Magic))
            {
                br.Close();
                s.Close();
                return null;
            }

            br.BaseStream.Position += 4;

            int platformInt = br.ReadInt32();
            if (platformInt != 1)
                platformInt = Util.ReverseBytes(platformInt);
            BundlePlatform platform = (BundlePlatform)platformInt;
            br.BigEndian = platform == BundlePlatform.X360 || platform == BundlePlatform.PS3;

            br.BaseStream.Position = 0x10;
            int fileCount = br.ReadInt32();
            int metaStart = br.ReadInt32();

            br.BaseStream.Position = metaStart;

            for (int i = 0; i < fileCount; i++)
            {
                uint id = br.ReadUInt32();
                br.BaseStream.Position += 0x3C;

                result.Add(id);
            }

            br.Close();
            s.Close();

            return result;
        }

        public static List<EntryInfo> GetEntryInfos(string path, bool console = false)
        {
            List<EntryInfo> result = new List<EntryInfo>();

            Stream s = File.Open(path, FileMode.Open, FileAccess.Read);
            BinaryReader2 br = new BinaryReader2(s);

            if (!br.VerifyMagic(Magic))
            {
                br.Close();
                s.Close();
                return null;
            }

            br.BaseStream.Position += 4;

            int platformInt = br.ReadInt32();
            if (platformInt != 1)
                platformInt = Util.ReverseBytes(platformInt);
            BundlePlatform platform = (BundlePlatform)platformInt;
            br.BigEndian = platform == BundlePlatform.X360 || platform == BundlePlatform.PS3;
            
            br.BaseStream.Position = 0x10;
            int fileCount = br.ReadInt32();
            int metaStart = br.ReadInt32();

            br.BaseStream.Position = metaStart;

            for (int i = 0; i < fileCount; i++)
            {
                uint id = br.ReadUInt32();
                br.BaseStream.Position += 0x34;
                EntryType type = (EntryType)br.ReadUInt32();
                br.BaseStream.Position += 0x4;

                result.Add(new EntryInfo(id, type, path));
            }

            br.Close();
            s.Close();

            return result;
        }

        public static EntryType GetEntryType(string path, uint id, bool console = false)
        {
            DebugTimer timer = DebugTimer.Start("GetEntryType");
            EntryType result = EntryType.Invalid;

            Stream s = File.Open(path, FileMode.Open, FileAccess.Read);
            BinaryReader2 br = new BinaryReader2(s);

            if (!br.VerifyMagic(Magic))
            {
                timer.StopLog();
                br.Close();
                s.Close();
                return EntryType.Invalid;
            }
            
            br.BaseStream.Position += 4;

            int platformInt = br.ReadInt32();
            if (platformInt != 1)
                platformInt = Util.ReverseBytes(platformInt);
            BundlePlatform platform = (BundlePlatform)platformInt;
            br.BigEndian = platform == BundlePlatform.X360 || platform == BundlePlatform.PS3;
            
            br.BaseStream.Position = 0x10;
            int fileCount = br.ReadInt32();
            int metaStart = br.ReadInt32();

            br.BaseStream.Position = metaStart;

            for (int i = 0; i < fileCount; i++)
            {
                uint id2 = br.ReadUInt32();
                br.BaseStream.Position += 0x34;
                EntryType type = (EntryType) br.ReadUInt32();
                br.BaseStream.Position += 0x4;

                if (id2 == id)
                {
                    result = type;
                    break;
                }
            }

            br.Close();
            s.Close();

            timer.StopLog();

            return result;
        }
    }
}
