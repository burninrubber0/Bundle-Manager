using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BundleUtilities;
using Microsoft.SqlServer.Server;

namespace BundleFormat
{
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

        public BundlePlatform Platform;
        public bool Console => Platform == BundlePlatform.X360 || Platform == BundlePlatform.PS3;

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

        public List<BundleDependency> GetDependencies()
        {
            List<BundleDependency> result = new List<BundleDependency>();

            MemoryStream ms = MakeStream();
            BinaryReader2 br = new BinaryReader2(ms);
            br.BigEndian = Console;

            br.BaseStream.Position = DependenciesListOffset;

            for (int i = 0; i < DependencyCount; i++)
            {
                BundleDependency bundleDependency = new BundleDependency();

                bundleDependency.EntryID = br.ReadUInt64();
                bundleDependency.EntryPointerOffset = br.ReadInt32();
                bundleDependency.Unknown2 = br.ReadInt32();

                BundleEntry entry = null;

                for (int j = 0; j < Archive.Entries.Count; j++)
                {
                    if (Archive.Entries[j].ID != bundleDependency.EntryID)
                        continue;

                    bundleDependency.EntryIndex = j;
                    entry = Archive.Entries[j];
                }

                bundleDependency.Entry = entry;

                result.Add(bundleDependency);
            }

            br.Close();
            ms.Close();

            return result;
        }

        public string DetectName()
        {
            string theName = "worldvault";
            ulong theID = Crc32.HashCrc32B(theName);
            if (theID == ID)
                return theName;
            theName = "postfxvault";
            theID = Crc32.HashCrc32B(theName);
            if (theID == ID)
                return theName;
            theName = "cameravault";
            theID = Crc32.HashCrc32B(theName);
            if (theID == ID)
                return theName;

            string path = Path.GetFileNameWithoutExtension(Archive.Path);
            string file = null;
            if (path != null)
                file = path.ToUpper();

            if (file != null && file.StartsWith("TRK_UNIT") && file.EndsWith("_GR"))
            {
                string trackID = file.Substring(8).Replace("_GR", "").ToLower();
                string name = "trk_unit" + trackID + "_list";
                ulong newID = Crc32.HashCrc32B(name);
                if (newID == ID)
                    return name;
            }

            if (file != null)
            {
                string aptName = file.ToLower() + ".swf";
                ulong aptID = Crc32.HashCrc32B(aptName);
                if (aptID == ID)
                    return aptName;
            }

            if (file != null && file.StartsWith("WHE_") && file.EndsWith("_GR"))
            {
                string wheelID = file.Substring(4).Replace("_GR", "").ToLower();
                string name = wheelID + "_graphics";
                ulong newID = Crc32.HashCrc32B(name);
                if (newID == ID)
                    return name;
            }

            if (file != null && file.StartsWith("VEH_"))
            {
                if (file.EndsWith("_AT"))
                {
                    string vehicleID = file.Substring(4).Replace("_AT", "").ToLower();
                    string name = vehicleID + "_attribsys";
                    ulong newID = Crc32.HashCrc32B(name);
                    if (newID == ID)
                        return name;
                    name = vehicleID + "deformationmodel";
                    newID = Crc32.HashCrc32B(name);
                    if (newID == ID)
                        return name;
                    name = vehicleID + "_bpr";
                    newID = Crc32.HashCrc32B(name);
                    if (newID == ID)
                        return name;
                    name = vehicleID + "_anim";
                    newID = Crc32.HashCrc32B(name);
                    if (newID == ID)
                        return name;
                    name = vehicleID + "_trafficstub";
                    newID = Crc32.HashCrc32B(name);
                    if (newID == ID)
                        return name;
                    name = vehicleID + "_vanm";
                    newID = Crc32.HashCrc32B(name);
                    if (newID == ID)
                        return name;
                } else if (file.EndsWith("_CD"))
                {
                    string vehicleID = file.Substring(4).Replace("_CD", "").ToLower();
                    string name = vehicleID;
                    ulong newID = Crc32.HashCrc32B(name);
                    if (newID == ID)
                        return name;
                } else if (file.EndsWith("_GR"))
                {
                    string vehicleID = file.Substring(4).Replace("_GR", "").ToLower();
                    string name = vehicleID + "_graphics";
                    ulong newID = Crc32.HashCrc32B(name);
                    if (newID == ID)
                        return name;
                }
            }

            // WorldCol Names
            for (int i = 0; i < Archive.Entries.Count; i++)
            {
                string name = "trk_col_" + i;
                ulong newID = Crc32.HashCrc32B(name);
                if (newID == ID)
                    return name;
                name = "trk_clil" + i;
                newID = Crc32.HashCrc32B(name);
                if (newID == ID)
                    return name;
            }

            return "N/A";
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
}
