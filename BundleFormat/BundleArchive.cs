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
    public class BundleArchive
    {
        public static readonly byte[] MAGIC = new byte[] { 0x62, 0x6E, 0x64, 0x32 };

        public string Path;

        public int Version;
        public int Unknown2; // Normally 1
        public int Unknown3; // Normally 30
        public int FileCount;
        public int MetadataStart; // Normally 30
        public int HeadStart;
        public int BodyStart;
        public int ArchiveSize;
        public CompressionType CompressionType; // Normally 7
        public int Unknown7; // Normally 0
        public int Unknown8; // Normally 0
        public bool Console;

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

        public BundleEntry GetEntryByID(uint id)
        {
            for (int i = 0; i < Entries.Count; i++)
            {
                BundleEntry entry = Entries[i];
                if (entry.ID == id)
                    return entry;
            }
            return null;
        }

        public static BundleArchive Read(string path, bool console = false)
        {
            try
            {
                Stream s = File.Open(path, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(s);

                BundleArchive result = br.ReadBND2Archive(console);
                result.Path = path;

                br.Close();
                s.Close();

                return result;
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return null;
            }
        }

        public static bool IsBundle(string path)
        {
            /*string ext = System.IO.Path.GetExtension(path)?.ToUpper();
            if (ext == null)
                return false;

            if (ext == ".BUNDLE" || ext == ".BNDL" || ext == ".FONT" || ext == ".TEX")
                return new FileInfo(path).Length > 0;

            if ((ext == ".DAT" || ext == ".RV2") && new FileInfo(path).Length > 0)
            {

                bool result;
    
                try
                {
                    Stream s = File.Open(path, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(s);
    
                    result = br.VerifyMagic(MAGIC);
    
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

            return false;*/

            bool result;

            try
            {
                Stream s = File.Open(path, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(s);

                result = br.VerifyMagic(MAGIC);

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
            BinaryReader br = new BinaryReader(s);

            if (!br.VerifyMagic(MAGIC))
            {
                br.Close();
                s.Close();
                return null;
            }

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
            BinaryReader br = new BinaryReader(s);

            if (!br.VerifyMagic(MAGIC))
            {
                br.Close();
                s.Close();
                return null;
            }

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
            BinaryReader br = new BinaryReader(s);

            if (!br.VerifyMagic(MAGIC))
            {
                timer.StopLog();
                br.Close();
                s.Close();
                return EntryType.Invalid;
            }

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

    public class EntryInfo
    {
        public uint ID;
        public EntryType Type;
        public string Path;

        public EntryInfo(uint id, EntryType type, string path)
        {
            ID = id;
            Type = type;
            Path = path;
        }
    }

    public class BundleEntry
    {
        public BundleArchive Archive;

        public int Index;

        public ulong ID;
        public int References;
        public int Unknown12;
        public int UncompressedHeaderSize;
        public int UncompressedHeaderSizeCache;
        public int UncompressedBodySize;
        public int UncompressedBodySizeCache;
        public int Unknown15;
        public int HeaderSize;
        public long BodySize;
        public int HeadOffset;
        public long BodyOffset;
        public int DependenciesListOffset;
        public short DependencyCount;
        public short Unknown;
        public int Unknown24;
        public int Unknown25;

        public byte[] Header;
        public byte[] Body;

        public byte[] CompressedHeader;
        public byte[] CompressedBody;

        public bool DataCompressed;
        public bool ExtraDataCompressed;

        public bool HasHeader => Header != null && Header.Length > 0;
        public bool HasBody => Body != null && Body.Length > 0;

        public EntryType Type;

        public bool Console;

        public bool Dirty;

        public BundleEntry(BundleArchive archive)
        {
            Archive = archive;
        }

        public MemoryStream MakeStream(bool body = false)
        {
            if (body)
                return new MemoryStream(Body);
            return new MemoryStream(Header);
        }

        public List<Dependency> GetDependencies()
        {
            List<Dependency> result = new List<Dependency>();

            MemoryStream ms = MakeStream();
            BinaryReader br = new BinaryReader(ms);

            br.BaseStream.Position = DependenciesListOffset;

            for (int i = 0; i < DependencyCount; i++)
            {
                Dependency dependency = new Dependency();

                dependency.EntryID = br.ReadUInt32();
                dependency.Unknown1 = br.ReadInt32();
                dependency.EntryPointerOffset = br.ReadInt32();
                dependency.Unknown2 = br.ReadInt32();

                BundleEntry entry = null;

                for (int j = 0; j < Archive.Entries.Count; j++)
                {
                    if (Archive.Entries[j].ID != dependency.EntryID)
                        continue;

                    dependency.EntryIndex = j;
                    entry = Archive.Entries[j];
                }

                dependency.Entry = entry;

                result.Add(dependency);
            }

            br.Close();
            ms.Close();

            return result;
        }

        public Color GetColor()
        {
            switch (Type)
            {
                case EntryType.RasterResourceType:
                    return Color.Orange;
                case EntryType.MaterialResourceType:
                    return Color.DeepPink;
                case EntryType.TextFileResourceType:
                    break;
                case EntryType.RwVertexDescResourceType:
                    break;
                case EntryType.RwRenderableResourceType:
                    return Color.Aquamarine;
                case EntryType.unknown_file_type_00D:
                    break;
                case EntryType.RwTextureStateResourceType:
                    break;
                case EntryType.MaterialStateResourceType:
                    break;
                case EntryType.RwShaderProgramBufferResourceType:
                    break;
                case EntryType.RwShaderParameterResourceType:
                    break;
                case EntryType.RwDebugResourceType:
                    break;
                case EntryType.KdTreeResourceType:
                    break;
                case EntryType.SnrResourceType:
                    break;
                case EntryType.AttribSysSchemaResourceType:
                    break;
                case EntryType.AttribSysVaultResourceType:
                    break;
                case EntryType.AptDataHeaderType:
                    break;
                case EntryType.GuiPopupResourceType:
                    break;
                case EntryType.FontResourceType:
                    break;
                case EntryType.LuaCodeResourceType:
                    break;
                case EntryType.InstanceListResourceType:
                    return Color.BlueViolet;
                case EntryType.IDList:
                    return Color.Tomato;
                case EntryType.LanguageResourceType:
                    break;
                case EntryType.SatNavTileResourceType:
                    break;
                case EntryType.SatNavTileDirectoryResourceType:
                    break;
                case EntryType.ModelResourceType:
                    return Color.Blue;
                case EntryType.RwColourCubeResourceType:
                    break;
                case EntryType.HudMessageResourceType:
                    break;
                case EntryType.HudMessageListResourceType:
                    break;
                case EntryType.unknown_file_type_02E:
                    break;
                case EntryType.unknown_file_type_02F:
                    break;
                case EntryType.WorldPainter2DResourceType:
                    break;
                case EntryType.PFXHookBundleResourceType:
                    break;
                case EntryType.ShaderResourceType:
                    break;
                case EntryType.ICETakeDictionaryResourceType:
                    break;
                case EntryType.VideoDataResourceType:
                    break;
                case EntryType.PolygonSoupListResourceType:
                    return Color.Goldenrod;
                case EntryType.CommsToolListDefinitionResourceType:
                    break;
                case EntryType.CommsToolListResourceType:
                    break;
                case EntryType.AnimationCollectionResourceType:
                    break;
                case EntryType.RegistryResourceType:
                    break;
                case EntryType.GenericRwacWaveContentResourceType:
                    break;
                case EntryType.GinsuWaveContentResourceType:
                    break;
                case EntryType.AemsBankResourceType:
                    break;
                case EntryType.CsisResourceType:
                    break;
                case EntryType.NicotineResourceType:
                    break;
                case EntryType.SplicerResourceType:
                    break;
                case EntryType.GenericRwacReverbIRContentResourceType:
                    break;
                case EntryType.SnapshotDataResourceType:
                    break;
                case EntryType.ZoneListResourceType:
                    break;
                case EntryType.LoopModelResourceType:
                    break;
                case EntryType.AISectionsResourceType:
                    break;
                case EntryType.TrafficDataResourceType:
                    break;
                case EntryType.TriggerResourceType:
                    break;
                case EntryType.VehicleListResourceType:
                    break;
                case EntryType.GraphicsSpecResourceType:
                    return Color.SeaGreen;
                case EntryType.ParticleDescriptionCollectionResourceType:
                    break;
                case EntryType.WheelListResourceType:
                    break;
                case EntryType.WheelGraphicsSpecResourceType:
                    break;
                case EntryType.TextureNameMapResourceType:
                    break;
                case EntryType.ProgressionResourceType:
                    break;
                case EntryType.PropPhysicsResourceType:
                    break;
                case EntryType.PropGraphicsListResourceType:
                    break;
                case EntryType.PropInstanceDataResourceType:
                    break;
                case EntryType.BrnEnvironmentKeyframeResourceType:
                    break;
                case EntryType.BrnEnvironmentTimeLineResourceType:
                    break;
                case EntryType.BrnEnvironmentDictionaryResourceType:
                    break;
                case EntryType.GraphicsStubResourceType:
                    break;
                case EntryType.StaticSoundMapResourceType:
                    break;
                case EntryType.StreetDataResourceType:
                    break;
                case EntryType.BrnVFXMeshCollectionResourceType:
                    break;
                case EntryType.MassiveLookupTableResourceType:
                    break;
                case EntryType.VFXPropCollectionResourceType:
                    break;
                case EntryType.StreamedDeformationSpecResourceType:
                    break;
                case EntryType.ParticleDescriptionResourceType:
                    break;
                case EntryType.PlayerCarColoursResourceType:
                    break;
                case EntryType.ChallengeListResourceType:
                    break;
                case EntryType.FlaptFileResourceType:
                    break;
                case EntryType.ProfileUpgradeResourceType:
                    break;
                case EntryType.VehicleAnimationResourceType:
                    break;
                case EntryType.BodypartRemappingResourceType:
                    break;
                case EntryType.LUAListResourceType:
                    break;
                case EntryType.LUAScriptResourceType:
                    break;
            }
            return Color.Transparent;
        }
    }

    public struct Dependency
    {
        public uint EntryID;
        public int Unknown1;
        public int EntryPointerOffset;
        public int Unknown2;

        public int EntryIndex;
        public BundleEntry Entry;

        public override string ToString()
        {
            string value = "";

            bool external = Entry == null;

            string extra = "";
            if (Entry == null)
            {
                string file = BundleCache.GetFileByEntryID(EntryID);
                if (!string.IsNullOrEmpty(file))
                {
                    extra = ", Path: " + BundleCache.GetRelativePath(file);
                    BundleArchive archive = BundleArchive.Read(file, false);
                    Entry = archive.GetEntryByID(EntryID);
                }
            }

            if (Entry != null && Entry.Type == EntryType.RwVertexDescResourceType)
            {
                VertexDesc desc = VertexDesc.Read(Entry);
                value = ", Stride: " + desc.Stride.ToString("D2");
            }

            string location = external ? "External" : "Internal";

            string info = "(External)";
            if (Entry != null)
                info = "(" + location + ": " + EntryIndex.ToString("D3") + ", " + Entry.Type + value + extra + ")";
            return "ID: 0x" + EntryID.ToString("X8") + ", PtrOffset: 0x" + EntryPointerOffset.ToString("X8") + " " + info;
        }
    }

    public enum EntryType
    {
        RasterResourceType = 0x00,
        MaterialResourceType = 0x01,
        TextFileResourceType = 0x03,
        RwVertexDescResourceType = 0x0A,
        RwRenderableResourceType = 0x0C,
        unknown_file_type_00D = 0x0D,
        RwTextureStateResourceType = 0x0E,
        MaterialStateResourceType = 0x0F,
        RwShaderProgramBufferResourceType = 0x12,
        RwShaderParameterResourceType = 0x14,
        RwDebugResourceType = 0x16,
        KdTreeResourceType = 0x17,
        SnrResourceType = 0x19,
        AttribSysSchemaResourceType = 0x1B,
        AttribSysVaultResourceType = 0x1C,
        AptDataHeaderType = 0x1E,
        GuiPopupResourceType = 0x1F,
        FontResourceType = 0x21,
        LuaCodeResourceType = 0x22,
        InstanceListResourceType = 0x23,
        IDList = 0x25,
        LanguageResourceType = 0x27,
        SatNavTileResourceType = 0x28,
        SatNavTileDirectoryResourceType = 0x29,
        ModelResourceType = 0x2A,
        RwColourCubeResourceType = 0x2B,
        HudMessageResourceType = 0x2C,
        HudMessageListResourceType = 0x2D,
        unknown_file_type_02E = 0x2E,
        unknown_file_type_02F = 0x2F,
        WorldPainter2DResourceType = 0x30,
        PFXHookBundleResourceType = 0x31,
        ShaderResourceType = 0x32,
        ICETakeDictionaryResourceType = 0x41,
        VideoDataResourceType = 0x42,
        PolygonSoupListResourceType = 0x43,
        CommsToolListDefinitionResourceType = 0x45,
        CommsToolListResourceType = 0x46,
        AnimationCollectionResourceType = 0x51,
        RegistryResourceType = 0xA000,
        GenericRwacWaveContentResourceType = 0xA020,
        GinsuWaveContentResourceType = 0xA021,
        AemsBankResourceType = 0xA022,
        CsisResourceType = 0xA023,
        NicotineResourceType = 0xA024,
        SplicerResourceType = 0xA025,
        GenericRwacReverbIRContentResourceType = 0xA028,
        SnapshotDataResourceType = 0xA029,
        ZoneListResourceType = 0xB000,
        LoopModelResourceType = 0x10000,
        AISectionsResourceType = 0x10001,
        TrafficDataResourceType = 0x10002,
        TriggerResourceType = 0x10003,
        VehicleListResourceType = 0x10005,
        GraphicsSpecResourceType = 0x10006,
        ParticleDescriptionCollectionResourceType = 0x10008,
        WheelListResourceType = 0x10009,
        WheelGraphicsSpecResourceType = 0x1000A,
        TextureNameMapResourceType = 0x1000B,
        ProgressionResourceType = 0x1000E,
        PropPhysicsResourceType = 0x1000F,
        PropGraphicsListResourceType = 0x10010,
        PropInstanceDataResourceType = 0x10011,
        BrnEnvironmentKeyframeResourceType = 0x10012,
        BrnEnvironmentTimeLineResourceType = 0x10013,
        BrnEnvironmentDictionaryResourceType = 0x10014,
        GraphicsStubResourceType = 0x10015,
        StaticSoundMapResourceType = 0x10016,
        StreetDataResourceType = 0x10018,
        BrnVFXMeshCollectionResourceType = 0x10019,
        MassiveLookupTableResourceType = 0x1001A,
        VFXPropCollectionResourceType = 0x1001B,
        StreamedDeformationSpecResourceType = 0x1001C,
        ParticleDescriptionResourceType = 0x1001D,
        PlayerCarColoursResourceType = 0x1001E,
        ChallengeListResourceType = 0x1001F,
        FlaptFileResourceType = 0x10020,
        ProfileUpgradeResourceType = 0x10021,
        VehicleAnimationResourceType = 0x10023,
        BodypartRemappingResourceType = 0x10024,
        LUAListResourceType = 0x10025,
        LUAScriptResourceType = 0x10026,

        Invalid = 0x99999
    }

    public enum CompressionType
    {
        Uncompressed = 6,
        ZLib = 7
    }
}
