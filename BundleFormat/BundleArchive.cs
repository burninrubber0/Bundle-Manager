using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BundleUtilities;

namespace BundleFormat
{
    [Flags]
    public enum Flags
    {
        Compressed = 1,
        UnknownFlag1 = 2,
        UnknownFlag2 = 4,
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
        public int FileCount;
        public int MetadataStart;
        public int HeadStart;
        public int BodyStart;
        public int ArchiveSize;
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
            br.BaseStream.Position += 4;

            result.RSTOffset = br.ReadInt32();
            result.FileCount = br.ReadInt32();
            result.MetadataStart = br.ReadInt32();
            result.HeadStart = br.ReadInt32();
            result.BodyStart = br.ReadInt32();
            result.ArchiveSize = br.ReadInt32();
            result.Flags = (Flags)br.ReadInt32();

			// 8 Bytes Padding

            //long dataOffset = result.HeadStart;

            br.BaseStream.Position = result.RSTOffset;

            result.ResourceStringTable = br.ReadCStr();

            br.BaseStream.Position = result.MetadataStart;

            for (int i = 0; i < result.FileCount; i++)
            {
                BundleEntry entry = new BundleEntry(result);

                entry.Index = i;

                entry.Platform = result.Platform;

                entry.ID = br.ReadUInt64();

                entry.References = br.ReadUInt64();
                int uncompressedHeaderSize = br.ReadInt32();
                long uncompressedBodySize = br.ReadInt64();
                entry.HeaderSize = br.ReadInt32();
                entry.BodySize = br.ReadInt32();
				entry.ThirdSize = br.ReadInt32();

                entry.HeadOffset = br.ReadInt32();
                entry.BodyOffset = br.ReadInt32();
				entry.ThirdOffset = br.ReadInt32();
				entry.DependenciesListOffset = br.ReadInt32();
                int fileType = br.ReadInt32();
                entry.DependencyCount = br.ReadInt16();

                br.BaseStream.Position += 2;

                entry.UncompressedHeaderSize = uncompressedHeaderSize & 0x0FFFFFFF;
                entry.UncompressedHeaderSizeCache = uncompressedHeaderSize >> 28;
                entry.UncompressedBodySize = (int)(uncompressedBodySize & 0x0FFFFFFF);
                entry.UncompressedBodySizeCache = (int)(uncompressedBodySize >> 28);

                entry.Type = (EntryType)fileType;

                // Header
                long offset = br.BaseStream.Position;
                br.BaseStream.Seek(result.HeadStart + entry.HeadOffset, SeekOrigin.Begin);

                byte[] data = br.ReadBytes(entry.HeaderSize);

                try
                {
                    entry.Unknown24 = br.ReadInt32();
                }
                catch (EndOfStreamException) { }
                
                entry.CompressedHeader = data;

                entry.Header = data.Decompress(entry.UncompressedHeaderSize);
                if (entry.Header == null || entry.Header.NoData())
                {
                    entry.DataCompressed = false;
                    entry.Header = data;
                }
                else
                {
                    entry.DataCompressed = true;
                }
                br.BaseStream.Seek(offset, SeekOrigin.Begin);

                // Body
                if (entry.BodySize > 0 && result.BodyStart != 0)
                {
                    br.BaseStream.Seek(result.BodyStart + entry.BodyOffset, SeekOrigin.Begin);

                    byte[] extra = br.ReadBytes((int)entry.BodySize);
                    try
                    {
                        entry.Unknown25 = br.ReadInt32();
                    }
                    catch (EndOfStreamException) { }

                    entry.CompressedBody = extra;

                    entry.Body = extra.Decompress(entry.UncompressedBodySize);
                    if (entry.Body == null || entry.Body.NoData())
                    {
                        entry.ExtraDataCompressed = false;
                        entry.Body = extra;
                    }
                    else
                    {
                        entry.ExtraDataCompressed = true;
                    }

                    br.BaseStream.Seek(offset, SeekOrigin.Begin);
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
