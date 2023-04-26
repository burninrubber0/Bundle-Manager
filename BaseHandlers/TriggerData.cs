using BundleFormat;
using BundleUtilities;
using PluginAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BaseHandlers
{
    class DescriptiveCollectionEditor : CollectionEditor
    {
        public DescriptiveCollectionEditor(Type type) : base(type) { }
        protected override CollectionForm CreateCollectionForm()
        {
            CollectionForm form = base.CreateCollectionForm();
            form.Shown += delegate
            {
                ShowDescription(form);
            };
            return form;
        }
        static void ShowDescription(Control control)
        {
            PropertyGrid grid = control as PropertyGrid;
            if (grid != null) grid.HelpVisible = true;
            foreach (Control child in control.Controls)
            {
                ShowDescription(child);
            }
        }
    }
    public class CgsID
    {
        private UInt64 m_id = 0;

        public UInt64 ID
        {
            get { return m_id; }
            set { m_id = value; }
        }

        public static CgsID FromByteArray(byte[] data, int startIndex)
        {
            UInt64 id = BitConverter.ToUInt64(data, startIndex);
            CgsID result = new CgsID();
            result.ID = id;
            return new CgsID();
        }

        public byte[] ToByteArray()
        {
            return BitConverter.GetBytes(m_id);
        }

        public void Read(BinaryReader reader)
        {
            m_id = reader.ReadUInt64();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(m_id);
        }

        public override string ToString()
        {
            return string.Format("{0:X16}", m_id);
        }
    }
    public enum RegionType
    {
        E_TYPE_LANDMARK = 0,
        E_TYPE_BLACKSPOT = 1,
        E_TYPE_GENERIC_REGION = 2,
        E_TYPE_VFXBOX_REGION = 3,
        E_TYPE_COUNT = 4
    }

    public class StartingGrid {
        public Vector3I[] StartingPositions { get; set; } = new Vector3I[1];
        public Vector3I[] StartingDirections { get; set; } = new Vector3I[1];

        public void Read(BinaryReader reader)
        {
            StartingPositions = new Vector3I[8];
            for (int i = 0; i < 8; i++)
            {
                StartingPositions[i] = new Vector3I(
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(), reader.ReadSingle());
            }

            StartingDirections = new Vector3I[8];
            for (int i = 0; i < 8; i++)
            {
                StartingDirections[i] = new Vector3I(
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(), reader.ReadSingle());
            }
        }

        public void Write(BinaryWriter writer)
        {
            for (int i = 0; i < 8; i++)
            {
                writer.Write(StartingPositions[i].X);
                writer.Write(StartingPositions[i].Y);
                writer.Write(StartingPositions[i].Z);
                writer.Write(StartingPositions[i].S);
            }

            for (int i = 0; i < 8; i++)
            {
                writer.Write(StartingDirections[i].X);
                writer.Write(StartingDirections[i].Y);
                writer.Write(StartingDirections[i].Z);
                writer.Write(StartingDirections[i].S);
            }
        }
    }

    public class BoxRegion
    {
        public float PositionX { get; set; } = 0;
        public float PositionY { get; set; } = 0;
        public float PositionZ { get; set; } = 0;
        public float RotationX { get; set; } = 0;
        public float RotationY { get; set; } = 0;
        public float RotationZ { get; set; } = 0;
        public float DimensionX { get; set; } = 0;
        public float DimensionY { get; set; } = 0;
        public float DimensionZ { get; set; } = 0;

        public void Read(BinaryReader reader)
        {
            PositionX = reader.ReadSingle();
            PositionY = reader.ReadSingle();
            PositionZ = reader.ReadSingle();
            RotationX = reader.ReadSingle();
            RotationY = reader.ReadSingle();
            RotationZ = reader.ReadSingle();
            DimensionX = reader.ReadSingle();
            DimensionY = reader.ReadSingle();
            DimensionZ = reader.ReadSingle();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(PositionX);
            writer.Write(PositionY);
            writer.Write(PositionZ);
            writer.Write(RotationX);
            writer.Write(RotationY);
            writer.Write(RotationZ);
            writer.Write(DimensionX);
            writer.Write(DimensionY);
            writer.Write(DimensionZ);
        }
    }

    public class TriggerRegion
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public BoxRegion mBoxRegion { get; set; } = new BoxRegion();
        public int mId { get; set; } = 0;
        public short miRegionIndex { get; set; } = 0;
        public RegionType meType { get; set; } = RegionType.E_TYPE_LANDMARK;
        private byte[] muPad { get; set; } = new byte[1];

        public virtual void Read(BinaryReader reader)
        {
            mBoxRegion = new BoxRegion();
            mBoxRegion.Read(reader);
            mId = reader.ReadInt32();
            miRegionIndex = reader.ReadInt16();
            meType = (RegionType)reader.ReadByte();
            muPad = reader.ReadBytes(1);
        }

        public virtual void Write(BinaryWriter writer)
        {
            mBoxRegion.Write(writer);

            writer.Write(mId);
            writer.Write(miRegionIndex);
            writer.Write((byte)meType);
            writer.Write(muPad);
        }
    }

    public class Landmark : TriggerRegion
    {
        public List<StartingGrid> mpaStartingGrids { get; set; } = new List<StartingGrid>();

        private long startingGridsOffset = 0;
        public byte miStartingGridCount { get; set; } = 0;
        public byte muDesignIndex { get; set; } = 0;
        public byte muDistrict { get; set; } = 0;
        public byte mu8Flags { get; set; } = 0;

        public void Read(BinaryReader reader)
        {
            base.Read(reader);
            startingGridsOffset = reader.ReadUInt32();
            miStartingGridCount = reader.ReadByte();
            muDesignIndex = reader.ReadByte();
            muDistrict = reader.ReadByte();
            mu8Flags = reader.ReadByte();

            long currentPosition = reader.BaseStream.Position;
            reader.BaseStream.Position = startingGridsOffset;

            for (int i = 0; i < miStartingGridCount; i++)
            {
                StartingGrid startingGrid = new StartingGrid();
                startingGrid.Read(reader);
                mpaStartingGrids.Add(startingGrid);
            }
            reader.BaseStream.Position = currentPosition;

        }

        public void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write((uint)startingGridsOffset);
            writer.Write(miStartingGridCount);
            writer.Write(muDesignIndex);
            writer.Write(muDistrict);
            writer.Write(mu8Flags);
            // To-Do: Does not handle saving startingGridsOffsetAtAll, not clear where it should be saved in the file
            /*
            for (int i = 0; i < miStartingGridCount; i++)
            {
                mpaStartingGrids[i].Write(writer);
            }
            */
        }
    }

    public enum GenericRegionType
    {
        E_TYPE_JUNK_YARD = 0,
        E_TYPE_BIKE_SHOP = 1,
        E_TYPE_GAS_STATION = 2,
        E_TYPE_BODY_SHOP = 3,
        E_TYPE_PAINT_SHOP = 4,
        E_TYPE_CAR_PARK = 5,
        E_TYPE_SIGNATURE_TAKEDOWN = 6,
        E_TYPE_KILLZONE = 7,
        E_TYPE_JUMP = 8,
        E_TYPE_SMASH = 9,
        E_TYPE_SIGNATURE_CRASH = 10,
        E_TYPE_SIGNATURE_CRASH_CAMERA = 11,
        E_TYPE_ROAD_LIMIT = 12,
        E_TYPE_OVERDRIVE_BOOST = 13,
        E_TYPE_OVERDRIVE_STRENGTH = 14,
        E_TYPE_OVERDRIVE_SPEED = 15,
        E_TYPE_OVERDRIVE_CONTROL = 16,
        E_TYPE_TIRE_SHOP = 17,
        E_TYPE_TUNING_SHOP = 18,
        E_TYPE_PICTURE_PARADISE = 19,
        E_TYPE_TUNNEL = 20,
        E_TYPE_OVERPASS = 21,
        E_TYPE_BRIDGE = 22,
        E_TYPE_WAREHOUSE = 23,
        E_TYPE_LARGE_OVERHEAD_OBJECT = 24,
        E_TYPE_NARROW_ALLEY = 25,
        E_TYPE_PASS_TUNNEL = 26,
        E_TYPE_PASS_OVERPASS = 27,
        E_TYPE_PASS_BRIDGE = 28,
        E_TYPE_PASS_WAREHOUSE = 29,
        E_TYPE_PASS_LARGEOVERHEADOBJECT = 30,
        E_TYPE_PASS_NARROWALLEY = 31,
        E_TYPE_RAMP = 32,
        E_TYPE_GOLD = 33,
        E_TYPE_ISLAND_ENTITLEMENT = 34,
        E_TYPE_COUNT = 35,
    }

    public class GenericRegion : TriggerRegion
    {
        public int GroupID { get; set; } = 0;
        public short CameraCut1 { get; set; } = 0;
        public short CameraCut2 { get; set; } = 0;
        public sbyte CameraType1 { get; set; } = 0;
        public sbyte CameraType2 { get; set; } = 0;
        public GenericRegionType Type { get; set; } = GenericRegionType.E_TYPE_JUNK_YARD;
        public sbyte IsOneWay { get; set; } = 0;

        public void Read(BinaryReader reader)
        {
            base.Read(reader);
            GroupID = reader.ReadInt32();
            CameraCut1 = reader.ReadInt16();
            CameraCut2 = reader.ReadInt16();
            CameraType1 = reader.ReadSByte();
            CameraType2 = reader.ReadSByte();
            Type = (GenericRegionType)reader.ReadByte();
            IsOneWay = reader.ReadSByte();
        }

        public void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(GroupID);
            writer.Write(CameraCut1);
            writer.Write(CameraCut2);
            writer.Write(CameraType1);
            writer.Write(CameraType2);
            writer.Write((byte)Type);
            writer.Write(IsOneWay);
        }
    }

    public enum BlackspotScoreType {
        E_SCORE_TYPE_DISTANCE = 0,
        E_SCORE_TYPE_CAR_COUNT = 1,
        E_SCORE_TYPE_COUNT = 2,
    }

    public class Blackspot : TriggerRegion
    {
        public BlackspotScoreType muScoreType { get; set; } = BlackspotScoreType.E_SCORE_TYPE_DISTANCE;
        public int miScoreAmount { get; set; } = 0;

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);

            muScoreType = (BlackspotScoreType) reader.ReadByte();
            reader.ReadBytes(3); // Padding
            miScoreAmount = reader.ReadInt32();
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);

            writer.Write((byte)muScoreType);
            writer.Write(new byte[3]); // Padding
            writer.Write(miScoreAmount);
        }
    }

    public class Killzone 
    {
        [Description("Triggers as GenericRegions. Uses region.mId")]
        public List<int> TriggerIds { get; set; } = new List<int>();
        public CgsID[] RegionIds { get; set; } = new CgsID[1];

        private long TriggerOffsetPosition = 0;

        private long CGSIDOffsetPosition = 0;
        public void Read(BinaryReader reader)
        {
            // Read the trigger pointer array
            long genericRegionOffset = reader.ReadUInt32();
            int TriggerCount = reader.ReadInt32();

            // Read the region ID array
            long cgsOffset = reader.ReadUInt32();
            int RegionIdCount = reader.ReadInt32();
            long currentPosition = reader.BaseStream.Position;

            reader.BaseStream.Position = genericRegionOffset;
            uint[]  Triggers = new uint[TriggerCount];
            for (int i = 0; i < TriggerCount; i++)
            {
                Triggers[i] = reader.ReadUInt32();
            }

            TriggerIds = new List<int>();
            foreach (uint trigger in Triggers)
            {
                reader.BaseStream.Position = trigger;
                GenericRegion region = new GenericRegion();
                region.Read(reader);
                TriggerIds.Add(region.mId);
            }

            reader.BaseStream.Position = cgsOffset;
            RegionIds = new CgsID[RegionIdCount];
            for (int i = 0; i < RegionIds.Length; i++)
            {
                RegionIds[i] = new CgsID();
                RegionIds[i].Read(reader);
            }

            reader.BaseStream.Position = currentPosition;
        }

        public void Write(BinaryWriter writer)
        {
            TriggerOffsetPosition = writer.BaseStream.Position;
            writer.WriteUniquePadding(4);
            writer.Write(TriggerIds.Count);
            CGSIDOffsetPosition = writer.BaseStream.Position;
            writer.WriteUniquePadding(4);
            writer.Write(RegionIds.Length);            
        }

        public void WritePointerStuff(BinaryWriter writer, Dictionary<int, uint> genericRegionOffsets) {

            long currentPosition = writer.BaseStream.Position;
            writer.BaseStream.Position = TriggerOffsetPosition;
            writer.Write((uint)currentPosition);
            writer.BaseStream.Position = currentPosition;
            foreach (int trigger in TriggerIds)
            {
                writer.Write(genericRegionOffsets[trigger]);
            }
            long paddingCount = 16 - (writer.BaseStream.Position % 16);
            if (paddingCount < 16)
            {
                for (int i = 0; i < paddingCount; i++)
                    writer.Write((byte)0);
            }
            currentPosition = writer.BaseStream.Position;
            writer.BaseStream.Position = CGSIDOffsetPosition;
            writer.Write((uint)currentPosition);
            writer.BaseStream.Position = currentPosition;
            foreach (CgsID ids in RegionIds)
            {
                ids.Write(writer);
            }
            paddingCount = 16 - (writer.BaseStream.Position % 16);
            if (paddingCount < 16)
            {
                for (int i = 0; i < paddingCount; i++)
                    writer.Write((byte)0);
            }

        }


    }

    public class SignatureStunt
    {
        public CgsID mId { get; set; } = new CgsID();
        public long miCamera { get; set; } = 0;
        public List<int> genericRegionIds { get; set; } = new List<int>();

        public void Read(BinaryReader reader)
        {
            mId = new CgsID();
            mId.Read(reader);
            miCamera = reader.ReadInt64();

            uint mppStuntElementsOffset = reader.ReadUInt32();
            int miStuntElementCount = reader.ReadInt32();

            long currentPosition = reader.BaseStream.Position;
            reader.BaseStream.Position = mppStuntElementsOffset;
            uint[] Triggers = new uint[miStuntElementCount];
            for (int i = 0; i < miStuntElementCount; i++)
            {
                Triggers[i] = reader.ReadUInt32();
            }

            genericRegionIds = new List<int>();
            foreach (uint trigger in Triggers)
            {
                reader.BaseStream.Position = trigger;
                GenericRegion region = new GenericRegion();
                region.Read(reader);
                genericRegionIds.Add(region.mId);
            }
            reader.BaseStream.Position = currentPosition;
        }

        // Write something in triggerIds, because we dont have the actual positions yet
        public void WriteWithEmpty(BinaryWriter writer)
        {
            mId.Write(writer);
            writer.Write(miCamera);
            long GenericRegionIdsPosition = writer.BaseStream.Position;
            writer.WriteUniquePadding(4);
            writer.Write(genericRegionIds.Count);

            long currentPosition = writer.BaseStream.Position;
            writer.BaseStream.Position = GenericRegionIdsPosition;
            writer.Write(currentPosition);
            writer.BaseStream.Position = currentPosition;
            foreach (int trigger in genericRegionIds)
            {
                writer.Write((uint)trigger);
            }
        }

        public void Write(BinaryWriter writer, Dictionary<int, uint> genericRegionOffsets)
        {
            mId.Write(writer);
            writer.Write(miCamera);
            long GenericRegionIdsPosition = writer.BaseStream.Position;
            writer.WriteUniquePadding(4);
            writer.Write(genericRegionIds.Count);


            long currentPosition = writer.BaseStream.Position;
            writer.BaseStream.Position = GenericRegionIdsPosition;
            writer.Write(currentPosition);
            writer.BaseStream.Position = currentPosition;
            foreach (int trigger in genericRegionIds)
            {
                writer.Write(genericRegionOffsets[trigger]);
            }
        }
    }

    public class RoamingLocation
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Vector3I Position { get; set; } = new Vector3I(0,0,0,0);
        public byte DistrictIndex { get; set; } = 0;

        public void Read(BinaryReader reader) { 

            Position = new Vector3I(
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(), reader.ReadSingle());
            DistrictIndex = reader.ReadByte();
            // Read and discard padding
            reader.ReadBytes(15);
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Position.X);
            writer.Write(Position.Y);
            writer.Write(Position.Z);
            writer.Write(Position.S);
            writer.Write(DistrictIndex);

            // Write padding
            writer.Write(new byte[15]);
        }
    }

    public class VFXBoxRegion : TriggerRegion
    {
        public VFXBoxRegion() : base()
        {
            // Set meType to E_TYPE_VFXBOX_REGION
            meType = RegionType.E_TYPE_VFXBOX_REGION;
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
        }
    }

    public class SpawnLocation
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Vector3I mPosition { get; set; } = new Vector3I(0, 0, 0, 0);
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Vector3I mDirection { get; set; } = new Vector3I(0, 0, 0, 0);
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public CgsID mJunkyardId { get; set; } = new CgsID();
        public byte muType { get; set; } = 0;
        private byte[] padding = new byte[7];

        public void Read(BinaryReader reader)
        {
            mPosition = new Vector3I(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            mDirection = new Vector3I(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            mJunkyardId = new CgsID();
            mJunkyardId.Read(reader);
            muType = reader.ReadByte();
            padding = reader.ReadBytes(7);
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(mPosition.X);
            writer.Write(mPosition.Y);
            writer.Write(mPosition.Z);
            writer.Write(mPosition.S);
            writer.Write(mDirection.X);
            writer.Write(mDirection.Y);
            writer.Write(mDirection.Z);
            writer.Write(mDirection.S);
            mJunkyardId.Write(writer);
            writer.Write(muType);
            writer.Write(padding);
        }

    }


public class TriggerData : IEntryData
    {
        public int miVersionNumber { get; set; }
        public uint muSize { get; set; }
        public Vector3I mPlayerStartPosition { get; set; } 
        public Vector3I mPlayerStartDirection { get; set; } 
        public List<Landmark> mpLandmarks { get; set; }
        public int miOnlineLandmarkCount { get; set; }
        public SignatureStunt[] mpSignatureStunts { get; set; }
        public GenericRegion[] mpGenericRegions { get; set; }

        [Editor(typeof(DescriptiveCollectionEditor), typeof(UITypeEditor))]
        public Killzone[] mpKillzones { get; set; }
        public Blackspot[] mpBlackspots { get; set; }
        public VFXBoxRegion[] mpVFXBoxRegions { get; set; }
        public RoamingLocation[] mpRoamingLocations { get; set; }
        public SpawnLocation[] mpSpawnLocations { get; set; }
        private List<uint> TriggerOffsets { get; set; }


        public bool Read(BundleEntry entry, ILoader loader = null)
        {
            MemoryStream ms = entry.MakeStream();
            BinaryReader2 reader = new BinaryReader2(ms);
            reader.BigEndian = entry.Console;

            miVersionNumber = reader.ReadInt32();
            muSize = reader.ReadUInt32();
            reader.ReadBytes(8);// skip 8 bytes of padding
            mPlayerStartPosition = new Vector3I(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            mPlayerStartDirection = new Vector3I(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            // read landmarks
            long LandmarkTriggersOffset = reader.ReadUInt32();
            int miLandmarkCount = reader.ReadInt32();
            miOnlineLandmarkCount = reader.ReadInt32();

            long SignatureStuntsOffset = reader.ReadUInt32();
            int miSignatureStuntCount = reader.ReadInt32();

            long GenericRegionsOffset = reader.ReadUInt32();
            int miGenericRegionCount = reader.ReadInt32();

            long KillzoneOffset = reader.ReadUInt32();
            int miKillzoneCount = reader.ReadInt32();

            long BlackspotOffset = reader.ReadUInt32();
            int miBlackspotCount = reader.ReadInt32();

            long VFXBoxRegionOffset = reader.ReadUInt32();
            int miVFXBoxRegionCount = reader.ReadInt32();

            long RoamingLocationOffset = reader.ReadUInt32();
            int miRoamingLocationCount = reader.ReadInt32();

            long SpawnLocationOffset = reader.ReadUInt32();
            int miSpawnLocationCount = reader.ReadInt32();

            long TriggerRegionOffset = reader.ReadUInt32();
            int miRegionCount = reader.ReadInt32();

            reader.BaseStream.Position = LandmarkTriggersOffset;
            mpLandmarks = new List<Landmark>();
            for (int i = 0; i < miLandmarkCount; i++)
            {
                Landmark landmark = new Landmark();
                landmark.Read(reader);
                mpLandmarks.Add(landmark);
            }

            reader.BaseStream.Position = SignatureStuntsOffset;
            // read signature stunts
            mpSignatureStunts = new SignatureStunt[miSignatureStuntCount];
            for (int i = 0; i < miSignatureStuntCount; i++)
            {
                mpSignatureStunts[i] = new SignatureStunt();
                mpSignatureStunts[i].Read(reader);
            }
            
            reader.BaseStream.Position = GenericRegionsOffset;
            // read generic regions
            mpGenericRegions = new GenericRegion[miGenericRegionCount];
            for (int i = 0; i < miGenericRegionCount; i++)
            {
                mpGenericRegions[i] = new GenericRegion();
                mpGenericRegions[i].Read(reader);
            }
            

            reader.BaseStream.Position = KillzoneOffset;
            // read killzones
            mpKillzones = new Killzone[miKillzoneCount];
            for (int i = 0; i < miKillzoneCount; i++)
            {
                mpKillzones[i] = new Killzone();
                mpKillzones[i].Read(reader);
            }

            // read blackspots
            reader.BaseStream.Position = BlackspotOffset;
            mpBlackspots = new Blackspot[miBlackspotCount];
            for (int i = 0; i < miBlackspotCount; i++)
            {
                mpBlackspots[i] = new Blackspot();
                mpBlackspots[i].Read(reader);
            }

            reader.BaseStream.Position = VFXBoxRegionOffset;

            // read VFX box regions
            mpVFXBoxRegions = new VFXBoxRegion[miVFXBoxRegionCount];
            for (int i = 0; i < miVFXBoxRegionCount; i++)
            {
                mpVFXBoxRegions[i] = new VFXBoxRegion();
                mpVFXBoxRegions[i].Read(reader);
            }

            reader.BaseStream.Position = RoamingLocationOffset;
            
            // read roaming locations
            mpRoamingLocations = new RoamingLocation[miRoamingLocationCount];
            for (int i = 0; i < miRoamingLocationCount; i++)
            {
            mpRoamingLocations[i] = new RoamingLocation();
             mpRoamingLocations[i].Read(reader);
            }
            
            reader.BaseStream.Position = SpawnLocationOffset;

            mpSpawnLocations = new SpawnLocation[miSpawnLocationCount];
            for (int i = 0; i < miSpawnLocationCount; i++)
            {
                mpSpawnLocations[i] = new SpawnLocation();
                mpSpawnLocations[i].Read(reader);
            }

            reader.BaseStream.Position = TriggerRegionOffset;

             // read TriggerRegion (miGenericCount + miLandmarkCount)
            TriggerOffsets = new List<uint>();
            for (int i = 0; i < miRegionCount; i++)
            {
                uint section6Entry = reader.ReadUInt32();
                TriggerOffsets.Add(section6Entry);
            }

            return true;
        }

        public IEntryEditor GetEditor(BundleEntry entry)
        {
            TriggerDataEditor triggerDataEditor = new TriggerDataEditor();
            triggerDataEditor.trigger = this;

            triggerDataEditor.EditEvent += () =>
            {
                Write(entry);
            };
            return triggerDataEditor;
        }

        public EntryType GetEntryType(BundleEntry entry)
        {
            return EntryType.TriggerData;
        }

        public bool Write(BundleEntry entry)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(ms);
            writer.Write(miVersionNumber);
            long SizePosition = writer.BaseStream.Position;
            writer.WriteUniquePadding(4);
            writer.WriteUniquePadding(8); //padding
            writer.Write(mPlayerStartPosition.X);
            writer.Write(mPlayerStartPosition.Y);
            writer.Write(mPlayerStartPosition.Z);
            writer.Write(mPlayerStartPosition.S);
            writer.Write(mPlayerStartDirection.X);
            writer.Write(mPlayerStartDirection.Y);
            writer.Write(mPlayerStartDirection.Z);
            writer.Write(mPlayerStartDirection.S);

            long LandmarkOffsetPosition = writer.BaseStream.Position;
            writer.WriteUniquePadding(4);
            writer.Write(mpLandmarks.Count);
            writer.Write(miOnlineLandmarkCount);
            long SignatureStuntskOffsetPosition = writer.BaseStream.Position;
            writer.WriteUniquePadding(4);
            writer.Write(mpSignatureStunts.Length);
            long GenericRegionsOffsetPosition = writer.BaseStream.Position;
            writer.WriteUniquePadding(4);
            writer.Write(mpGenericRegions.Length);
            long KillzoneOffsetPosition = writer.BaseStream.Position;
            writer.WriteUniquePadding(4);
            writer.Write(mpKillzones.Length);
            long BlackspotOffsetPosition = writer.BaseStream.Position;
            writer.WriteUniquePadding(4);
            writer.Write(mpBlackspots.Length);
            long VFXBoxRegionsOffsetPosition = writer.BaseStream.Position;
            writer.WriteUniquePadding(4);
            writer.Write(mpVFXBoxRegions.Length);
            long RoamingLocationsOffsetPosition = writer.BaseStream.Position;
            writer.WriteUniquePadding(4);
            writer.Write(mpRoamingLocations.Length);
            long SpawnLocationsOffsetPosition = writer.BaseStream.Position;
            writer.WriteUniquePadding(4);
            writer.Write(mpSpawnLocations.Length);
            long TriggerOffsetPosition = writer.BaseStream.Position;
            writer.WriteUniquePadding(4);
            writer.Write(mpLandmarks.Count + mpGenericRegions.Length + mpBlackspots.Length + mpVFXBoxRegions.Length);
            writer.WriteUniquePadding(4); // padding


            List<uint> TriggerRegionOffsets = new List<uint>();
            

            long currentPosition = writer.BaseStream.Position;
            writer.BaseStream.Position = LandmarkOffsetPosition;
            writer.Write((uint)currentPosition);
            writer.BaseStream.Position = currentPosition;
            foreach (Landmark landmark in mpLandmarks)
            {
                TriggerRegionOffsets.Add((uint)writer.BaseStream.Position);
                landmark.Write(writer);
            }
            writer.WritePadding();

            currentPosition = writer.BaseStream.Position;
            long offsetForSignatureStunts = writer.BaseStream.Position;
            writer.BaseStream.Position = SignatureStuntskOffsetPosition;
            writer.Write((uint)currentPosition);
            writer.BaseStream.Position = currentPosition;
            
            foreach (SignatureStunt stunt in mpSignatureStunts)
            {
                stunt.WriteWithEmpty(writer);
            }
            writer.WritePadding();

            currentPosition = writer.BaseStream.Position;
            writer.BaseStream.Position = GenericRegionsOffsetPosition;
            writer.Write((uint)currentPosition);
            writer.BaseStream.Position = currentPosition;
            Dictionary<int, uint> genericRegionOffsets = new Dictionary<int, uint>();
            foreach (GenericRegion region in mpGenericRegions)
            {
                genericRegionOffsets.Add(region.mId, (uint)writer.BaseStream.Position);
                TriggerRegionOffsets.Add((uint)writer.BaseStream.Position);
                region.Write(writer);
            }
            writer.WritePadding();

            // Go Back To SignatureStunt and Overwrite it with the actual positions in the file, then go back
            currentPosition = writer.BaseStream.Position;
            writer.BaseStream.Position = offsetForSignatureStunts;
            foreach (SignatureStunt stunt in mpSignatureStunts)
            {
                stunt.Write(writer, genericRegionOffsets);
            }
            writer.BaseStream.Position = currentPosition;

            currentPosition = writer.BaseStream.Position;
            writer.BaseStream.Position = KillzoneOffsetPosition;
            writer.Write((uint)currentPosition);
            writer.BaseStream.Position = currentPosition;
            foreach (Killzone killzone in mpKillzones)
            {
                killzone.Write(writer);
            };
            writer.WritePadding();

            currentPosition = writer.BaseStream.Position;
            writer.BaseStream.Position = BlackspotOffsetPosition;
            writer.Write((uint)currentPosition);
            writer.BaseStream.Position = currentPosition;
            foreach (Blackspot blackspot in mpBlackspots)
            {
                TriggerRegionOffsets.Add((uint)writer.BaseStream.Position);
                blackspot.Write(writer);
            }
            writer.WritePadding();

            currentPosition = writer.BaseStream.Position;
            writer.BaseStream.Position = VFXBoxRegionsOffsetPosition;
            writer.Write((uint)currentPosition);
            writer.BaseStream.Position = currentPosition;
            foreach (VFXBoxRegion region in mpVFXBoxRegions)
            {
                TriggerRegionOffsets.Add((uint)writer.BaseStream.Position);
                region.Write(writer);
            }
            writer.WritePadding();

            currentPosition = writer.BaseStream.Position;
            writer.BaseStream.Position = RoamingLocationsOffsetPosition;
            writer.Write((uint)currentPosition);
            writer.BaseStream.Position = currentPosition;
            foreach (RoamingLocation location in mpRoamingLocations)
            {
                location.Write(writer);
            }
            writer.WritePadding();

            currentPosition = writer.BaseStream.Position;
            writer.BaseStream.Position = SpawnLocationsOffsetPosition;
            writer.Write((uint)currentPosition);
            writer.BaseStream.Position = currentPosition;
            foreach (SpawnLocation location in mpSpawnLocations)
            {
                location.Write(writer);
            }
            writer.WritePadding();

            foreach (Killzone killzone in mpKillzones)
            {
                killzone.WritePointerStuff(writer, genericRegionOffsets);
                
            };

            currentPosition = writer.BaseStream.Position;
            writer.BaseStream.Position = TriggerOffsetPosition;
            writer.Write((uint)currentPosition);
            writer.BaseStream.Position = currentPosition;
            foreach (uint region in TriggerOffsets)
            {
                writer.Write(region);
            }

            currentPosition = writer.BaseStream.Position;
            writer.BaseStream.Position = SizePosition;
            writer.Write((uint)currentPosition);
            writer.BaseStream.Position = currentPosition;
            writer.WritePadding();

            writer.Flush();

            byte[] data = ms.ToArray();

            writer.Close();
            ms.Close();

            entry.EntryBlocks[0].Data = data;
            entry.Dirty = true;

            return true;
        }
    }
}
