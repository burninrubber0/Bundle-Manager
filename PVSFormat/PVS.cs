using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BundleFormat;
using BundleUtilities;

namespace PVSFormat
{
    public struct LocationData
    {
        public float X;
        public float Y;
        public float Unknown1;
        public int Unknown2;

        public override string ToString()
        {
            return "X: " + X + ", Y: " + Y + ", Unk1: " + Unknown1 + ", Unk2: " + Unknown2;
        }
    }

    public struct NeighborData
    {
        public int NeighborIndex;
        public uint NeighborPtr;
        public int Type;
        public int Unknown1;
        public int Unknown2;

        public override string ToString()
        {
            return "NeighborIndex: " + NeighborIndex + ", Type: " + Type + ", Unk1: " + Unknown1 + ", Unk2: " + Unknown2;
        }
    }

    public struct PVSEntry
    {
        public uint Address;
        public uint Ptr1;
        public int Unknown1;
        public uint Ptr2;
        public int Unknown2;
        public int TrackID;
        public int Unknown3;
        public short Unknown4;
        public short Count1;
        public short Unknown5;
        public short Count2;
        public int Unknown6;
        public int Unknown7;
        public int Unknown8;
        public int Unknown9;

        public List<LocationData> LocationData;
        public List<NeighborData> NeighborData;

        public override string ToString()
        {
            return "TrackID: " + TrackID;
        }
    }

    public class PVS : IEntryData
    {
		public static Image GameMap;

        public int Unknown1;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
        public int Unknown6;
        public int Unknown7;
        public int Unknown8;
        public List<PVSEntry> Entries;

        public PVS()
        {
            Entries = new List<PVSEntry>();
        }

		private void Clear()
		{
			Unknown1 = default;
			Unknown2 = default;
			Unknown3 = default;
			Unknown4 = default;
			Unknown6 = default;
			Unknown7 = default;
			Unknown8 = default;

			Entries.Clear();
		}

        public bool Read(BundleEntry entry)
        {
			Clear();

            Stream s = entry.MakeStream();
            BinaryReader2 br = new BinaryReader2(s);
            br.BigEndian = entry.Console;

            Unknown1 = br.ReadInt32();
            Unknown2 = br.ReadInt32();
            Unknown3 = br.ReadInt32();
            Unknown4 = br.ReadInt32();
            int numEntries = br.ReadInt32();
            Unknown6 = br.ReadInt32();
            Unknown7 = br.ReadInt32();
            Unknown8 = br.ReadInt32();

            for (int i = 0; i < numEntries; i++)
            {
                PVSEntry pvsEntry = new PVSEntry();

                pvsEntry.Address = (uint)br.BaseStream.Position;
                pvsEntry.Ptr1 = br.ReadUInt32();
                pvsEntry.Unknown1 = br.ReadInt32();
                pvsEntry.Ptr2 = br.ReadUInt32();
                pvsEntry.Unknown2 = br.ReadInt32();
                pvsEntry.TrackID = br.ReadInt32();
                pvsEntry.Unknown3 = br.ReadInt32();
                pvsEntry.Unknown4 = br.ReadInt16();
                pvsEntry.Count1 = br.ReadInt16();
                pvsEntry.Unknown5 = br.ReadInt16();
                pvsEntry.Count2 = br.ReadInt16();
                pvsEntry.Unknown6 = br.ReadInt32();
                pvsEntry.Unknown7 = br.ReadInt32();
                pvsEntry.Unknown8 = br.ReadInt32();
                pvsEntry.Unknown9 = br.ReadInt32();

                long pos = br.BaseStream.Position;

                br.BaseStream.Position = (long)pvsEntry.Ptr1;

                pvsEntry.LocationData = new List<LocationData>();
                for (int j = 0; j < pvsEntry.Count1; j++)
                {
                    LocationData data = new LocationData();

                    data.X = br.ReadSingle();
                    data.Y = br.ReadSingle();
                    data.Unknown1 = br.ReadSingle();
                    data.Unknown2 = br.ReadInt32();

                    pvsEntry.LocationData.Add(data);
                }

                br.BaseStream.Position = (long) pvsEntry.Ptr2;

                pvsEntry.NeighborData = new List<NeighborData>();
                for (int j = 0; j < pvsEntry.Count2; j++)
                {
                    NeighborData data = new NeighborData();

                    data.NeighborIndex = -1;

                    data.NeighborPtr = br.ReadUInt32();
                    data.Type = br.ReadInt32();
                    data.Unknown1 = br.ReadInt32();
                    data.Unknown2 = br.ReadInt32();

                    pvsEntry.NeighborData.Add(data);
                }

                br.BaseStream.Position = pos;

                Entries.Add(pvsEntry);
            }

            for (int i = 0; i < Entries.Count; i++)
            {
                for (int k = 0; k < Entries[i].NeighborData.Count; k++)
                {
                    NeighborData data = Entries[i].NeighborData[k];
                    uint ptr = data.NeighborPtr;
                    for (int j = 0; j < Entries.Count; j++)
                    {
                        if (Entries[j].Address == ptr)
                        {
                            data.NeighborIndex = j;
                            Entries[i].NeighborData[k] = data;
                            break;
                        }
                    }
                }
            }

            br.Close();
            s.Close();

            return true;
        }

        public bool Write(BundleEntry entry)
        {
            return true;
        }

		public EntryType GetEntryType(BundleEntry entry)
		{
			return EntryType.ZoneListResourceType;
		}

		public IEntryEditor GetEditor(BundleEntry entry)
		{
			PVSEditor pvsForm = new PVSEditor();
			pvsForm.GameMap = GameMap;
			pvsForm.Open(this);

			return pvsForm;
		}
	}
}
