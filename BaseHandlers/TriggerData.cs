using BundleFormat;
using BundleUtilities;
using PluginAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BaseHandlers
{
    public struct LandmarkTrigger
    {
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }
        public float SizeX { get; set; }
        public float SizeY { get; set; }
        public float SizeZ { get; set; }
        public int GameDBID { get; set; }
        public short GlobalIndex { get; set; }
        public byte Type { get; set; } // Should be 0
        public byte UnknownByte2B { get; set; }
        public uint UnknownOffset { get; set; }
        public byte UnknownByte30 { get; set; }
        public byte LocalIndex { get; set; }
        public byte Subtype { get; set; }
        public byte UnknownByte33 { get; set; }

        public override string ToString() => $"ID {GameDBID}";
    }

    public struct GenericRegionTrigger
    {
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }
        public float SizeX { get; set; }
        public float SizeY { get; set; }
        public float SizeZ { get; set; }
        public int GameDBID { get; set; }
        public short Index { get; set; }
        public byte Type { get; set; } // Should be 2
        public byte UnknownByte2B { get; set; }
        public int GameDBID2 { get; set; } // Only used for _some_ super jumps
        public short UnknownShort30 { get; set; }
        public short UnknownShort32 { get; set; }
        public byte UnknownByte34 { get; set; }
        public byte UnknownByte35 { get; set; }
        public byte Subtype { get; set; }
        public byte UnknownByte37 { get; set; } // most, but not all, super jumps have this set (to 1)
        public override string ToString() => $"ID {GameDBID}";
    }

    public struct TriggerSection4Entry
    {
        public uint TriggerOffsetListOffset { get; set; }
        public int TriggerOffsetListCount { get; set; }
        public uint GameDBIDListOffset { get; set; }
        public int GameDBIDListCount { get; set; }

        public List<GenericRegionTrigger> Triggers { get; set; }
        public List<long> GameDBIDs { get; set; }
    }

    public struct RoamingLocation
    {
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public uint UnknownHash { get; set; }
        public byte Subdistrict { get; set; }
        public byte UnknownByte11 { get; set; }
        public byte UnknownByte12 { get; set; }
        public byte UnknownByte13 { get; set; }
        public int UnknownInt14 { get; set; }
        public int UnknownInt18 { get; set; }
        public int UnknownInt1C { get; set; }
    }

    public struct SpawnLocation
    {
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public uint UnknownHash { get; set; }
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }
        public float UnknownFloat1C { get; set; }
        public long JunkyardGameDBID { get; set; }
        public GenericRegionTrigger JunkyardTrigger { get; set; }
        public byte UnknownByte28 { get; set; }
        public byte UnknownByte29 { get; set; }
        public byte UnknownByte30 { get; set; }
        public byte UnknownByte31 { get; set; }
        public int UnknownInt32 { get; set; }
    }

    public class TriggerData : IEntryData
    {
        public int FormatRevision { get; set; }
        public uint FileSize { get; set; }
        public int Unknown0C { get; set; }
        public int Unknown10 { get; set; }
        public float DevSpawnPositionX { get; set; }
        public float DevSpawnPositionY { get; set; }
        public float DevSpawnPositionZ { get; set; }
        public uint DevSpawnUnknownHash { get; set; }
        public float DevSpawnRotationX { get; set; }
        public float DevSpawnRotationY { get; set; }
        public float DevSpawnRotationZ { get; set; }
        public float DevSpawnUnknownFloat { get; set; }
        public uint LandmarkTriggersOffset { get; set; }
        public int LandmarkTriggersCount { get; set; }
        public int LandmarkNonFinishLineCount { get; set; }
        public uint BlackspotTriggersOffset { get; set; }
        public int BlackspotTriggersCount { get; set; }
        public uint GenericRegionTriggersOffset { get; set; }
        public int GenericRegionTriggersCount { get; set; }
        public uint Section4Offset { get; set; }
        public int Section4Count { get; set; }
        public uint VFXBoxRegionOffset { get; set; }
        public int VFXBoxRegionCount { get; set; }
        public uint StartPositionsOffset { get; set; }
        public int StartPositionsCount { get; set; }
        public uint RoamingLocationsOffset { get; set; }
        public int RoamingLocationsCount { get; set; }
        public uint SpawnLocationsOffset { get; set; }
        public int SpawnLocationsCount { get; set; }
        public uint TriggerOffsetListOffset { get; set; }
        public int TriggerOffsetListCount { get; set; }

        public List<LandmarkTrigger> LandmarkTriggers { get; set; }
        public List<GenericRegionTrigger> GenericRegionTriggers { get; set; }
        public List<TriggerSection4Entry> Section4Entries { get; set; }
        public List<RoamingLocation> RoamingLocationEntries { get; set; }
        public List<SpawnLocation> SpawnLocationEntries { get; set; }
        public List<uint> TriggerOffsets { get; set; }

        public TriggerData()
        {
            LandmarkTriggers = new List<LandmarkTrigger>();
            GenericRegionTriggers = new List<GenericRegionTrigger>();
            Section4Entries = new List<TriggerSection4Entry>();
            RoamingLocationEntries = new List<RoamingLocation>();
            SpawnLocationEntries = new List<SpawnLocation>();
            TriggerOffsets = new List<uint>();
        }

        private void Clear()
        {
            FormatRevision = default;
            FileSize = default;
            Unknown0C = default;
            Unknown10 = default;
            DevSpawnPositionX = default;
            DevSpawnPositionY = default;
            DevSpawnPositionZ = default;
            DevSpawnUnknownHash = default;
            DevSpawnRotationX = default;
            DevSpawnRotationY = default;
            DevSpawnRotationZ = default;
            DevSpawnUnknownFloat = default;
            LandmarkTriggersOffset = default;
            LandmarkTriggersCount = default;
            LandmarkNonFinishLineCount = default;
            BlackspotTriggersOffset = default;
            BlackspotTriggersCount = default;
            GenericRegionTriggersOffset = default;
            GenericRegionTriggersCount = default;
            Section4Offset = default;
            Section4Count = default;
            VFXBoxRegionOffset = default;
            VFXBoxRegionCount = default;
            StartPositionsOffset = default;
            StartPositionsCount = default;
            RoamingLocationsOffset = default;
            RoamingLocationsCount = default;
            SpawnLocationsOffset = default;
            SpawnLocationsCount = default;
            TriggerOffsetListOffset = default;
            TriggerOffsetListCount = default;

            LandmarkTriggers.Clear();
            GenericRegionTriggers.Clear();
            Section4Entries.Clear();
            RoamingLocationEntries.Clear();
            SpawnLocationEntries.Clear();
            TriggerOffsets.Clear();
        }

        public bool Read(BundleEntry entry, ILoader loader = null)
        {
            Clear();

            MemoryStream ms = entry.MakeStream();
            BinaryReader2 br = new BinaryReader2(ms);
            br.BigEndian = entry.Console;

            FormatRevision = br.ReadInt32();
            FileSize = br.ReadUInt32();
            Unknown0C = br.ReadInt32();
            Unknown10 = br.ReadInt32();
            DevSpawnPositionX = br.ReadSingle();
            DevSpawnPositionY = br.ReadSingle();
            DevSpawnPositionZ = br.ReadSingle();
            DevSpawnUnknownHash = br.ReadUInt32();
            DevSpawnRotationX = br.ReadSingle();
            DevSpawnRotationY = br.ReadSingle();
            DevSpawnRotationZ = br.ReadSingle();
            DevSpawnUnknownFloat = br.ReadSingle();
            LandmarkTriggersOffset = br.ReadUInt32();
            LandmarkTriggersCount = br.ReadInt32();
            LandmarkNonFinishLineCount = br.ReadInt32();
            BlackspotTriggersOffset = br.ReadUInt32();
            BlackspotTriggersCount = br.ReadInt32();
            GenericRegionTriggersOffset = br.ReadUInt32();
            GenericRegionTriggersCount = br.ReadInt32();
            Section4Offset = br.ReadUInt32();
            Section4Count = br.ReadInt32();
            VFXBoxRegionOffset = br.ReadUInt32();
            VFXBoxRegionCount = br.ReadInt32();
            StartPositionsOffset = br.ReadUInt32();
            StartPositionsCount = br.ReadInt32();
            RoamingLocationsOffset = br.ReadUInt32();
            RoamingLocationsCount = br.ReadInt32();
            SpawnLocationsOffset = br.ReadUInt32();
            SpawnLocationsCount = br.ReadInt32();
            TriggerOffsetListOffset = br.ReadUInt32();
            TriggerOffsetListCount = br.ReadInt32();

            br.BaseStream.Position = LandmarkTriggersOffset;

            for (int i = 0; i < LandmarkTriggersCount; i++)
            {
                LandmarkTrigger landmarkTrigger = new LandmarkTrigger();

                landmarkTrigger.PositionX = br.ReadSingle();
                landmarkTrigger.PositionY = br.ReadSingle();
                landmarkTrigger.PositionZ = br.ReadSingle();
                landmarkTrigger.RotationX = br.ReadSingle();
                landmarkTrigger.RotationY = br.ReadSingle();
                landmarkTrigger.RotationZ = br.ReadSingle();
                landmarkTrigger.SizeX = br.ReadSingle();
                landmarkTrigger.SizeY = br.ReadSingle();
                landmarkTrigger.SizeZ = br.ReadSingle();
                landmarkTrigger.GameDBID = br.ReadInt32();
                landmarkTrigger.GlobalIndex = br.ReadInt16();
                landmarkTrigger.Type = br.ReadByte();
                landmarkTrigger.UnknownByte2B = br.ReadByte();
                landmarkTrigger.UnknownOffset = br.ReadUInt32();
                landmarkTrigger.UnknownByte30 = br.ReadByte();
                landmarkTrigger.LocalIndex = br.ReadByte();
                landmarkTrigger.Subtype = br.ReadByte();
                landmarkTrigger.UnknownByte33 = br.ReadByte();

                LandmarkTriggers.Add(landmarkTrigger);
            }

            br.BaseStream.Position = GenericRegionTriggersOffset;

            for (int i = 0; i < GenericRegionTriggersCount; i++)
            {
                GenericRegionTrigger genericRegionTrigger = new GenericRegionTrigger();
                genericRegionTrigger.PositionX = br.ReadSingle();
                genericRegionTrigger.PositionY = br.ReadSingle();
                genericRegionTrigger.PositionZ = br.ReadSingle();
                genericRegionTrigger.RotationX = br.ReadSingle();
                genericRegionTrigger.RotationY = br.ReadSingle();
                genericRegionTrigger.RotationZ = br.ReadSingle();
                genericRegionTrigger.SizeX = br.ReadSingle();
                genericRegionTrigger.SizeY = br.ReadSingle();
                genericRegionTrigger.SizeZ = br.ReadSingle();
                genericRegionTrigger.GameDBID = br.ReadInt32();
                genericRegionTrigger.Index = br.ReadInt16();
                genericRegionTrigger.Type = br.ReadByte();
                genericRegionTrigger.UnknownByte2B = br.ReadByte();
                genericRegionTrigger.GameDBID2 = br.ReadInt32();
                genericRegionTrigger.UnknownShort30 = br.ReadInt16();
                genericRegionTrigger.UnknownShort32 = br.ReadInt16();
                genericRegionTrigger.UnknownByte34 = br.ReadByte();
                genericRegionTrigger.UnknownByte35 = br.ReadByte();
                genericRegionTrigger.Subtype = br.ReadByte();
                genericRegionTrigger.UnknownByte37 = br.ReadByte();

                GenericRegionTriggers.Add(genericRegionTrigger);
            }

            br.BaseStream.Position = Section4Offset;

            for (int i = 0; i < Section4Count; i++)
            {
                TriggerSection4Entry section4Entry = new TriggerSection4Entry();

                section4Entry.TriggerOffsetListOffset = br.ReadUInt32();
                section4Entry.TriggerOffsetListCount = br.ReadInt32();
                section4Entry.GameDBIDListOffset = br.ReadUInt32();
                section4Entry.GameDBIDListCount = br.ReadInt32();

                long oldPosition = br.BaseStream.Position;

                br.BaseStream.Position = section4Entry.TriggerOffsetListOffset;
                section4Entry.Triggers = new List<GenericRegionTrigger>();
                for (int j = 0; j < section4Entry.TriggerOffsetListCount; j++)
                {
                    GenericRegionTrigger region = new GenericRegionTrigger();
                    region.PositionX = br.ReadSingle();
                    region.PositionY = br.ReadSingle();
                    region.PositionZ = br.ReadSingle();
                    region.RotationX = br.ReadSingle();
                    region.RotationY = br.ReadSingle();
                    region.RotationZ = br.ReadSingle();
                    region.SizeX = br.ReadSingle();
                    region.SizeY = br.ReadSingle();
                    region.SizeZ = br.ReadSingle();
                    region.GameDBID = br.ReadInt32();
                    region.Index = br.ReadInt16();
                    region.Type  =  br.ReadByte();
                    region.UnknownByte2B = br.ReadByte();
                    region.GameDBID2 = br.ReadInt32();
                    region.UnknownShort30 = br.ReadInt16();
                    region.UnknownShort32 = br.ReadInt16();
                    region.UnknownByte34 = br.ReadByte();
                    region.UnknownByte35 = br.ReadByte();
                    region.Subtype =  br.ReadByte();
                    region.UnknownByte37 = br.ReadByte();
                    section4Entry.Triggers.Add(region);
                }

                br.BaseStream.Position = section4Entry.GameDBIDListOffset;
                section4Entry.GameDBIDs = new List<long>();
                for (int j = 0; j < section4Entry.GameDBIDListCount; j++)
                {
                    section4Entry.GameDBIDs.Add(br.ReadInt64());
                }

                br.BaseStream.Position = oldPosition;

                Section4Entries.Add(section4Entry);
            }

            br.BaseStream.Position = RoamingLocationsOffset;

            for (int i = 0; i < RoamingLocationsCount; i++)
            {
                RoamingLocation roamingLocation = new RoamingLocation();

                roamingLocation.PositionX = br.ReadSingle();
                roamingLocation.PositionY = br.ReadSingle();
                roamingLocation.PositionZ = br.ReadSingle();
                roamingLocation.UnknownHash = br.ReadUInt32();
                roamingLocation.Subdistrict = br.ReadByte();
                roamingLocation.UnknownByte11 = br.ReadByte();
                roamingLocation.UnknownByte12 = br.ReadByte();
                roamingLocation.UnknownByte13 = br.ReadByte();
                roamingLocation.UnknownInt14 = br.ReadInt32();
                roamingLocation.UnknownInt18 = br.ReadInt32();
                roamingLocation.UnknownInt1C = br.ReadInt32();

                RoamingLocationEntries.Add(roamingLocation);
            }

            br.BaseStream.Position = SpawnLocationsOffset;

            for (int i = 0; i < SpawnLocationsCount; i++)
            {
                SpawnLocation spawnLocation = new SpawnLocation();

                spawnLocation.PositionX = br.ReadSingle();
                spawnLocation.PositionY = br.ReadSingle();
                spawnLocation.PositionZ = br.ReadSingle();
                spawnLocation.UnknownHash = br.ReadUInt32();
                spawnLocation.RotationX = br.ReadSingle();
                spawnLocation.RotationY = br.ReadSingle();
                spawnLocation.RotationZ = br.ReadSingle();
                spawnLocation.UnknownFloat1C = br.ReadSingle();
                spawnLocation.JunkyardGameDBID = br.ReadInt64();
                spawnLocation.UnknownByte28 = br.ReadByte();
                spawnLocation.UnknownByte29 = br.ReadByte();
                spawnLocation.UnknownByte30 = br.ReadByte();
                spawnLocation.UnknownByte31 = br.ReadByte();
                spawnLocation.UnknownInt32 = br.ReadInt32();

                foreach (GenericRegionTrigger trigger in GenericRegionTriggers)
                {
                    if (trigger.GameDBID == spawnLocation.JunkyardGameDBID)
                    {
                        spawnLocation.JunkyardTrigger = trigger;
                        break;
                    }
                }

                SpawnLocationEntries.Add(spawnLocation);
            }

            br.BaseStream.Position = TriggerOffsetListOffset;

            for (int i = 0; i < TriggerOffsetListCount; i++)
            {
                uint section6Entry = br.ReadUInt32();
                TriggerOffsets.Add(section6Entry);
            }

            br.Close();
            ms.Close();

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
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(FormatRevision);
            long fileSizeOffset = bw.BaseStream.Position;
            bw.Write((int)0);
            bw.Write(Unknown0C);
            bw.Write(Unknown10);
            bw.Write(DevSpawnPositionX);
            bw.Write(DevSpawnPositionY);
            bw.Write(DevSpawnPositionZ);
            bw.Write(DevSpawnUnknownHash);
            bw.Write(DevSpawnRotationX);
            bw.Write(DevSpawnRotationY);
            bw.Write(DevSpawnRotationZ);
            bw.Write(DevSpawnUnknownFloat);
            bw.Write(LandmarkTriggersOffset);
            bw.Write(LandmarkTriggersCount);
            bw.Write(LandmarkNonFinishLineCount);
            bw.Write(BlackspotTriggersOffset);
            bw.Write(BlackspotTriggersCount);
            bw.Write(GenericRegionTriggersOffset);
            bw.Write(GenericRegionTriggersCount);
            bw.Write(Section4Offset);
            bw.Write(Section4Count);
            bw.Write(VFXBoxRegionOffset);
            bw.Write(VFXBoxRegionCount);
            bw.Write(StartPositionsOffset);
            bw.Write(StartPositionsCount);
            bw.Write(RoamingLocationsOffset);
            bw.Write(RoamingLocationsCount);
            bw.Write(SpawnLocationsOffset);
            bw.Write(SpawnLocationsCount);
            bw.Write(TriggerOffsetListOffset);
            bw.Write(TriggerOffsetListCount);

            bw.BaseStream.Position = LandmarkTriggersOffset;

            for (int i = 0; i < LandmarkTriggers.Count; i++)
            {
                LandmarkTrigger landmarkTrigger = LandmarkTriggers[i];

                bw.Write(landmarkTrigger.PositionX);
                bw.Write(landmarkTrigger.PositionY);
                bw.Write(landmarkTrigger.PositionZ);
                bw.Write(landmarkTrigger.RotationX);
                bw.Write(landmarkTrigger.RotationY);
                bw.Write(landmarkTrigger.RotationZ);
                bw.Write(landmarkTrigger.SizeX);
                bw.Write(landmarkTrigger.SizeY);
                bw.Write(landmarkTrigger.SizeZ);
                bw.Write(landmarkTrigger.GameDBID);
                bw.Write(landmarkTrigger.GlobalIndex);
                bw.Write(landmarkTrigger.Type);
                bw.Write(landmarkTrigger.UnknownByte2B);
                bw.Write(landmarkTrigger.UnknownOffset);
                bw.Write(landmarkTrigger.UnknownByte30);
                bw.Write(landmarkTrigger.LocalIndex);
                bw.Write(landmarkTrigger.Subtype);
                bw.Write(landmarkTrigger.UnknownByte33);
            }

            bw.BaseStream.Position = GenericRegionTriggersOffset;

            foreach (GenericRegionTrigger genericRegionTrigger in GenericRegionTriggers)
            {
                bw.Write(genericRegionTrigger.PositionX);
                bw.Write(genericRegionTrigger.PositionY);
                bw.Write(genericRegionTrigger.PositionZ);
                bw.Write(genericRegionTrigger.RotationX);
                bw.Write(genericRegionTrigger.RotationY);
                bw.Write(genericRegionTrigger.RotationZ);
                bw.Write(genericRegionTrigger.SizeX);
                bw.Write(genericRegionTrigger.SizeY);
                bw.Write(genericRegionTrigger.SizeZ);
                bw.Write(genericRegionTrigger.GameDBID);
                bw.Write(genericRegionTrigger.Index);
                bw.Write(genericRegionTrigger.Type);
                bw.Write(genericRegionTrigger.UnknownByte2B);
                bw.Write(genericRegionTrigger.GameDBID2);
                bw.Write(genericRegionTrigger.UnknownShort30);
                bw.Write(genericRegionTrigger.UnknownShort32);
                bw.Write(genericRegionTrigger.UnknownByte34);
                bw.Write(genericRegionTrigger.UnknownByte35);
                bw.Write(genericRegionTrigger.Subtype);
                bw.Write(genericRegionTrigger.UnknownByte37);
            }

            bw.BaseStream.Position = Section4Offset;

            foreach (TriggerSection4Entry section4Entry in Section4Entries)
            {
                bw.Write(section4Entry.TriggerOffsetListOffset);
                bw.Write(section4Entry.TriggerOffsetListCount);
                bw.Write(section4Entry.GameDBIDListOffset);
                bw.Write(section4Entry.GameDBIDListCount);

                long oldPosition = bw.BaseStream.Position;

                bw.BaseStream.Position = section4Entry.TriggerOffsetListOffset;
                foreach (GenericRegionTrigger trigger in section4Entry.Triggers)
                {
                    bw.Write(trigger.PositionX);
                    bw.Write(trigger.PositionY);
                    bw.Write(trigger.PositionZ);
                    bw.Write(trigger.RotationX);
                    bw.Write(trigger.RotationY);
                    bw.Write(trigger.RotationZ);
                    bw.Write(trigger.SizeX);
                    bw.Write(trigger.SizeY);
                    bw.Write(trigger.SizeZ);
                    bw.Write(trigger.GameDBID);
                    bw.Write(trigger.Index);
                    bw.Write(trigger.Type); 
                    bw.Write(trigger.UnknownByte2B);
                    bw.Write(trigger.GameDBID2); 
                    bw.Write(trigger.UnknownShort30);
                    bw.Write(trigger.UnknownShort32);
                    bw.Write(trigger.UnknownByte34);
                    bw.Write(trigger.UnknownByte35);
                    bw.Write(trigger.Subtype);
                    bw.Write(trigger.UnknownByte37);
                }
                bw.BaseStream.Position = section4Entry.GameDBIDListOffset;
                foreach(long id in section4Entry.GameDBIDs)
                {
                    bw.Write(id);
                }
                bw.BaseStream.Position = oldPosition;
            }

            bw.BaseStream.Position = RoamingLocationsOffset;

            for (int i = 0; i < RoamingLocationEntries.Count; i++)
            {
                RoamingLocation roamingLocation = RoamingLocationEntries[i];

                bw.Write(roamingLocation.PositionX);
                bw.Write(roamingLocation.PositionY);
                bw.Write(roamingLocation.PositionZ);
                bw.Write(roamingLocation.UnknownHash);
                bw.Write(roamingLocation.Subdistrict);
                bw.Write(roamingLocation.UnknownByte11);
                bw.Write(roamingLocation.UnknownByte12);
                bw.Write(roamingLocation.UnknownByte13);
                bw.Write(roamingLocation.UnknownInt14);
                bw.Write(roamingLocation.UnknownInt18);
                bw.Write(roamingLocation.UnknownInt1C);
            }

            bw.BaseStream.Position = SpawnLocationsOffset;

            for (int i = 0; i < SpawnLocationEntries.Count; i++)
            {
                SpawnLocation spawnLocation = SpawnLocationEntries[i];

                bw.Write(spawnLocation.PositionX);
                bw.Write(spawnLocation.PositionY);
                bw.Write(spawnLocation.PositionZ);
                bw.Write(spawnLocation.UnknownHash);
                bw.Write(spawnLocation.RotationX);
                bw.Write(spawnLocation.RotationY);
                bw.Write(spawnLocation.RotationZ);
                bw.Write(spawnLocation.UnknownFloat1C);
                bw.Write(spawnLocation.JunkyardGameDBID);
                bw.Write(spawnLocation.UnknownByte28);
                bw.Write(spawnLocation.UnknownByte29);
                bw.Write(spawnLocation.UnknownByte30);
                bw.Write(spawnLocation.UnknownByte31);
                bw.Write(spawnLocation.UnknownInt32);
            }

            bw.BaseStream.Position = TriggerOffsetListOffset;

            for (int i = 0; i < TriggerOffsets.Count; i++)
            {
                uint section6Entry = TriggerOffsets[i];
                bw.Write(section6Entry);
            }

            long fileSize = bw.BaseStream.Position;
            bw.BaseStream.Position = fileSizeOffset;
            bw.Write((int)fileSize);

            bw.BaseStream.Position = fileSize;

            long paddingCount = 16 - (bw.BaseStream.Position % 16);
            for (int i = 0; i < paddingCount; i++)
                bw.Write((byte)0);

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
