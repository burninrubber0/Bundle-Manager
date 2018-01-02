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
        public BND2Archive Archive;

        public int Index;

        public int ID;
        public int Checksum;
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
        //public int FileDef;
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

        //public EntryType Type
        //{
        //    get
        //    {
        //        /*if (ExtraData != null && ((Console && Data.Length == 48) || (!Console && Data.Length == 32)))
        //        {
        //            return EntryType.Image;
        //        }
        //        else
        //        {
        //            return EntryType.Unknown;
        //        }*/
        //        return FileDef;
        //    }
        //}

        public EntryType Type;

        public bool Console;

        public bool Dirty;

        public BND2Entry(BND2Archive archive)
        {
            Archive = archive;
        }
    }

    public enum EntryType
    {
        Texture = 0x00,
        Material = 0x01,
        ResourceStringTable = 0x03,
        VertexDesc = 0xA,
        Model = 0x0C,
        Shader = 0x12,
        AttribSysVault = 0x1C,
        Flash = 0x1E,
        Font = 0x21,
        LuaCode = 0x22,
        CollisionMeshData = 0x24,
        IDList = 0x25,
        Language = 0x27,
        Tile = 0x28,
        TileDirectory = 0x29,
        ColourCube = 0x2B,
        HudMessage = 0x2C,
        HudMessageDictionary = 0x2D,
        HudMessageSequence = 0x2E,
        HudMessageSequenceDictionary = 0x2F,
        WorldPainter2D = 0x30,
        Registry = 0xA000,
        GenericWaveContent = 0xA020,
        GinsuWaveContent = 0xA021,
        AEMSBank = 0xA022,
        CSIS = 0xA023,
        SplicerData = 0xA024,
        LoopModel = 0x10000,
        AIMapData = 0x10001,
        TrafficData = 0x10002,
        TriggerData = 0x10003,
        DeformationModel = 0x10004,
        VehicleList = 0x10005,
        ParticleDescriptionCollection = 0x10008,
        WheelList = 0x10009,
        TextureNameMap = 0x1000B,
        ICEList = 0x1000C,
        ICEData = 0x1000D,
        ProgressionData = 0x1000E,
        EnvironmentKeyframe = 0x10012,
        EnvironmentTimeLine = 0x10013,
        EnvironmentDictionary = 0x10014
    }
}
