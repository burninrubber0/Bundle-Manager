using BundleFormat;
using BundleUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BundleManager
{
    public struct LandmarkTrigger
    {
        public float PositionX;
        public float PositionY;
        public float PositionZ;
        public float RotationX;
        public float RotationY;
        public float RotationZ;
        public float SizeX;
        public float SizeY;
        public float SizeZ;
        public int GameDBID;
        public short GlobalIndex;
        public byte Type; // Should be 0
        public byte UnknownByte2B;
        public uint UnknownOffset;
        public byte UnknownByte30;
        public byte LocalIndex;
        public byte Subtype;
        public byte UnknownByte33;

        public override string ToString() => $"ID {GameDBID}";
    }

    public struct GenericRegionTrigger
    {
        public float PositionX;
        public float PositionY;
        public float PositionZ;
        public float RotationX;
        public float RotationY;
        public float RotationZ;
        public float SizeX;
        public float SizeY;
        public float SizeZ;
        public int GameDBID;
        public short Index;
        public byte Type; // Should be 2
        public byte UnknownByte2B;
        public int GameDBID2; // Only used for _some_ super jumps
        public short UnknownShort30;
        public short UnknownShort32;
        public byte UnknownByte34;
        public byte UnknownByte35;
        public byte Subtype;
        public byte UnknownByte37; // most, but not all, super jumps have this set (to 1)

        public override string ToString() => $"ID {GameDBID}";
    }

    public struct TriggerSection4Entry
    {
        public uint TriggerOffsetListOffset;
        public int TriggerOffsetListCount;
        public uint GameDBIDListOffset;
        public int GameDBIDListCount;

        public List<GenericRegionTrigger> Triggers;
        public List<long> GameDBIDs;
    }

    public struct RoamingLocation
    {
        public float PositionX;
        public float PositionY;
        public float PositionZ;
        public uint UnknownHash;
        public byte Subdistrict;
        public byte UnknownByte11;
        public byte UnknownByte12;
        public byte UnknownByte13;
        public int UnknownInt14;
        public int UnknownInt18;
        public int UnknownInt1C;
    }

    public struct SpawnLocation
    {
        public float PositionX;
        public float PositionY;
        public float PositionZ;
        public uint UnknownHash;
        public float RotationX;
        public float RotationY;
        public float RotationZ;
        public float UnknownFloat1C;
        public long JunkyardGameDBID;
        public GenericRegionTrigger JunkyardTrigger;
        public byte UnknownByte28;
        public byte UnknownByte29;
        public byte UnknownByte30;
        public byte UnknownByte31;
        public int UnknownInt32;
    }

    public class TriggerData
    {
        public int RevisionNumber;
        public uint FileSize;
        public int Unknown0C;
        public int Unknown10;
        public float DevSpawnPositionX;
        public float DevSpawnPositionY;
        public float DevSpawnPositionZ;
        public uint DevSpawnUnknownHash;
        public float DevSpawnRotationX;
        public float DevSpawnRotationY;
        public float DevSpawnRotationZ;
        public float DevSpawnUnknownFloat;
        public uint LandmarkTriggersOffset;
        public int LandmarkTriggersCount;
        public int LandmarkNonFinishLineCount;
        public uint BlackspotTriggersOffset;
        public int BlackspotTriggersCount;
        public uint GenericRegionTriggersOffset;
        public int GenericRegionTriggersCount;
        public uint Section4Offset;
        public int Section4Count;
        public uint VFXBoxRegionOffset;
        public int VFXBoxRegionCount;
        public uint StartPositionsOffset;
        public int StartPositionsCount;
        public uint RoamingLocationsOffset;
        public int RoamingLocationsCount;
        public uint SpawnLocationsOffset;
        public int SpawnLocationsCount;
        public uint TriggerOffsetListOffset;
        public int TriggerOffsetListCount;

        public List<LandmarkTrigger> LandmarkTriggers;
        public SortedDictionary<uint, GenericRegionTrigger> GenericRegionTriggers;
        public List<TriggerSection4Entry> Section4Entries;
        public List<RoamingLocation> RoamingLocationEntries;
        public List<SpawnLocation> SpawnLocationEntries;
        public List<uint> TriggerOffsets;

        public TriggerData()
        {
            LandmarkTriggers = new List<LandmarkTrigger>();
            GenericRegionTriggers = new SortedDictionary<uint, GenericRegionTrigger>();
            Section4Entries = new List<TriggerSection4Entry>();
            RoamingLocationEntries = new List<RoamingLocation>();
            SpawnLocationEntries = new List<SpawnLocation>();
            TriggerOffsets = new List<uint>();
        }

        public static TriggerData Read(BundleEntry entry)
        {
            TriggerData result = new TriggerData();

            MemoryStream ms = entry.MakeStream();
            BinaryReader2 br = new BinaryReader2(ms);
            br.BigEndian = entry.Console;

            result.RevisionNumber = br.ReadInt32();
            result.FileSize = br.ReadUInt32();
            result.Unknown0C = br.ReadInt32();
            result.Unknown10 = br.ReadInt32();
            result.DevSpawnPositionX = br.ReadSingle();
            result.DevSpawnPositionY = br.ReadSingle();
            result.DevSpawnPositionZ = br.ReadSingle();
            result.DevSpawnUnknownHash = br.ReadUInt32();
            result.DevSpawnRotationX = br.ReadSingle();
            result.DevSpawnRotationY = br.ReadSingle();
            result.DevSpawnRotationZ = br.ReadSingle();
            result.DevSpawnUnknownFloat = br.ReadSingle();
            result.LandmarkTriggersOffset = br.ReadUInt32();
            result.LandmarkTriggersCount = br.ReadInt32();
            result.LandmarkNonFinishLineCount = br.ReadInt32();
            result.BlackspotTriggersOffset = br.ReadUInt32();
            result.BlackspotTriggersCount = br.ReadInt32();
            result.GenericRegionTriggersOffset = br.ReadUInt32();
            result.GenericRegionTriggersCount = br.ReadInt32();
            result.Section4Offset = br.ReadUInt32();
            result.Section4Count = br.ReadInt32();
            result.VFXBoxRegionOffset = br.ReadUInt32();
            result.VFXBoxRegionCount = br.ReadInt32();
            result.StartPositionsOffset = br.ReadUInt32();
            result.StartPositionsCount = br.ReadInt32();
            result.RoamingLocationsOffset = br.ReadUInt32();
            result.RoamingLocationsCount = br.ReadInt32();
            result.SpawnLocationsOffset = br.ReadUInt32();
            result.SpawnLocationsCount = br.ReadInt32();
            result.TriggerOffsetListOffset = br.ReadUInt32();
            result.TriggerOffsetListCount = br.ReadInt32();

            br.BaseStream.Position = result.LandmarkTriggersOffset;

            for (int i = 0; i < result.LandmarkTriggersCount; i++)
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

                result.LandmarkTriggers.Add(landmarkTrigger);
            }

            br.BaseStream.Position = result.GenericRegionTriggersOffset;

            for (int i = 0; i < result.GenericRegionTriggersCount; i++)
            {
                long startPosition = br.BaseStream.Position;

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

                result.GenericRegionTriggers.Add((uint)startPosition, genericRegionTrigger);
            }

            br.BaseStream.Position = result.Section4Offset;

            for (int i = 0; i < result.Section4Count; i++)
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
                    uint offset = br.ReadUInt32();
                    section4Entry.Triggers.Add(result.GenericRegionTriggers[offset]);
                }

                br.BaseStream.Position = section4Entry.GameDBIDListOffset;
                section4Entry.GameDBIDs = new List<long>();
                for (int j = 0; j < section4Entry.GameDBIDListCount; j++)
                {
                    section4Entry.GameDBIDs.Add(br.ReadInt64());
                }

                br.BaseStream.Position = oldPosition;

                result.Section4Entries.Add(section4Entry);
            }

            br.BaseStream.Position = result.RoamingLocationsOffset;

            for (int i = 0; i < result.RoamingLocationsCount; i++)
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

                result.RoamingLocationEntries.Add(roamingLocation);
            }

            br.BaseStream.Position = result.SpawnLocationsOffset;

            for (int i = 0; i < result.SpawnLocationsCount; i++)
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

                foreach (GenericRegionTrigger trigger in result.GenericRegionTriggers.Values)
                {
                    if (trigger.GameDBID == spawnLocation.JunkyardGameDBID)
                    {
                        spawnLocation.JunkyardTrigger = trigger;
                        break;
                    }
                }

                result.SpawnLocationEntries.Add(spawnLocation);
            }

            br.BaseStream.Position = result.TriggerOffsetListOffset;

            for (int i = 0; i < result.TriggerOffsetListCount; i++)
            {
                uint section6Entry = br.ReadUInt32();
                result.TriggerOffsets.Add(section6Entry);
            }

            br.Close();
            ms.Close();

            return result;
        }

        public void Write(BundleEntry entry)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(RevisionNumber);
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

            foreach (GenericRegionTrigger genericRegionTrigger in GenericRegionTriggers.Values)
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

                // TODO: write list
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
                bw.Write((byte) 0);

            bw.Flush();

            byte[] data = ms.ToArray();

            bw.Close();
            ms.Close();

            entry.Header = data;
            entry.Dirty = true;
        }
    }
}
