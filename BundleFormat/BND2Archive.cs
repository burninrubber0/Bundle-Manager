using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundleFormat
{
    public class BND2Archive
    {
        public static readonly byte[] MAGIC = new byte[] { 0x62, 0x6E, 0x64, 0x32 };

        public int Version;
        public int Unknown2; // Normally 1
        public int Unknown3; // Normally 30
        public int FileCount;
        public int Unknown4; // Normally 30
        public int DataStart;
        public int ExtraDataStart;
        public int ArchiveSize;
        public int Unknown6; // Normally 7
        public int Unknown7; // Normally 0
        public int Unknown8; // Normally 0
        public bool Console;

        public List<BND2Entry> Entries;

        private bool _dirty;
        public bool Dirty
        {
            get
            {
                if (_dirty)
                    return true;
                foreach (BND2Entry entry in Entries)
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
                    foreach(BND2Entry entry in Entries)
                    {
                        entry.Dirty = false;
                    }
                }
            }
        }

        public BND2Archive()
        {
            this.Entries = new List<BND2Entry>();
        }
    }

    public class BND2Entry
    {
        public int Index;

        public long Unknown9;
        public int Unknown11;
        public int Unknown12;
        public int Unknown13;
        public int Unknown14;
        public int Unknown15;
        public int FileSize;
        public long ExtraSize;
        public int StartOff;
        public long ExtraStartOff;
        public int Unknown21;
        public int Unknown22;
        public int Unknown23;
        public int Unknown24;
        public int Unknown25;
        //public byte[] Unknown25;
        //public byte[] Unknown26;
        //public byte[] Unknown27;

        public byte[] Data;
        public byte[] ExtraData;

        public byte[] CData;
        public byte[] CExtraData;

        public bool DataCompressed;
        public bool ExtraDataCompressed;

        public EntryType Type
        {
            get
            {
                if (ExtraData != null && ((Console && Data.Length == 48) || (!Console && Data.Length == 32)))
                {
                    return EntryType.Image;
                }
                else
                {
                    return EntryType.Unknown;
                }
            }
        }

        public bool Console;

        public bool Dirty;
    }

    public enum EntryType
    {
        Unknown,
        Image
    }
}
