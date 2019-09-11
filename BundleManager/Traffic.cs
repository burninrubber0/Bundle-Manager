using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BundleFormat;
using BundleUtilities;

namespace BundleManager
{
    public class Traffic : IEntryData
    {
        public short Unknown1;
        public short Unknown2;
        public int FileSize;
        public int TrackCount;
        public int Unknown3;
        public int PointerOffset;
        public short Unknown4;
        public short Unknown5;
        public byte Unknown6;
        public byte Unknown7;
        public short Unknown8;
        public short Unknown9;
        public short Unknown10;
        public uint Section1Offset;
        public uint Section2Offset;
        public uint Section3Offset;
        public uint Section4Offset;
        public uint Section5Offset;
        public uint Section6Offset;
        public uint Section7Offset;
        public uint Section8Offset;
        public byte Unknown11;
        public byte Unknown12;
        public short Unknown13;
        public uint UnknownOffset1;
        public uint UnknownOffset2;
        public uint UnknownOffset3;
        public uint UnknownOffset4;
        public uint UnknownOffset5;
        public short Unknown14;
        public short Unknown15;

        public List<short> UnknownShorts;

        public Traffic()
        {
            UnknownShorts = new List<short>();
        }

		public IEntryEditor GetEditor(BundleEntry entry)
		{
			return null;
		}

		public EntryType GetEntryType(BundleEntry entry)
		{
			return EntryType.TrafficDataResourceType;
		}

		private void Clear()
		{
			Unknown1 = default;
			Unknown2 = default;
			FileSize = default;
			TrackCount = default;
			Unknown3 = default;
			PointerOffset = default;
			Unknown4 = default;
			Unknown5 = default;
			Unknown6 = default;
			Unknown7 = default;
			Unknown8 = default;
			Unknown9 = default;
			Unknown10 = default;
			Section1Offset = default;
			Section2Offset = default;
			Section3Offset = default;
			Section4Offset = default;
			Section5Offset = default;
			Section6Offset = default;
			Section7Offset = default;
			Section8Offset = default;
			Unknown11 = default;
			Unknown12 = default;
			Unknown13 = default;
			UnknownOffset1 = default;
			UnknownOffset2 = default;
			UnknownOffset3 = default;
			UnknownOffset4 = default;
			UnknownOffset5 = default;
			Unknown14 = default;
			Unknown15 = default;

			UnknownShorts.Clear();
		}

		public bool Read(BundleEntry entry, ILoader loader = null)
        {
			Clear();

            MemoryStream ms = entry.MakeStream();
            BinaryReader2 br = new BinaryReader2(ms);
            br.BigEndian = entry.Console;

            Unknown1 = br.ReadInt16();
            Unknown2 = br.ReadInt16();
            FileSize = br.ReadInt32();
            TrackCount = br.ReadInt32();
            Unknown3 = br.ReadInt32();
            PointerOffset = br.ReadInt32();
            Unknown4 = br.ReadInt16();
            Unknown5 = br.ReadInt16();
            Unknown6 = br.ReadByte();
            Unknown7 = br.ReadByte();
            Unknown8 = br.ReadInt16();
            Unknown9 = br.ReadInt16();
            Unknown10 = br.ReadInt16();
            Section1Offset = br.ReadUInt32();
            Section2Offset = br.ReadUInt32();
            Section3Offset = br.ReadUInt32();
            Section4Offset = br.ReadUInt32();
            Section5Offset = br.ReadUInt32();
            Section6Offset = br.ReadUInt32();
            Section7Offset = br.ReadUInt32();
            Section8Offset = br.ReadUInt32();
            Unknown11 = br.ReadByte();
            Unknown12 = br.ReadByte();
            Unknown13 = br.ReadInt16();
            UnknownOffset1 = br.ReadUInt32();
            UnknownOffset2 = br.ReadUInt32();
            UnknownOffset3 = br.ReadUInt32();
            UnknownOffset4 = br.ReadUInt32();
            UnknownOffset5 = br.ReadUInt32();
            Unknown14 = br.ReadInt16();
            Unknown15 = br.ReadInt16();

            for (int i = 0; i < 128; i++)
            {
                UnknownShorts.Add(br.ReadInt16());
            }

            br.Close();
            ms.Close();

            return true;
        }

        public bool Write(BundleEntry entry)
        {
			return true;
        }
    }
}
