using BundleFormat;
using BundleUtilities;
using PluginAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.IO;
using System.Numerics;
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

        public void Read(BinaryReader2 br)
        {
            m_id = br.ReadUInt64();
        }

        public void Write(BinaryWriter2 bw)
        {
            bw.Write(m_id);
        }

        public override string ToString()
        {
            return string.Format("{0:X16}", m_id);
        }
    }

    public class StartingGrid
    {
        public List<Vector4> StartingPositions { get; set; } = new List<Vector4>();
        public List<Vector4> StartingDirections { get; set; } = new List<Vector4>();

        public void Read(BinaryReader2 br)
        {
            StartingPositions = new List<Vector4>(8);
            for (int i = 0; i < 8; i++)
            {
                StartingPositions[i] = new Vector4(
                    br.ReadSingle(),
                    br.ReadSingle(),
                    br.ReadSingle(),
                    br.ReadSingle());
            }

            StartingDirections = new List<Vector4>(8);
            for (int i = 0; i < 8; i++)
            {
                StartingDirections[i] = new Vector4(
                    br.ReadSingle(),
                    br.ReadSingle(),
                    br.ReadSingle(),
                    br.ReadSingle());
            }
        }

        public void Write(BinaryWriter2 bw)
        {
            for (int i = 0; i < 8; i++)
            {
                bw.Write(StartingPositions[i].X);
                bw.Write(StartingPositions[i].Y);
                bw.Write(StartingPositions[i].Z);
                bw.Write(StartingPositions[i].W);
            }

            for (int i = 0; i < 8; i++)
            {
                bw.Write(StartingDirections[i].X);
                bw.Write(StartingDirections[i].Y);
                bw.Write(StartingDirections[i].Z);
                bw.Write(StartingDirections[i].W);
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

        public void Read(BinaryReader2 br)
        {
            PositionX = br.ReadSingle();
            PositionY = br.ReadSingle();
            PositionZ = br.ReadSingle();
            RotationX = br.ReadSingle();
            RotationY = br.ReadSingle();
            RotationZ = br.ReadSingle();
            DimensionX = br.ReadSingle();
            DimensionY = br.ReadSingle();
            DimensionZ = br.ReadSingle();
        }

        public void Write(BinaryWriter2 bw)
        {
            bw.Write(PositionX);
            bw.Write(PositionY);
            bw.Write(PositionZ);
            bw.Write(RotationX);
            bw.Write(RotationY);
            bw.Write(RotationZ);
            bw.Write(DimensionX);
            bw.Write(DimensionY);
            bw.Write(DimensionZ);
        }
    }

    public class TriggerRegion
    {
        public enum RegionType
        {
            E_TYPE_LANDMARK = 0,
            E_TYPE_BLACKSPOT = 1,
            E_TYPE_GENERIC_REGION = 2,
            E_TYPE_VFXBOX_REGION = 3
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public BoxRegion mBoxRegion { get; set; } = new BoxRegion();
        public int mId { get; set; } = 0;
        public short miRegionIndex { get; set; } = 0;
        public RegionType meType { get; set; } = RegionType.E_TYPE_LANDMARK;
        private byte[] muPad { get; set; } = new byte[1];

        public virtual void Read(BinaryReader2 br)
        {
            mBoxRegion = new BoxRegion();
            mBoxRegion.Read(br);
            mId = br.ReadInt32();
            miRegionIndex = br.ReadInt16();
            meType = (RegionType)br.ReadByte();
            muPad = br.ReadBytes(1);
        }

        public virtual void Write(BinaryWriter2 bw)
        {
            mBoxRegion.Write(bw);

            bw.Write(mId);
            bw.Write(miRegionIndex);
            bw.Write((byte)meType);
            bw.Write(muPad);
        }
    }

    public class Landmark : TriggerRegion
    {
        public Landmark() : base()
        {
            // Set meType to E_TYPE_LANDMARK
            meType = RegionType.E_TYPE_LANDMARK;
        }

        public List<StartingGrid> mpaStartingGrids { get; set; } = new List<StartingGrid>();

        private long startingGridOffsetPosition = 0;
        public byte muDesignIndex { get; set; } = 0;
        public byte muDistrict { get; set; } = 0;
        public byte mu8Flags { get; set; } = 0;

        public new void Read(BinaryReader2 br)
        {
            base.Read(br);
            long startingGridOffset = br.ReadUInt32();
            int miStartingGridCount = br.ReadByte();
            muDesignIndex = br.ReadByte();
            muDistrict = br.ReadByte();
            mu8Flags = br.ReadByte();

            long currentPosition = br.BaseStream.Position;
            br.BaseStream.Position = startingGridOffset;

            for (int i = 0; i < miStartingGridCount; i++)
            {
                StartingGrid startingGrid = new StartingGrid();
                startingGrid.Read(br);
                mpaStartingGrids.Add(startingGrid);
            }
            br.BaseStream.Position = currentPosition;

        }

        public new void Write(BinaryWriter2 bw)
        {
            base.Write(bw);
            startingGridOffsetPosition = bw.BaseStream.Position;
            bw.WriteUniquePadding(4);
            bw.Write((byte)mpaStartingGrids.Count);
            bw.Write(muDesignIndex);
            bw.Write(muDistrict);
            bw.Write(mu8Flags);
        }

        public void WriteStartingGrid(BinaryWriter2 bw){

            long currentPosition = bw.BaseStream.Position;
            bw.BaseStream.Position = startingGridOffsetPosition;
            bw.Write((uint)currentPosition);
            bw.BaseStream.Position = currentPosition;
            foreach (StartingGrid grid in mpaStartingGrids) {
                grid.Write(bw);
            }
        }
    }

    public class GenericRegion : TriggerRegion
    {
        public enum StuntCameraType
        {
            E_STUNT_CAMERA_TYPE_NO_CUTS = 0,
            E_STUNT_CAMERA_TYPE_CUSTOM = 1,
            E_STUNT_CAMERA_TYPE_NORMAL = 2
        }

        public enum Type
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
            E_TYPE_ISLAND_ENTITLEMENT = 34
        }

        public GenericRegion() : base()
        {
            // Set meType to E_TYPE_GENERIC_REGION
            meType = RegionType.E_TYPE_GENERIC_REGION;
        }

        public int GroupID { get; set; } = 0;
        public short CameraCut1 { get; set; } = 0;
        public short CameraCut2 { get; set; } = 0;
        public StuntCameraType CameraType1 { get; set; } = 0;
        public StuntCameraType CameraType2 { get; set; } = 0;
        public Type GenericRegionType { get; set; } = Type.E_TYPE_JUNK_YARD;
        public sbyte IsOneWay { get; set; } = 0;

        public new void Read(BinaryReader2 br)
        {
            base.Read(br);
            GroupID = br.ReadInt32();
            CameraCut1 = br.ReadInt16();
            CameraCut2 = br.ReadInt16();
            CameraType1 = (StuntCameraType)br.ReadSByte();
            CameraType2 = (StuntCameraType)br.ReadSByte();
            GenericRegionType = (Type)br.ReadByte();
            IsOneWay = br.ReadSByte();
        }

        public new void Write(BinaryWriter2 bw)
        {
            base.Write(bw);
            bw.Write(GroupID);
            bw.Write(CameraCut1);
            bw.Write(CameraCut2);
            bw.Write((sbyte)CameraType1);
            bw.Write((sbyte)CameraType2);
            bw.Write((byte)GenericRegionType);
            bw.Write(IsOneWay);
        }
    }

    public class Blackspot : TriggerRegion
    {
        public enum ScoreType
        {
            E_SCORE_TYPE_DISTANCE = 0,
            E_SCORE_TYPE_CAR_COUNT = 1
        }

        public Blackspot() : base()
        {
            // Set meType to E_TYPE_BLACKSPOT
            meType = RegionType.E_TYPE_BLACKSPOT;
        }

        public ScoreType muScoreType { get; set; } = ScoreType.E_SCORE_TYPE_DISTANCE;
        public int miScoreAmount { get; set; } = 0;

        public override void Read(BinaryReader2 br)
        {
            base.Read(br);

            muScoreType = (ScoreType)br.ReadByte();
            br.ReadBytes(3); // Padding
            miScoreAmount = br.ReadInt32();
        }

        public override void Write(BinaryWriter2 bw)
        {
            base.Write(bw);

            bw.Write((byte)muScoreType);
            bw.Write(new byte[3]); // Padding
            bw.Write(miScoreAmount);
        }
    }

    public class Killzone 
    {
        [Description("Triggers as GenericRegions. Uses region.mId")]
        public List<int> TriggerIds { get; set; } = new List<int>();
        public List<CgsID> RegionIds { get; set; } = new List<CgsID>();

        private long TriggerOffsetPosition = 0;

        private long CGSIDOffsetPosition = 0;
        public void Read(BinaryReader2 br)
        {
            // Read the trigger pointer array
            long genericRegionOffset = br.ReadUInt32();
            int TriggerCount = br.ReadInt32();

            // Read the region ID array
            long cgsOffset = br.ReadUInt32();
            int RegionIdCount = br.ReadInt32();
            long currentPosition = br.BaseStream.Position;

            br.BaseStream.Position = genericRegionOffset;
            uint[]  Triggers = new uint[TriggerCount];
            for (int i = 0; i < TriggerCount; i++)
            {
                Triggers[i] = br.ReadUInt32();
            }

            TriggerIds = new List<int>();
            foreach (uint trigger in Triggers)
            {
                br.BaseStream.Position = trigger;
                GenericRegion region = new GenericRegion();
                region.Read(br);
                TriggerIds.Add(region.mId);
            }

            br.BaseStream.Position = cgsOffset;
            RegionIds = new List<CgsID>();
            for (int i = 0; i < RegionIdCount; i++)
            {
                CgsID id = new CgsID();
                id.Read(br);
                RegionIds.Add(id);
            }

            br.BaseStream.Position = currentPosition;
        }

        public void Write(BinaryWriter2 bw)
        {
            TriggerOffsetPosition = bw.BaseStream.Position;
            bw.WriteUniquePadding(4);
            bw.Write(TriggerIds.Count);
            CGSIDOffsetPosition = bw.BaseStream.Position;
            bw.WriteUniquePadding(4);
            bw.Write(RegionIds.Count);            
        }

        public void WritePointerStuff(BinaryWriter2 bw, Dictionary<int, uint> genericRegionOffsets) {

            long currentPosition = bw.BaseStream.Position;
            bw.BaseStream.Position = TriggerOffsetPosition;
            bw.Write((uint)currentPosition);
            bw.BaseStream.Position = currentPosition;
            foreach (int trigger in TriggerIds)
            {
                bw.Write(genericRegionOffsets[trigger]);
            }
            long paddingCount = 16 - (bw.BaseStream.Position % 16);
            if (paddingCount < 16)
            {
                for (int i = 0; i < paddingCount; i++)
                    bw.Write((byte)0);
            }
            currentPosition = bw.BaseStream.Position;
            bw.BaseStream.Position = CGSIDOffsetPosition;
            bw.Write((uint)currentPosition);
            bw.BaseStream.Position = currentPosition;
            foreach (CgsID ids in RegionIds)
            {
                ids.Write(bw);
            }
            paddingCount = 16 - (bw.BaseStream.Position % 16);
            if (paddingCount < 16)
            {
                for (int i = 0; i < paddingCount; i++)
                    bw.Write((byte)0);
            }
        }
    }

    public class SignatureStunt
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public CgsID mId { get; set; } = new CgsID();
        public long miCamera { get; set; } = 0;
        public List<int> stuntElementRegions { get; set; } = new List<int>();

        private long StuntElementOffsetPosition = 0;

        public void Read(BinaryReader2 br)
        {
            mId = new CgsID();
            mId.Read(br);
            miCamera = br.ReadInt64();

            uint mppStuntElementsOffset = br.ReadUInt32();
            int miStuntElementCount = br.ReadInt32();

            long currentPosition = br.BaseStream.Position;
            br.BaseStream.Position = mppStuntElementsOffset;
            uint[] Triggers = new uint[miStuntElementCount];
            for (int i = 0; i < miStuntElementCount; i++)
            {
                Triggers[i] = br.ReadUInt32();
            }

            stuntElementRegions = new List<int>();
            foreach (uint trigger in Triggers)
            {
                br.BaseStream.Position = trigger;
                GenericRegion region = new GenericRegion();
                region.Read(br);
                stuntElementRegions.Add(region.mId);
            }
            br.BaseStream.Position = currentPosition;
        }

        // Write something in triggerIds, because we dont have the actual positions yet
        public void Write(BinaryWriter2 bw)
        {
            mId.Write(bw);
            bw.Write(miCamera);
            StuntElementOffsetPosition = bw.BaseStream.Position;
            bw.WriteUniquePadding(4);
            bw.Write(stuntElementRegions.Count);
        }

        public void WriteStuntElements(BinaryWriter2 bw, Dictionary<int, uint> genericRegionOffsets) {
            long currentPosition = bw.BaseStream.Position;
            bw.BaseStream.Position = StuntElementOffsetPosition;
            bw.Write(currentPosition);
            bw.BaseStream.Position = currentPosition;
            foreach (int trigger in stuntElementRegions)
            {
                bw.Write(genericRegionOffsets[trigger]);
            }
        }
    }

    public class RoamingLocation
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Vector4 Position { get; set; } = new Vector4(0,0,0,0);
        public byte DistrictIndex { get; set; } = 0;

        public void Read(BinaryReader2 br)
        {
            Position = new Vector4(
                br.ReadSingle(),
                br.ReadSingle(),
                br.ReadSingle(),
                br.ReadSingle());
            DistrictIndex = br.ReadByte();
            // Read and discard padding
            br.ReadBytes(15);
        }

        public void Write(BinaryWriter2 bw)
        {
            bw.Write(Position.X);
            bw.Write(Position.Y);
            bw.Write(Position.Z);
            bw.Write(Position.W);
            bw.Write(DistrictIndex);

            // Write padding
            bw.Write(new byte[15]);
        }
    }

    public class VFXBoxRegion : TriggerRegion
    {
        public VFXBoxRegion() : base()
        {
            // Set meType to E_TYPE_VFXBOX_REGION
            meType = RegionType.E_TYPE_VFXBOX_REGION;
        }

        public override void Read(BinaryReader2 br)
        {
            base.Read(br);
        }

        public override void Write(BinaryWriter2 bw)
        {
            base.Write(bw);
        }
    }

    public class SpawnLocation
    {
        public enum Type
        {
            E_TYPE_PLAYER_SPAWN = 0,
            E_TYPE_CAR_SELECT_LEFT = 1,
            E_TYPE_CAR_SELECT_RIGHT = 2,
            E_TYPE_CAR_UNLOCK = 3
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Vector4 mPosition { get; set; } = new Vector4(0, 0, 0, 0);
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Vector4 mDirection { get; set; } = new Vector4(0, 0, 0, 0);
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public CgsID mJunkyardId { get; set; } = new CgsID();
        public Type muType { get; set; } = 0;
        private byte[] padding = new byte[7];

        public void Read(BinaryReader2 br)
        {
            mPosition = new Vector4(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            mDirection = new Vector4(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            mJunkyardId = new CgsID();
            mJunkyardId.Read(br);
            muType = (Type)br.ReadByte();
            padding = br.ReadBytes(7);
        }

        public void Write(BinaryWriter2 bw)
        {
            bw.Write(mPosition.X);
            bw.Write(mPosition.Y);
            bw.Write(mPosition.Z);
            bw.Write(mPosition.W);
            bw.Write(mDirection.X);
            bw.Write(mDirection.Y);
            bw.Write(mDirection.Z);
            bw.Write(mDirection.W);
            mJunkyardId.Write(bw);
            bw.Write((byte)muType);
            bw.Write(padding);
        }
    }

    public class TriggerData : IEntryData
    {
        public int miVersionNumber { get; set; }
        public uint muSize { get; set; }
        public Vector4 mPlayerStartPosition { get; set; } 
        public Vector4 mPlayerStartDirection { get; set; } 
        public List<Landmark> mpLandmarks { get; set; }
        public int miOnlineLandmarkCount { get; set; }
        public List<SignatureStunt> mpSignatureStunts { get; set; }
        public List<GenericRegion> mpGenericRegions { get; set; }

        [Editor(typeof(DescriptiveCollectionEditor), typeof(UITypeEditor))]
        public List<Killzone> mpKillzones { get; set; }
        public List<Blackspot> mpBlackspots { get; set; }
        public List<VFXBoxRegion> mpVFXBoxRegions { get; set; }
        public List<RoamingLocation> mpRoamingLocations { get; set; }
        public List<SpawnLocation> mpSpawnLocations { get; set; }
        private List<uint> TriggerOffsets { get; set; }

        public bool Read(BundleEntry entry, ILoader loader = null)
        {
            MemoryStream ms = entry.MakeStream();
            BinaryReader2 reader = new BinaryReader2(ms);
            reader.BigEndian = entry.Console;

            miVersionNumber = reader.ReadInt32();
            muSize = reader.ReadUInt32();
            reader.ReadBytes(8);// skip 8 bytes of padding
            mPlayerStartPosition = new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            mPlayerStartDirection = new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
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
            mpSignatureStunts = new List<SignatureStunt>();
            for (int i = 0; i < miSignatureStuntCount; i++)
            {
                SignatureStunt stunt = new SignatureStunt();
                stunt.Read(reader);
                mpSignatureStunts.Add(stunt);
            }
            
            reader.BaseStream.Position = GenericRegionsOffset;
            // read generic regions
            mpGenericRegions = new List<GenericRegion>();
            for (int i = 0; i < miGenericRegionCount; i++)
            {
                GenericRegion region = new GenericRegion();
                region.Read(reader);
                mpGenericRegions.Add(region);
            }
            

            reader.BaseStream.Position = KillzoneOffset;
            // read killzones
            mpKillzones = new List<Killzone>();
            for (int i = 0; i < miKillzoneCount; i++)
            {
                Killzone killzone = new Killzone();
                killzone.Read(reader);
                mpKillzones.Add(killzone);
            }

            // read blackspots
            reader.BaseStream.Position = BlackspotOffset;
            mpBlackspots = new List<Blackspot>();
            for (int i = 0; i < miBlackspotCount; i++)
            {
                Blackspot spot = new Blackspot();
                spot.Read(reader);
                mpBlackspots.Add(spot);
            }

            reader.BaseStream.Position = VFXBoxRegionOffset;

            // read VFX box regions
            mpVFXBoxRegions = new List<VFXBoxRegion>();
            for (int i = 0; i < miVFXBoxRegionCount; i++)
            {
                VFXBoxRegion box = new VFXBoxRegion();
                box.Read(reader);
                mpVFXBoxRegions.Add(box);
            }

            reader.BaseStream.Position = RoamingLocationOffset;
            
            // read roaming locations
            mpRoamingLocations = new List<RoamingLocation>();
            for (int i = 0; i < miRoamingLocationCount; i++)
            {
                RoamingLocation location = new RoamingLocation();
                location.Read(reader);
                mpRoamingLocations.Add(location);
            }
            
            reader.BaseStream.Position = SpawnLocationOffset;

            mpSpawnLocations = new List<SpawnLocation>();
            for (int i = 0; i < miSpawnLocationCount; i++)
            {
                SpawnLocation location = new SpawnLocation();
                location.Read(reader);
                mpSpawnLocations.Add(location);
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
            BinaryWriter2 bw = new BinaryWriter2(ms);
            bw.BigEndian = entry.Console;
            bw.Write(miVersionNumber);
            long SizePosition = bw.BaseStream.Position;
            bw.WriteUniquePadding(4);
            bw.WriteUniquePadding(8); //padding
            bw.Write(mPlayerStartPosition.X);
            bw.Write(mPlayerStartPosition.Y);
            bw.Write(mPlayerStartPosition.Z);
            bw.Write(mPlayerStartPosition.W);
            bw.Write(mPlayerStartDirection.X);
            bw.Write(mPlayerStartDirection.Y);
            bw.Write(mPlayerStartDirection.Z);
            bw.Write(mPlayerStartDirection.W);

            long LandmarkOffsetPosition = bw.BaseStream.Position;
            bw.WriteUniquePadding(4);
            bw.Write(mpLandmarks.Count);
            bw.Write(miOnlineLandmarkCount);
            long SignatureStuntskOffsetPosition = bw.BaseStream.Position;
            bw.WriteUniquePadding(4);
            bw.Write(mpSignatureStunts.Count);
            long GenericRegionsOffsetPosition = bw.BaseStream.Position;
            bw.WriteUniquePadding(4);
            bw.Write(mpGenericRegions.Count);
            long KillzoneOffsetPosition = bw.BaseStream.Position;
            bw.WriteUniquePadding(4);
            bw.Write(mpKillzones.Count);
            long BlackspotOffsetPosition = bw.BaseStream.Position;
            bw.WriteUniquePadding(4);
            bw.Write(mpBlackspots.Count);
            long VFXBoxRegionsOffsetPosition = bw.BaseStream.Position;
            bw.WriteUniquePadding(4);
            bw.Write(mpVFXBoxRegions.Count);
            long RoamingLocationsOffsetPosition = bw.BaseStream.Position;
            bw.WriteUniquePadding(4);
            bw.Write(mpRoamingLocations.Count);
            long SpawnLocationsOffsetPosition = bw.BaseStream.Position;
            bw.WriteUniquePadding(4);
            bw.Write(mpSpawnLocations.Count);
            long TriggerOffsetPosition = bw.BaseStream.Position;
            bw.WriteUniquePadding(4);
            bw.Write(mpLandmarks.Count + mpGenericRegions.Count + mpBlackspots.Count + mpVFXBoxRegions.Count);
            bw.WriteUniquePadding(4); // padding

            long currentPosition = bw.BaseStream.Position;
            bw.BaseStream.Position = LandmarkOffsetPosition;
            bw.Write((uint)currentPosition);
            bw.BaseStream.Position = currentPosition;
            List<uint> landmarkOffsets = new List<uint>();
            foreach (Landmark landmark in mpLandmarks)
            {
                landmarkOffsets.Add((uint)bw.BaseStream.Position);
                landmark.Write(bw);
            }
            bw.WritePadding();

            currentPosition = bw.BaseStream.Position;
            bw.BaseStream.Position = SignatureStuntskOffsetPosition;
            bw.Write((uint)currentPosition);
            bw.BaseStream.Position = currentPosition;
            foreach (SignatureStunt stunt in mpSignatureStunts)
            {
                stunt.Write(bw);
            }
            bw.WritePadding();

            currentPosition = bw.BaseStream.Position;
            bw.BaseStream.Position = GenericRegionsOffsetPosition;
            bw.Write((uint)currentPosition);
            bw.BaseStream.Position = currentPosition;
            Dictionary<int, uint> genericRegionOffsets = new Dictionary<int, uint>();
            foreach (GenericRegion region in mpGenericRegions)
            {
                genericRegionOffsets.Add(region.mId, (uint)bw.BaseStream.Position);
                region.Write(bw);
            }
            bw.WritePadding();

            currentPosition = bw.BaseStream.Position;
            bw.BaseStream.Position = KillzoneOffsetPosition;
            bw.Write((uint)currentPosition);
            bw.BaseStream.Position = currentPosition;
            foreach (Killzone killzone in mpKillzones)
            {
                killzone.Write(bw);
            };
            bw.WritePadding();

            currentPosition = bw.BaseStream.Position;
            bw.BaseStream.Position = BlackspotOffsetPosition;
            bw.Write((uint)currentPosition);
            bw.BaseStream.Position = currentPosition;
            List<uint> blackspotOffsets = new List<uint>();
            foreach (Blackspot blackspot in mpBlackspots)
            {
                blackspotOffsets.Add((uint)bw.BaseStream.Position);
                blackspot.Write(bw);
            }
            bw.WritePadding();

            currentPosition = bw.BaseStream.Position;
            bw.BaseStream.Position = VFXBoxRegionsOffsetPosition;
            bw.Write((uint)currentPosition);
            bw.BaseStream.Position = currentPosition;
            List<uint> vfxBoxRegionOffsets = new List<uint>();
            foreach (VFXBoxRegion region in mpVFXBoxRegions)
            {
                vfxBoxRegionOffsets.Add((uint)bw.BaseStream.Position);
                region.Write(bw);
            }
            bw.WritePadding();

            currentPosition = bw.BaseStream.Position;
            bw.BaseStream.Position = RoamingLocationsOffsetPosition;
            bw.Write((uint)currentPosition);
            bw.BaseStream.Position = currentPosition;
            foreach (RoamingLocation location in mpRoamingLocations)
            {
                location.Write(bw);
            }
            bw.WritePadding();

            currentPosition = bw.BaseStream.Position;
            bw.BaseStream.Position = SpawnLocationsOffsetPosition;
            bw.Write((uint)currentPosition);
            bw.BaseStream.Position = currentPosition;
            foreach (SpawnLocation location in mpSpawnLocations)
            {
                location.Write(bw);
            }
            bw.WritePadding();

            foreach (Landmark land in mpLandmarks) {
                land.WriteStartingGrid(bw);
            }

            foreach (SignatureStunt stunt in mpSignatureStunts)
            {
                stunt.WriteStuntElements(bw, genericRegionOffsets);
            }

            foreach (Killzone killzone in mpKillzones)
            {
                killzone.WritePointerStuff(bw, genericRegionOffsets);
            };

            currentPosition = bw.BaseStream.Position;
            bw.BaseStream.Position = TriggerOffsetPosition;
            bw.Write((uint)currentPosition);
            bw.BaseStream.Position = currentPosition;
            foreach (uint region in vfxBoxRegionOffsets) bw.Write(region);
            foreach (uint region in blackspotOffsets) bw.Write(region);
            foreach (uint region in genericRegionOffsets.Values) bw.Write(region);
            foreach (uint region in landmarkOffsets) bw.Write(region);

            currentPosition = bw.BaseStream.Position;
            bw.BaseStream.Position = SizePosition;
            bw.Write((uint)currentPosition);
            bw.BaseStream.Position = currentPosition;
            bw.WritePadding();

            bw.Flush();

            byte[] data = ms.ToArray();

            bw.Close();
            ms.Close();

            entry.EntryBlocks[0].Data = data;
            entry.Dirty = true;

            return true;
        }
    }
}
