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
    public class Traffic
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

        public static Traffic Read(BundleEntry entry)
        {
            Traffic result = new Traffic();

            MemoryStream ms = entry.MakeStream();
            BinaryReader2 br = new BinaryReader2(ms);
            br.BigEndian = entry.Console;

            result.Unknown1 = br.ReadInt16();
            result.Unknown2 = br.ReadInt16();
            result.FileSize = br.ReadInt32();
            result.TrackCount = br.ReadInt32();
            result.Unknown3 = br.ReadInt32();
            result.PointerOffset = br.ReadInt32();
            result.Unknown4 = br.ReadInt16();
            result.Unknown5 = br.ReadInt16();
            result.Unknown6 = br.ReadByte();
            result.Unknown7 = br.ReadByte();
            result.Unknown8 = br.ReadInt16();
            result.Unknown9 = br.ReadInt16();
            result.Unknown10 = br.ReadInt16();
            result.Section1Offset = br.ReadUInt32();
            result.Section2Offset = br.ReadUInt32();
            result.Section3Offset = br.ReadUInt32();
            result.Section4Offset = br.ReadUInt32();
            result.Section5Offset = br.ReadUInt32();
            result.Section6Offset = br.ReadUInt32();
            result.Section7Offset = br.ReadUInt32();
            result.Section8Offset = br.ReadUInt32();
            result.Unknown11 = br.ReadByte();
            result.Unknown12 = br.ReadByte();
            result.Unknown13 = br.ReadInt16();
            result.UnknownOffset1 = br.ReadUInt32();
            result.UnknownOffset2 = br.ReadUInt32();
            result.UnknownOffset3 = br.ReadUInt32();
            result.UnknownOffset4 = br.ReadUInt32();
            result.UnknownOffset5 = br.ReadUInt32();
            result.Unknown14 = br.ReadInt16();
            result.Unknown15 = br.ReadInt16();

            for (int i = 0; i < 128; i++)
            {
                result.UnknownShorts.Add(br.ReadInt16());
            }

            br.Close();
            ms.Close();

            return result;
        }

        public void Write(BundleEntry entry)
        {
            
        }
    }
}
